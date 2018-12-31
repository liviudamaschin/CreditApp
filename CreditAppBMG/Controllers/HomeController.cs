using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CreditAppBMG.BL;
using CreditAppBMG.Entities;
using CreditAppBMG.Enums;
using CreditAppBMG.Models;
using CreditAppBMG.Models.Responses;
using CreditAppBMG.Pdf;
using CreditAppBMG.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;

namespace CreditAppBMG.Controllers
{

    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public HomeController(IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
       
        public IActionResult Index(string token)
        {

            var viewModel = GetInitialValues(token);


            if (!string.IsNullOrWhiteSpace(viewModel.CreditData.SigningUrl))
            {
                return Redirect(viewModel.CreditData.SigningUrl);
            }
            else if (!string.IsNullOrWhiteSpace(viewModel.CreditData.AdobeSignAgreementId))
            {
                AdobeSignWS ws = new AdobeSignWS();
                var signingUrlResp = ws.GetAgreementSigningUrl(viewModel.CreditData.AdobeSignAgreementId, viewModel.CreditData.Id.Value);
                if (signingUrlResp?.SigningUrlSetInfos != null)
                {
                    // update the signing url
                    using (var context = new CreditAppContext())
                    {
                        var creditDataEntity =
                            context.CreditData.SingleOrDefault(x => x.Id == viewModel.CreditData.Id.Value);
                        creditDataEntity.SigningUrl = signingUrlResp.SigningUrlSetInfos[0].SigningUrls[0].EsignUrl;
                        context.Update(creditDataEntity);
                        context.SaveChanges();
                    }
                    return Redirect(signingUrlResp.SigningUrlSetInfos[0].SigningUrls[0].EsignUrl);
                }
                else
                    return View("NotAvailableView", viewModel.Distributor);
            }
            else
            {
                return View(viewModel);
            }

            
        }

        private void FillDistributorFromRetailerInfo(CreditAppModel viewModel, RetailerInfo retailerInfo)
        {
            //viewModel.Distributor = new Distributor();
            if (viewModel.Distributor == null)
                viewModel.Distributor = new Distributor();
            viewModel.Distributor.DistributorId = retailerInfo.DistributorId.ToString();
            viewModel.Distributor.DistributorName = retailerInfo.DistributorName;
            viewModel.Distributor.DistributorAddress = retailerInfo.DistributorAddress;
            viewModel.Distributor.DistributorCity = retailerInfo.DistributorCity;
            viewModel.Distributor.DistributorState = retailerInfo.DistributorState;
            viewModel.Distributor.DistributorZip = retailerInfo.DistributorZip;
            viewModel.Distributor.DistributorPhone = retailerInfo.DistributorPhone;
            viewModel.Distributor.DistributorWebSiteURL = retailerInfo.DistributorWebSiteURL;
            
            // download logo:
            if (!string.IsNullOrEmpty(retailerInfo.DistributorLogoURL))
            {
                viewModel.Distributor.DistributorLogoURL = retailerInfo.DistributorLogoURL;
                var localFileName = Path.GetFileName(retailerInfo.DistributorLogoURL);
                var localFileLocation = Path.Combine(_hostingEnvironment.WebRootPath, $"images/Logos/{localFileName}");
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "CreditApp");
                // in debug mode sometimes index is called twice gets an error that file is in use 
                try
                {
                    client.DownloadFile(retailerInfo.DistributorLogoURL, localFileLocation);
                }
                catch (Exception)
                {
                    //throw;
                }
                    
                viewModel.LocalLogo = localFileLocation;
            }
        }

