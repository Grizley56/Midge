﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Midge.Server.Windows.Utils;

namespace Midge.Server.Windows.Services
{
	public enum AppState
	{
		Undefined = 0,
		Running = 1,
		Exiting = 2,
		Exited = 3,
	}

	public class ConsoleOutputEventArgs : EventArgs
	{
		public ConsoleOutputEventArgs(string line, bool isError)
		{
			Line = line;
			IsError = isError;
		}

		public string Line { get; private set; }
		public bool IsError { get; private set; }
	}

	public class ConsoleApp : IDisposable
	{
		public Process Process { get; private set; }

		private AutoResetEvent _processEvent;
		private ConcurrentQueue<ConsoleOutputEventArgs> _consoleOutputQueue;

		private Task _processMonitor;
		private CancellationTokenSource _cancellationTokenSource;

		private readonly object _stateLock = new object();
		private readonly Win32.ConsoleCtrlEventHandler _consoleCtrlEventHandler;

		/// <summary>
		/// ConsoleApp constructor
		/// </summary>
		/// <param name="fileName">File name or DOS command</param>
		/// <param name="cmdLine">Command-line arguments</param>
		public ConsoleApp(string fileName, string cmdLine)
		{
			FileName = fileName;
			CmdLine = cmdLine;

			_consoleCtrlEventHandler += ConsoleCtrlHandler;
		}

		/// <summary>
		/// File name of the app, e.g. "cmd.exe"
		/// </summary>
		public string FileName { get; private set; }

		/// <summary>
		/// Command line of the app, e.g. "/c dir"
		/// </summary>
		public string CmdLine { get; private set; }

		/// <summary>
		/// Current state of the app.
		/// </summary>
		public AppState State { get; private set; }

		/// <summary>
		/// Exit code of the app.
		/// </summary>
		public int? ExitCode { get; private set; }

		/// <summary>
		/// Time the app has exited.
		/// </summary>
		public DateTime? ExitTime { get; private set; }

		/// <summary>
		/// Start the app.
		/// </summary>
		public void Run()
		{
			ThrowIfDisposed();

			lock (_stateLock)
			{
				if (State == AppState.Running)
					throw new InvalidOperationException("App is already running.");

				if (State == AppState.Exiting)
					throw new InvalidOperationException("App is exiting.");

				StartProcessAsync();

				State = AppState.Running;
			}
		}

		/// <summary>
		/// Stop the app.
		/// </summary>
		/// <param name="closeKey">Special key to send to close the app [default=Ctrl-C]</param>
		/// <param name="forceCloseMillisecondsTimeout">Timeout to wait before closing the app forcefully [default=infinite]</param>
		public void Stop(ConsoleSpecialKey closeKey = ConsoleSpecialKey.ControlC, int forceCloseMillisecondsTimeout = Timeout.Infinite)
		{
			ThrowIfDisposed();

			lock (_stateLock)
			{
				if (State == AppState.Undefined || State == AppState.Exited)
					throw new InvalidOperationException("App is not running.");

				if (State == AppState.Exiting)
					throw new InvalidOperationException("App is already exiting.");

				State = AppState.Exiting;

				Task.Factory.StartNew(() => CloseConsole(closeKey, forceCloseMillisecondsTimeout),
						TaskCreationOptions.LongRunning);
			}
		}

		/// <summary>
		/// Wait until the app exits.
		/// </summary>
		/// <param name="millisecondsTimeout">Timeout to wait until the app is exited [default=infinite]</param>
		/// <returns>True if exited or False if timeout elapsed</returns>
		public bool WaitForExit(int millisecondsTimeout = Timeout.Infinite)
		{
			ThrowIfDisposed();

			if (State == AppState.Undefined || _processMonitor == null)
			{
				Trace.TraceWarning("App hasn't started yet");
				return true;
			}

			if (_processMonitor.IsCompleted)
				return true;

			Trace.TraceInformation("Waiting for app exit: Timeout={0}", millisecondsTimeout);
			return _processMonitor.Wait(millisecondsTimeout);
		}

