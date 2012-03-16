using System;

namespace MBlogBuilder
{
    public static class BuildMeA
    {
        public static UserBuilder User(string email, string name, string password)
        {
            return (UserBuilder) new UserBuilder().With(u =>
                                                            {
                                                                u.Email = email;
                                                                u.Name = name;
                                                                u.Password = password;
                                                                u.Salt = "salt";
                                                            });
        }

        public static BlogBuilder Blog(string title, string description, string nickname, DateTime updated)
        {
            return (BlogBuilder) new BlogBuilder().With(b =>
                                                            {
                                                                b.Nickname = nickname;
                                                                b.Title = title;
                                                                b.Description = description;
                                                                b.LastUpdated = updated;
                                                            });
        }

        public static PostBuilder Post(string title, string entry, DateTime posted, DateTime edited, bool commentsEnabled = true)
        {
            return (PostBuilder) new PostBuilder().With(p =>
                                                            {
                                                                p.AddPost(title, entry);
                                                                p.Posted = posted;
                                                                p.CommentsEnabled = commentsEnabled;
                                                                p.Edited = edited;
                                                            });
        }

        public static CommentBuilder Comment(string text, DateTime commented, bool approved = true)
        {
            return (CommentBuilder) new CommentBuilder().With(c =>
                                                                  {
                                                                      c.CommentText = text;
                                                                      c.Commented = commented;
                                                                      c.Approved = approved;
                                                                  });
        }

        public static BlacklistBuilder Blacklist(string name)
        {
            return (BlacklistBuilder) new BlacklistBuilder().With(b => { b.Name = name; });
        }

        public static MediaBuilder Media(string fileName, string title, string caption,
                string description, string alternate, int userId,
                string mimeType, int alignment, int size, byte[] imageData)
        {
            return (MediaBuilder)new MediaBuilder().With(m =>
                                {
                                    m.Alignment = alignment;
                                    m.Alternate = alternate;
                                    m.Caption = caption;
                                    m.Data = imageData;
                                    m.Description = description;
                                    m.FileName = fileName;
                                    m.Title = title;
                                    m.UserId = userId;
                                    m.MimeType = mimeType;
                                    m.Size = size;
                                });
        }

        public static MediaBuilder Media(string fileName, string title, string caption,
        string description, string alternate, 
        string mimeType, int alignment, int size, byte[] imageData)
        {
            return (MediaBuilder)new MediaBuilder().With(m =>
            {
                m.Alignment = alignment;
                m.Alternate = alternate;
                m.Caption = caption;
                m.Description = description;
                m.FileName = fileName;
                m.Title = title;
                m.MimeType = mimeType;
                m.Size = size;
                m.Data = imageData;
            });
        }

    }
}