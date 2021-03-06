﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("CreditDataFiles")]
    public partial class CreditDataFilesEntity
    {
        [Key]
        public int? Id { get; set; }
        public int CreditDataId { get; set; }
        public string LicenseFileName { get; set; }
        public byte[] LicenseFile { get; set; }
        public string TaxCertificateFileName { get; set; }
        public byte[] TaxCertificateFile { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdateLicense { get; set; }
        public DateTime? LastUpdateCertificate { get; set; }
 
        public virtual CreditDataEntity CreditData { get; set; }
    }
}
