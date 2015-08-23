using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Netcorex.Common;

namespace Netcorex.Windows
{
	/// <summary>
	/// Basic application window including general logic
	/// </summary>
	public abstract class WindowBase : Window
	{
		private readonly double _defaultWidth = 960;
		private readonly double _defaultHeight = 600;


		protected WindowBase(double defaultWidth, double defaultHeight, double defaultMinWidth, double defaultMinHeight)
			: this(defaultWidth, defaultHeight)
		{
			MinWidth = defaultMinWidth;
			MinHeight = defaultMinHeight;
		}

		protected WindowBase(double defaultWidth, double defaultHeight)
			: this()
		{
			Width = _defaultWidth = defaultWidth;
			Height = _defaultHeight = defaultHeight;
		}

		protected WindowBase()
		{
			CloseCommand = new RelayCommand(CloseCommandAction);
			CenterScreenCommand = new RelayCommand(CenterScreenCommandAction);
			DefaultSizeCommand = new RelayCommand(DefaultSizeCommandAction);
		}


		public double DefaultWidth
		{
			get { return _defaultWidth; }
		}
		public double DefaultHeight
		{
			get { return _defaultHeight; }
		}
		public ICommand CloseCommand { get; set; }
		public ICommand CenterScreenCommand { get; set; }
		public ICommand DefaultSizeCommand { get; set; }


		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if ((Left + ActualWidth) > SystemParameters.WorkArea.Width
				|| (Top + ActualHeight) > SystemParameters.WorkArea.Height)
			{
				CenterScreenCommand.Execute(null);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			try
			{
				SaveSettings();
			}
			finally
			{
				base.OnClosing(e);
			}
		}

		protected override void OnClosed(EventArgs e)
		{
			try
			{
				IDisposable disposable = DataContext as IDisposable;
				disposable?.Dispose();
			}
			finally
			{
				base.OnClosed(e);
			}
		}

		protected abstract void SaveSettings();

		private void CloseCommandAction(object parameter)
		{
			Close();
		}

		private void CenterScreenCommandAction(object parameter)
		{
			double left = (SystemParameters.WorkArea.Width / 2) - (Width / 2);
			double top = (SystemParameters.WorkArea.Height / 2) - (Height / 2);
			Left = left > 0 ? left : 0;
			Top = top > 0 ? top : 0;
		}

		private void DefaultSizeCommandAction(object parameter)
		{
			Width = DefaultWidth;
			Height = DefaultHeight;
		}
	}
}
