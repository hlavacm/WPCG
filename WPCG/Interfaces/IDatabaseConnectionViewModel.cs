using MySql.Data.MySqlClient;

namespace Netcorex.Interfaces
{
	public interface IDatabaseConnectionViewModel : IViewModel, ISettingsable
	{
		new IDatabaseConnectionModel Model { get; }
		bool IsChecked { get; }
		string CheckedTitle { get; }


		bool Check();
		MySqlConnection GetConnection();
		bool TryGetConnection(out MySqlConnection connection);
	}
}
