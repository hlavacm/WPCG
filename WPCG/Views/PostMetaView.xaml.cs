using Netcorex.ViewModels;

namespace Netcorex.Views
{
	public partial class PostMetaView
	{
		public PostMetaView()
		{
			InitializeComponent();
		}


		public new PostMetaViewModel DataContext
		{
			get { return base.DataContext as PostMetaViewModel; }
			set { base.DataContext = value; }
		}
	}
}
