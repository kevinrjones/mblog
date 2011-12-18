using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IImageRepository
    {
        Image GetImage(int id);
        Image GetImage(int year, int month, int day, string fileName);
        Image WriteImage(Image image);
    }
}