using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MBlogModel
{
    public class Blog
    {
        public virtual int Id { get; private set; }
        [Required]
        public virtual string Title { get; private set; }
        [Required]
        public virtual string Description { get; private set; }
        [Required]
        public virtual string Nickname { get; private set; }

        public virtual ICollection<Post> Posts { get; set; }

        public User User { get; set; }
    }
}
