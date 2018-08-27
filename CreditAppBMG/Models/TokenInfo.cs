using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Models
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public string DistribuitorID { get; set; }
        public string UserID { get; set; }
        public DateTime Endtamp { get; set; }
    }
}
