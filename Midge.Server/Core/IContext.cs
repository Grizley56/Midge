using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Core
{
	public interface IContext
	{
		IEnumerable<MidgeUser> Users { get; }
		MidgeUser CurrentUser { get; }

	}
}
