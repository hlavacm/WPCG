using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class TaxonomyView
	{
		public TaxonomyView()
		{
			InitializeComponent();
		}


		public new TaxonomyViewModel DataContext
		{
			get { return base.DataContext as TaxonomyViewModel; }
			set { base.DataContext = value; }
		}
	}
}
