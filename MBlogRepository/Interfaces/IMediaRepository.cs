using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IMediaRepository
    {
        Media GetMedia(int id);
        Media GetMedia(int year, int month, int day, string title);
        Media WriteMedia(Media media);
    }
}