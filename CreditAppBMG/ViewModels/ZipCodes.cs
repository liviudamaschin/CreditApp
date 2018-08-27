using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class ZipCodes
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Abbreviation { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string ZipCodeLow { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string ZipCodeHigh { get; set; }
    }
}