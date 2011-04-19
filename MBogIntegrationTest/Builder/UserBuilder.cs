using MBlogModel;

namespace MBogIntegrationTest.Builder
{
    internal class UserBuilder : Builder<User>
    {
        public UserBuilder()
        {
            Instance = new User();
        }

        public UserBuilder WithBlog(Blog blog)
        {
            Instance.Blogs.Add(blog);
            return this;
        }
    }
}