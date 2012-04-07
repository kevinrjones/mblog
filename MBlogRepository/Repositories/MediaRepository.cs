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

        #region IMediaRepository Members

        public Media GetMedia(int id)
        {
            return (from i in Entities
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        public Media GetMedia(int year, int month, int day, string linkKey)
        {
            return (from i in Entities
                    where i.Year == year
                          && i.Month == month
                          && i.Day == day
                          && i.LinkKey == linkKey
                    select i).FirstOrDefault();
        }

        public IEnumerable<Media> GetMedia(int pageNumber, int numberOfItems, int userId)
        {
            return Entities
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1)*numberOfItems)
                .Take(numberOfItems)
                .ToList();
        }

        public Media WriteMedia(Media media)
        {
            Create(media);
            return media;
        }

        public void UpdateMedia(Media media)
        {
            Update(media);
        }

        #endregion
    }
}