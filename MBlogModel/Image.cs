using System.ComponentModel.DataAnnotations;

namespace MBlogModel
{
    public class Image
    {
        public Image()
        {
            
        }

        public Image(string fileName)
        {
            FileName = fileName;
        }

        public int Id { get; set; }

        [Required, Column("file_name")]
        public virtual string FileName { get; set; }
        [Required]
        public virtual string Title { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Description { get; set; }
        public virtual string Alternate { get; set; }
        [Required, Column("url_prefix")]
        public virtual string UrlPrefix { get; set; }
        [Required, Column("mime_type")]
        public virtual string MimeType { get; set; }
        [Required]
        public virtual string Alignment { get; set; }
        [Required]
        public virtual string Size { get; set; }
        [Required, Column("image")]
        public virtual byte[] ImageData { get; set; }
        

        [Required, Column("user_id")]
        public virtual int UserId { get; set; }


        public virtual User User { get; set; }

    }
}