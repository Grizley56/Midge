using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Midge.Server.Database
{
	public class MidgeDbContext
	{
		public SQLiteConnection Connection { get; private set; }

		public MidgeDbContext(string databasePath)
		{
			var db = new SQLiteConnection(databasePath);

			db.CreateTable<User>();
			db.CreateTable<UserAccess>();
			db.CreateTable<AccessToken>();
			db.CreateTable<StorageAccess>();
			db.CreateTable<SoundAccess>();
			db.CreateTable<EmulatorsAccess>();
			db.CreateTable<OsAccess>();
			db.CreateTable<ProcessAccess>();
			db.CreateTable<Log>();

			Connection = db;
		}
	}
}
