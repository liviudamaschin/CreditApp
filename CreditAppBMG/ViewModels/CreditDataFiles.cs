using System;
using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class CreditDataFiles
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int CreditDataId { get; set; }

        [Display(Name = "NYS Liquor License:")]
        public string LicenseFileName { get; set; }

        public byte[] LicenseFile { get; set; }

        [Display(Name = "NYS Sales Tax Certificate:")]
        public string TaxCertificateFileName { get; set; }

        public byte[] TaxCertificateFile { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}