        private void FillCreditDataFromRetailerInfo(CreditAppModel viewModel, RetailerInfo retailerInfo, TokenInfo tokenInfo)
        {

            viewModel.CreditData = new CreditData();
            viewModel.CreditData.DistributorId = retailerInfo.DistributorId.ToString();
            viewModel.CreditData.RetailerId = Convert.ToInt32(tokenInfo.UserID);
            viewModel.CreditData.Token = tokenInfo.Token;
            viewModel.CreditData.BusinessName = retailerInfo.Business_Information_BusinessName;
            viewModel.CreditData.TradeName = retailerInfo.Business_Information_TradeName;
            viewModel.CreditData.LicenseNumber = retailerInfo.Business_Information_LicenseNumber;
            viewModel.CreditData.LicenseExpirationDate = retailerInfo.Business_Information_LicenseExpirationDate;
            viewModel.CreditData.EIN = retailerInfo.Business_Information_EINunmber;
            viewModel.CreditData.NYStateTaxId = retailerInfo.Business_Information_StateTaxId;
            viewModel.CreditData.DeliveryTime = retailerInfo.Business_Information_DeliveryTime;
            viewModel.CreditData.CompanyType = retailerInfo.Business_Information_CompanyType;
            viewModel.CreditData.Phone = retailerInfo.Business_Information_Phone;
            viewModel.CreditData.Address1 = retailerInfo.Business_Information_Address1;
            viewModel.CreditData.Address2 = retailerInfo.Business_Information_Address2;
            viewModel.CreditData.City = retailerInfo.Business_Information_City;

            viewModel.CreditData.State = retailerInfo.Business_Information_State;
            viewModel.CreditData.ZipCode = retailerInfo.Business_Information_Zip;
            // principal
            viewModel.CreditData.PrincipalName = retailerInfo.Principal_Contact_Name;
            viewModel.CreditData.PrincipalTitle = retailerInfo.Principal_Contact_Title;
            viewModel.CreditData.PrincipalEmail = retailerInfo.Principal_Contact_Email;
            viewModel.CreditData.PrincipalPhone = retailerInfo.Principal_Contact_Phone;
            viewModel.CreditData.PrincipalSSN = retailerInfo.Principal_Contact_SSN;
            viewModel.CreditData.PrincipalAddress1 = retailerInfo.Principal_Contact_Address1;
            viewModel.CreditData.PrincipalAddress2 = retailerInfo.Principal_Contact_Address2;
            viewModel.CreditData.PrincipalCity = retailerInfo.Principal_Contact_City;
            
            viewModel.CreditData.PrincipalState = retailerInfo.Principal_Contact_State;
            viewModel.CreditData.PrincipalZipCode = retailerInfo.Principal_Contact_Zip;
            // property owned
            bool.TryParse(retailerInfo.Property_Location_Do_You_Own_The_Property, out var propOwned);
            viewModel.CreditData.PropertyOwned = propOwned;
            viewModel.CreditData.PropertyType = retailerInfo.Property_Location_Property_Type;
            viewModel.CreditData.PropertyAddress1 = retailerInfo.Property_Location_Contact_Address1;
            viewModel.CreditData.PropertyAddress2 = retailerInfo.Property_Location_Contact_Address2;
            viewModel.CreditData.PropertyCity = retailerInfo.Property_Location_Contact_City;
            
            viewModel.CreditData.PropertyState = retailerInfo.Property_Location_Contact_State;
            viewModel.CreditData.PropertyZipCode = retailerInfo.Property_Location_Contact_Zip;
            // prior business
            bool.TryParse(retailerInfo.Prior_Business_Location_Have_You_Done_Business_With, out var businessBefore);
            viewModel.CreditData.PriorBusiness = businessBefore;
            viewModel.CreditData.PriorBusinessAddress1 = retailerInfo.Prior_Business_Location_Contact_Address1;
            viewModel.CreditData.PriorBusinessAddress2 = retailerInfo.Prior_Business_Location_Contact_Address2;
            viewModel.CreditData.PriorBusinessCity = retailerInfo.Prior_Business_Location_Contact_City;

            viewModel.CreditData.PriorBusinessState = retailerInfo.Prior_Business_Location_Contact_State;
            viewModel.CreditData.PriorBusinessZipCode = retailerInfo.Prior_Business_Location_Contact_Zip;
            // billing contact
            viewModel.CreditData.BillingContactName = retailerInfo.Billing_Contact_Name;
            viewModel.CreditData.BillingContactEmail = retailerInfo.Billing_Contact_Email;
            viewModel.CreditData.BillingContactPhone = retailerInfo.Billing_Contact_Phone;
            viewModel.CreditData.BillingContactAddress1 = retailerInfo.Billing_Contact_Address1;
            viewModel.CreditData.BillingContactAddress2 = retailerInfo.Billing_Contact_Address2;
            viewModel.CreditData.BillingContactCity = retailerInfo.Billing_Contact_City;
           
            viewModel.CreditData.BillingContactState = retailerInfo.Billing_Contact_State;
            viewModel.CreditData.BillingContactZipCode = retailerInfo.Billing_Contact_Zip;
            //bank reference
            viewModel.CreditData.BankReferenceName = retailerInfo.Banc_Reference_Name;
            viewModel.CreditData.BankReferencePhone = retailerInfo.Banc_Reference_Phone;
            viewModel.CreditData.BankReferenceAccountType = retailerInfo.Banc_Reference_AccountType;
            viewModel.CreditData.BankReferenceAccountNumber = retailerInfo.Banc_Reference_AccountNumber;
            viewModel.CreditData.BankReferenceRoutingNumber = retailerInfo.Banc_Reference_RountingNumber;
            viewModel.CreditData.BankReferenceAddress1 = retailerInfo.Banc_Reference_Address1;
            viewModel.CreditData.BankReferenceAddress2 = retailerInfo.Banc_Reference_Address2;
            viewModel.CreditData.BankReferenceCity = retailerInfo.Banc_Reference_City;
           
            viewModel.CreditData.BankReferenceState = retailerInfo.Banc_Reference_State;
            viewModel.CreditData.BankReferenceZipCode = retailerInfo.Banc_Reference_Zip;
            // tradereference 1
            viewModel.CreditData.TradeReference1Name = retailerInfo.Trade_Reference1_Name;
            viewModel.CreditData.TradeReference1Phone = retailerInfo.Trade_Reference1_Phone;
            viewModel.CreditData.TradeReference1AccountNumber = "";
            viewModel.CreditData.TradeReference1Address1 = retailerInfo.Trade_Reference1_Address1;
            viewModel.CreditData.TradeReference1Address2 = retailerInfo.Trade_Reference1_Address2;
            viewModel.CreditData.TradeReference1City = retailerInfo.Trade_Reference1_City;
            viewModel.CreditData.TradeReference1State = retailerInfo.Trade_Reference1_State;
            viewModel.CreditData.TradeReference1ZipCode = retailerInfo.Trade_Reference1_Zip;
            // tradereference 2
            viewModel.CreditData.TradeReference2Name = retailerInfo.Trade_Reference2_Name;
            viewModel.CreditData.TradeReference2Phone = retailerInfo.Trade_Reference2_Phone;
            viewModel.CreditData.TradeReference2AccountNumber = "";
            viewModel.CreditData.TradeReference2Address1 = retailerInfo.Trade_Reference2_Address1;
            viewModel.CreditData.TradeReference2Address2 = retailerInfo.Trade_Reference2_Address2;
            viewModel.CreditData.TradeReference2City = retailerInfo.Trade_Reference2_City;
           
            viewModel.CreditData.TradeReference2State = retailerInfo.Trade_Reference2_State;
            viewModel.CreditData.TradeReference2ZipCode = retailerInfo.Trade_Reference2_Zip;
        }

