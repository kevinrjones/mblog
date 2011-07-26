namespace MBlog.Logging
{
    public interface IMessageInformation
    {
        string Path { get; set; }
        string RawUrl { get; set; }
    }
}