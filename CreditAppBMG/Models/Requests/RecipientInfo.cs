using System.Collections.Generic;

namespace CreditAppBMG.Models.Requests
{
    public class RecipientInfo
    {
        /// <summary>
        ///  Information about the members of the recipient set
        /// </summary>
        public List<RecipientMemberInfo> recipientSetMemberInfos { get; set; }
        /// <summary>
        /// ['SIGNER' or 'APPROVER' or 'DELEGATE_TO_SIGNER' or 'DELEGATE_TO_APPROVER']: Specify the role of recipient set
        /// </summary>
        public string recipientSetRole { get; set; }
        /// <summary>
        /// Specify the name of Recipient set. Maximum no of characters in recipient set name is restricted to 255
        /// </summary>
        public string recipientSetName { get; set; }

        public int? signingOrder { get; set; }
    }
}
