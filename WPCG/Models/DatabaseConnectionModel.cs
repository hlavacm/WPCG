using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Netcorex.Common;
using Netcorex.Interfaces;
using Netcorex.Properties;

namespace Netcorex.Models
{
	public class DatabaseConnectionModel : ModelBase, IDatabaseConnectionModel
	{
		public const string DefaultDatabaseName = "wordpress";
		public const string DefaultUserName = "root";
		public const string DefaultPassword = "";
		public const string DefaultServerName = "localhost";
		public const string DefaultTablePrefix = "wp_";
		private string _databaseName = Encryption.Decrypt(Settings.Default.DatabaseName) ?? DefaultDatabaseName;
		private string _userName = Encryption.Decrypt(Settings.Default.DatabaseUserName) ?? DefaultUserName;
		private string _password = Encryption.Decrypt(Settings.Default.DatabasePassword) ?? DefaultPassword;
		private string _serverName = Encryption.Decrypt(Settings.Default.DatabaseServerName) ?? DefaultServerName;
		private string _tablePrefix = Encryption.Decrypt(Settings.Default.DatabaseTablePrefix) ?? DefaultTablePrefix;


		[Required]
		[MaxLength(50)]
		public string DatabaseName
		{
			get { return _databaseName; }
			set { SetProperty(ref _databaseName, value); }
		}
		[Required]
		[MaxLength(50)]
		public string UserName
		{
			get { return _userName; }
			set { SetProperty(ref _userName, value); }
		}
		[MaxLength(50)]
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}
		[Required]
		[MaxLength(100)]
		public string ServerName
		{
			get { return _serverName; }
			set { SetProperty(ref _serverName, value); }
		}
		[MaxLength(10)]
		public string TablePrefix
		{
			get { return _tablePrefix; }
			set { SetProperty(ref _tablePrefix, value); }
		}


		public string GetConnectionString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(string.Format("DataSource={0};", ServerName));
			builder.Append(string.Format("Database={0};", DatabaseName));
			string userName = UserName;
			if (!string.IsNullOrEmpty(userName))
			{
				builder.Append(string.Format("UserId={0};", userName));
			}
			string password = Password;
			if (!string.IsNullOrEmpty(password))
			{
				builder.Append(string.Format("Password={0};", password));
			}
			return builder.ToString();
		}

		public string GetTablePrefixed(string table)
		{
			if (string.IsNullOrEmpty(table))
			{
				throw new ArgumentNullException("table");
			}
			string tablePrefix = TablePrefix;
			if (string.IsNullOrEmpty(tablePrefix) || table.StartsWith(tablePrefix))
			{
				return table;
			}
			return string.Format("{0}{1}", tablePrefix, table);
		}
	}
}
