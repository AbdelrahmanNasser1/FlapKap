﻿using AutoMapper;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
using FlapKap.Results;
using FlapKap.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlapKap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ResetController : Controller
    {
        private readonly ILogger<ResetController> _logger;
        private readonly IMapper _mapper;
        private readonly ILogger<ResetService> _Servicelogger;
        private readonly IRepository<User> _userRepo = null;
        public ResetController(ILogger<ResetController> logger, IRepository<User> repository, ILogger<ResetService> Servicelogger, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _Servicelogger = Servicelogger;
            _mapper = mapper;
        }

        // POST api/<ResetController>
        [HttpPost]
        public ResetResult Post([FromBody] UserInfo model)
        {
            _logger.LogInformation(string.Format("Reset deposit for buyer user : {0}", model.UserName));
            ResetResult objResult = new ResetResult();

            string userName = User.Claims.First(i => i.Type == "UserName").Value;
            string password = User.Claims.First(i => i.Type == "Password").Value;
            if (userName == model.UserName && password == model.Password)
            {
                try
                {
                    objResult = new ResetService(_Servicelogger, _userRepo, _mapper).ResetDeposit(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to reset deposit.");
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
