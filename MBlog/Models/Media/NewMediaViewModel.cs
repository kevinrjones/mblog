using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Linq;

namespace MBlog.Models.Media
{
    public class NewMediaViewModel : IValidatableObject
    {
        private readonly Dictionary<string, string> _validExtensions = new Dictionary<string, string>();


        public NewMediaViewModel()
        {
            _validExtensions.Add("jpg", "image/jpeg");
            _validExtensions.Add("png", "image/png");
            _validExtensions.Add("gif", "image/gif");
            _validExtensions.Add("pdf", "application/pdf");
            _validExtensions.Add("ppt", "application/vnd.ms-powerpoint");
            _validExtensions.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            _validExtensions.Add("doc", "application/msword");
            _validExtensions.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.documen");
            _validExtensions.Add("xls", "application/vnd.ms-excel");
            _validExtensions.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            _validExtensions.Add("zip", "application/zip");
        }
       
        [Required]
        public HttpPostedFileBase File { get; set; }

        public bool IsAllowed(string extension)
        {
            var s = (from a in _validExtensions.Keys
                     where a == extension
                     select a).FirstOrDefault();

            return s != null;
        }

        public string GetExtension(string fileName)
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
        public string QqFile { get; set; }

        public string ContentType
        {
            get
            {
                if (File != null)
                {
                    return File.ContentType;
                }
                return _validExtensions[GetExtension(QqFile).ToLower()];
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string ext;
            if (File != null)
            {
                ext = GetExtension(File.FileName);
            }
            else
            {
                ext = GetExtension(QqFile);
            }
            if (!IsAllowed(ext))
                yield return new ValidationResult("Files with this extension not allowed", new[] { "File" });
        }
    }
}