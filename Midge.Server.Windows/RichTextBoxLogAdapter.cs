using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Midge.Server.Windows
{
	public class RichTextBoxLogAdapter: ILogger
	{
		private RichTextBox _rtb;

		public RichTextBoxLogAdapter(RichTextBox rtb)
		{
			_rtb = rtb;
		}

		public void Log(string text)
		{
			_rtb.Document.Blocks.Add(new Paragraph(new Run($"{DateTime.Now}: {text}")));
		}

		public void Log(Exception ex)
		{
			_rtb.Document.Blocks.Add(new Paragraph(new Run($"{DateTime.Now}: {ex.Message}")));
		}
	}
}
