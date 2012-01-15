using System.Collections.Generic;
using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IMediaRepository
    {
        Media GetMedia(int id);
        Media GetMedia(int year, int month, int day, string title);
        IEnumerable<Media> GetMedia(int pageNumber, int pageSize, int userId);
        Media WriteMedia(Media media);
    }
}