using Netcorex.Common;

namespace Netcorex.Views.Controls
{
	public partial class RangeControl
	{
		public RangeControl()
		{
			InitializeComponent();
		}


		public new IRangable DataContext
		{
			get { return base.DataContext as IRangable; }
			set { base.DataContext = value; }
		}
	}
}
