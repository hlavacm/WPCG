using System.Windows;
using Netcorex.Enums;
using Netcorex.Interfaces;
using Netcorex.Localizations;

namespace Netcorex.Views
{
	public abstract class WpViewBase : ViewBase
	{
		protected WpViewBase()
		{
			Loaded += OnLoaded;
			DataContextChanged += OnDataContextChanged;
		}


		public new IWpViewModel DataContext
		{
			get { return base.DataContext as IWpViewModel; }
			set { base.DataContext = value; }
		}


		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Initialize();
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (IsLoaded)
			{
				Initialize();
			}
		}

		private void Initialize()
		{
			IWpViewModel dataContext = DataContext;
			if (dataContext == null)
			{
				return;
			}
			dataContext.InitializeAsync();
		}
	}
}
