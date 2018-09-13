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
using CreditAppBMG.Entities;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using CreditAppBMG.Pdf;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Reflection;

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
            
            CreditAppModel viewModel = new CreditAppModel();
            //viewModel.CreditData = new CreditData();
            //viewModel.Distributor = new Distributor();
            viewModel.Retailer = new Retailer();
            //read data from database
            //CreditData creditData = null;
            int? distributorId = null;
            viewModel.Distributor = new Distributor();
            viewModel.CreditData = new CreditData();
            viewModel.CreditDataFiles = new CreditDataFiles();
            if (!string.IsNullOrWhiteSpace(token))
            {
                TokenInfo tokenInfo = VerifyToken(token, out string tokenErrorMessage);
                RetailerInfo retailerInfo = GetRetailerInfo(tokenInfo, out string retailerErrorMessage);

                using (var context = new CreditAppContext())
                {
                    var creditDataEntity = context.CreditData.SingleOrDefault(x => x.RetailerId == tokenInfo.UserID && x.DistributorId.ToString()==tokenInfo.DistribuitorID);
                    if (creditDataEntity == null)
                    {
                        creditDataEntity = context.CreditData.SingleOrDefault(x => x.RetailerId == tokenInfo.UserID);
                        var creditDataFiles = context.CreditDataFiles.SingleOrDefault(x => x.CreditDataId == creditDataEntity.Id);
                        if (creditDataFiles != null)
                            viewModel.CreditDataFiles = _mapper.Map<CreditDataFiles>(creditDataFiles);
                        //this.FillRetailerInfoOnly();
                    }
                    else {
                        viewModel.CreditData = _mapper.Map<CreditData>(creditDataEntity);
                    }
                    
                    var distributorEntity = context.Distributors.SingleOrDefault(x => x.DistributorId.ToString() == tokenInfo.DistribuitorID);
                    if (distributorEntity != null)
                        viewModel.Distributor =_mapper.Map<Distributor>(distributorEntity);
                }

                if (viewModel.CreditData == null)
                {
                    FillCreditDataFromRetailerInfo(viewModel, retailerInfo, tokenInfo);
                }

                
                FillDistributorFromRetailerInfo(viewModel, retailerInfo, tokenInfo);
            }
            
            viewModel.StatesListItems = this.GetStatesListItems();
            viewModel.States = this.GetStates();

            return View(viewModel);
        }

        private void FillDistributorFromRetailerInfo(CreditAppModel viewModel, RetailerInfo retailerInfo, TokenInfo tokenInfo)
        {
            //viewModel.Distributor = new Distributor();
            if (viewModel.Distributor == null)
                viewModel.Distributor = new Distributor();
            viewModel.Distributor.DistributorId = retailerInfo.DistributorId;
            viewModel.Distributor.DistributorName = retailerInfo.DistributorName;
            viewModel.Distributor.DistributorAddress = retailerInfo.DistributorAddress;
            viewModel.Distributor.DistributorCity = retailerInfo.DistributorCity;
            viewModel.Distributor.DistributorState = retailerInfo.DistributorState;
            viewModel.Distributor.DistributorZip = retailerInfo.DistributorZip;
            viewModel.Distributor.DistributorPhone = retailerInfo.DistributorPhone;
            viewModel.Distributor.DistributorWebSiteURL = retailerInfo.DistributorWebSiteURL;

            string localFileName;
            // download logo:
            if (!string.IsNullOrEmpty(retailerInfo.DistributorLogoURL))
            {
                try
                {
                    localFileName = Path.GetFileName(retailerInfo.DistributorLogoURL);
                    var localFileLocation = Path.Combine(this._hostingEnvironment.WebRootPath, $"images/Logos/{localFileName}");
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", "CreditApp");
                    // in debug mode sometymes index is called twice gets an error thet file is in use 
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
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void FillCreditDataFromRetailerInfo(CreditAppModel viewModel, RetailerInfo retailerInfo, TokenInfo tokenInfo)
        {

            //RetailerBo retailer = new RetailerBo();
            // business
            //retailerInfo.DistributorId
            viewModel.CreditData = new CreditData();
            viewModel.CreditData.DistributorId = retailerInfo.DistributorId;
            viewModel.CreditData.RetailerId = tokenInfo.UserID;
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
            bool propOwned = false;
            bool.TryParse(retailerInfo.Property_Location_Do_You_Own_The_Property, out propOwned);
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
            bool businessBefore = false;
            bool.TryParse(retailerInfo.Prior_Business_Location_Have_You_Done_Business_With, out businessBefore);
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

        private void FillFromRetailerInfo2(CreditAppModel viewModel, RetailerInfo retailerInfo, string userId)
        {

            viewModel.Distributor.DistributorId = retailerInfo.DistributorId;
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
            viewModel.CreditData.RetailerId = userId;
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
            bool propOwned = false;
            bool.TryParse(retailerInfo.Property_Location_Do_You_Own_The_Property, out propOwned);
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
            bool businessBefore = false;
            bool.TryParse(retailerInfo.Prior_Business_Location_Have_You_Done_Business_With, out businessBefore);
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
        public async Task<IActionResult> GeneratePDF(CreditAppModel model, IFormFile fileUploadCertificate, IFormFile fileUploadLicense)
        {
            CreditDataEntity creditDataEntity = null;
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
                byte[] licenseFileContent = null;
                string licenseFileName = null;

                var licenseCreditDataFilesEntity = context.CreditDataFiles.FirstOrDefault(x => x.CreditDataId == creditDataEntity.Id.Value);
                if (licenseCreditDataFilesEntity == null)
                {
                    licenseCreditDataFilesEntity = new CreditDataFilesEntity();
                }
                if (creditDataEntity.Id.HasValue)
                {
                    licenseCreditDataFilesEntity.CreditDataId = creditDataEntity.Id.Value;
                }
                if (fileUploadLicense != null)
                {
                    hasUploadedFiles = true;
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileUploadLicense.CopyToAsync(memoryStream);
                        licenseFileContent = memoryStream.ToArray();
                        licenseFileName = fileUploadLicense.FileName;
                        licenseCreditDataFilesEntity.LicenseFile = licenseFileContent;
                        licenseCreditDataFilesEntity.LicenseFileName = licenseFileName;
                    }
                }

                byte[] certificateFileContent = null;
                string certificateFileName = null;
                if (fileUploadCertificate != null)
                {
                    hasUploadedFiles = true;
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileUploadCertificate.CopyToAsync(memoryStream);
                        certificateFileContent = memoryStream.ToArray();
                        certificateFileName = fileUploadCertificate.FileName;
                        licenseCreditDataFilesEntity.TaxCertificateFile = certificateFileContent;
                        licenseCreditDataFilesEntity.TaxCertificateFileName = certificateFileName;
                    }
                }

                if (hasUploadedFiles)
                {
                    if (!licenseCreditDataFilesEntity.Id.HasValue)
                        context.Add(licenseCreditDataFilesEntity);
                    else
                        context.Update(licenseCreditDataFilesEntity);
                    context.SaveChanges();
                    model.CreditDataFiles = _mapper.Map<CreditDataFiles>(licenseCreditDataFilesEntity);
                }
               // check files
              
            }
            this.ValidateCreditData(model.CreditData);
            this.ValidateCreditDataFiles(model.CreditDataFiles);
            if (ModelState.IsValid)
            {
                var templateLocation = Path.Combine(this._hostingEnvironment.WebRootPath, $"PdfTemplate/BMG_Credit_Application_Form_BLANK.pdf");
                var fileName = $"D{model.CreditData.DistributorId}_R{model.CreditData.RetailerId}_Document.pdf";
                var outputPath = Path.Combine(this._hostingEnvironment.WebRootPath, $"Pdfs/{fileName}");
                PdfGenerator pdfGenerator = new PdfGenerator(model);
                var fileGenerated = pdfGenerator.GeneratePdf(templateLocation, outputPath);

                if (fileGenerated)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(outputPath);
                    return File(fileBytes, "application/octet-stream", fileName);
                }
            }

            model.StatesListItems = this.GetStatesListItems();
            return View("Index", model);

            //return Ok();
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
            List<SelectListItem> returnStates = new List<SelectListItem>();
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
            List<States> states = new List<States>();
            
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
                ModelState.AddModelError("CreditDataFiles.LicenseFileName", "Missing Licence File");
            }

            if (creditDataFilesModel == null || String.IsNullOrWhiteSpace(creditDataFilesModel.TaxCertificateFileName))
            {
                ModelState.AddModelError("CreditDataFiles.TaxCertificateFileName", "Missing Certificate File");
            }

        }
        private void ValidateCreditData(CreditData creditDataModel)
        {
            string errorMessage;
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
            if (creditDataModel.PriorBusiness)
            {
                errorMessage = ValidateZipCode(creditDataModel.PriorBusinessZipCode, creditDataModel.PriorBusinessState);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    ModelState.AddModelError("CreditData.PriorBusinessZipCode", errorMessage);
            }

            if (creditDataModel.PropertyOwned)
            {
                errorMessage = ValidateZipCode(creditDataModel.PropertyZipCode, creditDataModel.PropertyState);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    ModelState.AddModelError("CreditData.PropertyZipCode", errorMessage);
            }

            errorMessage = ValidateZipCode(creditDataModel.TradeReference1ZipCode, creditDataModel.TradeReference1State);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.TradeReference1ZipCode", errorMessage);

            errorMessage = ValidateZipCode(creditDataModel.TradeReference2ZipCode, creditDataModel.TradeReference2State);
            if (!string.IsNullOrWhiteSpace(errorMessage))
                ModelState.AddModelError("CreditData.TradeReference2ZipCode", errorMessage);
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
    }
}
