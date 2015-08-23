using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public class TaxonomyTermRelationViewModel : WpViewModelBase<TaxonomyTermRelationModel>
	{
		private readonly TermModel _termModel;
		private readonly TaxonomyModel _taxonomyModel;

		public TaxonomyTermRelationViewModel(IMainContext parentContext)
			: base(new TaxonomyTermRelationModel(), parentContext)
		{
			_termModel = new TermModel();
			_taxonomyModel = new TaxonomyModel();
		}


		public TermModel TermModel
		{
			get { return _termModel; }
		}
		public TaxonomyModel TaxonomyModel
		{
			get { return _taxonomyModel; }
		}


		public override bool Initialize()
		{
			if (IsInitialized)
			{
				return false;
			}

			// term
			//TermModel.Taxonomies = GetAvailableTaxonomies();

			// taxonomy
			TaxonomyModel.Taxonomies = GetAvailableTaxonomies(true, false);
			IDictionary<long, string> empty = new Dictionary<long, string> { { 0, Titles.Empty } };
			IDictionary<long, string> availableTaxonomies = GetAvailableTermTaxonomies() ?? new Dictionary<long, string>();
			TaxonomyModel.Parents = empty.Concat(availableTaxonomies).ToDictionary(x => x.Key, x => x.Value);

			// relation
			Model.Posts = GetAvailablePosts();

			return IsInitialized = (TaxonomyModel.Taxonomies != null) && (TaxonomyModel.Parents != null);
		}


		protected override void GenerateStep(long itemId, MySqlConnection dbConnection, IDatabaseConnectionModel connectionModel)
		{
			// term
			string insertTermCommandText = TermModel.GetInsertCommandText(connectionModel);
			using (MySqlCommand termCommand = new MySqlCommand(insertTermCommandText, dbConnection))
			{
				foreach (MySqlParameter parameter in TermModel.GetInsertCommandParameters(itemId))
				{
					termCommand.Parameters.Add(parameter);
				}
				termCommand.ExecuteNonQuery();
				TaxonomyModel.TermId = termCommand.LastInsertedId;
			}
			// taxonomy
			string insertTaxonomyCommandText = TaxonomyModel.GetInsertCommandText(connectionModel);
			using (MySqlCommand taxonomyCommand = new MySqlCommand(insertTaxonomyCommandText, dbConnection))
			{
				foreach (MySqlParameter parameter in TaxonomyModel.GetInsertCommandParameters())
				{
					taxonomyCommand.Parameters.Add(parameter);
				}
				taxonomyCommand.ExecuteNonQuery();
				Model.TermTaxonomyId = taxonomyCommand.LastInsertedId;
			}
			// relation
			ICollection<long> selectedPostIds = Model.SelectedPostIds;
			if (selectedPostIds.Count > 0)
			{
				foreach (int selectedPostId in selectedPostIds)
				{
					Model.ObjectId = selectedPostId;
					string insertRelationCommandText = Model.GetInsertCommandText(connectionModel);
					using (MySqlCommand relationCommand = new MySqlCommand(insertRelationCommandText, dbConnection))
					{
						foreach (MySqlParameter parameter in Model.GetInsertCommandParameters())
						{
							relationCommand.Parameters.Add(parameter);
						}
						relationCommand.ExecuteNonQuery();
					}
				}
			}
		}
	}
}
