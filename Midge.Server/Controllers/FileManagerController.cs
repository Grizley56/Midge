using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	[MidgeController("fileManager")]
	class FileManagerController: ControllerBase
	{
		public FileManagerController(MidgeContext context, IServiceManager serviceManager) : base(context, serviceManager)
		{

		}


		[MidgeCommand("getLogicalDrives")]
		public void GetLogicalDrives()
		{
			var allDrives = DriveInfo.GetDrives().Where(i => i.DriveType == DriveType.Fixed);
			
			Response = new JArray(from drive in allDrives
				select
					new JObject(
						new JProperty("name", drive.Name),
						new JProperty("path", drive.Name)));
		}


		[MidgeCommand("getDirectory")]
		public void GetDirectory([MidgeParameter("path", true)] string path)
		{
			DirectoryInfo info = new DirectoryInfo(path);
			Response = new JObject(
				new JProperty("directories",
					new JArray(from dir in info.GetDirectories() where !dir.Attributes.HasFlag(FileAttributes.Hidden) select dir.FullName)),
				new JProperty("files",
					new JArray(from file in info.EnumerateFiles()
						select new JObject(
							new JProperty("name", file.FullName),
							new JProperty("size", file.Length)))));
		}

		[MidgeCommand("remove")]
		public void Remove([MidgeParameter("path")] string path)
		{
			File.Delete(path);
		}

		[MidgeCommand("downloadFile")]
		public void DownloadFile([MidgeParameter("path")] string path)
		{

		}

	}
}
