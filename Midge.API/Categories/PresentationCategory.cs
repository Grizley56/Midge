using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.Categories
{
	public class PresentationCategory: Category
	{
		public PresentationCategory(IMidgeInvoke midgeInvoke) : base(midgeInvoke)
		{

		}

		public Task NextSlide()
		{
			return MidgeInvoke?.SendAsync("presentation.nextSlide", MidgeParameters.Empty);
		}

		public Task PreviousSlide()
		{
			return MidgeInvoke?.SendAsync("presentation.previousSlide", MidgeParameters.Empty);
		}

		public Task Play()
		{
			return MidgeInvoke?.SendAsync("presentation.play", MidgeParameters.Empty);
		}
	}
}
