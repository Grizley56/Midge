using System;
using System.Collections.Generic;
using Midge.Server.Core;

namespace Midge.Server
{
	public class MidgeServiceManager: IServiceManager
	{
		private readonly Dictionary<Type, object> _services;
		private readonly object _lock = new object();

		public MidgeServiceManager()
		{
			_services = new Dictionary<Type, object>();
		}


		public bool AddService<T>(T service) where T : class
		{
			return AddService(typeof(T), service);
		}

		public bool AddService(Type type, object value)
		{
			lock (_lock)
			{
				if (_services.ContainsKey(type))
					return false;

				_services.Add(type, value);
				return true;
			}
		}

		public T GetService<T>() where T : class
		{
			lock (_lock)
			{
				return (T)_services[typeof(T)];
			}
		}

		public T TryGetService<T>() where T : class
		{
			lock (_lock)
			{
				if (_services.ContainsKey(typeof(T)))
					return null;

				return (T) _services[typeof(T)];
			}
		}
	}
}
