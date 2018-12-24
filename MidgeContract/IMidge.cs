using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MidgeContract
{
	[ServiceContract]
	public interface IMidge: IConnectionCheckable
	{
		[OperationContract]
		[WebGet]
		void SetVolume(int value);

		[OperationContract]
		[WebGet]
		int GetVolume();

		[OperationContract]
		[WebGet]
		void SetMute(bool value);

		[OperationContract]
		[WebGet]
		bool IsMute();

		[OperationContract]
		[WebGet]
		int StartProcess(string name, params string[] args);

		[OperationContract]
		[WebGet]
		bool KillProcess(int id);

		[OperationContract]
		[WebGet]
		IEnumerable<ProcessInfo> GetProcesses(bool withSvchosts = false);
	}

}
