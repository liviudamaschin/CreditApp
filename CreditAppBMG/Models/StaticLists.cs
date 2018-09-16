using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Models
{
    public class StaticLists
    {
        public Dictionary<string, string> CompanyTypes = new Dictionary<string, string> {
            { "MX", "Limited liability company" },
            { "CA", "S Corporation" },
            { "US", "Sole proprietor" },
            { "PS", "Partnership" }
        };

        public string GetCompanyTypeName(string companyType)
        {
            string companyTypeName = string.Empty;
            if (CompanyTypes.ContainsKey(companyType))
            {
                companyTypeName = CompanyTypes[companyType];
            }
            return CompanyTypes[companyType];
        }

    }
}
