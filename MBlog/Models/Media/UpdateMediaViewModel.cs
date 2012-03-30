namespace MBlog.Models.Media
{
    public class UpdateMediaViewModel : ShowMediaViewModel
    {
        public UpdateMediaViewModel()
        {
        }

        public UpdateMediaViewModel(MBlogModel.Media media) : base(media)
        {
        }
    }
}