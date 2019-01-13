﻿using System.Collections.Generic;

namespace CreditAppBMG.Models.Responses
{
    public class AgreementMemberInfo
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
        /// All the child participants of the current participant. The possible values for the status of these participants are, SHARE and DELEGATE
        /// </summary>
        public List<ParticipantSetInfo> alternateParticipants { get; set; }

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
