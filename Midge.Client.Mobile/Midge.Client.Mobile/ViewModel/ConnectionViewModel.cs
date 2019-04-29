using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Midge.Client.Mobile.Core;
using Midge.Client.Mobile.Model;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class ConnectionViewModel
	{
		public ConnectionModel Connection { get; set; }

		public ICommand ReconnectCommand { get; private set; }

		public ConnectionViewModel()
		{
			ReconnectCommand = new Command(async () => { await MidgeCore.Instance.Restart(); });
			Connection = new ConnectionModel { State = MidgeCore.Instance.State };
			MidgeCore.Instance.ConnectionStateChanged += ConnectionStateChanged;
		}

		private void ConnectionStateChanged(object sender, EventArgs e)
		{
			Connection.State = ((MidgeCore) sender).State;
		}
	}
}
