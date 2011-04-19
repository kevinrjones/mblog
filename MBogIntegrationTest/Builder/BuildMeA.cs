﻿using System;

namespace MBogIntegrationTest.Builder
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
        public static PostBuilder Post(string title, string entry, DateTime posted)
        {
            return (PostBuilder)new PostBuilder().With(p => { p.AddPost(title, entry); p.Posted = posted; }); 
        }
    }
}