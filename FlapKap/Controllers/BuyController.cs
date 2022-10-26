using AutoMapper;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Results;
using FlapKap.Services;
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
    public class BuyController : Controller
    {
        private readonly ILogger<BuyController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepo = null;
        private readonly ILogger<BuyService> _Servicelogger;
        private readonly IRepository<User> _userRepo = null;
        public BuyController(ILogger<BuyController> logger, IRepository<User> userRepository, IRepository<Product> productRepository, ILogger<BuyService> Servicelogger, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepository;
            _productRepo = productRepository;
            _Servicelogger = Servicelogger;
            _mapper = mapper;
        }

        // POST api/<BuyController>
        [HttpPost]
        public BuyResult Post([FromBody] BuyModel model)
        {
            BuyResult objResult = null;
            _logger.LogInformation(string.Format("Buy Products by user name: {0}", model.user.UserName));
            try
            {
                objResult = new BuyService(_Servicelogger, _userRepo, _productRepo,  _mapper).BuyProduct(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to Puy products.");
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }


    }
}
