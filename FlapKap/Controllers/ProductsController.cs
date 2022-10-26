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

    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ILogger<ProductService> _Servicelogger;
        private readonly IMapper _mapper;
        private readonly IRepository<Product> _productRepo = null;
        private readonly IRepository<User> _userRepo = null;
        public ProductsController(ILogger<ProductsController> logger, IRepository<Product> productRepository, IRepository<User> userRepository, ILogger<ProductService> Servicelogger, IMapper mapper)
        {
            _logger = logger;
            _productRepo = productRepository;
            _userRepo = userRepository;
            _Servicelogger = Servicelogger;
            _mapper = mapper;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        [AllowAnonymous]
        public ProductsforGetAll Get()
        {
            ProductsforGetAll objResult = null;
            _logger.LogInformation("Retrieve Products... ");
            try
            {
                objResult = new ProductService(_Servicelogger, _productRepo, _userRepo, _mapper).GetAllProducts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can not Retrieve Product.");
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // GET api/<ProductsController>/5
        [HttpGet("id")]
        public ProductForGet Get(int id,  string  user_name , string password)
        {
            ProductForGet objResult = null;
            UserInfo model = new UserInfo {UserName= user_name,Password=password };
            _logger.LogInformation(string.Format("Retrieve Product: {0}", id));
            try
            {
                objResult = new ProductService(_Servicelogger, _productRepo, _userRepo, _mapper).GetProduct(id,model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can not Retrieve Product.");
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ProductResult Post([FromBody] ProductModel model)
        {
            ProductResult objResult = null;
            _logger.LogInformation(string.Format("Create Product: {0}",model.ProductName));
            try
            {
                objResult = new ProductService(_Servicelogger, _productRepo,_userRepo, _mapper).CreateProduct(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create User.");
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public UpdateProduct Put(int id, [FromBody] UpdateProductModel model)
        {
            UpdateProduct objResult = null;
            _logger.LogInformation(string.Format("Update Product: {0}", id));
            try
            {
                objResult = new ProductService(_Servicelogger, _productRepo, _userRepo, _mapper).UpdateProduct(id, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can not Update Product.");
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public Status Delete(int id,[FromBody] UserInfo model)
        {
            Status objResult = null;
            _logger.LogInformation(string.Format("Delete Product: {0}", id));
            try
            {
                objResult = new ProductService(_Servicelogger, _productRepo, _userRepo, _mapper).DeleteProduct(id, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Can not Delete Product.");
                objResult = StatusMessages.InvalidParams;
            }
            return objResult;
        }
    }
}
