using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Results
{
    public static class StatusMessages
    {
        public static Status Success = new Status() { is_success = true };

        public static Status Unknown = new Status() { is_success = false, error_code = "E001", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E001"] };
        public static Status InvalidParams = new Status() { is_success = false, error_code = "E002", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E002"] };
        public static Status InvalidAmounts = new Status() { is_success = false, error_code = "E003", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E003"] };
        public static Status InvalidRole = new Status() { is_success = false, error_code = "E004", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E004"] };
        public static Status DuplicatedUserName = new Status() { is_success = false, error_code = "E005", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E005"] };
        public static Status UserNotFound = new Status() { is_success = false, error_code = "E006", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E006"] };
        public static Status UnAuthorized = new Status() { is_success = false, error_code = "E007", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E007"] };
        public static Status DuplicatedProductName = new Status() { is_success = false, error_code = "E008", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E008"] };
        public static Status UnAuthenticated = new Status() { is_success = false, error_code = "E009", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E009"] };
        public static Status UnAuthorizedDeposit = new Status() { is_success = false, error_code = "E010", error_message = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Status")["E010"] };







    }
    public class Status
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool is_success { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error_code { set; get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string error_message { set; get; }

    }
}
