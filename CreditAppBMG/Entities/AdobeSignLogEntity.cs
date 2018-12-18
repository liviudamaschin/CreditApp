using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Entities
{
    [Table("AdobeSignLog")]
    public class AdobeSignLogEntity
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

    }
}
