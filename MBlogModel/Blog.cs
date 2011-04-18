using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MBlogModel
{
    public class Blog
    {
        public Blog(string title, string description, string nickname) : this()
        {
            this.Title = title;
            this.Description = description;
            this.Nickname = nickname;
        }
        public Blog()
        {
            Posts = new List<Post>();
        }
        public virtual int Id { get; private set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual string Nickname { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public User User { get; set; }
    }
}
