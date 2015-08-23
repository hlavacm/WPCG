using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public class TermModel : WpModelBase
	{
		public const string TableName = "terms";
		private string _name;
		private string _slug;
		private long _taxonomyId;
		private IDictionary<long, string> _taxonomies;
		private int _termGroup;


		public TermModel()
		{
		}

		public TermModel(long id, string name, string slug, int taxonomyId, int? termGroup = null)
			: base(id)
		{
			_name = name;
			_slug = slug;
			_taxonomyId = taxonomyId;
			if (termGroup.HasValue)
			{
				_termGroup = termGroup.Value;
			}
		}


		[Required]
		[MaxLength(200)]
		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}

		[Required]
		[MaxLength(200)]
		public string Slug
		{
			get { return _slug; }
			set { SetProperty(ref _slug, value); }
		}

		[Required]
		public long TaxonomyId
		{
			get { return _taxonomyId; }
			set { SetProperty(ref _taxonomyId, value); }
		}

		public IDictionary<long, string> Taxonomies
		{
			get { return _taxonomies; }
			set
			{
				SetProperty(ref _taxonomies, value);
			}
		}

		[Required]
		public int TermGroup
		{
			get { return _termGroup; }
			set { SetProperty(ref _termGroup, value); }
		}


		public override string GetInsertCommandText(IDatabaseConnectionModel databaseConnectionModel)
		{
			string table = databaseConnectionModel.GetTablePrefixed(TableName);
			return string.Format("INSERT INTO {0} (name, slug, term_group) VALUES (@name, @slug, @termGroup)", table);
		}

		public override IList<MySqlParameter> GetInsertCommandParameters(long? currentNumber = null)
		{
			string name = Name;
			string slug = Slug;
			if (currentNumber != null)
			{
				name = string.Format("{0} {1}", name, currentNumber);
				slug = string.Format("{0}-{1}", slug, currentNumber);
			}
			return new List<MySqlParameter>
							 {
								 new MySqlParameter("@name", name),
								 new MySqlParameter("@slug", slug),
								 new MySqlParameter("@termGroup", TermGroup)
							 };
		}
	}
}
