using MBlogModel;

namespace MBlogRepository.Interfaces
{
    public interface IImageRepository
    {
        Image GetImage(int id);
        Image GetImage(string urlPrefix, string fileName);
        void WriteImage(string fileName, string title, string caption, string description, string alternate,
            string prefix, int userId, string mimeType, string alignment, string size, byte[] data);
    }
}