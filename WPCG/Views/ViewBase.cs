using System.Windows.Controls;
using Netcorex.Interfaces;

namespace Netcorex.Views
{
	public class ViewBase : ContentControl, IView
	{
		public new IViewModel DataContext
		{
			get { return base.DataContext as IViewModel; }
			set { base.DataContext = value; }
		}
	}
}
