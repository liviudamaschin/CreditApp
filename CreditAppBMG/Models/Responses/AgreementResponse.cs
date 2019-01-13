using System;
using System.Collections.Generic;

namespace CreditAppBMG.Models.Responses
{
    public class AgreementResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ParticipantSetInfo> participantSetsInfo { get; set; }
        public string senderEmail { get; set; }
        public DateTime createdDate { get; set; }
        public string signatureType { get; set; }
        public string locale { get; set; }
        public string status { get; set; }
        public bool documentVisibilityEnabled { get; set; }
    }
}
