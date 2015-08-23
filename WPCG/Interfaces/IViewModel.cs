using System;
using System.ComponentModel;

namespace Netcorex.Interfaces
{
	public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChanging, IDisposable
	{
		IModel Model { get; }
	}
}
