using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public class TaxonomyModel : WpModelBase
	{
		public const string TableName = "term_taxonomy";
		private long _termId;
		private IDictionary<long, string> _terms;
		private string _taxonomy;
		private IDictionary<long, string> _taxonomies;
		private string _description;
		private long _parent;
		private IDictionary<long, string> _parents;
		private int _count;


		public TaxonomyModel()
		{
		}

		public TaxonomyModel(long id, string taxonomy, string description, long parent = 0, int count = 0)
			: base(id)
		{
			_taxonomy = taxonomy;
			_description = description;
			_parent = parent;
			_count = count;
		}


		[Required]
		public long TermId
		{
			get { return _termId; }
			set { SetProperty(ref _termId, value); }
		}
		public IDictionary<long, string> Terms
		{
			get { return _terms; }
			set { SetProperty(ref _terms, value); }
		}
		[Required]
		[MaxLength(32)]
		public string Taxonomy
		{
			get { return _taxonomy; }
			set { SetProperty(ref _taxonomy, value); }
		}
		public IDictionary<long, string> Taxonomies
		{
			get { return _taxonomies; }
			set { SetProperty(ref _taxonomies, value); }
		}
		[Required]
		[MaxLength(10000)]
		public string Description
		{
			get { return _description; }
			set { SetProperty(ref _description, value); }
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
		public int Count
		{
			get { return _count; }
			set { SetProperty(ref _count, value); }
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("INSERT INTO {0} (term_id, taxonomy, description, parent) VALUES (@termId, @taxonomy, @description, @parent)", table);
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@termId", TermId),
								 new MySqlParameter("@taxonomy", Taxonomy),
								 new MySqlParameter("@description", Description ?? string.Empty),
								 new MySqlParameter("@parent", Parent)
							 };
		}
	}
}
