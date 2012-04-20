using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MBlog.Models.Media
{
    public class ShowMediaViewModel
    {
        private readonly Dictionary<int, string> _alignLookup = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _sizeLookup = new Dictionary<int, string>();


        protected ShowMediaViewModel()
        {
            _sizeLookup.Add((int) MBlogModel.Media.ValidSizes.Fullsize, "img-fullsize");
            _sizeLookup.Add((int) MBlogModel.Media.ValidSizes.Thumbnail, "img-thumbnail");
            _sizeLookup.Add((int) MBlogModel.Media.ValidSizes.Medium, "img-medium");
            _sizeLookup.Add((int) MBlogModel.Media.ValidSizes.Large, "img-large");

            _alignLookup.Add((int) MBlogModel.Media.ValidAllignments.None, "");
            _alignLookup.Add((int) MBlogModel.Media.ValidAllignments.Right, "img-align-right");
            _alignLookup.Add((int) MBlogModel.Media.ValidAllignments.Left, "img-align-left");
        }

        public ShowMediaViewModel(MBlogModel.Media media)
            : this()
        {
            Id = media.Id;
            FileName = media.FileName;
            Title = media.Title;
            LinkKey = media.LinkKey;
            ContentType = media.MimeType;
            Caption = media.Caption;
            Description = media.Description;
            Alternate = media.Alternate;
            Year = media.Year;
            Month = media.Month;
            Day = media.Day;
            Url = media.Url;
            UserId = media.UserId;
            Alignment = media.Alignment;
            Size = media.Size;
            DisplayDate = new DateTime(media.Year, media.Month, media.Day).ToShortDateString();
        }

        // create styles from data passed in
        private string _classString;
        public string ClassString
        {
            get
            {
                return string.Format("{0} {1}", _sizeLookup[Size], _alignLookup[Alignment]);
            }
        }

        [Required]
        public string Title { get; set; }

        public string LinkKey { get; set; }
        public string ContentType { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alternate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Url { get; set; }
        public string DisplayDate { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Alignment { get; set; }
        public int Size { get; set; }

        public string Extension
        {
            get
            {
                if (!string.IsNullOrEmpty(FileName) && FileName.Contains("."))
                {
                    return FileName.Split('.').Last().ToUpper();
                }
                return "Unknown File Type";
            }
        }

        public string Author { get; set; }

        public string FileName { get; set; }
    }
}