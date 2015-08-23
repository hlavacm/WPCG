using System;
using System.Windows.Controls;
using System.Windows.Input;
using Netcorex.Interfaces;

namespace Netcorex
{
	public partial class MainWindow
	{
		public MainWindow()
			: base(800, 600, 640, 400)
		{
			InitializeComponent();
		}


		public new MainContext DataContext
		{
			get { return base.DataContext as MainContext; }
			set { base.DataContext = value; }
		}


		private void OnContentRendered(object sender, EventArgs e)
		{
			try
			{
				DataContext = new MainContext();
				InputBindings.Add(new KeyBinding(DataContext.ShowStatusBarCommand, new KeyGesture(Key.F4)));
				InputBindings.Add(new KeyBinding(DataContext.GenerateCommand, new KeyGesture(Key.F5)));
				InputBindings.Add(new KeyBinding(DataContext.OptionsCommand, new KeyGesture(Key.F12)));
				InputBindings.Add(new KeyBinding(DataContext.HelpCommand, new KeyGesture(Key.F1)));
			}
			finally
			{
				ContentRendered -= OnContentRendered;
			}
		}

		private void MainTabControlOnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MainContext dataContext = DataContext;
			if (dataContext == null)
			{
				return;
			}
			DataContext.SelectedViewModel = GetSelectedViewModel();
		}

		private IViewModel GetSelectedViewModel()
		{
			TabItem selectedTabItem = MainTabControl.SelectedItem as TabItem;
			if (selectedTabItem == null)
			{
				return null;
			}
			IView selectedView = selectedTabItem.Content as IView;
			if (selectedView == null)
			{
				return null;
			}
			return selectedView.DataContext;
		}

		protected override void SaveSettings()
		{
			DataContext.UpdateSettings();
		}
	}
}
