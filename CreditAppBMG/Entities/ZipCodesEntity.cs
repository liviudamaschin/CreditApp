using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("ZipCodes")]
    public partial class ZipCodesEntity
    {
        [Key]
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string ZipCodeLow { get; set; }
        public string ZipCodeHigh { get; set; }
    }
}
