using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Netcorex.Common
{
	public abstract class BindableBase : INotifyPropertyChanged, INotifyPropertyChanging
	{
		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;


		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentNullException("propertyName");
			}
			OnPropertyChanging(propertyName);
			if (Equals(storage, value))
			{
				return false;
			}
			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}


		protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
		{
			PropertyChangingEventHandler eventHandler = PropertyChanging;
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(this, new PropertyChangingEventArgs(propertyName));
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler eventHandler = PropertyChanged;
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
