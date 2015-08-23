using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MySql.Data.MySqlClient;
using Netcorex.Common;
using Netcorex.Enums;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Properties;

namespace Netcorex.Models
{
	public class PostModel : WpModelBase
	{
		public const string TableName = "posts";
		private string _title;
		private string _name;
		private string _excerpt;
		private string _content;
		private int _menuOrder;
		private long _author;
		private IDictionary<long, string> _authors;
		private string _type = "post";
		private IList<string> _types;
		private long _parent;
		private IDictionary<long, string> _parents;
		private string _guid;
		private PostStatuses _postStatus = Enums.PostStatuses.Published;
		private readonly IDictionary<PostStatuses, string> _postStatuses = new Dictionary<PostStatuses, string>
																																				{
																																					{ Enums.PostStatuses.Published, Statuses.Published },
																																					{ Enums.PostStatuses.Future, Statuses.Future },
																																					{ Enums.PostStatuses.Draft, Statuses.Draft },
																																					{ Enums.PostStatuses.Pending, Statuses.Pending },
																																					{ Enums.PostStatuses.Private, Statuses.Private },
																																					{ Enums.PostStatuses.Trash, Statuses.Trash },
																																					{ Enums.PostStatuses.AutoDraft, Statuses.AutoDraft },
																																					{ Enums.PostStatuses.Inherit, Statuses.Inherit },
																																				};
		private PostPingStatuses _pingStatus = PostPingStatuses.Open;
		private readonly IDictionary<PostPingStatuses, string> _pingStatuses = new Dictionary<PostPingStatuses, string>
																																						{
																																							{ PostPingStatuses.Open, Statuses.Open },
																																							{ PostPingStatuses.Closed, Statuses.Closed }
																																						};
		private PostCommentStatuses _commentStatus = PostCommentStatuses.Open;
		private readonly IDictionary<PostCommentStatuses, string> _commentStatuses = new Dictionary<PostCommentStatuses, string>
																																									{
																																										{ PostCommentStatuses.Open, Statuses.Open },
																																										{ PostCommentStatuses.Closed, Statuses.Closed }
																																									};
		private DateTime _date = DateTime.Now;
		private DateTime _dateGmt = DateTime.UtcNow;
		private DateTime _modified = DateTime.Now;
		private DateTime _modifiedGmt = DateTime.UtcNow;
		private string _contentFiltered;
		private string _toPing;
		private string _pinged;
		private string _password;
		private string _mimeType;
		private int _commentCount;


		public PostModel()
		{
		}

		public PostModel(long id)
			: base(id)
		{
		}


