using System.ComponentModel;

namespace Netcorex.Interfaces
{
	public interface IView
	{
		IViewModel DataContext { get; set; }
	}
}
