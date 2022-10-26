using FlapKap.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlapKap.Results
{
    public class BuyModel
    {
        [JsonPropertyName("product_list")]
        public List<ProductBuyList> product_list { get; set; }
        public UserInfo user { get; set; }
    }
    public class BuyResult
    {
        [JsonPropertyName("total_spent")]
        public int TotalSpent { get; set; }

        public List<ProductInfo> products { get; set; }

        [JsonPropertyName("change")]
        public int Change { get; set; }
        public Status status { get; set; }
    }

    public class ProductInfo
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("product_amount")]
        public int ProductAmount { get; set; }
    }
    public class ProductBuyList
    {
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }
        [JsonPropertyName("product_amount")]
        public int ProductAmount { get; set; }
    }
    public class NotAvaiableProduct
    {
        public bool available { get; set; }
        public string ProductName { get; set; }
        public int Id { get; set; }
    }
    public class NotAvaiableCost
    {
        public bool available { get; set; }
        public string message { get; set; }

    }
}
