using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MBlog.Models.Comment;

namespace MBlog.Models.Post
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            Comments = new List<CommentViewModel>();
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Post { get; set; }
        public string YearPosted { get; set; }
        public string MonthPosted { get; set; }
        public string DayPosted { get; set; }
        public int CommentCount { get { return Comments.Count; }   }
        public List<CommentViewModel> Comments { get; set; }

        private DateTime _datePosted;

        public PostViewModel(MBlogModel.Post post) : this()
        {
            this.DateLastEdited = post.Edited;
            this.DatePosted = post.Posted;
            this.Id = post.Id;
            this.Title = post.Title;
            this.Post = post.BlogPost;
            foreach (var comment in post.Comments)
            {
                CommentViewModel cvm = new CommentViewModel(comment);
                this.Comments.Add(cvm);
            }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set
            {
                _datePosted = value;
                YearPosted = value.Year.ToString("D4");
                MonthPosted = value.Month.ToString("D2");
                DayPosted = value.Day.ToString("D2");
            }
        }

        public DateTime? DateLastEdited { get; set; }
        public string Link { get; set; }

    }
}