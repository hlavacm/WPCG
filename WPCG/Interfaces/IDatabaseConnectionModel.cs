namespace Netcorex.Interfaces
{
	public interface IDatabaseConnectionModel : IModel
	{
		string DatabaseName { get; set; }
		string UserName { get; set; }
		string Password { get; set; }
		string ServerName { get; set; }
		string TablePrefix { get; set; }


		string GetConnectionString();
		string GetTablePrefixed(string table);
	}
}
