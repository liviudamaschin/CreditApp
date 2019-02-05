using CreditAppBMG.Models;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace CreditAppBMG.BL
{
    public class BevMediaService
    {
        private readonly string _baseUrl;
        public BevMediaService(string baseUrl)
        {
            this._baseUrl = baseUrl;
        }
        public TokenInfo VerifyToken(string token, out string errorMessage)
        {
            TokenInfo tokenInfo = default(TokenInfo);
            errorMessage = "";

            //string url = repository.GetKeyValue(verifyTokenKey);
            //string url = @"https://webservice.bevmedia.com/BMGOrderWebService/api/verifyToken";
            var client = new RestClient($"{this._baseUrl}/verifyToken");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Token", token);
            IRestResponse response = client.Execute(request);
            try
            {
                tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(response.Content);
            }
            catch (Exception)
            {
                errorMessage = response.Content;
            }
            return tokenInfo;
        }

        public RetailerInfo GetRetailerInfo(TokenInfo tokenInfo, out string errorMessage)
        {
            RetailerInfo retailerInfo = default(RetailerInfo);
            errorMessage = "";
            //string url = repository.GetKeyValue(retailerInfoUrl);
            //string url = @"https://webservice.bevmedia.com/BMGOrderWebService/api/getRetailerInfo";
            var client = new RestClient($"{this._baseUrl}/getRetailerInfo");
            var request = new RestRequest(Method.POST);
            request.AddHeader("TokenValue", tokenInfo.Token);
            request.AddHeader("DistributorId", tokenInfo.DistributorID);
            request.AddHeader("UserId", tokenInfo.UserID);
            IRestResponse response = client.Execute(request);
            try
            {
                retailerInfo = JsonConvert.DeserializeObject<RetailerInfo>(response.Content);
            }
            catch (Exception)
            {
                errorMessage = response.Content;
            }
            return retailerInfo;
        }

        public DistributorInfo GetDistributorInfo(TokenInfo tokenInfo, out string errorMessage)
        {
            DistributorInfo distributorInfo = default(DistributorInfo);
            errorMessage = "";
            //string url = repository.GetKeyValue(retailerInfoUrl);
            //string url = @"https://webservice.bevmedia.com/BMGOrderWebService/api/getRetailerInfo";
            var client = new RestClient($"{this._baseUrl}/getDistributorInfo");
            var request = new RestRequest(Method.POST);
            request.AddHeader("TokenValue", tokenInfo.Token);
            request.AddHeader("DistributorId", tokenInfo.DistributorID);
            request.AddHeader("UserId", tokenInfo.UserID);
            IRestResponse response = client.Execute(request);
            try
            {
                distributorInfo = JsonConvert.DeserializeObject<DistributorInfo>(response.Content);
            }
            catch (Exception)
            {
                errorMessage = response.Content;
            }
            return distributorInfo;
        }
    }
}
