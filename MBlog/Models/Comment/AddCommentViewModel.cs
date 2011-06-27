using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBlog.Models.Post;
using MBlog.Models.Validators;

namespace MBlog.Models.Comment
{
    public class AddCommentViewModel
    {
        public AddCommentViewModel()
        {

        }
        public AddCommentViewModel(int postId, bool commentsEnabled)
        {
            Comments = new List<CommentViewModel>();
            PostId = postId;
            CommentsEnabled = commentsEnabled;
        }

        public bool CommentsEnabled;

        [Required]
        public int PostId { get; set; }

        public string Name { get; set; }

        [Required]
        [BBCodeValidator(ErrorMessage = "Text is in an invalid format")]
        public string Comment { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public int CommentCount
        {
            get { return Comments != null ? Comments.Count : 0; }
        }

        public void AddComment(CommentViewModel commentViewModel)
        {
            Comments.Add(commentViewModel);
        }
    }
}