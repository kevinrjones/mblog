using MBlogModel;

namespace MBlogIntegrationTest.Builder
{
    internal class BlacklistBuilder : Builder<Blacklist>
    {
        public BlacklistBuilder()
        {
            Instance = new Blacklist();
        }
    }
}