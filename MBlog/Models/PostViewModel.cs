using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MBlog.Models.Comment;

namespace MBlog.Models
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