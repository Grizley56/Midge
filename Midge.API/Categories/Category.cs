using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.Categories
{
	public class Category
	{
		protected IMidgeInvoke MidgeInvoke;
		public int Timeout { get; set; } = 5000000;
		public Category(IMidgeInvoke midgeInvoke)
		{
			MidgeInvoke = midgeInvoke;
		}
	}
}
