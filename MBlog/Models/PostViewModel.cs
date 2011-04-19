using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBlog.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Post { get; set; }
        public string YearPosted { get; set; }
        public string MonthPosted { get; set; }
        public string DayPosted { get; set; }

        private DateTime _datePosted;
        public DateTime DatePosted
        {
            get { return _datePosted; }
            set
            {
                _datePosted = value;
                YearPosted = value.Year.ToString("D4");
                MonthPosted = value.Month.ToString("D2");
                DayPosted = value.Day.ToString("D2");
            }
        }

        public DateTime? DateLastEdited { get; set; }
        public string Link
        {
            get
            {
                return string.Format("{0}", Title.Replace(' ', '-').Replace('/','-').ToLower());
            }
        }
    }
}