        private void FillFromRetailerInfo2(CreditAppModel viewModel, RetailerInfo retailerInfo, string userId)
        {

            viewModel.Distributor.DistributorId = retailerInfo.DistributorId.ToString();
            viewModel.Distributor.DistributorName = retailerInfo.DistributorName;
            viewModel.Distributor.DistributorAddress = retailerInfo.DistributorAddress;
            viewModel.Distributor.DistributorCity = retailerInfo.DistributorCity;
            viewModel.Distributor.DistributorState = retailerInfo.DistributorState;
            viewModel.Distributor.DistributorZip = retailerInfo.DistributorZip;
            viewModel.Distributor.DistributorPhone = retailerInfo.DistributorPhone;
            viewModel.Distributor.DistributorWebSiteURL = retailerInfo.DistributorWebSiteURL;

            //RetailerBo retailer = new RetailerBo();
            // business
            //retailerInfo.DistributorId
            viewModel.CreditData.RetailerId = Convert.ToInt32(userId);
            viewModel.CreditData.BusinessName = retailerInfo.Business_Information_BusinessName;
            viewModel.CreditData.TradeName = retailerInfo.Business_Information_TradeName;
            viewModel.CreditData.LicenseNumber = retailerInfo.Business_Information_LicenseNumber;
            viewModel.CreditData.LicenseExpirationDate = retailerInfo.Business_Information_LicenseExpirationDate;
            viewModel.CreditData.EIN = retailerInfo.Business_Information_EINunmber;
            viewModel.CreditData.NYStateTaxId = retailerInfo.Business_Information_StateTaxId;
            viewModel.CreditData.DeliveryTime = retailerInfo.Business_Information_DeliveryTime;
            viewModel.CreditData.CompanyType = retailerInfo.Business_Information_CompanyType;
            viewModel.CreditData.Phone = retailerInfo.Business_Information_Phone;
            viewModel.CreditData.Address1 = retailerInfo.Business_Information_Address1;
            viewModel.CreditData.Address2 = retailerInfo.Business_Information_Address2;
            viewModel.CreditData.City = retailerInfo.Business_Information_City;
            //var businessState = new StatesBoService().GetByAbbreviation(retailerInfo.Business_Information_State);
            //if (businessState == null)
            //    viewModel.CreditData.State = "";
            //else
            //    viewModel.CreditData.State = businessState.Abbreviation;
            viewModel.CreditData.State = retailerInfo.Business_Information_State;
            viewModel.CreditData.ZipCode = retailerInfo.Business_Information_Zip;
            // principal
            viewModel.CreditData.PrincipalName = retailerInfo.Principal_Contact_Name;
            viewModel.CreditData.PrincipalTitle = retailerInfo.Principal_Contact_Title;
            viewModel.CreditData.PrincipalEmail = retailerInfo.Principal_Contact_Email;
            viewModel.CreditData.PrincipalPhone = retailerInfo.Principal_Contact_Phone;
            viewModel.CreditData.PrincipalSSN = retailerInfo.Principal_Contact_SSN;
            viewModel.CreditData.PrincipalAddress1 = retailerInfo.Principal_Contact_Address1;
            viewModel.CreditData.PrincipalAddress2 = retailerInfo.Principal_Contact_Address2;
            viewModel.CreditData.PrincipalCity = retailerInfo.Principal_Contact_City;
            //var principalState = new StatesBoService().GetByAbbreviation(retailerInfo.Principal_Contact_State);
            //if (principalState == null)
            //    viewModel.CreditData.PrincipalState = "";
            //else
            //    viewModel.CreditData.State = principalState.Abbreviation;
            viewModel.CreditData.PrincipalState = retailerInfo.Principal_Contact_State;
            viewModel.CreditData.PrincipalZipCode = retailerInfo.Principal_Contact_Zip;
            // property owned
            bool.TryParse(retailerInfo.Property_Location_Do_You_Own_The_Property, out var propOwned);
            viewModel.CreditData.PropertyOwned = propOwned;
            viewModel.CreditData.PropertyType = retailerInfo.Property_Location_Property_Type;
            viewModel.CreditData.PropertyAddress1 = retailerInfo.Property_Location_Contact_Address1;
            viewModel.CreditData.PropertyAddress2 = retailerInfo.Property_Location_Contact_Address2;
            viewModel.CreditData.PropertyCity = retailerInfo.Property_Location_Contact_City;
            //var propertyState = new StatesBoService().GetByAbbreviation(retailerInfo.Property_Location_Contact_State);
            //if (propertyState == null)
            //    viewModel.CreditData.PropertyState = "";
            //else
            //    viewModel.CreditData.PropertyState = propertyState.Abbreviation;
            viewModel.CreditData.PropertyState = retailerInfo.Property_Location_Contact_State;
            viewModel.CreditData.PropertyZipCode = retailerInfo.Property_Location_Contact_Zip;
            // prior business
            bool.TryParse(retailerInfo.Prior_Business_Location_Have_You_Done_Business_With, out var businessBefore);
            viewModel.CreditData.PriorBusiness = businessBefore;
            viewModel.CreditData.PriorBusinessAddress1 = retailerInfo.Prior_Business_Location_Contact_Address1;
            viewModel.CreditData.PriorBusinessAddress2 = retailerInfo.Prior_Business_Location_Contact_Address2;
            viewModel.CreditData.PriorBusinessCity = retailerInfo.Prior_Business_Location_Contact_City;
            //var priorBusinessState = new StatesBoService().GetByAbbreviation(retailerInfo.Prior_Business_Location_Contact_State);
            //if (priorBusinessState == null)
            //    viewModel.CreditData.PriorBusinessState = "";
            //else
            //    viewModel.CreditData.PriorBusinessState = priorBusinessState.Abbreviation;
            viewModel.CreditData.PriorBusinessState = retailerInfo.Prior_Business_Location_Contact_State;
            viewModel.CreditData.PriorBusinessZipCode = retailerInfo.Prior_Business_Location_Contact_Zip;
            // billing contact
            viewModel.CreditData.BillingContactName = retailerInfo.Billing_Contact_Name;
            viewModel.CreditData.BillingContactEmail = retailerInfo.Billing_Contact_Email;
            viewModel.CreditData.BillingContactPhone = retailerInfo.Billing_Contact_Phone;
            viewModel.CreditData.BillingContactAddress1 = retailerInfo.Billing_Contact_Address1;
            viewModel.CreditData.BillingContactAddress2 = retailerInfo.Billing_Contact_Address2;
            viewModel.CreditData.BillingContactCity = retailerInfo.Billing_Contact_City;
            //var billingContactState = new StatesBoService().GetByAbbreviation(retailerInfo.Billing_Contact_State);
            //if (billingContactState == null)
            //    viewModel.CreditData.BillingContactState = "";
            //else
            //    viewModel.CreditData.BillingContactState = billingContactState.Abbreviation;
            viewModel.CreditData.BillingContactState = retailerInfo.Billing_Contact_State;
            viewModel.CreditData.BillingContactZipCode = retailerInfo.Billing_Contact_Zip;
            //bank reference
            viewModel.CreditData.BankReferenceName = retailerInfo.Banc_Reference_Name;
            viewModel.CreditData.BankReferencePhone = retailerInfo.Banc_Reference_Phone;
            viewModel.CreditData.BankReferenceAccountType = retailerInfo.Banc_Reference_AccountType;
            viewModel.CreditData.BankReferenceAccountNumber = retailerInfo.Banc_Reference_AccountNumber;
            viewModel.CreditData.BankReferenceRoutingNumber = retailerInfo.Banc_Reference_RountingNumber;
            viewModel.CreditData.BankReferenceAddress1 = retailerInfo.Banc_Reference_Address1;
            viewModel.CreditData.BankReferenceAddress2 = retailerInfo.Banc_Reference_Address2;
            viewModel.CreditData.BankReferenceCity = retailerInfo.Banc_Reference_City;
            //var bankReferenceState = new StatesBoService().GetByAbbreviation(retailerInfo.Banc_Reference_State);
            //if (bankReferenceState == null)
            //    viewModel.CreditData.BankReferenceState = "";
            //else
            //    viewModel.CreditData.BankReferenceState = bankReferenceState.Abbreviation;
            viewModel.CreditData.BankReferenceState = retailerInfo.Banc_Reference_State;
            viewModel.CreditData.BankReferenceZipCode = retailerInfo.Banc_Reference_Zip;
            // tradereference 1
            viewModel.CreditData.TradeReference1Name = retailerInfo.Trade_Reference1_Name;
            viewModel.CreditData.TradeReference1Phone = retailerInfo.Trade_Reference1_Phone;
            viewModel.CreditData.TradeReference1AccountNumber = "";
            viewModel.CreditData.TradeReference1Address1 = retailerInfo.Trade_Reference1_Address1;
            viewModel.CreditData.TradeReference1Address2 = retailerInfo.Trade_Reference1_Address2;
            viewModel.CreditData.TradeReference1City = retailerInfo.Trade_Reference1_City;
            //var tradeReference1State = new StatesBoService().GetByAbbreviation(retailerInfo.Trade_Reference1_State);
            //if (tradeReference1State == null)
            //    viewModel.CreditData.TradeReference1State = "";
            //else
            //    viewModel.CreditData.TradeReference1State = tradeReference1State.Abbreviation;
            viewModel.CreditData.TradeReference1State = retailerInfo.Trade_Reference1_State;
            viewModel.CreditData.TradeReference1ZipCode = retailerInfo.Trade_Reference1_Zip;
            // tradereference 1
            viewModel.CreditData.TradeReference2Name = retailerInfo.Trade_Reference2_Name;
            viewModel.CreditData.TradeReference2Phone = retailerInfo.Trade_Reference2_Phone;
            viewModel.CreditData.TradeReference2AccountNumber = "";
            viewModel.CreditData.TradeReference2Address1 = retailerInfo.Trade_Reference2_Address1;
            viewModel.CreditData.TradeReference2Address2 = retailerInfo.Trade_Reference2_Address2;
            viewModel.CreditData.TradeReference2City = retailerInfo.Trade_Reference2_City;
            //var tradeReference2State = new StatesBoService().GetByAbbreviation(retailerInfo.Trade_Reference2_State);
            //if (tradeReference2State == null)
            //    viewModel.CreditData.TradeReference2State = "";
            //else
            //    viewModel.CreditData.TradeReference2State = tradeReference2State.Abbreviation;
            viewModel.CreditData.TradeReference2State = retailerInfo.Trade_Reference2_State;
            viewModel.CreditData.TradeReference2ZipCode = retailerInfo.Trade_Reference2_Zip;
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
        public IActionResult GeneratePdf(CreditAppModel model, IFormFile fileUploadCertificate, IFormFile fileUploadLicense)
        {
            // to force updating the view with current viewModel values
            // we need to preserve the error messages, clear modelState and the add the errors back.
            Dictionary<string, string> previousErrors = new Dictionary<string, string>();

            foreach (KeyValuePair<string, ModelStateEntry> modelStateItem in ModelState)
            {
                if (modelStateItem.Value.Errors.Any())
                {
                    previousErrors.Add(modelStateItem.Key, modelStateItem.Value.Errors[0].ErrorMessage);
                }
            }
            ModelState.Clear();
            //ModelState.Remove("CreditData.Id");

            CreditDataEntity creditDataEntity;
            UpdateWithNulls(model);
            //update DB
            using (var context = new CreditAppContext())
            {
                creditDataEntity = _mapper.Map<CreditDataEntity>(model.CreditData);
                if (creditDataEntity.Id.HasValue)
                {
                    context.Update(creditDataEntity);
                }
                else
                {
                    //creditDataEntity.Status = CreditAppStatusEnum.CREATED.ToString();
                    context.Add(creditDataEntity);
                }

                var distributorEntity = _mapper.Map<DistributorEntity>(model.Distributor);
                // additional check 
                var existingDistributor = context.Distributors.Any(x => x.DistributorId == model.CreditData.DistributorId);

                if (existingDistributor)
                {
                    context.Update(distributorEntity);
                }
                else
                {
                    context.Add(distributorEntity);
                }
                context.SaveChanges();

                model.CreditData.Id = creditDataEntity.Id;
                model.Distributor.Id = distributorEntity.Id;
                var hasUploadedFiles = false;

                var licenseCreditDataFilesEntity = context.CreditDataFiles.FirstOrDefault(x => x.CreditDataId == creditDataEntity.Id.Value) ??
                                                   new CreditDataFilesEntity();
                if (creditDataEntity.Id.HasValue)
                {
                    licenseCreditDataFilesEntity.CreditDataId = creditDataEntity.Id.Value;
                }
                if (fileUploadLicense != null)
                {
                    hasUploadedFiles = true;
                    using (var memoryStream = new MemoryStream())
                    {
                        fileUploadLicense.CopyToAsync(memoryStream);
                        var licenseFileContent = memoryStream.ToArray();
                        var licenseFileName = fileUploadLicense.FileName;
                        licenseCreditDataFilesEntity.LicenseFile = licenseFileContent;
                        licenseCreditDataFilesEntity.LicenseFileName = licenseFileName;
                        licenseCreditDataFilesEntity.LastUpdateLicense = DateTime.Now;
                    }
                }

                if (fileUploadCertificate != null)
                {
                    hasUploadedFiles = true;
                    using (var memoryStream = new MemoryStream())
                    {
                        fileUploadCertificate.CopyToAsync(memoryStream);
                        var certificateFileContent = memoryStream.ToArray();
                        var certificateFileName = fileUploadCertificate.FileName;
                        licenseCreditDataFilesEntity.TaxCertificateFile = certificateFileContent;
                        licenseCreditDataFilesEntity.TaxCertificateFileName = certificateFileName;
                        licenseCreditDataFilesEntity.LastUpdateCertificate = DateTime.Now;
                    }
                }

                if (hasUploadedFiles)
                {
                    if (!licenseCreditDataFilesEntity.Id.HasValue)
                        context.Add(licenseCreditDataFilesEntity);
                    else
                        context.Update(licenseCreditDataFilesEntity);
                    context.SaveChanges();

                }
                model.CreditDataFiles = _mapper.Map<CreditDataFiles>(licenseCreditDataFilesEntity);
                // check files

                ValidateCreditData(model.CreditData, previousErrors);
                ValidateCreditDataFiles(model.CreditDataFiles);
                foreach (KeyValuePair<string, string> previousError in previousErrors)
                {
                    ModelState.AddModelError(previousError.Key, previousError.Value);
                }
                if (ModelState.IsValid)
                {
                    var templateLocation = Path.Combine(_hostingEnvironment.WebRootPath, $"PdfTemplate/BMG_Credit_Application_Form_BLANK.pdf");
                    var fontPath = Path.Combine(_hostingEnvironment.WebRootPath, $"PdfTemplate/48920384.ttf");
                    var fileName = $"D{model.CreditData.DistributorId}_R{model.CreditData.RetailerId}_Document.pdf";
                    var outputPath = Path.Combine(_hostingEnvironment.WebRootPath, $"Pdfs/{fileName}");
                    PdfGenerator pdfGenerator = new PdfGenerator(model);
                    var fileGenerated = pdfGenerator.GeneratePdf(templateLocation, fontPath, outputPath);

                    if (fileGenerated)
                    {
                        creditDataEntity.Status = CreditAppStatusEnum.SENT_FOR_SIGNATURE.ToString();//"PdfGenerated";
                        
                        byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                        AdobeSignWS ws = new AdobeSignWS();

                    
                        var resp = ws.SendDocumentForSignature(fileBytes, fileName, creditDataEntity.Id.Value,
                            model.CreditData.PrincipalEmail);
                        if (resp != null && !string.IsNullOrWhiteSpace(resp.agreementId))
                        {
                            creditDataEntity.AdobeSignAgreementId = resp.agreementId;
                            creditDataEntity.SigningUrl = resp.signingUrl;
                            context.Update(creditDataEntity);
                            context.SaveChanges();
                            if (!string.IsNullOrWhiteSpace(resp.signingUrl))
                                return Redirect(resp.signingUrl);
                        }

                        return View("NotAvailableView",model.Distributor);
                        //model.StatesListItems = GetStatesListItems();
                        //return View("Index", model);
                    }
                }
            }

            model.StatesListItems = GetStatesListItems();

            return View("Index", model);
        }

        private void UpdateWithNulls(CreditAppModel model)
        {
            foreach (var propertyInfo in model.CreditData.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any()))
                {
                if (propertyInfo.GetValue(model.CreditData)!=null && propertyInfo.GetValue(model.CreditData).ToString() == "--")
                {
                    propertyInfo.SetValue(model.CreditData, null);
                }
            }

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

        private List<SelectListItem> GetStatesListItems()
        {
            List<SelectListItem> returnStates;
            using (var context = new CreditAppContext())
            {
                var statesEntity = context.States.ToList();
                var states = _mapper.Map<List<States>>(statesEntity);


                returnStates = states.Select(x => new SelectListItem() { Value= x.Abbreviation, Text = x.State}).ToList();
                
            }
            return returnStates;
        }

        private List<States> GetStates()
        {
            List<States> states;
            
            using (var context = new CreditAppContext())
            {
                var statesEntity = context.States.ToList();
                states = _mapper.Map<List<States>>(statesEntity);
            }
            return states;
        }

        private void ValidateCreditDataFiles(CreditDataFiles creditDataFilesModel)
        {
            if (creditDataFilesModel==null || String.IsNullOrWhiteSpace(creditDataFilesModel.LicenseFileName))
            {
                ModelState.AddModelError("CreditDataFiles.LicenseFileName", "Missing License File");
            }

            if (creditDataFilesModel == null || String.IsNullOrWhiteSpace(creditDataFilesModel.TaxCertificateFileName))
            {
                ModelState.AddModelError("CreditDataFiles.TaxCertificateFileName", "Missing Certificate File");
            }

        }

        private void ValidateCreditData(CreditData creditDataModel, Dictionary<string, string> previousErrors)
        {
            string errorMessage;

            //ValidateLicenseExpirationDate
            errorMessage = ValidateLicenseExpirationDate(creditDataModel.LicenseExpirationDate);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("creditData.LicenseExpirationDate", errorMessage);

            errorMessage = ValidateZipCode(creditDataModel.ZipCode, creditDataModel.State);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.ZipCode",errorMessage );

            errorMessage = ValidateZipCode(creditDataModel.BankReferenceZipCode, creditDataModel.BankReferenceState);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.BankReferenceZipCode", errorMessage);

            errorMessage = ValidateZipCode(creditDataModel.BillingContactZipCode, creditDataModel.BillingContactState);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.BillingContactZipCode", errorMessage);

            errorMessage = ValidateZipCode(creditDataModel.PrincipalZipCode, creditDataModel.PrincipalState);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.PrincipalZipCode", errorMessage);

            if (creditDataModel.PropertyOwned)
            {
                if (creditDataModel.PropertyType == "--" || creditDataModel.PropertyType == null)
                {
                    ModelState.AddModelError("CreditData.PropertyType", "Please select property type");
                }

                if (creditDataModel.PropertyState == "--" || creditDataModel.PropertyState == null)
                {
                    ModelState.AddModelError("CreditData.PropertyState", "Please select state");
                }

                errorMessage = ValidateZipCode(creditDataModel.PropertyZipCode, creditDataModel.PropertyState);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    ModelState.AddModelError("CreditData.PropertyZipCode", errorMessage);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(creditDataModel.PropertyAddress1))
                {
                    //ModelState.AddModelError("CreditData.PropertyAddress1", string.Empty);
                    ModelState.Remove("CreditData.PropertyAddress1");
                    previousErrors.Remove("CreditData.PropertyAddress1");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PropertyCity))
                {
                    //ModelState.AddModelError("CreditData.PropertyCity", string.Empty);
                    ModelState.Remove("CreditData.PropertyCity");
                    previousErrors.Remove("CreditData.PropertyCity");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PropertyState))
                {
                    //ModelState.AddModelError("CreditData.PropertyState", string.Empty);
                    ModelState.Remove("CreditData.PropertyState");
                    previousErrors.Remove("CreditData.PropertyState");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PropertyZipCode))
                {
                    //ModelState.AddModelError("CreditData.PropertyZipCode", string.Empty);
                    ModelState.Remove("CreditData.PropertyZipCode");
                    previousErrors.Remove("CreditData.PropertyZipCode");
                }
            }

            if (creditDataModel.PriorBusiness)
            {
                errorMessage = ValidateZipCode(creditDataModel.PriorBusinessZipCode, creditDataModel.PriorBusinessState);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    ModelState.AddModelError("CreditData.PriorBusinessZipCode", errorMessage);

                if (creditDataModel.PriorBusinessState == "--" || creditDataModel.PriorBusinessState == null)
                {
                    ModelState.AddModelError("CreditData.PriorBusinessState", "Please select state");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(creditDataModel.PriorBusinessAddress1))
                {
                    //ModelState.AddModelError("CreditData.PriorBusinessAddress1", "Required field");
                    ModelState.Remove("CreditData.PriorBusinessAddress1");
                    previousErrors.Remove("CreditData.PriorBusinessAddress1");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PriorBusinessCity))
                {
                    //ModelState.AddModelError("CreditData.PriorBusinessCity", string.Empty);
                    ModelState.Remove("CreditData.PriorBusinessCity");
                    previousErrors.Remove("CreditData.PriorBusinessCity");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PriorBusinessState))
                {
                    //ModelState.AddModelError("CreditData.PriorBusinessState", string.Empty);
                    ModelState.Remove("CreditData.PriorBusinessState");
                    previousErrors.Remove("CreditData.PriorBusinessState");
                }

                if (string.IsNullOrWhiteSpace(creditDataModel.PriorBusinessZipCode))
                {
                    //ModelState.AddModelError("CreditData.PriorBusinessZipCode", string.Empty);
                    ModelState.Remove("CreditData.PriorBusinessZipCode");
                    previousErrors.Remove("CreditData.PriorBusinessZipCode");
                }
            }


            ModelState.Remove("CreditData.TradeReference1ZipCode");
            errorMessage = ValidateZipCode(creditDataModel.TradeReference1ZipCode, creditDataModel.TradeReference1State);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.TradeReference1ZipCode", errorMessage);

            ModelState.Remove("CreditData.TradeReference2ZipCode");
            errorMessage = ValidateZipCode(creditDataModel.TradeReference2ZipCode, creditDataModel.TradeReference2State);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.TradeReference2ZipCode", errorMessage);


            //-----------------
            if (creditDataModel.CompanyType == "--" || creditDataModel.CompanyType == null)
            {
                ModelState.AddModelError("CreditData.CompanyType", "Please select company type");
            }

            if (creditDataModel.State == "--" || creditDataModel.State == null)
            {
                ModelState.AddModelError("CreditData.State", "Please select state");
            }

            if (creditDataModel.BillingContactState == "--" || creditDataModel.BillingContactState == null)
            {
                ModelState.AddModelError("CreditData.BillingContactState", "Please select state");
            }

            if (creditDataModel.PrincipalState == "--" || creditDataModel.PrincipalState == null)
            {
                ModelState.AddModelError("CreditData.PrincipalState", "Please select state");
            }

            if (creditDataModel.BankReferenceState == "--" || creditDataModel.BankReferenceState == null)
            {
                ModelState.AddModelError("CreditData.BankReferenceState", "Please select state");
            }

            if (creditDataModel.BankReferenceAccountType == "--" || creditDataModel.BankReferenceAccountType == null)
            {
                ModelState.AddModelError("CreditData.BankReferenceAccountType", "Please select state");
            }

        }

        private string ValidateLicenseExpirationDate(DateTime licenseDate)
        {
            string errorMessage = string.Empty;

            if (licenseDate <= DateTime.Today)
            {
                errorMessage = "License expired";
            }
            else if (licenseDate <= DateTime.Today.AddDays(30))
            {
                errorMessage = "Cannot accept license expiration within 30 days";
            }

            return string.IsNullOrWhiteSpace(errorMessage) ? null : errorMessage;
        }

        private string ValidateZipCode(string zipCode, string state)
        {
            bool retVal = true;
            if (!string.IsNullOrEmpty(zipCode) && !string.IsNullOrEmpty(state))
            {
                using (var context = new CreditAppContext())
                {
                    retVal = context.USZipCodes.Any(x => x.State == state && x.ZipCode == int.Parse(zipCode));
                }
            }
            return retVal ? null : "Wrong ZIP for state";
        }

        [HttpPost]
        public void ValidateZipCodeServer2([FromBody]string model, [FromBody] string propName)
        {
        }

        [HttpPost]
        public void ValidateZipCodeServer([FromQuery]string propName, [FromBody]CreditAppModel model )
        {
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF2(IFormFile file)
        {
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok();
        }

        private CreditAppModel GetInitialValues(string token)
        {
            CreditAppModel viewModel = new CreditAppModel();
            viewModel.Token = token;
            //viewModel.CreditData = new CreditData();
            //viewModel.Distributor = new Distributor();
            viewModel.Retailer = new Retailer();
            //read data from database
            //CreditData creditData = null;
            //int? distributorId = null;
            viewModel.Distributor = new Distributor();
            viewModel.CreditData = new CreditData();
            viewModel.CreditDataFiles = new CreditDataFiles();

            if (!string.IsNullOrWhiteSpace(token))
            {
               TokenInfo tokenInfo = VerifyToken(token, out string tokenErrorMessage);
                
                if (string.IsNullOrWhiteSpace(tokenErrorMessage))
                {
                    RetailerInfo retailerInfo = GetRetailerInfo(tokenInfo, out string retailerErrorMessage);
                    if (string.IsNullOrWhiteSpace(retailerErrorMessage))
                    {
                        using (var context = new CreditAppContext())
                        {
                            var creditDataEntity = context.CreditData.SingleOrDefault(x =>
                                x.RetailerId == Convert.ToInt32(tokenInfo.UserID) &&
                                x.DistributorId.ToString() == tokenInfo.DistribuitorID);
                            if (creditDataEntity == null)
                            {
                                creditDataEntity = context.CreditData.SingleOrDefault(x =>
                                    x.RetailerId == Convert.ToInt32(tokenInfo.UserID));
                                if (creditDataEntity != null)
                                {
                                    var creditDataFiles =
                                        context.CreditDataFiles.SingleOrDefault(x =>
                                            x.CreditDataId == creditDataEntity.Id);
                                    if (creditDataFiles != null)
                                        viewModel.CreditDataFiles = _mapper.Map<CreditDataFiles>(creditDataFiles);
                                }

                                //this.FillRetailerInfoOnly();
                            }
                            else
                            {
                                viewModel.CreditData = _mapper.Map<CreditData>(creditDataEntity);
                                var creditDataFiles =
                                    context.CreditDataFiles.FirstOrDefault(x => x.CreditDataId == creditDataEntity.Id);
                                if (creditDataFiles != null)
                                {
                                    viewModel.CreditDataFiles = _mapper.Map<CreditDataFiles>(creditDataFiles);
                                }
                            }

                            var distributorEntity =
                                context.Distributors.SingleOrDefault(x => x.DistributorId == tokenInfo.DistribuitorID);
                            if (distributorEntity != null)
                                viewModel.Distributor = _mapper.Map<Distributor>(distributorEntity);

                            if (creditDataEntity == null)
                            {
                                FillCreditDataFromRetailerInfo(viewModel, retailerInfo, tokenInfo);
                                viewModel.CreditData.Status= CreditAppStatusEnum.CREATED.ToString();
                                //creditDataEntity.Status= CreditAppStatusEnum.CREATED.ToString();
                            }

                            //if (!string.IsNullOrWhiteSpace(creditDataEntity.SigningUrl))
                            //{
                            //    return Redirect(creditDataEntity.SigningUrl);
                            //}
                        }

                        FillDistributorFromRetailerInfo(viewModel, retailerInfo);
                    }
                }
            }

            viewModel.StatesListItems = GetStatesListItems();
            viewModel.States = GetStates();
            return viewModel;
        }

        [HttpPost]
        public IActionResult ClearForm([FromBody]CreditAppModel model)
        {
            if (model.CreditData.Id.HasValue)
            {
                //delete reccord
                using (var context = new CreditAppContext())
                {
                    CreditDataEntity creditDataEntity = new CreditDataEntity() { Id = model.CreditData.Id };
                    if (creditDataEntity != null)
                    {
                        context.CreditData.Attach(creditDataEntity);
                        context.CreditData.Remove(creditDataEntity);
                        context.SaveChanges();
                    }
                }

            }
            var viewModel = GetInitialValues(model.Token);

            return View("Index", viewModel);
        }

        [HttpGet]
        [Route("Home/DownloadFile/{creditDataId}/{fileType}")]
        public FileResult DownloadFile(int creditDataId, string fileType)
        {
            byte[] bytes;
            string fileName;
            using (var context = new CreditAppContext())
            {
                var crFile = context.CreditDataFiles.FirstOrDefault(x => x.CreditDataId == creditDataId);
                if (fileType.ToLower() == "licensefile")
                {
                    bytes = crFile.LicenseFile;
                    fileName = crFile.LicenseFileName;
                }
                else
                {
                    bytes = crFile.TaxCertificateFile;
                    fileName = crFile.TaxCertificateFileName;
                }
            }
            return File(bytes, "application/octet-stream", fileName);

        }
    }
}
