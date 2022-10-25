using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int  AvaliableAmount { get; set; }
        public int Cost { get; set; }
        public int SellerId { get; set; }
    }
}
