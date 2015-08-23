using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class TaxonomyTermRelationForm
	{
		public TaxonomyTermRelationForm()
		{
			InitializeComponent();
		}


		public new TaxonomyTermRelationModel DataContext
		{
			get { return base.DataContext as TaxonomyTermRelationModel; }
			set { base.DataContext = value; }
		}


		private void PostsListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (IsLoaded)
			{
				TaxonomyTermRelationModel dataContext = DataContext;
				if (dataContext == null)
				{
					return;
				}
				ObservableCollection<long> selectedPostIds = dataContext.SelectedPostIds;
				IList addedItems = e.AddedItems;
				if (addedItems != null && addedItems.Count > 0)
				{
					foreach (KeyValuePair<long, string> addedItem in e.AddedItems)
					{
						selectedPostIds.Add(addedItem.Key);
					}
				}
				IList removedItems = e.RemovedItems;
				if (removedItems != null && removedItems.Count > 0)
				{
					foreach (KeyValuePair<long, string> removedItem in e.RemovedItems)
					{
						selectedPostIds.Remove(removedItem.Key);
					}
				}
			}
		}
	}
}
