using Netcorex.Models;

namespace Netcorex.Windows
{
	public partial class HelpWindow
	{
		public HelpWindow()
			: base(400, 300, 400, 300)
		{
			InitializeComponent();
			DataContext = new HelpModel();
		}


		public new HelpModel DataContext
		{
			get { return base.DataContext as HelpModel; }
			set { base.DataContext = value; }
		}


		protected override void SaveSettings()
		{
			// TODO
		}
	}
}
