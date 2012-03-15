using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace MBlogModel
{
    public class User
    {
        public User()
        {
            Blogs = new List<Blog>();
        }

        public User(string name, string email, string password, bool isAdmin) : this()
        {
            Name = name;
            Email = email;
            IsSiteAdmin = isAdmin;
            GeneratePassword(password);
        }

        public virtual int Id { get; set; }

        [Required]
        public virtual string Email { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required, Column("hashed_password")]
        public virtual string HashedPassword { get; private set; }

        [Required]
        public virtual string Salt { get; set; }

        [Column("is_site_admin")]
        public virtual bool IsSiteAdmin { get; private set; }

        public virtual ICollection<Blog> Blogs { get; set; }

        [NotMapped]
        public virtual bool IsBlogOwner { get; set; }
        [NotMapped]
        public virtual string Password
        {
            set { HashedPassword = GenerateHashedPasswordFromPlaintext(value); }
        }

        private void GeneratePassword(string password)
        {
            SHA256 shaM = new SHA256Managed();
            byte[] data;
            Salt = Convert.ToBase64String(Encoding.UTF32.GetBytes(GetHashCode() + new Random().ToString()));
            data = Encoding.UTF32.GetBytes(password + "wibble" + Salt);
            HashedPassword = Convert.ToBase64String(shaM.ComputeHash(data));
        }

        public bool MatchPassword(string password)
        {
            string hashedPassword = GenerateHashedPasswordFromPlaintext(password);

            return HashedPassword == hashedPassword;
        }

        private string GenerateHashedPasswordFromPlaintext(string password)
        {
            SHA256 shaM = new SHA256Managed();
            byte[] data = Encoding.UTF32.GetBytes(password + "wibble" + Salt);

            return Convert.ToBase64String(shaM.ComputeHash(data));
        }
    }
}