		/// <summary>
		/// Writes a string to the console.
		/// </summary>
		public void Write(string value)
		{
			if (Process == null || Process.HasExited)
				return;

			Trace.TraceInformation("stdin< {0}", value);
			Process.StandardInput.Write(value);
		}

		/// <summary>
		/// Writes a string followed by a line terminator to the console.
		/// </summary>
		public void WriteLine(string value)
		{
			if (Process == null || Process.HasExited)
				return;

			Trace.TraceInformation("stdin< {0} <eol", value);
			Process.StandardInput.WriteLine(value);
		}

		/// <summary>
		/// Fires when the app exits.
		/// </summary>
		public event EventHandler<EventArgs> Exited;

		/// <summary>
		/// Fires when the console outputs a new line or error.
		/// </summary>
		/// <remarks>The lines are queued and guaranteed to follow the output order</remarks>
		public event EventHandler<ConsoleOutputEventArgs> ConsoleOutput;

		private void StartProcessAsync()
		{
			Process = new Process
			{
				EnableRaisingEvents = true,
				StartInfo =
								{
										FileName = FileName,
										Arguments = CmdLine,
										CreateNoWindow = true,
										RedirectStandardError = true,
										RedirectStandardOutput = true,
										RedirectStandardInput = true,
										UseShellExecute = false,
								},
			};

			try
			{
				Trace.TraceInformation("Starting app: '{0} {1}'", FileName, CmdLine);

				_processEvent = new AutoResetEvent(false);
				_consoleOutputQueue = new ConcurrentQueue<ConsoleOutputEventArgs>();

				_cancellationTokenSource = new CancellationTokenSource();
				var cancellationToken = _cancellationTokenSource.Token;
				_processMonitor = new Task(MonitoringHandler, cancellationToken, TaskCreationOptions.LongRunning);
				_processMonitor.Start();

				Process.OutputDataReceived += OnOutputLineReceived;
				Process.ErrorDataReceived += OnErrorLineReceived;
				Process.Exited += OnProcessExited;

				Process.Start();

				Process.BeginErrorReadLine();
				Process.BeginOutputReadLine();
			}
			catch (Exception ex)
			{
				Trace.TraceError("Could not start app: '{0} {1}', Error={2}", FileName, CmdLine, ex);

				FreeProcessResources();
				if (_cancellationTokenSource != null)
				{
					_cancellationTokenSource.Cancel();
					_processMonitor = null;
				}
				throw;
			}
		}

		private void MonitoringHandler(object obj)
		{
			var cancellationToken = (CancellationToken)obj;
			var supportedEvents = new[] { _processEvent, cancellationToken.WaitHandle };

			while (!cancellationToken.IsCancellationRequested)
			{
				WaitHandle.WaitAny(supportedEvents);

				// always dispatch output is case more than one event becomes signaled
				DispatchProcessOutput();
			}

			HandleProcessExit();
		}

		private void DispatchProcessOutput()
		{
			ConsoleOutputEventArgs args;
			while (_consoleOutputQueue.TryDequeue(out args))
			{
				try
				{
					OnConsoleOutput(args);
				}
				catch (Exception ex)
				{
					Trace.TraceError("OnConsoleOutput exception ignored: Error={0}", ex.ToString());
				}
			}
		}

		private void HandleProcessExit()
		{
			if (Process == null)
				return;

			lock (_stateLock)
			{
				ExitCode = Process.ExitCode;
				ExitTime = Process.ExitTime;

				FreeProcessResources();
				State = AppState.Exited;
			}
			try
			{
				OnExited(new EventArgs());
			}
			catch (Exception ex)
			{
				Trace.TraceError("OnExited exception ignored: Error={0}", ex.ToString());
			}
		}

