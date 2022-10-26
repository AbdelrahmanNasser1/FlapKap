using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlapKap.Results
{
    public class ResetResult
    {
        [JsonPropertyName("deposit")]
        public int Deposit { get; set; }
        public Status status { get; set; }
    }
}
