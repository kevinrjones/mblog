using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models.Validators;

namespace MBlog.Models.Comment
{
    public class AddCommentViewModel : PostViewModel
    {
        public bool CommentsEnabled;

        [Required]
        public int PostId { get; set; }

        public string Name { get; set; }
        [Required]
        [BBCodeValidator(ErrorMessage = "Text is in an invalid format")]
        public string Comment { get; set; }
    }
}