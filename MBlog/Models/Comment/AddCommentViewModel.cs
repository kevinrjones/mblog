using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MBlog.Models.Comment
{
    public class AddCommentViewModel : PostViewModel
    {
        [Required]
        public int PostId { get; set; }

        public string Name { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}