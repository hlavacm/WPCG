using Netcorex.Localizations;

namespace Netcorex.Models
{
	public class HelpModel : ModelBase
	{
		private string _text;


		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}


		public override void Dispose()
		{
			try
			{
				_text = null;
			}
			finally
			{
				base.Dispose();
			}
		}


		protected override void Initialize()
		{
			try
			{
				_text = Messages.Help;
			}
			finally
			{
				base.Initialize();
			}
		}
	}
}
