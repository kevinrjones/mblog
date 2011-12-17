using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class ImageRepository : BaseEfRepository<Image>, IImageRepository
    {
        public ImageRepository(string connectionString)
            : base(new ImageDbContext(connectionString))
        {
        }

        public Image GetImage(int id)
        {
            return (from i in Entities
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public Image GetImage(string urlPrefix, string fileName)
        {
            return (from i in Entities
                    where i.UrlPrefix == urlPrefix 
                    && i.FileName == fileName
                    select i).FirstOrDefault();
        }

        public void WriteImage(string fileName, string title, string caption, string description, string alternate, 
            string prefix, int userId, string mimeType, string alignment, string size, byte[] data)
        {
            var image = new Image { 
                FileName = fileName,
                Title = title,
                Caption = caption,
                Description = description,
                UserId = userId,                    
                Alternate = alternate,
                UrlPrefix = prefix,
                MimeType = mimeType,
                Alignment = alignment,
                Size = size,
                ImageData = data, 
            };
            Create(image);
        }
    }
}