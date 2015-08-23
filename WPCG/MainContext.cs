using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using Netcorex.Common;
using Netcorex.Enums;
using Netcorex.Interfaces;
using Netcorex.Localizations;
using Netcorex.Properties;
using Netcorex.ViewModels;
using Netcorex.Windows;

namespace Netcorex
{
	public class MainContext : BindableBase, IMainContext
	{
		private readonly IDatabaseConnectionViewModel _databaseConnectionViewModel;
		private readonly PostViewModel _postViewModel;
		private readonly PostMetaViewModel _postMetaViewModel;
		private readonly TaxonomyTermRelationViewModel _taxonomyTermRelationViewModel;
		private IViewModel _selectedViewModel;
		private readonly ObservableCollection<Guid> _activeTasks;
		private ApplicationStates _state = ApplicationStates.Ready;
		private int _workingValue;
		private int _workingMinimum;
		private int _workingMaximum;
		private string _statusBarMenuTitle = Titles.HideStatusBar;
		private RelayCommand _exitCommand;
		private RelayCommand _showStatusBarCommand;
		private RelayCommand _generateCommand;
		private RelayCommand _optionsCommand;
		private RelayCommand _helpCommand;
		private RelayCommand _aboutCommand;


		public MainContext()
		{
			_activeTasks = new ObservableCollection<Guid>();
			_activeTasks.CollectionChanged += ActiveTasksOnCollectionChanged;
			_databaseConnectionViewModel = new DatabaseConnectionViewModel(this);
			_postViewModel = new PostViewModel(this);
			_postMetaViewModel = new PostMetaViewModel(this);
			_taxonomyTermRelationViewModel = new TaxonomyTermRelationViewModel(this);
		}


		public IDatabaseConnectionViewModel DatabaseConnectionViewModel
		{
			get { return _databaseConnectionViewModel; }
		}
		public PostViewModel PostViewModel
		{
			get { return _postViewModel; }
		}
		public PostMetaViewModel PostMetaViewModel
		{
			get { return _postMetaViewModel; }
		}
		public TaxonomyTermRelationViewModel TaxonomyTermRelationViewModel
		{
			get { return _taxonomyTermRelationViewModel; }
		}
		public IViewModel SelectedViewModel
		{
			get { return _selectedViewModel; }
			set { SetProperty(ref _selectedViewModel, value); }
		}
		public ApplicationStates State
		{
			get { return _state; }
			private set
			{
				if (SetProperty(ref _state, value))
				{
					OnPropertyChanged("StateTitle");
					if (_state == ApplicationStates.Working)
					{
						WorkingMinimum = WorkingValue = WorkingMaximum = 0;
					}
				}
			}
		}
		public string StateTitle
		{
			get
			{
				switch (State)
				{
					case ApplicationStates.Ready:
						return Titles.Ready;
					case ApplicationStates.Working:
						return Titles.Working;
					default:
						throw new NotImplementedException(string.Format("Application State: {0}", State));
				}
			}
		}
		public int WorkingValue
		{
			get { return _workingValue; }
			set { SetProperty(ref _workingValue, value); }
		}
		public int WorkingMinimum
		{
			get { return _workingMinimum; }
			set { SetProperty(ref _workingMinimum, value); }
		}
		public int WorkingMaximum
		{
			get { return _workingMaximum; }
			set { SetProperty(ref _workingMaximum, value); }
		}
		public string StatusBarMenuTitle
		{
			get { return _statusBarMenuTitle; }
			private set { SetProperty(ref _statusBarMenuTitle, value); }
		}
		public RelayCommand ExitCommand
		{
			get { return _exitCommand ?? (_exitCommand = new RelayCommand(ExitAction)); }
		}
		public RelayCommand ShowStatusBarCommand
		{
			get { return _showStatusBarCommand ?? (_showStatusBarCommand = new RelayCommand(ShowStatusBarAction)); }
		}
		public RelayCommand GenerateCommand
		{
			get { return _generateCommand ?? (_generateCommand = new RelayCommand(GenerateAction, GenerationCheckAction)); }
		}
		public RelayCommand OptionsCommand
		{
			get { return _optionsCommand ?? (_optionsCommand = new RelayCommand(OptionsAction)); }
		}
		public RelayCommand HelpCommand
		{
			get { return _helpCommand ?? (_helpCommand = new RelayCommand(HelpAction)); }
		}
		public RelayCommand AboutCommand
		{
			get { return _aboutCommand ?? (_aboutCommand = new RelayCommand(AboutAction)); }
		}

		public void UpdateSettings(bool save = true)
		{
			DatabaseConnectionViewModel.UpdateSettings();
			if (save)
			{
				Settings.Default.Save();
			}
		}

		/// <summary>
		/// Zavede hint na nový task (dlouho trvající operaci)
		/// </summary>
		/// <returns>UID (nového) tasku</returns>
		public Guid RunTask()
		{
			Guid taskUid = Guid.NewGuid();
			_activeTasks.Add(taskUid);
			return taskUid;
		}

		/// <summary>
		/// Zruší (existující) task (dlouho trvající operaci)
		/// </summary>
		/// <param name="taskUid">UID (existujícího) tasku</param>
		/// <returns>Ověření provedení operace</returns>
		public bool CloseTask(Guid taskUid)
		{
			return _activeTasks.Remove(taskUid);
		}


		private void ActiveTasksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					if (State != ApplicationStates.Working)
					{
						State = ApplicationStates.Working;
					}
					break;
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Reset:
					if (_activeTasks.Count == 0)
					{
						State = ApplicationStates.Ready;
					}
					break;
			}
		}

		private void ExitAction(object parameter)
		{
			foreach (Window window in Application.Current.Windows)
			{
				window.Close();
			}
		}

		private void ShowStatusBarAction(object parameter)
		{
			Visibility statusBarVisibility = Settings.Default.MainWindowStatusBarVisibility;
			if (statusBarVisibility == Visibility.Visible)
			{
				Settings.Default.MainWindowStatusBarVisibility = Visibility.Collapsed;
				StatusBarMenuTitle = Titles.ShowStatusBar;
			}
			else
			{
				Settings.Default.MainWindowStatusBarVisibility = Visibility.Visible;
				StatusBarMenuTitle = Titles.HideStatusBar;
			}
		}

		private void GenerateAction(object parameter)
		{
			IWpViewModel selectedWpViewModel = SelectedViewModel as IWpViewModel;
			selectedWpViewModel.GenerateCommand.Execute(null);
		}

		private bool GenerationCheckAction(object parameter)
		{
			IWpViewModel selectedWpViewModel = SelectedViewModel as IWpViewModel;
			return selectedWpViewModel != null;
		}

		private void OptionsAction(object parameter)
		{
			OptionsWindow window = new OptionsWindow(this)
				{
					Owner = Application.Current.MainWindow
				};
			window.ShowDialog();
		}

		private void HelpAction(object parameter)
		{
			HelpWindow window = new HelpWindow
				{
					Owner = Application.Current.MainWindow
				};
			window.ShowDialog();
		}

		private void AboutAction(object parameter)
		{
			AboutWindow window = new AboutWindow
				{
					Owner = Application.Current.MainWindow
				};
			window.ShowDialog();
		}
	}
}
