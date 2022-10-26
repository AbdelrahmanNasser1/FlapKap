using AutoMapper;
using FlapKap.Constants;
using FlapKap.Models;
using FlapKap.Repository;
using FlapKap.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Services
{
    public class BuyService
    {
        private readonly ILogger<BuyService> _logger;
        private readonly IRepository<User> _userRepo = null;
        private readonly IRepository<Product> _productRepo = null;
        private readonly IMapper _mapper;
        public BuyService(ILogger<BuyService> logger, IRepository<User> userRepository, IRepository<Product> productRepository, IMapper mapper)
        {
            _logger = logger;
            _userRepo = userRepository;
            _productRepo = productRepository;
            _mapper = mapper;
        }
        public BuyResult BuyProduct (BuyModel model)
        {
            //Intialize object
            _logger.LogInformation(string.Format("Buy products by user: {0}", model.user.UserName));
            BuyResult objResult = new BuyResult();

            //Check user and Role 
            User existUser = _userRepo.GetAll(i => i.UserName == model.user.UserName && i.Password == model.user.Password).FirstOrDefault();
            if (existUser == null)
            {
                _logger.LogInformation(string.Format("User Not Found:  {0}", model.user.UserName));
                objResult.status = StatusMessages.UserNotFound;
                return objResult;
            }
            if (existUser.RoleId != ((int)UserRole.Buyer))
            {
                _logger.LogInformation(string.Format("Unauthorized Only Buyer User Can buy products:  {0}", model.user.UserName));
                objResult.status = StatusMessages.UnAuthorizedDeposit;
                return objResult;
            }
            //check availabilty of products
            NotAvaiableProduct notAvaiableProduct = CheckProductAvailability(model.product_list);
            if (notAvaiableProduct.available != true && !string.IsNullOrEmpty(notAvaiableProduct.ProductName))
            {
                _logger.LogInformation(string.Format("Unavailability for product name {0} to be bought.", notAvaiableProduct.ProductName));
                objResult.status = StatusMessages.UnAvailabilityProduct;
                objResult.status.error_message = string.Format("Unavailability for product name {0} to be bought.", notAvaiableProduct.ProductName);
                return objResult;
            }
            else if(notAvaiableProduct.available != true && string.IsNullOrEmpty(notAvaiableProduct.ProductName))
            {
                _logger.LogInformation(string.Format("product with id {0} not found to be bought.",notAvaiableProduct.Id));
                objResult.status = StatusMessages.ProductNotFound;
                objResult.status.error_message = string.Format("product with id {0} not found to be bought.", notAvaiableProduct.Id);
                return objResult;
            }
            //check availabiity of amount 
            NotAvaiableCost notAvaiableCost = CheckCostAvailability(model.product_list, existUser);
            if (notAvaiableCost.available != true)
            {
                _logger.LogInformation(notAvaiableCost.message);
                objResult.status = StatusMessages.UnAvailableDeposit;
                objResult.status.error_message = notAvaiableCost.message;
                return objResult;
            }
            // buy product


            try
            {
                int totel_spent = 0, change = 0;
                change = _userRepo.GetById(existUser.Id).Deposit;
                List<ProductInfo> list = new List<ProductInfo>();
                foreach (ProductBuyList item in model.product_list)
                {
                    Product product = _productRepo.GetById(item.ProductId);
                    ProductInfo productInfo = new ProductInfo
                    {
                        ProductAmount = item.ProductAmount,
                        ProductName = product.ProductName
                    };
                    list.Add(productInfo);
                    //update available amount of product
                    product.AvaliableAmount -= item.ProductAmount;
                    _productRepo.Update(product);
                    _productRepo.Save();

                    // update deposit for seller 
                    User seller = _userRepo.GetById(product.SellerId);
                    seller.Deposit += item.ProductAmount * product.Cost;
                    _userRepo.Update(seller);
                    _userRepo.Save();

                    // update deposit for buyer 
                    User buyer = _userRepo.GetById(existUser.Id);
                    buyer.Deposit -= item.ProductAmount * product.Cost;
                    _userRepo.Update(buyer);
                    _userRepo.Save();


                    totel_spent += item.ProductAmount * product.Cost;
                }
                objResult.TotalSpent = totel_spent;
                objResult.Change = change-totel_spent;
                objResult.products = list;
                objResult.status = StatusMessages.Success;
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex, string.Format("Failed to buy products: {0}", model.user.UserName));
                objResult.status = StatusMessages.InvalidParams;
            }
            return objResult;

        }
        private NotAvaiableProduct CheckProductAvailability(List<ProductBuyList> produtBuyLists)
        {
            //intialize object 
            _logger.LogInformation("Check Availability for products to buy by user.");
            NotAvaiableProduct notAvaiableProduct = new NotAvaiableProduct();
            foreach (ProductBuyList item in produtBuyLists)
            {
                try
                {
                    Product product = _productRepo.GetById(item.ProductId);
                    if (item.ProductAmount> product.AvaliableAmount)
                    {
                        notAvaiableProduct.available = false;
                        notAvaiableProduct.ProductName = product.ProductName;
                        return notAvaiableProduct;
                    }
                    else
                    {
                        notAvaiableProduct.available = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, string.Format("Failed to get product: {0}", item.ProductId));
                    notAvaiableProduct.available = false;
                    notAvaiableProduct.Id = item.ProductId;
                    return notAvaiableProduct;

                }
            }
            return notAvaiableProduct;
        }
        private NotAvaiableCost CheckCostAvailability(List<ProductBuyList> produtBuyLists , User user )
        {
            //intialize object 
            NotAvaiableCost notAvaiableCost = new NotAvaiableCost();
            _logger.LogInformation("Check Availability deposit for user to buy .");

            int totalCost = 0;
            foreach (ProductBuyList item in produtBuyLists)
            {
                try
                {
                    Product product = _productRepo.GetById(item.ProductId);
                    totalCost += product.Cost * item.ProductAmount;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, string.Format("Failed to get product: {0}", item.ProductId));
                    notAvaiableCost.available = false;
                    notAvaiableCost.message = string.Format("Failed to get product: {0}", item.ProductId);
                    return notAvaiableCost;
                }
            }
            if (totalCost > user.Deposit)
            {
                notAvaiableCost.available = false;
                notAvaiableCost.message = string.Format("User deposit {0} doesn't enough to complete purchase process with amount {1}", user.Deposit, totalCost);
                return notAvaiableCost;
            }
            notAvaiableCost.available = true;
            return notAvaiableCost;
        }
    }
}
