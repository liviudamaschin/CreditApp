
using System.Collections.Generic;
using System.Security.Policy;
using CreditAppBMG.Entities;
using CreditAppBMG.Extensions;
using CreditAppBMG.Models.Requests;
using CreditAppBMG.Models.Responses;
using RestSharp;

namespace CreditAppBMG.BL
{
    public class AdobeSignWS
    {
        private string Url;
        private RestClient client;
        private readonly CreditAppRepository repository = new CreditAppRepository();
        private const string AdobeSignProxyApiUrlKey = "AdobeSignProxyApiUrlKey";

        public AdobeSignWS()
        {
            Url = repository.GetKeyValue(AdobeSignProxyApiUrlKey);
            //Url = @"http://localhost:51201/api/AdobeSign";
            client = new RestClient(Url);
        }

        public TransientDocument PostTransientDocument(byte[] fileBytes, string fileName)
        {
            //string url = @"http://localhost:51201/api/AdobeSign";
            //var client = new RestClient(Url);
            var request = new RestRequest("PostTransientDocument", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", fileBytes, fileName);
            //request.AddParameter("fileName", fileName);
            request.AddQueryParameter("fileName", fileName);
            IRestResponse<TransientDocument> result = client.Execute<TransientDocument>(request);
            //log
            repository.AddAdobeSignLog("PostTransientDocument", fileName, result.Data.ToJson());

            return result.Data;
        }

        public SigningDocumentResponse SendDocumentForSignature(byte[] fileBytes, string fileName, int creditDataId, string recipientEmail)
        {
            var request = new RestRequest("SendDocumentForSignature", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", fileBytes, fileName);
            
            request.AddQueryParameter("fileName", fileName);
            request.AddQueryParameter("creditDataId", creditDataId.ToString());
            request.AddQueryParameter("recipientEmail", recipientEmail);
            IRestResponse<SigningDocumentResponse> result = client.Execute<SigningDocumentResponse>(request);
            //log
            //repository.AddAdobeSignLog("PostTransientDocument", fileName, result.Data.ToJson());

            return result.Data;
        }

        public AgreementCreationResponse CreateAgreement(string transientDocumentId, string agreementName, string recipientEmail, int AgreementId)
        {
            var newAgreementRequest = new RestRequest("CreateAgreement", Method.POST);
            var agreementRequest = new AgreementMinimalRequest
            {
                fileInfos = new List<FileInfo>
                {
                    new FileInfo
                    {
                        transientDocumentId = transientDocumentId
                    }
                },
                name = agreementName,
                participantSetsInfo = new List<ParticipantInfo>
                {
                    new ParticipantInfo
                    {
                        memberInfos = new List<MemberInfo>
                        {
                            new MemberInfo
                            {
                                email = recipientEmail
                            }
                        },
                        order = 1,
                        role = "SIGNER"
                    }
                },
                signatureType = "ESIGN",
                state = "IN_PROCESS"
                //state="AUTHORING"
            };

            newAgreementRequest.RequestFormat = DataFormat.Json;
            newAgreementRequest.AddJsonBody(agreementRequest);

            IRestResponse<AgreementCreationResponse> agreementResponse = client.Execute<AgreementCreationResponse>(newAgreementRequest);

            //log
            repository.AddAdobeSignLog("CreateAgreement", agreementRequest.ToJson(), agreementResponse.Data.ToJson());

            return agreementResponse.Data;
        }

        public SigningUrlResponse GetAgreementSigningUrl(string agreementId)
        {
            var request = new RestRequest("GetAgreementSigningUrl", Method.GET);
            request.AddQueryParameter("agreementId", agreementId);
            IRestResponse<SigningUrlResponse> result = client.Execute<SigningUrlResponse>(request);

            //log
            repository.AddAdobeSignLog("GetAgreementSigningUrl", request.Parameters.ToJson(), result.Data.ToJson());

            return result.Data;
        }

        public string GetAgreement(string agreementId)
        {
            var request = new RestRequest("GetAgreement", Method.GET);
            request.AddQueryParameter("agreementId", agreementId);
            var result = client.Execute(request);

            //log
            repository.AddAdobeSignLog("GetAgreement", request.Parameters.ToJson(), result.Content.ToJson());

            return result.Content;
        }

        public SigningUrlResponse AgreementSigningPosition(string agreementId, float height, float left, float top, float width)
        {
            var request = new RestRequest("PutAgreementSigningPosition", Method.PUT);
            request.AddQueryParameter("agreementId", agreementId);
            request.AddQueryParameter("height", height.ToString());
            request.AddQueryParameter("left", left.ToString());
            request.AddQueryParameter("top", top.ToString());
            request.AddQueryParameter("width", width.ToString());
            IRestResponse<SigningUrlResponse> result = client.Execute<SigningUrlResponse>(request);
            return result.Data;
        }
    }
}
