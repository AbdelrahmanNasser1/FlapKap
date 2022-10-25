using AutoMapper;
using FlapKap.Constants;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Services
{
    public class DepositService
    {
        private readonly ILogger<DepositService> _logger;
        private readonly IRepository<User> _userRepo = null;
        private readonly IMapper _mapper;
        public DepositService(ILogger<DepositService> logger, IRepository<User> repository, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _mapper = mapper;
        }
        public DepositResult UpdateDeposit(DepositModel model)
        {
            //Intialze object

            _logger.LogInformation(string.Format("Add deposit amount to account of user: {0}", model.user.UserName));
            DepositResult objResult = new DepositResult();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.user.UserName && i.Password == model.user.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.user.UserName));
                objResult.status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Buyer))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Buyer User Can add a deposit:  {0}", model.user.UserName));
                objResult.status = StatusMessages.UnAuthorizedDeposit;
                return objResult;
            }

            // check the deposit amount.
            if (model.Deposit != 5 && model.Deposit != 10 && model.Deposit != 20 && model.Deposit != 50 && model.Deposit != 100)
            {
                objResult.status = StatusMessages.InvalidAmounts;
                _logger.LogInformation(string.Format("Invalid deposit amount {0}", model.Deposit));
                return objResult;
            }

            
           
            //Intialize user object
            User updatedUser = new User();
            updatedUser = _mapper.Map<User>(model);
            updatedUser.RoleId = existUser.RoleId;
            updatedUser.Id = existUser.Id;
            updatedUser.Deposit += existUser.Deposit;

            try
            {
                _userRepo.Update(updatedUser);
                _userRepo.Save();
                objResult = _mapper.Map<DepositResult>(updatedUser);
                objResult.status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to Create product: {0}", model.user.UserName));
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
    }
}
