using System.Collections.Generic;

namespace CreditAppBMG.Models.Responses
{
    public class SigningUrlSetInfo
    {
        public List<SigningUrl> SigningUrls { get; set; }
        public string SigningUrlSetName { get; set; }
    }
}
