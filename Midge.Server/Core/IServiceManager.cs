using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Core
{
	public interface IServiceManager
	{
		bool AddService<T>(T service) where T: class;
		bool AddService(Type type, object value);
		T GetService<T>() where T: class;
		T TryGetService<T>() where T: class;
	}
}
