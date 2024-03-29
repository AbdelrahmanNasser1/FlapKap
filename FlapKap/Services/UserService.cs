﻿using AutoMapper;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
using FlapKap.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IRepository<User> _userRepo = null;
        private readonly IMapper _mapper;
        public UserService(ILogger<UserService> logger, IRepository<User> repository, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _mapper = mapper;
        }
        public UserResult CreateUser(UserModel model)
        {
            _logger.LogInformation(string.Format("create user {0}",model.UserName));
            UserResult objResult = new UserResult();
            User existUser = _userRepo.GetAll(i => i.UserName == model.UserName).FirstOrDefault();
            if (existUser != null )
            {
                _logger.LogInformation(string.Format("Duplicated user name {0}", model.UserName));
                objResult.Status = StatusMessages.DuplicatedUserName;
                return objResult;
            }
            User user = _mapper.Map<User>(model);
            try
            {
                _userRepo.Insert(user);
                _userRepo.Save();
                objResult.Id = ReturnId(model);
                objResult.UserName = user.UserName;
                objResult.Deposit = user.Deposit.ToString();
                objResult.RoleId = user.RoleId;

                objResult.Status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to insert user {0}", model.UserName));
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        private int ReturnId(UserModel model)
        {
            _logger.LogInformation(string.Format("Return user id {0}", model.UserName));
            return _userRepo.GetAll(i => i.UserName == model.UserName).FirstOrDefault().Id;
             
        }
        public UserInfo ReturnUser(int id)
        {
            UserInfo userInfo = new UserInfo();
            _logger.LogInformation(string.Format("Return user with id {0}", id));
            userInfo = _mapper.Map<UserInfo>(_userRepo.GetById(id));
            return userInfo;

        }
        public UserResultForGet RetrieveUsers()
        {
            _logger.LogInformation("Retrieve users ...");
            UserResultForGet result = new UserResultForGet();
            try
            {
                IQueryable<User> users = _userRepo.GetAll(i => true);
                result.users = _mapper.Map<List<User>, List<UserResultsForGet>>(users.ToList());
                result.status = StatusMessages.Success;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"Failed to Retrieve users");
                result.status = StatusMessages.InvalidParams;
            }
            return result;
        }
        public UserResult GetUser(int id)
        {
            _logger.LogInformation(string.Format("Get user with id {0}", id));
            UserResult objResult = new UserResult();
            try
            {
                User user=_userRepo.GetById(id);
                objResult = _mapper.Map<UserResult>(user);
                objResult.Status = StatusMessages.Success;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, string.Format("Failed to Get user with id {0}", id));
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public UpdateModel UpdateUser(UpdateUserModel model)
        {
            _logger.LogInformation(string.Format("Update user {0}: ", model.UserName));
            UpdateModel objResult = new UpdateModel();
            try
            {
                User user = _userRepo.GetAll(i=>i.Id == model.Id).FirstOrDefault();
                if (user != null)
                {
                    User updateUser = _mapper.Map<User>(model);
                    updateUser.Deposit = user.Deposit;
                    updateUser.RoleId = user.RoleId;
                    _userRepo.Update(updateUser);
                    _userRepo.Save();
                    objResult.user = _mapper.Map<UserModel>(updateUser);
                    objResult.status = StatusMessages.Success;

                }
                else
                {
                    _logger.LogInformation(string.Format("Failed to update user: {0}", model.UserName));
                    objResult.status = StatusMessages.InvalidParams;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, string.Format("Failed to update user: {0}", model.UserName)); 
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public Status RemoveUser(int id)
        {
            _logger.LogInformation(string.Format("Remove user with id {0}", id));
            Status objResult = new Status();
            try
            {
                _userRepo.Delete(id);
                _userRepo.Save();
                objResult = StatusMessages.Success;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, string.Format("Failed to remove user with id {0}", id));
                objResult = StatusMessages.InvalidParams;
            }
            return objResult;
        }
    }
}
