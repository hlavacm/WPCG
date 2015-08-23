using Netcorex.Interfaces;
using Netcorex.ViewModels;

namespace Netcorex.Windows
{
	public partial class OptionsWindow
	{
		public OptionsWindow(IMainContext parentContext)
			: base(400, 300, 400, 300)
		{
			InitializeComponent();
			DataContext = new OptionsViewModel(parentContext);
		}


		public new OptionsViewModel DataContext
		{
			get { return base.DataContext as OptionsViewModel; }
			set { base.DataContext = value; }
		}


		protected override void SaveSettings()
		{
			DataContext.UpdateSettings(true);
		}
	}
}
