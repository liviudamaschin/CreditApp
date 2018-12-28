using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("AdobeSignLog")]
    public class AdobeSignLogEntity
    {
        [Key]
        public int Id { get; set; }
        public int CreditDataId { get; set; }
        public string Action { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

    }
}
