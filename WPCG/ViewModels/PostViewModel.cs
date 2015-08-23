using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public class PostViewModel : WpViewModelBase<PostModel>
	{
		private readonly PostMetaModel _postMeta = new PostMetaModel();
		private readonly ObservableCollection<TaxonomyTermRelationModel> _taxonomyTermRelations = new ObservableCollection<TaxonomyTermRelationModel>();
		private List<TaxonomyModel> _allTaxonomies;


		public PostViewModel(IMainContext parentContext)
			: base(new PostModel(), parentContext)
		{
		}


		public PostMetaModel PostMeta
		{
			get { return _postMeta; }
		}
		public ObservableCollection<TaxonomyTermRelationModel> TaxonomyTermRelations
		{
			get { return _taxonomyTermRelations; }
		}
		public List<TaxonomyModel> AllTaxonomies
		{
			get { return _allTaxonomies; }
			set { SetProperty(ref _allTaxonomies, value); }
		}


		public override bool Initialize()
		{
			if (IsInitialized)
			{
				return false;
			}
			Model.Authors = GetAvailableAuthors();
			Model.Types = GetAvailablePostTypes();
			IDictionary<long, string> empty = new Dictionary<long, string> { { 0, Titles.Empty } };
			IDictionary<long, string> availablePosts = GetAvailablePosts() ?? new Dictionary<long, string>();
			Model.Parents = empty.Concat(availablePosts).ToDictionary(x => x.Key, x => x.Value);
			return IsInitialized = (Model.Authors != null);
		}
	}
}
