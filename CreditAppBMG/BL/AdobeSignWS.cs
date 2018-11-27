
using System.Collections.Generic;
using System.Security.Policy;
using CreditAppBMG.Models.Requests;
using CreditAppBMG.Models.Responses;
using RestSharp;

namespace CreditAppBMG.BL
{
    public class AdobeSignWS
    {
        private string Url;
        private RestClient client;

        public AdobeSignWS()
        {
            Url = @"http://localhost:62378/api/AdobeSign";
            client = new RestClient(Url);
        }

        public TransientDocument PostTransientDocument(byte[] fileBytes, string fileName)
        {
            //string url = @"http://localhost:62378/api/AdobeSign";
            //var client = new RestClient(Url);
            var request = new RestRequest("PostTransientDocument", Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", fileBytes, fileName);
            request.AddParameter("fileName", fileName);
            IRestResponse<TransientDocument> result = client.Execute<TransientDocument>(request);
            return result.Data;
        }

        public AgreementCreationResponse CreateAgreement(string transientDocumentId, string agreementName, string recipientEmail)
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
            };

            newAgreementRequest.RequestFormat = DataFormat.Json;
            newAgreementRequest.AddJsonBody(agreementRequest);

            IRestResponse<AgreementCreationResponse> agreementResponse = client.Execute<AgreementCreationResponse>(newAgreementRequest);
            return agreementResponse.Data;
        }

        public SigningUrlResponse GetAgreementSigningUrl(string agreementId)
        {
            var request = new RestRequest("GetAgreementSigningUrl", Method.GET);
            request.AddQueryParameter("agreementId", agreementId);
            IRestResponse<SigningUrlResponse> result = client.Execute<SigningUrlResponse>(request);
            return result.Data;
        }
    }
}
