using System.ComponentModel.DataAnnotations;
using Netcorex.Properties;

namespace Netcorex.Models
{
	public class OptionsModel : ModelBase
	{
		public const string DefaultServerBaseUrl = "http://localhost";
		private string _serverBaseUrl = Settings.Default.ServerBaseUrlOption ?? DefaultServerBaseUrl;
		private bool _isLocalhostServer = Settings.Default.IsLocalhostServerOption;


		[Required]
		[MaxLength(100)]
		public string ServerBaseUrl
		{
			get { return _serverBaseUrl; }
			set { SetProperty(ref _serverBaseUrl, value); }
		}
		[Required]
		public bool IsLocalhostServer
		{
			get { return _isLocalhostServer; }
			set { SetProperty(ref _isLocalhostServer, value); }
		}


		public override void Dispose()
		{
			try
			{
				_serverBaseUrl = null;
				_isLocalhostServer = false;
			}
			finally
			{
				base.Dispose();
			}
		}
	}
}
