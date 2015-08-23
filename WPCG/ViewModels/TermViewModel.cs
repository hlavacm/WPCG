using Netcorex.Interfaces;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public class TermViewModel : WpViewModelBase<TermModel>
	{
		public TermViewModel(IMainContext parentContext)
			: base(new TermModel(), parentContext)
		{
		}


		public override bool Initialize()
		{
			if (IsInitialized)
			{
				return false;
			}
			Model.Taxonomies = GetAvailableTaxonomies();
			return IsInitialized = (Model.Taxonomies != null);
		}
	}
}
