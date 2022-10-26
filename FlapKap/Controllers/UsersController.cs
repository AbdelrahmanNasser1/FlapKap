using AutoMapper;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
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
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ILogger<UserService> _Servicelogger;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepo = null;
        public UsersController(ILogger<UsersController> logger , IRepository<User> repository, ILogger<UserService> Servicelogger, IMapper mapper)
        {
            _logger = logger;
            _userRepo = repository;
            _Servicelogger = Servicelogger;
            _mapper = mapper;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public UserResultForGet Get()
        {
            UserResultForGet objResult = null;
            _logger.LogInformation("Retrieving Users ...");
            try
            {
                objResult = new UserService(_Servicelogger, _userRepo, _mapper).RetrieveUsers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to Retrieving Users.");
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public UserResult Get(int id)
        {
            UserResult objResult = new UserResult();
            _logger.LogInformation(string.Format("Retrieve User with id {0}: ",id));

            string userName = User.Claims.First(i => i.Type == "UserName").Value;
            string password = User.Claims.First(i => i.Type == "Password").Value;

            UserInfo userInfo = new UserService(_Servicelogger, _userRepo, _mapper).ReturnUser(id);

            if (userName == userInfo.UserName && password == userInfo.Password)
            {
                try
                {
                    objResult = new UserService(_Servicelogger, _userRepo, _mapper).GetUser(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Format("Unable to Get User with id {0}: ", id));
                    objResult.Status = StatusMessages.InvalidParams;
                }
            }
            else
            {
                _logger.LogInformation("Invalid Credentials.");
                objResult.Status = StatusMessages.InvalidParams;
                objResult.Status.error_message = "Invalid Credentials.";
                return objResult;
            }
            return objResult;
        }

        // POST api/<UsersController>
        [AllowAnonymous]
        [HttpPost]
        public UserResult Post([FromBody] UserModel model)
        {
            UserResult objResult = null;
            _logger.LogInformation("Creating User ...");
            try
            {
                objResult= new UserService(_Servicelogger, _userRepo,_mapper).CreateUser(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create User.");
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public UpdateModel Put([FromBody] UpdateUserModel model)
        {
            UpdateModel objResult = new UpdateModel();
            _logger.LogInformation(string.Format("Update User {0}: ",model.UserName));

            string userName = User.Claims.First(i => i.Type == "UserName").Value;
            string password = User.Claims.First(i => i.Type == "Password").Value;

            if (userName == model.UserName && password == model.Password)
            {
                try
                {
                    objResult = new UserService(_Servicelogger, _userRepo, _mapper).UpdateUser(model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Format("Unable to Update User: {0}.", model.UserName));
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

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public Status Delete(int id)
        {
            Status objResult = new Status();
            _logger.LogInformation(string.Format("Delete User with id:  {0}: ",id));

            string userName = User.Claims.First(i => i.Type == "UserName").Value;
            string password = User.Claims.First(i => i.Type == "Password").Value;

            UserInfo userInfo = new UserService(_Servicelogger, _userRepo, _mapper).ReturnUser(id);

            if (userName == userInfo.UserName && password == userInfo.Password)
            {
                try
                {
                    objResult = new UserService(_Servicelogger, _userRepo, _mapper).RemoveUser(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, string.Format("Unable to Update User: {0}.", id));
                    objResult = StatusMessages.InvalidParams;
                }
            }
            else
            {
                _logger.LogInformation("Invalid Credentials.");
                objResult = StatusMessages.InvalidParams;
                objResult.error_message = "Invalid Credentials.";
                return objResult;
            }
            return objResult;
        }
    }
}
