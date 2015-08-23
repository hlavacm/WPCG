using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using Netcorex.Common;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Models;
using Netcorex.Properties;

namespace Netcorex.ViewModels
{
	public class DatabaseConnectionViewModel : ViewModelBase<DatabaseConnectionModel>, IDatabaseConnectionViewModel
	{
		private ICommand _checkCommand;
		private bool _isChecked;
		private string _checkedTitle = Titles.NotChecked;


		public DatabaseConnectionViewModel(IMainContext parentContext)
			: base(new DatabaseConnectionModel(), parentContext)
		{
		}


		IDatabaseConnectionModel IDatabaseConnectionViewModel.Model
		{
			get { return Model; }
		}
		public ICommand CheckCommand
		{
			get { return _checkCommand ?? (_checkCommand = new RelayCommand(CheckAction)); }
		}
		public bool IsChecked
		{
			get { return _isChecked; }
			private set
			{
				if (SetProperty(ref _isChecked, value))
				{
					CheckedTitle = (_isChecked) ? Titles.Checked : Titles.NotChecked;
				}
			}
		}
		public string CheckedTitle
		{
			get { return _checkedTitle; }
			private set { SetProperty(ref _checkedTitle, value); }
		}


		public async Task CheckAsync()
		{
			try
			{
				BusyContent = Messages.Initializing;
				IsBusy = true;
				Guid taskUid = ParentContext.RunTask();
				ParentContext.WorkingMaximum += 1;
				await Task.Run(() => { Check(); });
				ParentContext.WorkingValue += 1;
				ParentContext.CloseTask(taskUid);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public bool Check()
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(Model.GetConnectionString()))
				{
					connection.Open();
				}
				return IsChecked = true;
			}
			catch (MySqlException ex)
			{
				CheckedTitle = ex.Message;
				Settings.Default.MainTabControlSelectedIndex = 0;
				return IsChecked = false;
			}
		}

		public MySqlConnection GetConnection()
		{
			if (Check())
			{
				return new MySqlConnection(Model.GetConnectionString());
			}
			return null;
		}

		public bool TryGetConnection(out MySqlConnection connection)
		{
			connection = GetConnection();
			return connection != null;
		}

		public void UpdateSettings(bool save = false)
		{
			Settings settings = Settings.Default;
			DatabaseConnectionModel model = Model;
			settings.DatabaseName = Encryption.Encrypt(model.DatabaseName);
			settings.DatabaseUserName = Encryption.Encrypt(model.UserName);
			settings.DatabasePassword = Encryption.Encrypt(model.Password);
			settings.DatabaseServerName = Encryption.Encrypt(model.ServerName);
			settings.DatabaseTablePrefix = Encryption.Encrypt(model.TablePrefix);
			if (save)
			{
				settings.Save();
			}
		}


		private async void CheckAction(object parameter)
		{
			await CheckAsync();
		}
	}
}
