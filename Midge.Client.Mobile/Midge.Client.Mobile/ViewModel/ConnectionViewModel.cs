using System;
using System.Collections.Generic;
using System.Text;
using Midge.Client.Mobile.Core;
using Midge.Client.Mobile.Model;

namespace Midge.Client.Mobile.ViewModel
{
	public class ConnectionViewModel
	{
		public ConnectionModel Connection { get; set; }

		public ConnectionViewModel()
		{
			Connection = new ConnectionModel { State = MidgeCore.Instance.State };
			MidgeCore.Instance.ConnectionStateChanged += ConnectionStateChanged;
		}

		private void ConnectionStateChanged(object sender, EventArgs e)
		{
			Connection.State = ((MidgeCore) sender).State;
		}
	}
}
