using System.Windows;
using System.Windows.Threading;
using Netcorex.Localizations;

namespace Netcorex
{
	public partial class App
	{
		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message, Titles.Error, MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
		}
	}
}
