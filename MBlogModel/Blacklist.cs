using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Blacklist
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}