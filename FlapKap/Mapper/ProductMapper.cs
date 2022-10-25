using AutoMapper;
using FlapKap.Models;
using FlapKap.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Mapper
{
    public class ProductMapper :Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductModel, Product>()
                .ForMember(i => i.SellerId, opt => opt.Ignore())
                .ForMember(i => i.Id, opt => opt.Ignore());
            CreateMap<Product, ProductForGet>();
            CreateMap<UpdateProductModel, Product>();
            CreateMap<Product, productList>();
        }
    }
}
