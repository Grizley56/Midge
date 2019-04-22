using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Models;

namespace Midge.API.Categories
{
	public class FileManagerCategory: Category
	{
		public FileManagerCategory(IMidgeInvoke midgeInvoke) : base(midgeInvoke)
		{

		}

		public DirectoryModel GetDirectory(string path)
		{
			return MidgeInvoke?.SendAndWait<DirectoryModel>("fileManager.getDirectory", new MidgeParameters
			{
				{ "path", path }
			});
		}

		public Task<DirectoryModel> GetDirectoryAsync(string path)
		{
			return MidgeInvoke?.SendAndWaitAsync<DirectoryModel>("fileManager.getDirectory", new MidgeParameters
			{
				{ "path", path }
			});
		}

		public LogicalDriveModel[] GetLogicalDrives()
		{
			return MidgeInvoke?.SendAndWait<LogicalDriveModel[]>("fileManager.getLogicalDrives", MidgeParameters.Empty);
		}

		public Task<LogicalDriveModel[]> GetLogicalDrivesAsync()
		{
			return MidgeInvoke?.SendAndWaitAsync<LogicalDriveModel[]>("fileManager.getLogicalDrives", MidgeParameters.Empty);
		}


		public Task Remove(string path)
		{
			return MidgeInvoke.SendAsync("fileManager.remove", new MidgeParameters
			{
				{ "path", path }
			});
		}

	}
}
