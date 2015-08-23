using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class PostView
	{
		public PostView()
		{
			InitializeComponent();
		}


		public new PostViewModel DataContext
		{
			get { return base.DataContext as PostViewModel; }
			set { base.DataContext = value; }
		}
	}
}
