using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public class TaxonomyViewModel : WpViewModelBase<TaxonomyModel>
	{
		private readonly ObservableCollection<TermModel> _terms = new ObservableCollection<TermModel>();


		public TaxonomyViewModel(IMainContext parentContext)
			: base(new TaxonomyModel(), parentContext)
		{
			Terms.CollectionChanged += TermsOnCollectionChanged;
		}


		public ObservableCollection<TermModel> Terms
		{
			get { return _terms; }
		}


		public override bool Initialize()
		{
			if (IsInitialized)
			{
				return false;
			}
			Model.Terms = GetAvailableTerms();
			Model.Taxonomies = GetAvailableTaxonomies(true);
			IDictionary<long, string> empty = new Dictionary<long, string> { { 0, Titles.Empty } };
			IDictionary<long, string> availableTaxonomies = GetAvailableTaxonomies() ?? new Dictionary<long, string>();
			Model.Parents = empty.Concat(availableTaxonomies).ToDictionary(x => x.Key, x => x.Value);
			return IsInitialized = (Model.Terms != null && Model.Taxonomies != null);
		}


		private void TermsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Model.Count = Terms.Count;
		}
	}
}
