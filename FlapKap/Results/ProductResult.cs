using FlapKap.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlapKap.Results
{
    public class ProductResult
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
        public Status Status { get; set; }
    }
    public class ProductModel
    {
        public UserInfo User { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
    }
    public class ProductForGet
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
        public Status Status { get; set; }

    }
    public class UpdateProduct
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
        public Status Status { get; set; }
    }
    public class UpdateProductModel
    {
        public UserInfo User { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
    }
    public class ProductsforGetAll
    {
        public List<productList> products { get; set; }
        public Status status { get; set; }
    }
    public class productList
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("available_amount")]
        public int AvaliableAmount { get; set; }
        [JsonPropertyName("cost")]
        public int Cost { get; set; }
        [JsonPropertyName("seller_id")]
        public int sellerId { get; set; }
    }
}