		private void CloseConsole(ConsoleSpecialKey closeKey, int forceCloseMillisecondsTimeout)
		{
			if (Process == null || Process.HasExited)
				return;

			Trace.TraceInformation("Closing app input by sending Ctrl-Z signal");
			Process.StandardInput.Close();

			if (Process == null || Process.HasExited)
				return;

			Trace.TraceInformation("Trying to close the app gracefully by sending " + closeKey);
			Win32.AttachConsole((uint)Process.Id);
			Win32.SetConsoleCtrlHandler(_consoleCtrlEventHandler, true);
			var ctrlType = closeKey == ConsoleSpecialKey.ControlC ? Win32.CtrlType.CtrlCEvent : Win32.CtrlType.CtrlBreakEvent;
			Win32.GenerateConsoleCtrlEvent(ctrlType, 0);

			if (Process == null || Process.HasExited)
				return;

			// close console forcefully if not finished within allowed timeout
			Trace.TraceInformation("Waiting for voluntary app exit: Timeout={0}", forceCloseMillisecondsTimeout);
			var exited = Process.WaitForExit(forceCloseMillisecondsTimeout);
			if (!exited)
			{
				Trace.TraceWarning("Closing the app forcefully");
				Process.Kill();
			}
		}

		private static bool ConsoleCtrlHandler(Win32.CtrlType ctrlType)
		{
			const bool ignore = true;
			return ignore;
		}

		private void FreeProcessResources()
		{
			if (Process != null)
			{
				Process.OutputDataReceived -= OnOutputLineReceived;
				Process.ErrorDataReceived -= OnErrorLineReceived;
				Process.Exited -= OnProcessExited;

				Process.Close();
				Process.Dispose();
				Process = null;
			}
		}

		private void OnOutputLineReceived(object sender, DataReceivedEventArgs e)
		{
			_consoleOutputQueue.Enqueue(new ConsoleOutputEventArgs(e.Data, false));
			_processEvent.Set();
		}

		private void OnErrorLineReceived(object sender, DataReceivedEventArgs e)
		{
			_consoleOutputQueue.Enqueue(new ConsoleOutputEventArgs(e.Data, true));
			_processEvent.Set();
		}

		private void OnProcessExited(object sender, EventArgs e)
		{
			_cancellationTokenSource.Cancel();
			_processEvent.Set();
		}

		protected virtual void OnConsoleOutput(ConsoleOutputEventArgs e)
		{
			if (e.Line == null)
				return;


			var handler = ConsoleOutput;
			handler?.Invoke(this, e);
		}

		protected virtual void OnExited(EventArgs e)
		{
			var handler = Exited;
			handler?.Invoke(this, e);
		}


		#region IDisposable

		private bool _disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					CloseConsole(ConsoleSpecialKey.ControlBreak, 500);
					WaitForExit(500);
					FreeProcessResources();
				}

				_disposed = true;
			}
		}

		~ConsoleApp()
		{
			Dispose(false);
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
				throw new ObjectDisposedException("Object was disposed.");
		}

		#endregion


		#region Static utility methods

		public class Result
		{
			public int ExitCode;
			public string Output;
		}

		/// <summary>
		/// Run console app synchronously and capture all its output including standard error stream.
		/// </summary>
		/// <param name="fileName">File name or DOS command</param>
		/// <param name="cmdLine">Command-line arguments</param>
		/// <returns>Execution result</returns>
		public static Result Run(string fileName, string cmdLine = null)
		{
			using (var app = new ConsoleApp(fileName, cmdLine))
			{
				var outputStringBuilder = new StringBuilder();
				app.ConsoleOutput += (o, args) => outputStringBuilder.AppendLine(args.Line);

				app.Run();
				app.WaitForExit();

				var result = new Result
				{
					ExitCode = app.ExitCode.GetValueOrDefault(),
					Output = outputStringBuilder.ToString()
				};

				return result;
			}
		}

		#endregion
	}
}
