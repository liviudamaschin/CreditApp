using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CreditAppBMG.ViewModels;
using Newtonsoft.Json;
using CreditAppBMG.Models;
using RestSharp;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

namespace CreditAppBMG.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                TokenInfo tokenInfo = VerifyToken(token, out string tokenErrorMessage);
                RetailerInfo retailerInfo = GetRetailerInfo(tokenInfo, out string retailerErrorMessage);
            }

            CreditAppModel viewModel = new CreditAppModel();
            //viewModel.CompanyTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            viewModel.CreditData = new CreditData();
            viewModel.Distributor = new Distributor();
            viewModel.Retailer = new Retailer();

            //load data from database or webservice
            viewModel.Retailer.BusinessName = "test1";
            return View(viewModel);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public string UpdateAction([FromBody] CreditAppModel model)
        {
            //JsonConvert.DeserializeObject<>
            //Handle your logic here
            //return $"You typed : {model}." ;
            return "zzz";
        }


        [HttpPost]
        public ActionResult GeneratePDF(CreditAppModel model)
        {
            //update DB
           // if (ModelState.IsValid)
            {
                //generate pdf
                using (var reader = new PdfReader(@"C:\Liviu\Dev\SORINAKE\testApps\BMG_Credit_Application_Form_BLANK_2.pdf"))
                {
                    using (var fileStream = new FileStream(@"C:\Liviu\Dev\SORINAKE\testApps\Output.pdf", FileMode.Create, FileAccess.Write))
                    {
                        var document = new Document(reader.GetPageSizeWithRotation(1));
                        var writer = PdfWriter.GetInstance(document, fileStream);

                        document.Open();

                        for (var i = 1; i <= reader.NumberOfPages; i++)
                        {
                            document.NewPage();

                            var baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            var importedPage = writer.GetImportedPage(reader, i);

                            var contentByte = writer.DirectContent;
                            contentByte.BeginText();
                            contentByte.SetFontAndSize(baseFont, 8);

                            var firststring = "guguta";
                            contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, firststring, 88, 665, 0);
                            contentByte.EndText();
                            contentByte.AddTemplate(importedPage, 0, 0);
                        }

                        document.Close();
                        writer.Close();
                    }
                }
            }
           
            return Ok();
        }

        private TokenInfo VerifyToken(string token, out string errorMessage)
        {
            TokenInfo tokenInfo = default(TokenInfo);
            errorMessage = "";

            //string url = WebConfigurationManager.AppSettings["verifyToken"].ToString();
            string url = @"https://webservice.bevmedia.com/BMGOrderWebService/api/verifyToken";
            var client = new RestClient(url);
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

        private RetailerInfo GetRetailerInfo(TokenInfo tokenInfo, out string errorMessage)
        {
            RetailerInfo retailerInfo = default(RetailerInfo);
            errorMessage = "";
            //string url = WebConfigurationManager.AppSettings["getRetailerInfo"];
            string url = @"https://webservice.bevmedia.com/BMGOrderWebService/api/getRetailerInfo";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("TokenValue", tokenInfo.Token);
            request.AddHeader("DistribuitorId", tokenInfo.DistribuitorID);
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
    }
}
