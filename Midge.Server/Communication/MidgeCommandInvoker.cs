using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Midge.Server.Communication.Core;
using Midge.Server.Core;

namespace Midge.Server.Communication
{
	public class MidgeCommandInvoker
	{
		public MidgeControllerInfo Controller { get; }
		public MidgeCommandInfo Command { get; }

		private readonly object[] _args;

		public MidgeCommandInvoker(MidgeControllerInfo controller, MidgeCommandInfo command, object[] args)
		{
			Controller = controller;
			Command = command;
			_args = args;
		}

		public Task InvokeAsync([NotNull] MidgeContext context, [NotNull] IServiceManager serviceManager, Action<ControllerBase> onComplete = null, 
			Action<ControllerBase, Exception> onFailed = null)
		{
			var controllerInstance = (ControllerBase)Activator.CreateInstance(Controller.ControllerType, 
				context, serviceManager);

			return Task.Run(() =>
			{
				try
				{
					Command.MethodInfo.Invoke(controllerInstance, _args);
					onComplete?.Invoke(controllerInstance);
				}
				catch (Exception ex)
				{
					onFailed?.Invoke(controllerInstance, ex);
				}
			});
		}

		public void Invoke([NotNull] MidgeContext context, [NotNull] IServiceManager serviceManager, Action<ControllerBase> onComplete = null,
			Action<ControllerBase, Exception> onFailed = null)
		{
			var controllerInstance = (ControllerBase)Activator.CreateInstance(Controller.ControllerType,
				context, serviceManager);

			try
			{
				Command.MethodInfo.Invoke(controllerInstance, _args);
				onComplete?.Invoke(controllerInstance);
			}
			catch (Exception ex)
			{
				onFailed?.Invoke(controllerInstance, ex);
			}
		}
	}
}