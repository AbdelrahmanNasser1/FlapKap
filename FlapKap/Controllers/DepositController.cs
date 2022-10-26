using AutoMapper;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Results;
using FlapKap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlapKap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DepositController : Controller
    {
        private readonly ILogger<DepositController> _logger;
        private readonly IMapper _mapper;
        private readonly ILogger<DepositService> _Servicelogger;
        private readonly IRepository<User> _userRepo = null;
        public DepositController(ILogger<DepositController> logger, IRepository<User> repository, ILogger<DepositService> Servicelogger, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _Servicelogger = Servicelogger;
            _mapper = mapper;
        }
        // POST api/<DepositController>
        [HttpPost]
        public DepositResult Post([FromBody] DepositModel model)
        {
            DepositResult objResult = new DepositResult();
            _logger.LogInformation(string.Format("Add deposit for buyer user : {0}", model.user.UserName));

            string userName = User.Claims.First(i => i.Type == "UserName").Value;
            string password = User.Claims.First(i => i.Type == "Password").Value;

            if (userName == model.user.UserName && password == model.user.Password)
            {
                try
                {
                    objResult = new DepositService(_Servicelogger, _userRepo, _mapper).UpdateDeposit(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to add deposit.");
                    objResult.status = StatusMessages.InvalidParams;
                }
            }
            else
            {
                _logger.LogInformation("Invalid Credentials.");
                objResult.status = StatusMessages.InvalidParams;
                objResult.status.error_message = "Invalid Credentials.";
                return objResult;
            }
            return objResult;
        }


    }
}
