using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace MBlog.Models.Media
{
    public class UpdateMediaViewModel 
    {
        public UpdateMediaViewModel()
        {
        }

        public UpdateMediaViewModel(MBlogModel.Media media)
        {
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
            DisplayDate = new DateTime(media.Year, media.Month, media.Day).ToShortDateString();
        }

        public string FileName { get; set; }
        public string LinkKey { get; set; }
        public string ContentType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DisplayDate { get; set; }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alternate { get; set; }
    }
}