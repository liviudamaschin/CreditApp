
namespace CreditAppBMG.Models.Requests
{
    public class MemberInfo
    {
        /// <summary>
        /// The email address of the participant
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// The unique identifier of the participant
        /// </summary>
        public string participantId { get; set; }
        /// <summary>
        /// The company of the participant, if available
        /// </summary>
        public string company { get; set; }
        /// <summary>
        /// The name of the participant, if available
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ['PASSWORD' or 'WEB_IDENTITY' or 'KBA' or 'PHONE' or 'OTHER']: Security options that apply to the participant
        /// </summary>
        public string[] securityOptions { get; set; }
        /// <summary>
        /// The title of the participant, if available
        /// </summary>
        public string title { get; set; }
    }
}
