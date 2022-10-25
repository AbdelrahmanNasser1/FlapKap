using FlapKap.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlapKap.Response
{
    public class UserResult :BaseResult
    {
        public int Id  { get; set; }
        public string UserName { get; set; }
    }
    public class UserResultForGet 
    {
        public List<UserResultsForGet> users { get; set; }
        public Status status { get; set; }
    }

    public class UserResultsForGet
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
    public class UpdateModel
    {
        public UserModel user { get; set; }
        public Status status { get; set; }
    }
    public class UserModel
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("passsword")]
        public string Password { get; set; }
        [JsonPropertyName("deposit")]
        public int Deposit { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
    }
    public class UserInfo
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("passsword")]
        public string Password { get; set; }
    }
    public class UpdateUserModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("passsword")]
        public string Password { get; set; }
        [JsonPropertyName("deposit")]
        public int Deposit { get; set; }
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
    }
}
