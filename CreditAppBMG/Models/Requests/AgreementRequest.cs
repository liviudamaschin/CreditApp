
namespace CreditAppBMG.Models.Requests
{
    public class AgreementRequest
    {
        /// <summary>
        ///  Information about the document you want to send,
        /// </summary>
        public DocumentCreationInfo documentCreationInfo { get; set; }
        /// <summary>
        ///  Options for authoring and sending the agreement
        /// </summary>
        public InteractiveOptions options { get; set; }
    }
}
