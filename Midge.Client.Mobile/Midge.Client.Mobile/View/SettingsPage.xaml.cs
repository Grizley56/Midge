﻿using System;
using System.Net;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.View
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
		}

		private void ContentPageAppearing(object sender, EventArgs e)
		{
			IpEntry.Text = Settings.IpAddress;
			PortEntry.Text = Settings.Port.ToString();
			MouseSensitivitySlider.Value = Settings.MouseSensitivity;
		}

		private void ButtonClicked(object sender, EventArgs e)
		{
			if (!int.TryParse(PortEntry.Text, out var port) || !(port > 1024 && port < 65535))
			{
				DisplayAlert("Invalid port", "Operation failed", "OK");
				return;
			}

			if (!IPAddress.TryParse(IpEntry.Text, out var ip))
			{
				DisplayAlert("Invalid ip-address", "Operation failed", "OK");
				return;
			}

			Settings.IpAddress = ip.ToString();
			Settings.Port = port;
			Settings.MouseSensitivity = (float)MouseSensitivitySlider.Value;
		}
	}
}
