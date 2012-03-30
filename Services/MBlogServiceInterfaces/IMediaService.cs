using System.Collections.Generic;
using System.IO;
using MBlogModel;

namespace MBlogServiceInterfaces
{
    public interface IMediaService
    {
        Media GetMedia(int year, int month, int day, string linkKey);
        Media GetMedia(int mediaId, int userId);
        IEnumerable<Media> GetMedia(int pageNumber, int pageItems, int userId);
        string WriteMedia(string fileName, int userId, string contentType, Stream inputStream, int contentLength);

        void WriteMedia(string fileName, string title, string caption, string description, string alternate, int userId,
                        string contentType, int alignment, int size, Stream inputStream, int contentLength);

        Media UpdateMediaDetails(int id, string fileName, string caption, string description, string alternate,
                                 int userId);
    }
}