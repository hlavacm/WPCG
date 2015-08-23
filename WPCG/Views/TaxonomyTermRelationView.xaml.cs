using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class TaxonomyTermRelationView
	{
		public TaxonomyTermRelationView()
		{
			InitializeComponent();
		}


		public new TaxonomyTermRelationViewModel DataContext
		{
			get { return base.DataContext as TaxonomyTermRelationViewModel; }
			set { base.DataContext = value; }
		}
	}
}
