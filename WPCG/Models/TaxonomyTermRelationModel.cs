using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public class TaxonomyTermRelationModel : WpModelBase
	{
		public const string TableName = "term_relationships";
		private long _objectId;
		private IDictionary<long, string> _posts;
		private readonly ObservableCollection<long> _selectedPostIds = new ObservableCollection<long>();
		private long _termTaxonomyId;
		private int _termOrder;


		public TaxonomyTermRelationModel()
		{
		}

		public TaxonomyTermRelationModel(int id, int termTaxonomyId, int termOrder = 0)
			: base(id)
		{
			_termTaxonomyId = termTaxonomyId;
			_termOrder = termOrder;
		}


		[Required]
		public long ObjectId
		{
			get { return _objectId; }
			set { SetProperty(ref _objectId, value); }
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
		public long TermTaxonomyId
		{
			get { return _termTaxonomyId; }
			set { SetProperty(ref _termTaxonomyId, value); }
		}
		[Required]
		public int TermOrder
		{
			get { return _termOrder; }
			set { SetProperty(ref _termOrder, value); }
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("INSERT INTO {0} (object_id, term_taxonomy_id, term_order) VALUES (@objectId, @termTaxonomyId, @termOrder)", table);
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@objectId", ObjectId),
								 new MySqlParameter("@termTaxonomyId", TermTaxonomyId),
								 new MySqlParameter("@termOrder", TermOrder)
							 };
		}
	}
}
