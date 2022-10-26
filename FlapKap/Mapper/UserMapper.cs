using AutoMapper;
using FlapKap.Models;
using FlapKap.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Mapper
{
    public class UserMapper :Profile
    {
        public UserMapper()
        {
            CreateMap<UserModel, User>()
                .ForMember(i => i.Id, opt => opt.Ignore());

            CreateMap<UpdateUserModel, User>();
            CreateMap<User, UserModel>();
            CreateMap<User, UserInfo>();
            CreateMap<User, UserResult>();
            CreateMap<User, UserResultsForGet>();
        }
        
    }
}
