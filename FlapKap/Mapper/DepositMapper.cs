using AutoMapper;
using FlapKap.Models;
using FlapKap.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Mapper
{
    public class DepositMapper :Profile
    {
        public DepositMapper()
        {
            CreateMap<DepositModel, User>()
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.user.UserName))
                .ForMember(des => des.Password, opt => opt.MapFrom(src => src.user.Password));

            CreateMap<User, DepositResult>()
                .ForMember(des => des.status, opt => opt.Ignore());
            
        }
    }
}
