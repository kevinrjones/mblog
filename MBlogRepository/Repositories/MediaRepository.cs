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

        public Media GetMedia(int year, int month, int day, string title)
        {
            return (from i in Entities
                    where i.Year == year
                    && i.Month == month
                    && i.Day == day
                    && i.Title == title
                    select i).FirstOrDefault();
        }

        public IEnumerable<Media> GetMedia(int pageNumber, int pageSize, int userId)
        {
            return Entities
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Media WriteMedia(Media media)
        {
            Create(media);
            return media;
        }
    }
}