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

        public Media GetMedia(int year, int month, int day, string title)
        {
            Media media = null;
            try
            {
                if((media = _mediaRepository.GetMedia(year, month, day, title)) != null)
                {
                    return media;
                }
                throw new MBlogMediaNotFoundException();
            }
            catch (MBlogMediaNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not get media", e);
            }
        }

        public IEnumerable<Media> GetMedia(int pageNumber, int pageItems, int userId)
        {
            return _mediaRepository.GetMedia(pageNumber, pageItems, userId);
        }

        public string WriteMedia(string fileName, int userId, string contentType, Stream inputStream, int contentLength)
        {
            var mediaToCreate = new Media(fileName, userId, contentType, inputStream, contentLength);
            try
            {
                var media = _mediaRepository.GetMedia(mediaToCreate.Year, mediaToCreate.Month, mediaToCreate.Day, mediaToCreate.FileName);
                if (media == null)
                {
                    _mediaRepository.WriteMedia(mediaToCreate);
                    return string.Format("{0}/{1}/{2}/{3}", mediaToCreate.Year, mediaToCreate.Month, mediaToCreate.Day,
                                         mediaToCreate.FileName);
                }
                throw new MBlogInsertItemException("Unable to add item. This item already exists in the database");
            }
            catch (MBlogInsertItemException)
            {
                throw;
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
            var media = new Media(fileName, title, caption, description, alternate, id, contentType, alignment, size, bytes);
            try
            {
                _mediaRepository.WriteMedia(media);
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not create media", e);
            }
        }

        public Media GetMedia(int mediaId, int userId)
        {
            var medium = _mediaRepository.GetMedia(mediaId);
            if(medium == null)
            {
                throw new MBlogMediaNotFoundException();
            }
            if (medium.UserId == userId)
                return medium;

            throw new MBlogMediaNotFoundException("The requested media does not belong to the user");
        }

        private static byte[] ReadBytes(Stream inputStream, int contentLength)
        {
            byte[] bytes = new byte[contentLength];
            inputStream.Read(bytes, 0, contentLength);
            return bytes;
        }

    }
}
