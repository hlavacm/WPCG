using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Netcorex.Enums;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public abstract class WpModelBase : ModelBase, IWpModel
	{
		private long _id;


		protected WpModelBase()
		{
		}

		protected WpModelBase(long id)
			: this()
		{
			_id = id;
		}


		[Key]
		[Required]
		public long Id
		{
			get { return _id; }
			set { SetProperty(ref _id, value); }
		}


		public abstract string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel);

		public abstract IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null);

		public virtual string GetInsertPostProcessingCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			return null;
		}

		public virtual IList<MySqlParameter> GetInsertPostProcessingCommandParameters(long lastInsertedId)
		{
			return null;
		}


		protected string ToSqlDateTime(DateTime date)
		{
			return date.ToString("yyyy-MM-dd HH:mm:ss");
		}

		protected string ToSqlDate(DateTime date)
		{
			return date.ToString("yyyy-MM-dd");
		}

		protected string ToSqlPostStatus(PostStatuses postStatus)
		{
			switch (postStatus)
			{
				case PostStatuses.Published:
					return "publish";
				case PostStatuses.Future:
					return "future";
				case PostStatuses.Draft:
					return "draft";
				case PostStatuses.Pending:
					return "pending";
				case PostStatuses.Private:
					return "private";
				case PostStatuses.Trash:
					return "trash";
				case PostStatuses.AutoDraft:
					return "auto-draft";
				case PostStatuses.Inherit:
					return "inherit";
				default:
					throw new NotImplementedException(string.Format("Post Status: {0}", postStatus));
			}
		}
	}
}
