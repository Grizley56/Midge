using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Midge.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TerminalPage : ContentPage
	{
		public TerminalPage ()
		{
			InitializeComponent ();
		}
	}
}