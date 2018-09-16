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

        public DateTime? LastUpdateLicence { get; set; }

        public DateTime? LastUpdateCertificate { get; set; }

        public string LicenceFileMessage
        {
            get
            {
                return this.GetLicenceFileMessage();
            }
        }

        public string CertificateFileMessage
        {
            get
            {
                return this.GetCertificateFileMessage();
            }
        }

        public string GetLicenceFileMessage()
        {
            var retVal = String.Empty;
            if (this.LastUpdateLicence.HasValue)
                retVal = $"Last upload: {this.LicenseFileName} on {this.LastUpdateLicence.Value.ToShortDateString()} at {this.LastUpdateLicence.Value.ToLongTimeString() }";

            return retVal;
        }

        public string GetCertificateFileMessage()
        {
            var retVal = String.Empty;
            if (this.LastUpdateCertificate.HasValue)
                retVal = $"Last upload: {this.TaxCertificateFile} on {this.LastUpdateCertificate.Value.ToShortDateString()} at {this.LastUpdateCertificate.Value.ToLongTimeString()}";

            return retVal;
        }
    }
}