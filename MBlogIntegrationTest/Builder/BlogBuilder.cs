using System.Collections.Generic;
using MBlogModel;

namespace MBlogIntegrationTest.Builder
{
    internal class BlogBuilder : Builder<Blog>
    {
        public BlogBuilder()
        {
            Instance = new Blog();
        }

        public BlogBuilder WithPosts(IEnumerable<Post> posts)
        {
            foreach (Post post in posts)
            {
                post.Blog = Instance;
                Instance.Posts.Add(post);
            }
            return this;
        }

        public BlogBuilder WithPost(Post p)
        {
            if (p != null)
            {
                p.Blog = Instance;
                Instance.Posts.Add(p);
            }
            return this;
        }
    }
}