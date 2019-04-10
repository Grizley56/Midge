using System;
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
		private static ISettings AppSettings
		{
			get { return CrossSettings.Current; }
		}

		#region Setting Constants
		private const string IpAddressKey = "ip_address";
		private static readonly string IpAddressDefault = "192.168.1.111";

		private const string PortKey = "port";
		private static readonly int PortDefault = 8733;

		private const string ReconnectionTimeoutKey = "reconnectionTimeout";
		private static readonly int ReconnectionTimeoutDefault = 5000;

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

		public static int ReconnectionTimeout
		{
			get => AppSettings.GetValueOrDefault(ReconnectionTimeoutKey, ReconnectionTimeoutDefault);
			set => AppSettings.AddOrUpdateValue(ReconnectionTimeoutKey, value);
		}
	}

}
