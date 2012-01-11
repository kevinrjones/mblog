using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBlogModel;

namespace MBlogDomainInterfaces
{
    public interface IMediaDomain
    {
        Media GetMedia(int year, int month, int day, string title);
        string WriteMedia(string fileName, int userId, string contentType, Stream inputStream, int contentLength);
        void WriteMedia(string fileName, string title, string caption, string description, string alternate, int id, string contentType, int alignment, int size, Stream inputStream, int contentLength);
    }
}
