using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace MBlog.Models.Media
{
    public class NewMediaViewModel : IValidatableObject
    {
        private readonly List<string> _validExtensions = new List<string> {"jpg",
                                                                           "png",
                                                                           "gif",
                                                                           "ppt",
                                                                           "pptx",
                                                                           "doc",
                                                                           "docx",
                                                                           "xls",
                                                                           "xlsx",
                                                                           "zip", };
                                                                          
        [Required]
        public HttpPostedFileBase File { get; set; }

        internal bool IsAllowed(string extension)
        {
            var s = (from a in _validExtensions
                     where a == extension
                     select a).FirstOrDefault();

            return s != null;
        }

        internal string GetExtension(string fileName)
        {
            return fileName.Split('.').Last();
        }

        public int BlogId { get; set; }
        public string Nickname { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Alternate { get; set; }
        public int Alignment { get; set; }
        public int Size { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string ext = GetExtension(File.FileName);
            if (!IsAllowed(ext))
                yield return new ValidationResult("Files with this extension not allowed", new[] {"File"});            
        }
    }
}