
namespace CreditAppBMG.Models.Requests
{
    public class RequestFormField
    {
        public FormFieldLocation[] locations { get; set; }

        public string name { get; set; }

        public string defaultValue { get; set; }

        public bool required { get; set; }

        public bool readOnly { get; set; }
    }
}
