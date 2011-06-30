using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBlog.Models.Validators;

namespace MBlog.Models.Post
{
    public class CreatePostViewModel
    {
        public string Nickname { get; set; }
        public int BlogId { get; set; }
        public string Title { get; set; }
        [BBCodeValidator(ErrorMessage = "Text is in an invalid format")]
        public string Post { get; set; }

        public bool IsCreate { get; set; }
    }
}