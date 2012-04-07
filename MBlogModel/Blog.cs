using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBlogModel
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }

        public Blog(string title, string description, bool approveComments, bool commentsEnabled, string nickname,
                    int userId) : this()
        {
            ApproveComments = approveComments;
            Nickname = nickname;
            UserId = userId;
            Title = title;
            Description = description;
            CommentsEnabled = commentsEnabled;
        }

        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, Column("comments_enabled")]
        public bool CommentsEnabled { get; set; }

        [Required, Column("comment_approval")]
        public bool ApproveComments { get; set; }

        [Required, Column("last_updated")]
        public DateTime LastUpdated { get; set; }

        [Required, Column("total_posts")]
        public int TotalPosts { get; set; }

        [Required]
        public string Nickname { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual User User { get; set; }
    }
}