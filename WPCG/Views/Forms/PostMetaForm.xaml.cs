using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class PostMetaForm
	{
		public static readonly DependencyProperty PostVisibilityProperty
			= DependencyProperty.Register("PostVisibility", typeof(Visibility), typeof(PostMetaForm), new PropertyMetadata(Visibility.Visible));
		public static readonly DependencyProperty PostsVisibilityProperty
			= DependencyProperty.Register("PostsVisibility", typeof(Visibility), typeof(PostMetaForm), new PropertyMetadata(Visibility.Collapsed));


		public PostMetaForm()
		{
			InitializeComponent();
		}


		public new PostMetaModel DataContext
		{
			get { return base.DataContext as PostMetaModel; }
			set { base.DataContext = value; }
		}
		public Visibility PostVisibility
		{
			get { return (Visibility)GetValue(PostVisibilityProperty); }
			set { SetValue(PostVisibilityProperty, value); }
		}
		public Visibility PostsVisibility
		{
			get { return (Visibility)GetValue(PostsVisibilityProperty); }
			set { SetValue(PostsVisibilityProperty, value); }
		}


		private void PostsListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (IsLoaded)
			{
				PostMetaModel dataContext = DataContext;
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
