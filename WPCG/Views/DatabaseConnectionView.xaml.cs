using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class DatabaseConnectionView
	{
		public DatabaseConnectionView()
		{
			InitializeComponent();
		}


		public new DatabaseConnectionViewModel DataContext
		{
			get { return base.DataContext as DatabaseConnectionViewModel; }
			set { base.DataContext = value; }
		}
	}
}
