using System.Collections.Generic;
using Newtonsoft.Json;

namespace CreditAppBMG.Entities
{
    public class WebHookInfo
    {
        public string webHookId { get; set; }
        public string webHookName { get; set; }
        public string webHookNotificationId { get; set; }
        public List<WebHookNotificationApplicableUser> webHookNotificationApplicableUsers { get; set; }
        public WebHookUrlInfo webHookUrlInfo { get; set; }
        public string webHookScope { get; set; }
        [JsonProperty(PropertyName = "event")]
        public string Event { get; set; }
        public string eventDate { get; set; }
        public string eventResourceParentType { get; set; }
        public string eventResourceParentId { get; set; }
        public string eubEvent { get; set; }
        public string eventResourceType { get; set; }
        public string earticipantRole { get; set; }
        public string ectionType { get; set; }
        public string earticipantUserId { get; set; }
        public string earticipantUserEmail { get; set; }
        public string ectingUserId { get; set; }
        public string ectingUserEmail { get; set; }
        public string enitiatingUserId { get; set; }
        public string enitiatingUserEmail { get; set; }
        public string ectingUserIpAddress { get; set; }
        public WebHookAgreement agreement { get; set; }
    }

    public class WebHookNotificationApplicableUser
    {
        public string id { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string payloadApplicable { get; set; }
    }
    public class WebHookUrlInfo
    {
        public string url { get; set; }
    }
    public class WebHookAgreement
    {
        public string id { get; set; }
        public string name { get; set; }
        public string signatureType { get; set; }
        public string status { get; set; }
        public List<Cc> ccs { get; set; }
        public DeviceInfo deviceInfo { get; set; }
        public string documentVisibilityEnabled { get; set; }
        public string createdDate { get; set; }
        public string expirationTime { get; set; }
        public ExternalId externalId { get; set; }
        public PostSignOption postSignOption { get; set; }
        public string firstReminderDelay { get; set; }
        public string locale { get; set; }
        public string message { get; set; }
        public string reminderFrequency { get; set; }
        public string senderEmail { get; set; }
        public VaultingInfo vaultingInfo { get; set; }
        public string workflowId { get; set; }
        public WebHookParticipantSetsInfo participantSetsInfo { get; set; }
        public WebHookDocumentsInfo documentsInfo { get; set; }
    }
    public class Cc
    {
        public string email { get; set; }
        public string label { get; set; }
        public List<string> visiblePages { get; set; }
    }
    public class DeviceInfo
    {
        public string applicationDescription { get; set; }
        public string deviceDescription { get; set; }
        public Location location { get; set; }
        public string deviceTime { get; set; }
    }
    public class Location
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
    public class ExternalId
    {
        public string id { get; set; }
    }
    public class PostSignOption
    {
        public string redirectDelay { get; set; }
        public string redirectUrl { get; set; }
    }
    public class VaultingInfo
    {
        public string enabled { get; set; }
    }
    public class WebHookParticipantSetsInfo
    {
        public List<WebHookParticipantSet> participantSets { get; set; }
        public List<WebHookParticipantSet> nextParticipantSets { get; set; }
    }
    public class WebHookParticipantSet
    {
        public List<WebHookMemberInfo> memberInfos { get; set; }
        public string order { get; set; }
        public string role { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string privateMessage { get; set; }
    }
    public class WebHookMemberInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string name { get; set; }
        public string privateMessage { get; set; }
        public string status { get; set; }
    }
    public class WebHookDocumentsInfo
    {
        public List<WebHookDocument> documents { get; set; }
        public List<WebHookDocument> supportingDocuments { get; set; }
    }
    public class WebHookDocument
    {
        public string id { get; set; }
        public string label { get; set; }
        public string numPages { get; set; }
        public string mimeType { get; set; }
        public string name { get; set; }
    }
}
