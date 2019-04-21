using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Client.Mobile.View
{
	public class MainPageMenuItem
	{
		public MainPageMenuItem()
		{
			//TargetType = typeof(SoundPage);
		}

		public int Id { get; set; }
		public string Title { get; set; }

		public Type TargetType { get; set; }
		public string Image { get; set; }
	}
}