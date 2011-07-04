﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MBlogModel
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required, Column("comments_enabled")]
        public bool CommentsEnabled{ get; set; }

        [Required, Column("comment_approval")]
        public bool ApproveComments { get; set; }
        
        [Required]
        public string Nickname { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual User User { get; set; }
    }
}
