using System.Windows;
using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class DatabaseConnectionForm
	{
		public DatabaseConnectionForm()
		{
			InitializeComponent();
		}


		public new DatabaseConnectionModel DataContext
		{
			get { return base.DataContext as DatabaseConnectionModel; }
			set { base.DataContext = value; }
		}


		private void PasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
		{
			if (IsLoaded)
			{
				DataContext.Password = u_PasswordBox.Password;
			}
		}
	}
}
