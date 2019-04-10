using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API
{
	public class MidgeParameters: Dictionary<string, string>
	{
		public static MidgeParameters Empty => new MidgeParameters();

		public MidgeParameters(IDictionary<string, string> dict)
			:base(dict)
		{
			
		}

		public MidgeParameters()
		{
			
		}

		public void Add(string key, long value)
		{
			Remove(key);
			Add(key, value.ToString());
		}

		public void Add(string key, byte value)
		{
			Add(key, (long)value);
		}

		public void Add(string key, short value)
		{
			Add(key, (long)value);
		}

		public void Add(string key, int value)
		{
			Add(key, (long) value);
		}

		public void Add(string key, DateTime? value)
		{
			Remove(key);

			if (value == null)
			{
				return;
			}


			var totalSeconds =
				(value.Value.ToUniversalTime()
				 - new DateTime(1970,
					 1,
					 1,
					 0,
					 0,
					 0,
					 DateTimeKind.Utc)).TotalSeconds;

			var offset = Convert.ToInt64(value: totalSeconds);

			Add(key, offset);
		}

		public void Add(string key, Guid value)
		{
			Remove(key);

			Add(key, value.ToString("D"));
		}

		public void Add(string key, bool value)
		{
			Remove(key);

			Add(key, value.ToString());
		}

		public void Add(string key, double value)
		{
			Remove(key);

			Add(key, value.ToString(CultureInfo.InvariantCulture));
		}

		public void Add(string key, float value)
		{
			Remove(key);

			Add(key, value.ToString(CultureInfo.InvariantCulture));
		}
	}
}
