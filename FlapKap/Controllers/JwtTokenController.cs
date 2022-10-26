using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
using FlapKap.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlapKap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : Controller
    {
        private IConfiguration _configuration;
        private readonly IRepository<User> _userRepo = null;

        public JwtTokenController(IConfiguration configuration, IRepository<User> repository)
        {
            _configuration = configuration;
            _userRepo = repository;
        }
        [HttpPost]
        public ActionResult Post(UserInfo model)
        {
            if (model != null && model.UserName != null && model.Password!= null)
            {
                User user = _userRepo.GetAll(i => i.UserName == model.UserName && i.Password == model.Password).FirstOrDefault();
                JWT jwt = _configuration.GetSection("JWT").Get<JWT>();
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("Id",user.Id.ToString()),
                        new Claim("UserName",user.UserName),
                        new Claim("Password",user.Password),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("InValid Credentials");
                }
            }
            else
            {
                return BadRequest("InValid Credentials");
            }
        }
    }
}
