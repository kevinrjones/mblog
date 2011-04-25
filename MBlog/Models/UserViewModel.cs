using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;

namespace MBlog.Models
{
    public class UserViewModel : IPrincipal, IIdentity, IValidatableObject
    {
        public UserViewModel()
        {
            Nicknames = new List<string>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Repeat Password")]
        [Required]
        public string RepeatPassword { get; set; }

        [Editable(false)]
        public bool IsLoggedIn { get; set; }

        public string AuthenticationType
        {
            get { return "Cookie"; }
        }

        public bool IsAuthenticated
        {
            get { return IsLoggedIn; }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity
        {
            get { return this; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != RepeatPassword)
                yield return new ValidationResult("The passwords fields must match", new[] { "Password" });
        }

        public bool IsBlogOwner(string nickname)
        {
            var result = (from n in Nicknames
                          where n == nickname
                          select n).Count();

            return result != 0;
        }

        public List<string> Nicknames { get; set; }
    }
}