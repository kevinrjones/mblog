using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBlogDomainInterfaces;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class MediaDomain : IMediaDomain
    {
        private IMediaRepository _mediaRepository;

        public MediaDomain(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public Media GetMedia(int year, int month, int day, string fileName)
        {
            try
            {
                return _mediaRepository.GetMedia(year, month, day, fileName);
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not get media", e);
            }
        }

        public string WriteMedia(string fileName, int userId, string contentType, Stream inputStream, int contentLength)
        {
            string url = "";

            var img = new Media(fileName, userId, contentType, inputStream, contentLength);
            try
            {
                _mediaRepository.WriteMedia(img);
                return url;
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not create media", e);
            }
        }

        public void WriteMedia(string fileName, string title, string caption, string description, string alternate, int id, string contentType, int alignment, int size, Stream inputStream, int contentLength)
        {
            var bytes = ReadBytes(inputStream, contentLength);

            // todo: url?                
            var img = new Media(fileName, caption, caption, description, alternate, id, contentType, alignment, size, bytes);
            try
            {
                _mediaRepository.WriteMedia(img);
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not create media", e);
            }
        }

        private static byte[] ReadBytes(Stream inputStream, int contentLength)
        {
            byte[] bytes = new byte[contentLength];
            inputStream.Read(bytes, 0, contentLength);
            return bytes;
        }

    }
}
