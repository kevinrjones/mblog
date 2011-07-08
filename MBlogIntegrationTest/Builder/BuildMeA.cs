using System;

namespace MBlogIntegrationTest.Builder
{
    internal static class BuildMeA
    {
        public static UserBuilder User(string email, string name, string password)
        {
            return (UserBuilder)new UserBuilder().With(u => { u.Email = email; u.Name = name; u.Password = password; u.Salt = "salt"; });
        }
        public static BlogBuilder Blog(string title, string description, string nickname)
        {
            return (BlogBuilder)new BlogBuilder().With(b => { b.Nickname = nickname; b.Title = title; b.Description = description; });
        }
        public static PostBuilder Post(string title, string entry, DateTime posted, bool commentsEnabled = true)
        {
            return (PostBuilder)new PostBuilder().With(p => { p.AddPost(title, entry); p.Posted = posted; p.CommentsEnabled = commentsEnabled; });
        }
        public static CommentBuilder Comment(string text, DateTime commented, bool approved = true)
        {
            return (CommentBuilder)new CommentBuilder().With(c => { c.CommentText = text; c.Commented = commented; c.Approved = approved; });
        }
        public static BlacklistBuilder Blacklist(string name)
        {
            return (BlacklistBuilder)new BlacklistBuilder().With(b => { b.Name = name; });
        }
    }
}