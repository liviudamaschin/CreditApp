using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Models.Responses
{
    public class SigningDocumentResponse
    {
        public string agreementId { get; set; }
        public string signingUrl { get; set; }
    }
}
