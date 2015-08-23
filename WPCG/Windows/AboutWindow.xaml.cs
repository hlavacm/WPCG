using Netcorex.Models;

namespace Netcorex.Windows
{
	public partial class AboutWindow
	{
		public AboutWindow()
			: base(400, 300, 400, 300)
		{
			InitializeComponent();
			DataContext = new AboutModel();
		}


		public new AboutModel DataContext
		{
			get { return base.DataContext as AboutModel; }
			set { base.DataContext = value; }
		}


		protected override void SaveSettings()
		{
			// TODO
		}
	}
}
