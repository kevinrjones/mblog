using MBlogModel;

namespace MBlogBuilder
{
    public class BlacklistBuilder : Builder<Blacklist>
    {
        public BlacklistBuilder()
        {
            Instance = new Blacklist();
        }
    }
}