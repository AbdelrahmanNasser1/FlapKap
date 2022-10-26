using AutoMapper;
using FlapKap.Constants;
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
    public class ResetService
    {
        private readonly ILogger<ResetService> _logger;
        private readonly IRepository<User> _userRepo = null;
        private readonly IMapper _mapper;
        public ResetService(ILogger<ResetService> logger, IRepository<User> repository, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _mapper = mapper;
        }
        public ResetResult ResetDeposit(UserInfo model)
        {
            _logger.LogInformation(string.Format("Reset deposit amount for user: {0}", model.UserName));
            ResetResult objResult = new ResetResult();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.UserName && i.Password == model.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.UserName));
                objResult.status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Buyer))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Buyers Users Can Reset their deposits:  {0}", model.UserName));
                objResult.status = StatusMessages.UnAuthorizedReset;

                return objResult;
            }

            try
            {
                if (existUser.Deposit == 0 )
                {
                    objResult.status = StatusMessages.Unknown;
                    objResult.status.error_message = "Deposit has been already reseted";
                    return objResult;
                }
                objResult.Deposit = existUser.Deposit;
                existUser.Deposit = 0;
                _userRepo.Update(existUser);
                _userRepo.Save();
                objResult.status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to reset deposit for user: {0}", model.UserName));
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;

        }
    }
}
