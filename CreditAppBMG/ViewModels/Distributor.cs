using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class Distributor
    {
        public int? Id { get; set; }
        
        public int? DistributorId { get; set; }

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
        public string DistributorLogoURL { get; set; }

    }
}