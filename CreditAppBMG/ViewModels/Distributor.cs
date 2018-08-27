using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class Distributor
    {
        [Required(ErrorMessage = "Required field")]
        public int DistributorId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorAddress { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorState { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorZip { get; set; }
        public string DistributorPhone { get; set; }
        public string DistributorWebSiteURL { get; set; }
    }
}