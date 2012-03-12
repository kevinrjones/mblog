using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using CollectionHelpers;

namespace MBlogModel
{
    public class Media
    {
        public enum ValidSizes { Thumbnail, Medium, Large, Fullsize };
        public enum ValidAllignments { None, Left, Right };

        public Media()
        {
            var date = DateTime.Now;
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        public Media(string fileName, string title, string caption,
                string description, string alternate, int userId,
                string mimeType, int alignment, int size, byte[] imageData)
            : this()
        {
            FileName = fileName;
            if (!string.IsNullOrEmpty(title))
            {
                Title = title;
            }
            else
            {
                Title = FileName.Split('.').First();
            }
            LinkKey = Title.Replace(" ", "");
            Caption = caption;
            Description = description;
            Alternate = alternate;
            UserId = userId;
            MimeType = mimeType;
            Alignment = alignment;
            Size = size;
            Data = imageData;
        }

        public Media(string fileName, int id, string contentType, Stream inputStream, int contentLength)
            : this(fileName, "", "", "", "", id, contentType, 0, 0, null)
        {
            Data = new byte[contentLength];
            inputStream.Read(Data, 0, contentLength);
        }
        public int Id { get; set; }

        [Required, Column("file_name")]
        public virtual string FileName { get; set; }
        [Required(AllowEmptyStrings = true)]
        public virtual string Title { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Description { get; set; }
        public virtual string Alternate { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual int Day { get; set; }

        [Required, Column("mime_type")]
        public virtual string MimeType { get; set; }

        private int _alignment;
        [Required]
        public virtual int Alignment
        {
            get { return _alignment; }
            set
            {
                if (Enum.IsDefined(typeof(ValidAllignments), value))
                {
                    _alignment = value;
                }
                else
                {
                    _alignment = (int)ValidAllignments.None;
                }
            }
        }

        private int _size;

        [Required]
        public virtual int Size
        {
            get { return _size; }
            set
            {
                if (Enum.IsDefined(typeof(ValidSizes), value))
                {
                    _size = value;
                }
                else
                {
                    _size = (int)ValidSizes.Fullsize;
                }
            }
        }

        [Required, Column("bytes")]
        public virtual byte[] Data { get; set; }


        [Required, Column("user_id")]
        public virtual int UserId { get; set; }

        [Required, Column("link_key")]
        public string LinkKey { get; set; }

        public virtual User User { get; set; }

        public string Url
        {
            get { return string.Format("{0}/{1}/{2}/{3}", Year, Month, Day, LinkKey); }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Media)) return false;
            return Equals((Media)obj);
        }

        public bool Equals(Media other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}