using Netcorex.Common;
using Netcorex.Interfaces;

namespace Netcorex.Models
{
	public abstract class ModelBase : BindableBase, IModel
	{
		protected ModelBase()
		{
			Initialize();
		}


		public virtual void Dispose()
		{
		}


		protected virtual void Initialize()
		{
		}
	}
}
