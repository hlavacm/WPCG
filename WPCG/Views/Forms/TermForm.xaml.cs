using System.Windows;
using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class TermForm
	{
		public static readonly DependencyProperty TaxonomyVisibilityProperty
			= DependencyProperty.Register("TaxonomyVisibility", typeof(Visibility), typeof(TermForm), new PropertyMetadata(Visibility.Visible));


		public TermForm()
		{
			InitializeComponent();
		}


		public new TermModel DataContext
		{
			get { return base.DataContext as TermModel; }
			set { base.DataContext = value; }
		}
		public Visibility TaxonomyVisibility
		{
			get { return (Visibility)GetValue(TaxonomyVisibilityProperty); }
			set { SetValue(TaxonomyVisibilityProperty, value); }
		}
	}
}
