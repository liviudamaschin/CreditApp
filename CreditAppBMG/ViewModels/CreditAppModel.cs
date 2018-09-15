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

        public List<SelectListItem> StatesListItems { get; set; } 

        public IEnumerable<States> States { get; set; }

        // keep track of downloaded logo:
        public string LocalLogo { get; set; }

        public List<SelectListItem> CompanyTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Limited liability company" },
            new SelectListItem { Value = "CA", Text = "S Corporation" },
            new SelectListItem { Value = "US", Text = "Sole proprietor"  },
            new SelectListItem { Value = "PS", Text = "Partnership"  }
        };

        public List<SelectListItem> DeliveryTime { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "8am - 12pm", Text = "8am - 12pm" },
            new SelectListItem { Value = "1pm - 8pm", Text = "1pm - 8pm" },
            new SelectListItem { Value = "8pm - 8am", Text = "8pm - 8am"  }
        };

        public List<SelectListItem> PropertyType { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Residential", Text = "Residential" },
            new SelectListItem { Value = "Commercial", Text = "Commercial" }
        };

        public List<SelectListItem> AccountType { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Checking", Text = "Checking" },
            new SelectListItem { Value = "Savings", Text = "Savings" }
        };
    }
}
