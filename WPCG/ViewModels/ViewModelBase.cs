using System;
using Netcorex.Common;
using Netcorex.Interfaces;
using Netcorex.Localizations;

namespace Netcorex.ViewModels
{
	public class ViewModelBase<T> : BindableBase, IViewModel
		where T : class, IModel, new()
	{
		private readonly T _model;
		private IMainContext _parentContext;
		private bool _isBusy;
		private string _busyContent = Messages.Working;


		public ViewModelBase(T model, IMainContext parentContext)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (parentContext == null)
			{
				throw new ArgumentNullException("parentContext");
			}
			_model = model;
			_parentContext = parentContext;
		}


		public T Model
		{
			get { return _model; }
		}
		IModel IViewModel.Model
		{
			get { return Model; }
		}
		public IMainContext ParentContext
		{
			get { return _parentContext; }
		}
		public bool IsBusy
		{
			get { return _isBusy; }
			protected set { SetProperty(ref _isBusy, value); }
		}
		public string BusyContent
		{
			get { return _busyContent; }
			protected set { SetProperty(ref _busyContent, value); }
		}


		public virtual void Dispose()
		{
			Model.Dispose();
			_parentContext = null;
			_isBusy = false;
			_busyContent = null;
		}
	}
}
