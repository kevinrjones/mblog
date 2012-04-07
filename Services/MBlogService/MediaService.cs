﻿using System;
using System.Collections.Generic;
using System.IO;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogServiceInterfaces;

namespace MBlogService
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaService(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        #region IMediaService Members

        public Media GetMedia(int year, int month, int day, string linkKey)
        {
            Media media = null;
            try
            {
                if ((media = _mediaRepository.GetMedia(year, month, day, linkKey)) != null)
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

        public Media WriteMedia(string fileName, int userId, string contentType, Stream inputStream, int contentLength)
        {
            var mediaToCreate = new Media(fileName, userId, contentType, inputStream, contentLength);

            try
            {
                Media media = _mediaRepository.GetMedia(mediaToCreate.Year, mediaToCreate.Month, mediaToCreate.Day,
                                                        mediaToCreate.LinkKey);
                if (media == null)
                {
                    mediaToCreate = _mediaRepository.WriteMedia(mediaToCreate);
                    return mediaToCreate;
                }
                throw new MBlogInsertItemException("Unable to add media. The media already exists in the database");
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

        public void WriteMedia(string fileName, string title, string caption, string description, string alternate,
                               int userId, string contentType, int alignment, int size, Stream inputStream,
                               int contentLength)
        {
            byte[] bytes = ReadBytes(inputStream, contentLength);

            // todo: url?
            var media = new Media(fileName, title, caption, description, alternate, userId, contentType, alignment, size,
                                  bytes);
            try
            {
                _mediaRepository.WriteMedia(media);
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not create media", e);
            }
        }

        public Media UpdateMediaDetails(int mediaId, string title, string caption, string description, string alternate,
                                        int userId)
        {
            Media medium;
            try
            {
                medium = _mediaRepository.GetMedia(mediaId);
            }
            catch (Exception)
            {
                throw new MBlogException("Unable to find media");
            }
            if (medium.UserId != userId)
                throw new MBlogException("Unable to update medium. This user does not have permission");
            medium.Title = title;
            medium.Caption = caption;
            medium.Description = description;
            medium.Alternate = alternate;

            try
            {
                _mediaRepository.UpdateMedia(medium);
            }
            catch (Exception e)
            {
                throw new MBlogException("Could not update media", e);
            }
            return medium;
        }

        public void DeleteMedia(int mediaId, int userId)
        {
            Media medium;
            try
            {
                medium = _mediaRepository.GetMedia(mediaId);
            }
            catch (Exception)
            {
                throw new MBlogException("Unable to find media");
            }
            if (medium.UserId != userId)
            {
                throw new MBlogException("Unable to delete medium. This user does not have permission");
            }

            try
            {
                _mediaRepository.Delete(medium);
            }
            catch (Exception)
            {
                throw new MBlogException("Unable to delete media");
            }
        }

        public Media GetMedia(int mediaId, int userId)
        {
            Media medium;
            try
            {
                medium = _mediaRepository.GetMedia(mediaId);
            }
            catch (Exception)
            {
                throw new MBlogException("Unable to find media");
            }

            if (medium == null)
            {
                throw new MBlogMediaNotFoundException();
            }
            if (medium.UserId == userId)
                return medium;

            throw new MBlogMediaNotFoundException("The requested media does not belong to the user");
        }

        #endregion

        private static byte[] ReadBytes(Stream inputStream, int contentLength)
        {
            var bytes = new byte[contentLength];
            inputStream.Read(bytes, 0, contentLength);
            return bytes;
        }
    }
}