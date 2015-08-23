using Netcorex.Models;

namespace Netcorex.Views.Forms
{
	public partial class PostForm
	{
		public PostForm()
		{
			InitializeComponent();
		}


		public new PostModel DataContext
		{
			get { return base.DataContext as PostModel; }
			set { base.DataContext = value; }
		}
	}
}
