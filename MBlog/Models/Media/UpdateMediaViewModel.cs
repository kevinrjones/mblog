using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace MBlog.Models.Media
{
    public class UpdateMediaViewModel : ShowMediaViewModel
    {
        public UpdateMediaViewModel()  { }

        public UpdateMediaViewModel(MBlogModel.Media media) : base(media)
        {
        }
    }
}