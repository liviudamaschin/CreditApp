using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CreditAppBMG.ViewModels
{
    public class CreditAppModel
    {
        public Distributor Distributor { get; set; }

        public Retailer Retailer { get; set; }

        public CreditData CreditData { get; set; }

        public CreditDataFiles CreditDataFiles { get; set; }

        // keep track of downloaded logo:
        public string LocalLogo { get; set; }

        public List<SelectListItem> CompanyTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Limited liability company" },
            new SelectListItem { Value = "CA", Text = "S Corporation" },
            new SelectListItem { Value = "US", Text = "Sole proprietor"  },
            new SelectListItem { Value = "PS", Text = "Partnership"  }
        };
    }
}
