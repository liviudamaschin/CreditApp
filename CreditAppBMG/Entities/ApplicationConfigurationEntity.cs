using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("ApplicationConfiguration")]
    public class ApplicationConfigurationEntity
    {
        public int Id { get; set; }

        [Required] public string ConfigKey { get; set; }

        [Required] public string ConfigValue { get; set; }


    }
}