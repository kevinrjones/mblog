﻿using System;
using System.Collections.Generic;
using Logging;
using MBlogDomainInterfaces;
using MBlogDomainInterfaces.ModelState;
using MBlogModel;
using MBlogRepository.Interfaces;

namespace MBlogDomain
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserRepository _userRepository;
        private readonly IUsernameBlacklistRepository _usernameBlacklistRepository;
        private readonly ILogger _logger;

        public UserDomain(IUserRepository userRepository, IUsernameBlacklistRepository usernameBlacklistRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _usernameBlacklistRepository = usernameBlacklistRepository;
            _logger = logger;
        }

        public User GetUser(string email)
        {
            try
            {
                return _userRepository.GetUser(email);
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to retrieve user", e);
            }
        }

        public User CreateUser(string name, string email, string password)
        {
            try
            {
                User user = new User(name, email, password, false);
                _userRepository.Create(user);

                return user;
            }
            catch (Exception e)
            {
                throw new MBlogException("Unable to create user", e);
            }
        }

        public List<ErrorDetails> IsUserRegistrationValid(string name, string email)
        {
            try
            {
                var errorDetails = new List<ErrorDetails>();
                User user = GetUser(email);
                if (user == null)
                {
                    errorDetails.Add(new ErrorDetails { FieldName = "EMail", Message = "EMail already exists in database" });
                }

                Blacklist blacklist = _usernameBlacklistRepository.GetName(name);
                if (blacklist == null)
                {
                    errorDetails.Add(new ErrorDetails { FieldName = "Name", Message = "That user name is reserved" });
                }
                return errorDetails;
            }
            catch (Exception e)
            {

                throw new MBlogException("Unable to access repository", e);
            }
        }
    }
}
