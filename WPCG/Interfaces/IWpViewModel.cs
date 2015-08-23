using System.Threading.Tasks;
using System.Windows.Input;

namespace Netcorex.Interfaces
{
	public interface IWpViewModel : IViewModel
	{
		new IWpModel Model { get; }
		ICommand GenerateCommand { get; }


		Task InitializeAsync();
	}
}