		[Required]
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}
		[Required]
		[MaxLength(200)]
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}
		[Required]
		[MaxLength(10000)]
		public string Excerpt
		{
			get { return _excerpt; }
			set { SetProperty(ref _excerpt, value); }
		}
		[Required]
		[MaxLength(100000)]
		public string Content
		{
			get { return _content; }
			set { SetProperty(ref _content, value); }
		}
		[Required]
		public int MenuOrder
		{
			get { return _menuOrder; }
			set { SetProperty(ref _menuOrder, value); }
		}
		[Required]
		public long Author
		{
			get { return _author; }
			set { SetProperty(ref _author, value); }
		}
		public IDictionary<long, string> Authors
		{
			get { return _authors; }
			set
			{
				if (SetProperty(ref _authors, value))
				{
					if (_authors != null)
					{
						Author = _authors.FirstOrDefault().Key;
					}
				}
			}
		}
		[Required]
		[MaxLength(20)]
		public string Type
		{
			get { return _type; }
			set { SetProperty(ref _type, value); }
		}
		public IList<string> Types
		{
			get { return _types; }
			set { SetProperty(ref _types, value); }
		}
		[Required]
		public long Parent
		{
			get { return _parent; }
			set { SetProperty(ref _parent, value); }
		}
		public IDictionary<long, string> Parents
		{
			get { return _parents; }
			set { SetProperty(ref _parents, value); }
		}
		[Required]
		[MaxLength(255)]
		public string Guid
		{
			get { return _guid; }
			set { SetProperty(ref _guid, value); }
		}
		[Required]
		[MaxLength(20)]
		public PostStatuses PostStatus
		{
			get { return _postStatus; }
			set { SetProperty(ref _postStatus, value); }
		}
		public IDictionary<PostStatuses, string> PostStatuses
		{
			get { return _postStatuses; }
		}
		[Required]
		[MaxLength(20)]
		public PostPingStatuses PingStatus
		{
			get { return _pingStatus; }
			set { SetProperty(ref _pingStatus, value); }
		}
		public IDictionary<PostPingStatuses, string> PingStatuses
		{
			get { return _pingStatuses; }
		}
		[Required]
		[MaxLength(20)]
		public PostCommentStatuses CommentStatus
		{
			get { return _commentStatus; }
			set { SetProperty(ref _commentStatus, value); }
		}
		public IDictionary<PostCommentStatuses, string> CommentStatuses
		{
			get { return _commentStatuses; }
		}
		[Required]
		public DateTime Date
		{
			get { return _date; }
			set { SetProperty(ref _date, value); }
		}
		[Required]
		public DateTime DateGmt
		{
			get { return _dateGmt; }
			set { SetProperty(ref _dateGmt, value); }
		}
		[Required]
		public DateTime Modified
		{
			get { return _modified; }
			set { SetProperty(ref _modified, value); }
		}
		[Required]
		public DateTime ModifiedGmt
		{
			get { return _modifiedGmt; }
			set { SetProperty(ref _modifiedGmt, value); }
		}
		[Required]
		public string ContentFiltered
		{
			get { return _contentFiltered; }
			set { SetProperty(ref _contentFiltered, value); }
		}
		[Required]
		public string ToPing
		{
			get { return _toPing; }
			set { SetProperty(ref _toPing, value); }
		}
		[Required]
		public string Pinged
		{
			get { return _pinged; }
			set { SetProperty(ref _pinged, value); }
		}
		[Required]
		[MaxLength(20)]
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}
		[Required]
		[MaxLength(100)]
		public string MimeType
		{
			get { return _mimeType; }
			set { SetProperty(ref _mimeType, value); }
		}
		[Required]
		public int CommentCount
		{
			get { return _commentCount; }
			set { SetProperty(ref _commentCount, value); }
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("INSERT INTO {0} (post_title, post_name, post_excerpt, post_content, menu_order, post_type, post_author, post_parent, guid, post_status, ping_status, comment_status, post_date, post_date_gmt, post_modified, post_modified_gmt, post_content_filtered, to_ping, pinged, post_password, post_mime_type) VALUES (@title, @name, @excerpt, @content, @menuOrder, @type, @author, @parent, @guid, @postStatus, @pingStatus, @commentStatus, @date, @dateGmt, @modified, @modifiedGmt, @contentFiltered, @toPing, @pinged, @password, @mimeType)", table);
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			string title = Title;
			string name = Name;
			if (currentNumber != null)
			{
				title = string.Format("{0} {1}", title, currentNumber);
				name = string.Format("{0}-{1}", name, currentNumber);
			}
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@title", title ?? string.Empty),
								 new MySqlParameter("@name", name ?? string.Empty),
								 new MySqlParameter("@excerpt", Excerpt ?? string.Empty),
								 new MySqlParameter("@content", Content ?? string.Empty),
								 new MySqlParameter("@menuOrder", MenuOrder),
								 new MySqlParameter("@type", Type ?? string.Empty),
								 new MySqlParameter("@author", Author),
								 new MySqlParameter("@parent", Parent),
								 new MySqlParameter("@guid", Guid ?? string.Empty),
								 new MySqlParameter("@postStatus", ToSqlPostStatus(PostStatus)),
								 new MySqlParameter("@pingStatus", PingStatus.ToString().ToLower()),
								 new MySqlParameter("@commentStatus", CommentStatus.ToString().ToLower()),
								 new MySqlParameter("@date", ToSqlDateTime(Date)),
								 new MySqlParameter("@dateGmt", ToSqlDateTime(DateGmt)),
								 new MySqlParameter("@modified", ToSqlDateTime(Modified)),
								 new MySqlParameter("@modifiedGmt", ToSqlDateTime(ModifiedGmt)),
								 new MySqlParameter("@contentFiltered", ContentFiltered ?? string.Empty),
								 new MySqlParameter("@toPing", ToPing ?? string.Empty),
								 new MySqlParameter("@pinged", Pinged ?? string.Empty),
								 new MySqlParameter("@password", Password ?? string.Empty),
								 new MySqlParameter("@mimeType", MimeType ?? string.Empty)
							 };
		}

		public override string GetInsertPostProcessingCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("UPDATE {0} SET guid = @guid WHERE ID = @id", table);
		}

		public override IList<MySqlParameter> GetInsertPostProcessingCommandParameters(long lastInsertedId)
		{
			string guid = string.Format("{0}{1}", Guid, lastInsertedId);
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@guid", guid),
								 new MySqlParameter("@id", lastInsertedId),
							 };
		}


		protected override void Initialize()
		{
			string serverBaseUrl = Settings.Default.ServerBaseUrlOption;
			if (!string.IsNullOrEmpty(serverBaseUrl))
			{
				if (Settings.Default.IsLocalhostServerOption)
				{
					_guid = string.Format("{0}/{1}/?p=", serverBaseUrl.TrimEnd('/'), Encryption.Decrypt(Settings.Default.DatabaseName));
				}
				else
				{
					_guid = string.Format("{0}/?p=", serverBaseUrl.TrimEnd('/'));
				}
			}
		}
	}
}
