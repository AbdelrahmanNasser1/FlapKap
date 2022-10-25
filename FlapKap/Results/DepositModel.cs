using FlapKap.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlapKap.Results
{
    public class DepositModel
    {
        public UserInfo user { get; set; }
        public int Deposit { get; set; }
    }
    public class DepositResult
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("deposit")]
        public int Deposit { get; set; }
        public Status status { get; set; }
    }
}
