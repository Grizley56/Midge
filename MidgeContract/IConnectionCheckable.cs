using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace MidgeContract
{
	[ServiceContract]
	public interface IConnectionCheckable
	{
		[OperationContract]
		[WebGet]
		void IsConnected();
	}
}
