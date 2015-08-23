using System.Windows;
using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class TaxonomyForm
	{
		public static readonly DependencyProperty TermVisibilityProperty 
			= DependencyProperty.Register("TermVisibility", typeof (Visibility), typeof (TaxonomyForm), new PropertyMetadata(Visibility.Visible));


		public TaxonomyForm()
		{
			InitializeComponent();
		}


		public new TaxonomyModel DataContext
		{
			get { return base.DataContext as TaxonomyModel; }
			set { base.DataContext = value; }
		}
		public Visibility TermVisibility
		{
			get { return (Visibility)GetValue(TermVisibilityProperty); }
			set { SetValue(TermVisibilityProperty, value); }
		}
	}
}
