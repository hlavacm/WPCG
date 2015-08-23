using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class TermView
	{
		public TermView()
		{
			InitializeComponent();
		}


		public new TermViewModel DataContext
		{
			get { return base.DataContext as TermViewModel; }
			set { base.DataContext = value; }
		}
	}
}
