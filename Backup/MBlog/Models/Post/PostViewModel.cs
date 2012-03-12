﻿using System;
using System.ComponentModel.DataAnnotations;
using MBlog.Models.Comment;

namespace MBlog.Models.Post
{
    public class PostViewModel : BasePostViewModel
    {
        private DateTime _datePosted;

        public PostViewModel()
        {
            Title = "";
            Post = "";
            DatePosted = new DateTime();
        }

        public PostViewModel(MBlogModel.Post post) : this()
        {
            DateLastEdited = post.Edited;
            DatePosted = post.Posted;
            Id = post.Id;
            Title = post.Title;
            Post = post.BlogPost;
            Link = post.TitleLink;
            AddCommentViewModel = new AddCommentViewModel(post.Id, post.CommentsEnabled);
            foreach (MBlogModel.Comment comment in post.Comments)
            {
                if (comment.Approved)
                {
                    var cvm = new CommentViewModel(comment);
                    AddCommentViewModel.AddComment(cvm);
                }
            }
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Post { get; set; }

        public string YearPosted { get; set; }
        public string MonthPosted { get; set; }
        public string DayPosted { get; set; }

        public int CommentCount
        {
            get { return AddCommentViewModel.CommentCount; }
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

        public bool CommentsEnabled { get; set; }

        public AddCommentViewModel AddCommentViewModel { get; set; }
    }
}