using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlapKap.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string  Password { get; set; }
        public int Deposit { get; set; }
        public int RoleId { get; set; }
    }
}
