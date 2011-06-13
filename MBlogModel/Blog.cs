using System;
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
        public virtual int Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string Description { get; set; }

        [Required, Column("comments_enabled")]
        public virtual bool CommentsEnabled{ get; set; }

        [Required, Column("comment_approval")]
        public virtual bool ApproveComments { get; set; }
        
        [Required]
        public virtual string Nickname { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public User User { get; set; }
    }
}
