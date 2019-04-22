using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Midge.API.Models
{
	[Serializable]
	public class DirectoryModel
	{
		[JsonProperty("directories")]
		public string[] Directories { get; set; }
		[JsonProperty("files")]
		public FileModel[] Files { get; set; }
	}

	[Serializable]
	public class FileModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("size")]
		public long Size { get; set; }
	}
}
