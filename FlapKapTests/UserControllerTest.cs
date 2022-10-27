using AutoMapper;
using FakeItEasy;
using FlapKap.Controllers;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
using FlapKap.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlapKapTests
{
    public class UserControllerTest
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ILogger<UserService> _Servicelogger;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepo ;
        private readonly UsersController _userController;

        public UserControllerTest()
        {
            _userRepo = A.Fake<IRepository<User>>();
            _logger = A.Fake<ILogger<UsersController>>();
            _Servicelogger = A.Fake<ILogger<UserService>>();
            _mapper = A.Fake<IMapper>();
            _userController = new UsersController(_logger,_userRepo,_Servicelogger,_mapper);
        }

        [Fact]
        public void UserControllerTest_GetUsers_ReturnUsers()
        {
            //Arrange

            var users = A.Fake<IQueryable<User>>();
            A.CallTo(() => _userRepo.GetAll(I => true)).Returns(users);
            var userList = _mapper.Map<List<User>, List<UserResultsForGet>>(users.ToList());

            //Act

            var result = _userController.Get();

            //Assert
            Assert.Equal( result.users, userList);
        }
    }
    public class UserMapper:Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserResultsForGet>();
        }
    }
}
