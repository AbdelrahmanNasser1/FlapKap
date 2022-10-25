using AutoMapper;
using FlapKap.Constants;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Response;
using FlapKap.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Services
{
    public class ProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IRepository<Product> _productRepo = null;
        private readonly IRepository<User> _userRepo = null;
        private readonly IMapper _mapper;
        public ProductService(ILogger<ProductService> logger, IRepository<Product> productRepository, IRepository<User> userRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepo = productRepository;
            _userRepo = userRepository;
            _mapper = mapper;
        }
        public ProductsforGetAll GetAllProducts()
        {
            //Intialze object

            _logger.LogInformation("Retrieve all products...");
            ProductsforGetAll objResult = new ProductsforGetAll();
            try
            {
                IQueryable<Product> products= _productRepo.GetAll(i=>true);
                objResult.products = _mapper.Map<List<Product>, List<productList>>(products.ToList());
                objResult.status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, "Failed to retrieve all products ...");
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public ProductResult CreateProduct( ProductModel model)
        {
            //Intialze object

            _logger.LogInformation(string.Format("create Product: {0}", model.ProductName));
            ProductResult objResult = new ProductResult();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.User.UserName && i.Password == model.User.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.User.UserName));
                objResult.Status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Seller))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Seller User Can create a product:  {0}", model.User.UserName));
                objResult.Status = StatusMessages.UnAuthorized;
                return objResult;
            }


            //Check name of product with this user
            List<Product> products = _productRepo.GetAll(i => i.SellerId == existUser.Id).ToList();
            if (products.Count != 0)
            {
                foreach (var item in products)
                {
                    if (model.ProductName == item.ProductName)
                    {
                        _logger.LogInformation(string.Format("Duplicated product:  {0}", model.User.UserName));
                        objResult.Status = StatusMessages.DuplicatedProductName;
                        return objResult;
                    }
                }
            }
            //Intialize product object
            Product newProduct = new Product();
            newProduct = _mapper.Map<Product>(model);
            newProduct.SellerId = existUser.Id;
            try
            {
                _productRepo.Insert(newProduct);
                _productRepo.Save();
                objResult.ProductName = newProduct.ProductName;
                objResult.AvaliableAmount = newProduct.AvaliableAmount;
                objResult.Cost = newProduct.Cost;
                objResult.Status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to Create product: {0}", model.ProductName));
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public ProductForGet GetProduct(int id, UserInfo model )
        {
            //Intialze object

            _logger.LogInformation(string.Format("create Product with id : {0}", id));
            ProductForGet objResult = new ProductForGet();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.UserName && i.Password == model.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.UserName));
                objResult.Status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Seller))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Seller User Can Get his product:  {0}", model.UserName));
                objResult.Status = StatusMessages.UnAuthorized;
                return objResult;
            }
            
            //Intialize product object
            Product newProduct = new Product();
            try
            {
                newProduct =_productRepo.GetById(id);
                if (newProduct.SellerId != existUser.Id)
                {
                    _logger.LogInformation(string.Format("UnAuthenticated Can't get product {0} for another Seller.", newProduct.ProductName));
                    objResult.Status = StatusMessages.UnAuthenticated;
                    return objResult;
                }
                objResult = _mapper.Map<ProductForGet>(newProduct);
                objResult.UserName = model.UserName;
                objResult.Status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to Retrieve product with id : {0}", id));
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public UpdateProduct UpdateProduct(int id, UpdateProductModel model)
        {
            //Intialze object

            _logger.LogInformation(string.Format("Update Product with id : {0}", id));
            UpdateProduct objResult = new UpdateProduct();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.User.UserName && i.Password == model.User.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.User.UserName));
                objResult.Status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Seller))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Seller User Can Update his product:  {0}", model.User.UserName));
                objResult.Status = StatusMessages.UnAuthorized;
                return objResult;
            }

            //Intialize product object
            Product newProduct = new Product();
            try
            {
                newProduct = _productRepo.GetById(id);
                if (newProduct.SellerId != existUser.Id)
                {
                    _logger.LogInformation(string.Format("UnAuthenticated Can't Update product {0} for another Seller.", newProduct.ProductName));
                    objResult.Status = StatusMessages.UnAuthenticated;
                    return objResult;
                }

                newProduct = _mapper.Map<Product>(model);
                newProduct.SellerId = existUser.Id;
                newProduct.Id = id;
                _productRepo.Update(newProduct);
                _productRepo.Save();

                objResult.AvaliableAmount = newProduct.AvaliableAmount;
                objResult.Cost = newProduct.Cost;
                objResult.ProductName = newProduct.ProductName;
                objResult.UserName = model.User.UserName;
                objResult.Status = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to Update product with id : {0}", id));
                objResult.Status = StatusMessages.InvalidParams;
            }
            return objResult;
        }
        public Status DeleteProduct(int id, UserInfo model)
        {
            //Intialze object

            _logger.LogInformation(string.Format("Delete Product with id : {0}", id));
            Status objResult = new Status();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.UserName && i.Password == model.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.UserName));
                objResult = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Seller))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Seller User Can Delete his product:  {0}", model.UserName));
                objResult = StatusMessages.UnAuthorized;
                return objResult;
            }

            //Intialize product object
            Product newProduct = new Product();
            try
            {
                newProduct = _productRepo.GetById(id);
                if (newProduct.SellerId != existUser.Id)
                {
                    _logger.LogInformation(string.Format("UnAuthenticated Can't Delete product {0} for another Seller.", newProduct.ProductName));
                    objResult = StatusMessages.UnAuthenticated;
                    return objResult;
                }


                _productRepo.Delete(id);
                _productRepo.Save();

                objResult = StatusMessages.Success;

            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to Delete product with id : {0}", id));
                objResult = StatusMessages.InvalidParams;
            }
            return objResult;
        }
    }
}
