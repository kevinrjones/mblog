using System.Collections.Generic;
using System.Linq;
using MBlogModel;
using MBlogRepository.Contexts;
using MBlogRepository.Interfaces;
using Repository;

namespace MBlogRepository.Repositories
{
    public class MediaRepository : BaseEfRepository<Media>, IMediaRepository
    {
        public MediaRepository(string connectionString)
            : base(new MediaDbContext(connectionString))
        {
        }

        public Media GetMedia(int id)
        {
            return (from i in Entities
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public Media GetMedia(int year, int month, int day, string fileName)
        {
            var images = Entities.ToList();
            return (from i in images
                    where i.Year == year
                    && i.Month == month
                    && i.Day == day
                    && i.FileName == fileName
                    select i).FirstOrDefault();
        }

        public Media WriteMedia(Media media)
        {
            Create(media);
            return media;
        }
    }
}