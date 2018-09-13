using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Entities
{
    [Table("Distributors")]
    public class DistributorEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? DistributorId { get; set; }
        public string DistributorName { get; set; }
        public string DistributorAddress { get; set; }
        public string DistributorCity { get; set; }
        public string DistributorState { get; set; }
        public string DistributorZip { get; set; }
        public string DistributorPhone { get; set; }
        public string DistributorWebSiteURL { get; set; }
        public string DistributorLogoURL { get; set; }
    }
}
