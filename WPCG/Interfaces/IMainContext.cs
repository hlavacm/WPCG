using System;
using Netcorex.Enums;

namespace Netcorex.Interfaces
{
	public interface IMainContext : ISettingsable
	{
		IDatabaseConnectionViewModel DatabaseConnectionViewModel { get; }
		IViewModel SelectedViewModel { get; }
		ApplicationStates State { get; }
		string StateTitle { get; }
		int WorkingValue { get; set; }
		int WorkingMinimum { get; set; }
		int WorkingMaximum { get; set; }


		Guid RunTask();
    bool CloseTask(Guid taskUid);
	}
}
