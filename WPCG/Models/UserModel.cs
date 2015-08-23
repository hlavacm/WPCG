using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	[Obsolete("Ve vývoji...")]
	public class UserModel : WpModelBase
	{
		public const string TableName = "users";


		public UserModel()
		{
		}

		public UserModel(int id)
			: base(id)
		{
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			throw new NotImplementedException();
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			throw new NotImplementedException();
		}
	}
}
