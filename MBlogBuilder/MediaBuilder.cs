using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBlogModel;

namespace MBlogBuilder
{
    public class MediaBuilder : Builder<Media>
    {
        public MediaBuilder()
        {
            Instance = new Media();
        }

        public MediaBuilder(string fileName, string title, string caption,
                string description, string alternate, int userId,
                string mimeType, int alignment, int size, byte[] imageData)
        {
            Instance = new Media(fileName, title, caption, description,
                alternate, userId, mimeType, alignment, size, imageData);
        }

        public MediaBuilder WithUser(User user)
        {
            Instance.User = user;
            return this;
        }
    }
}
