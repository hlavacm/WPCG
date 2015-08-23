using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public class PostMetaModel : WpModelBase
	{
		public const string TableName = "postmeta";
		private long _postId;
		private IDictionary<long, string> _posts;
		private readonly ObservableCollection<long> _selectedPostIds = new ObservableCollection<long>();
		private string _metaKey;
		private string _metaValue;


		public PostMetaModel()
		{
		}

		public PostMetaModel(long id, long postId, string metaKey, string metaValue)
			: base(id)
		{
			_postId = postId;
			_metaKey = metaKey;
			_metaValue = metaValue;
		}


		[Required]
		public long PostId
		{
			get { return _postId; }
			set { SetProperty(ref _postId, value); }
		}
		public IDictionary<long, string> Posts
		{
			get { return _posts; }
			set { SetProperty(ref _posts, value); }
		}
		public ObservableCollection<long> SelectedPostIds
		{
			get { return _selectedPostIds; }
		}
		[Required]
		[MaxLength(255)]
		public string MetaKey
		{
			get { return _metaKey; }
			set { SetProperty(ref _metaKey, value); }
		}
		[Required]
		[MaxLength(1000)]
		public string MetaValue
		{
			get { return _metaValue; }
			set { SetProperty(ref _metaValue, value); }
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("INSERT INTO {0} (post_id, meta_key, meta_value) VALUES (@postId, @metaKey, @metaValue)", table);
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			string metaKey = MetaKey;
			string metaValue = MetaValue;
			if (currentNumber != null)
			{
				metaKey = string.Format("{0} {1}", metaKey, currentNumber);
				metaValue = string.Format("{0} {1}", metaValue, currentNumber);
			}
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@postId", PostId),
								 new MySqlParameter("@metaKey", metaKey),
								 new MySqlParameter("@metaValue", metaValue)
							 };
		}
	}
}
