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

        public Image GetImage(int year, int month, int day, string fileName)
        {
            return (from i in Entities
                    where i.Year == year
                    && i.Month == month
                    && i.Day == day
                    && i.FileName == fileName
                    select i).FirstOrDefault();
        }

        public Image WriteImage(Image image)
        {
            Create(image);
            return image;
        }
    }
}