using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Netcorex.Interfaces;
using Netcorex.Models;

namespace Netcorex.ViewModels
{
	public class PostMetaViewModel : WpViewModelBase<PostMetaModel>
	{
		public PostMetaViewModel(IMainContext parentContext)
			: base(new PostMetaModel(), parentContext)
		{
		}


		public override bool Initialize()
		{
			if (IsInitialized)
			{
				return false;
			}
			Model.Posts = GetAvailablePosts();
			return IsInitialized = (Model.Posts != null);
		}

		protected override IList<long> GetGenerateItemsIds()
		{
			return Model.SelectedPostIds.ToList();
		}

		protected override void GenerateStep(long itemId, MySqlConnection dbConnection, IDatabaseConnectionModel connectionModel)
		{
			Model.PostId = itemId;
			base.GenerateStep(0, dbConnection, connectionModel);
		}
	}
}
