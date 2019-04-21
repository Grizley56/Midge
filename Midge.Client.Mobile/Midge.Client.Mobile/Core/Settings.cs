using System;
using System.Net;
using JetBrains.Annotations;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Midge.Client.Mobile.Core
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings => CrossSettings.Current;

		#region Setting Constants
		private const string IpAddressKey = "ip_address";
		private static readonly string IpAddressDefault = "192.168.1.111";

		private const string PortKey = "port";
		private static readonly int PortDefault = 8733;

		private const string StreamingServerPortKey = "s_streaming_port";
		private static readonly int StreamingServerPortDefault = 8734;


		private const string StreamingClientPortKey = "streaming_port";
		private static readonly int StreamingClientPortDefault = 6602;

		private const string ReconnectionTimeoutKey = "reconnectionTimeout";
		private static readonly int ReconnectionTimeoutDefault = 5000;

		private const string MouseSensitivityKey = "mouseSensitivity";
		private static readonly float MouseSensitivityDefault = 1f;

		#endregion

		public static string IpAddress
		{
			get => AppSettings.GetValueOrDefault(IpAddressKey, IpAddressDefault);
			set => AppSettings.AddOrUpdateValue(IpAddressKey, value);
		}

		public static int Port
		{
			get => AppSettings.GetValueOrDefault(PortKey, PortDefault);
			set => AppSettings.AddOrUpdateValue(PortKey, value);
		}

		public static int StreamingServerPort
		{
			get => AppSettings.GetValueOrDefault(StreamingServerPortKey, StreamingServerPortDefault);
			set => AppSettings.AddOrUpdateValue(StreamingServerPortKey, value);
		}

		public static int StreamingClientPort
		{
			get => AppSettings.GetValueOrDefault(StreamingClientPortKey, StreamingClientPortDefault);
			set => AppSettings.AddOrUpdateValue(StreamingClientPortKey, value);
		}

		public static int ReconnectionTimeout
		{
			get => AppSettings.GetValueOrDefault(ReconnectionTimeoutKey, ReconnectionTimeoutDefault);
			set => AppSettings.AddOrUpdateValue(ReconnectionTimeoutKey, value);
		}

		public static float MouseSensitivity
		{
			get => AppSettings.GetValueOrDefault(MouseSensitivityKey, MouseSensitivityDefault);
			set => AppSettings.AddOrUpdateValue(MouseSensitivityKey, value);
		}

		[CanBeNull]
		public static IPEndPoint ServerAddress
		{
			get
			{
				IPAddress ip;

				int port = Settings.Port;

				if (!IPAddress.TryParse(Settings.IpAddress, out ip))
					return null;

				return new IPEndPoint(ip, port);
			}
		}

		[CanBeNull]
		public static IPEndPoint ServerStreamAddress
		{
			get
			{
				IPAddress ip;

				int port = Settings.StreamingServerPort;

				if (!IPAddress.TryParse(Settings.IpAddress, out ip))
					return null;

				return new IPEndPoint(ip, port);
			}
		}
	}

}
