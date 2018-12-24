using System;
using System.Collections.Generic;
using System.Text;
using Midge.Core;
using Midge.Model;
using MidgeContract;

namespace Midge.ViewModel
{
	public class ConnectionViewModel
	{
		public ConnectionModel Connection { get; set; }
		public ConnectionViewModel()
		{
			Connection = new ConnectionModel {State = ReconnectableChannel<IMidge>.MidgeInstance.State};

			ReconnectableChannel<IMidge>.MidgeInstance.ChannelStateChanged += ChannelStateChanged;
		}

		private void ChannelStateChanged(object sender, EventArgs e)
		{
			Connection.State = ReconnectableChannel<IMidge>.MidgeInstance.State;
		}
	}
}
