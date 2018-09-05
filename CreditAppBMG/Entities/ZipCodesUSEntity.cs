using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Entities
{
    [Table("ZipCodesUS")]
    public partial class ZipCodesUSEntity
    {
        [Key]
        public int ZipCode { get; set; }
        public string PlaceName { get; set; }
        public string State { get; set; }
    }
}
