using Netcorex.Interfaces;
using Netcorex.Models;
using Netcorex.Properties;

namespace Netcorex.ViewModels
{
	public class OptionsViewModel : ViewModelBase<OptionsModel>
	{
		public OptionsViewModel(IMainContext parentContext)
			: base(new OptionsModel(), parentContext)
		{
		}


		public void UpdateSettings(bool save = false)
		{
			Settings settings = Settings.Default;
			OptionsModel model = Model;
			settings.ServerBaseUrlOption = model.ServerBaseUrl;
			settings.IsLocalhostServerOption = model.IsLocalhostServer;
			if (save)
			{
				settings.Save();
			}
		}
	}
}
