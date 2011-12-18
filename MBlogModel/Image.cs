using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CollectionHelpers;

namespace MBlogModel
{
    public class Image
    {
        public enum ValidSizes  { Thumbnail, Medium, Large, Fullsize};

        public Image()
        {
            var date = DateTime.Now;
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;             
        }

        public Image(string fileName, string title, string caption,
                string description , string alternate , int userId , 
                string mimeType , string alignment , int size , byte[] imageData) : this()
        {
            FileName = fileName;
            Title = title;
            Caption = caption;
            Description = description;
            Alternate = alternate;
            UserId = userId;
            MimeType = mimeType;
            Alignment = alignment;
            Size = size;
            ImageData = imageData;
        }

        public int Id { get; set; }

        [Required, Column("file_name")]
        public virtual string FileName { get; set; }
        [Required]
        public virtual string Title { get; set; }
        public virtual string Caption { get; set; }
        public virtual string Description { get; set; }
        public virtual string Alternate { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual int Day { get; set; }

        [Required, Column("mime_type")]
        public virtual string MimeType { get; set; }
        [Required]
        public virtual string Alignment { get; set; }

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
                    _size = (int)ValidSizes.Medium;
                }
            }
        }

        [Required, Column("image")]
        public virtual byte[] ImageData { get; set; }
        

        [Required, Column("user_id")]
        public virtual int UserId { get; set; }


        public virtual User User { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Image)) return false;
            return Equals((Image) obj);
        }

        public bool Equals(Image other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && Equals(other.FileName, FileName) 
                && Equals(other.Title, Title) 
                && Equals(other.Caption, Caption) 
                && Equals(other.Description, Description) 
                && Equals(other.Alternate, Alternate)
                && other.Year == Year
                && other.Month == Month
                && other.Day == Day
                && Equals(other.MimeType, MimeType) 
                && Equals(other.Alignment, Alignment) 
                && other.Size == Size
                && other.ImageData.CollectionEquals(ImageData) 
                && other.UserId == UserId 
                && Equals(other.User, User);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
                result = (result*397) ^ (Title != null ? Title.GetHashCode() : 0);
                result = (result*397) ^ (Caption != null ? Caption.GetHashCode() : 0);
                result = (result*397) ^ (Description != null ? Description.GetHashCode() : 0);
                result = (result*397) ^ (Alternate != null ? Alternate.GetHashCode() : 0);
                result = (result * 397) ^ Year;
                result = (result * 397) ^ Month;
                result = (result * 397) ^ Day;                
                result = (result*397) ^ (MimeType != null ? MimeType.GetHashCode() : 0);
                result = (result*397) ^ (Alignment != null ? Alignment.GetHashCode() : 0);
                result = (result*397) ^ Size;
                result = (result*397) ^ (ImageData != null ? ImageData.CollectionGetHashCode() : 0);
                result = (result*397) ^ UserId;
                result = (result*397) ^ (User != null ? User.GetHashCode() : 0);
                return result;
            }
        }
    }
}