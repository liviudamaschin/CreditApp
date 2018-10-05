using System;
using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class CreditDataFiles
    {
        [Key]
        //[Required(ErrorMessage = "Required field")]
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Required field")]
        public int CreditDataId { get; set; }

        [Display(Name = "NYS Liquor License:")]
        public string LicenseFileName { get; set; }

        public byte[] LicenseFile { get; set; }

        [Display(Name = "NYS Sales Tax Certificate:")]
        public string TaxCertificateFileName { get; set; }

        public byte[] TaxCertificateFile { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdateLicense { get; set; }

        public DateTime? LastUpdateCertificate { get; set; }

        public string LicenseFileMessage => GetLicenseFileMessage();


        public string CertificateFileMessage => GetCertificateFileMessage();

        public string GetLicenseFileMessage()
        {
            var retVal = String.Empty;
            if (LastUpdateLicense.HasValue)
                retVal = $"Last upload: {LicenseFileName} on {LastUpdateLicense.Value.ToShortDateString()} at {LastUpdateLicense.Value.ToLongTimeString() }";

            return retVal;
        }

        public string GetCertificateFileMessage()
        {
            var retVal = String.Empty;
            if (LastUpdateCertificate.HasValue)
                retVal = $"Last upload: {TaxCertificateFileName} on {LastUpdateCertificate.Value.ToShortDateString()} at {LastUpdateCertificate.Value.ToLongTimeString()}";

            return retVal;
        }
    }
}