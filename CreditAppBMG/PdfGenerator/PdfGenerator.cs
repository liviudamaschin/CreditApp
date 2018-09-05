using CreditAppBMG.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG.Pdf
{
    public class PdfGenerator
    {
        private CreditAppModel obj;

        public PdfGenerator(CreditAppModel creditAppModel)
        {
            this.obj = creditAppModel;
        }
        public bool GeneratePdf()
        {
            //generate pdf
            var fileTemplatePath = @"D:\_GIT\CreditApp\CreditAppBMG\PdfTemplate\BMG_Credit_Application_Form_BLANK.pdf";

            using (var reader = new PdfReader(fileTemplatePath))
            {
                using (var fileStream = new FileStream(@"D:\Output.pdf", FileMode.Create, FileAccess.Write))
                {
                    var document = new Document(reader.GetPageSizeWithRotation(1));
                    var writer = PdfWriter.GetInstance(document, fileStream);

                    document.Open();

                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();

                        //var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        //var baseFont = BaseFont.CreateFont("Arial", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = writer.GetImportedPage(reader, i);
                        var contentByte = writer.DirectContent;

                        //contentByte.BeginText();
                        //contentByte.SetFontAndSize(baseFont, 8);
                        //contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, obj.CreditData.BusinessName, 88, 665, 0);
                        //contentByte.EndText();

                        //contentByte.BeginText();
                        //contentByte.SetFontAndSize(baseFont, 8);
                        //contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, obj.CreditData.LicenseNumber, 352, 665, 0);
                        //contentByte.EndText();


                        CreateBiggerTexBox("DistributorName", 20, 740, 300, obj.Distributor.DistributorId.ToString(), contentByte);
                        CreateSmallerTexBox("DistributorAddress", 330, 770, 400, "Address: " + obj.Distributor.DistributorAddress + obj.Distributor.DistributorCity + ", " + obj.Distributor.DistributorState + ", " + obj.Distributor.DistributorZip, contentByte);
                        CreateSmallerTexBox("DistributorPhone", 330, 755, 400, "Phone: " + obj.Distributor.DistributorPhone, contentByte);
                        CreateSmallerTexBox("DistributorWebsite", 330, 740, 400, "Website: " + obj.Distributor.DistributorWebSiteURL, contentByte);

                        CreateTexBox("BusinessName", 88, 665, 215, obj.CreditData.BusinessName, contentByte);
                        CreateTexBox("LicenseNumber", 352, 665, 90, obj.CreditData.LicenseNumber, contentByte);
                        CreateTexBox("LicenseExpirationDate", 530, 665, 60, obj.CreditData.LicenseExpirationDate.ToString("MM/dd/yyyy"), contentByte);

                        CreateTexBox("TradeName", 72, 646, 125, obj.CreditData.TradeName, contentByte);
                        CreateTexBox("CompanyType", 265, 646, 180, obj.CreditData.CompanyType, contentByte);
                        CreateTexBox("DeliveryTime", 510, 646, 80, obj.CreditData.DeliveryTime, contentByte);

                        CreateTexBox("Address1", 55, 628, 220, obj.CreditData.Address1, contentByte);
                        CreateTexBox("Address2", 325, 628, 120, obj.CreditData.Address2, contentByte);
                        CreateTexBox("Phone", 480, 628, 110, obj.CreditData.Phone, contentByte);
                        
                        CreateTexBox("City", 38, 609, 240, obj.CreditData.City, contentByte);
                        CreateTexBox("State", 305, 609, 140, obj.CreditData.State, contentByte);
                        CreateTexBox("Zip", 468, 609, 120, obj.CreditData.ZipCode, contentByte);

                        CreateTexBox("EIN", 35, 591, 240, obj.CreditData.EIN, contentByte);
                        CreateTexBox("NYSTaxId", 330, 591, 260, obj.CreditData.NYStateTaxId, contentByte);

                        CreateTexBox("PrincipalContactName", 45, 557, 260, obj.CreditData.PrincipalName, contentByte);
                        CreateTexBox("PrincipalContactTitle", 327, 557, 260, obj.CreditData.PrincipalTitle, contentByte);

                        CreateTexBox("PrincipalContactPhhone", 48, 539, 148, obj.CreditData.PrincipalPhone, contentByte);
                        CreateTexBox("PrincipalContactEmail", 225, 539, 220, obj.CreditData.PrincipalEmail, contentByte);
                        CreateTexBox("PrincipalContactSSN", 478, 539, 110, obj.CreditData.PrincipalSSN, contentByte);

                        CreateTexBox("PrincipalContactAddress1", 58, 520, 220, obj.CreditData.PrincipalAddress1, contentByte);
                        CreateTexBox("PrincipalContactAddress2", 327, 520, 260, obj.CreditData.PrincipalAddress2, contentByte);

                        CreateTexBox("PrincipalContactCity", 38, 502, 240, obj.CreditData.PrincipalCity, contentByte);
                        CreateTexBox("PrincipalContactState", 305, 502, 140, obj.CreditData.PrincipalState, contentByte);
                        CreateTexBox("PrincipalContactZip", 468, 502, 120, obj.CreditData.PrincipalZipCode, contentByte);

                        //CreateRadioGroup("OwnProperty", 113, 480, 6, 30, obj.CreditData.PropertyOwned ? 0 : 1, contentByte);
                        //CreateRadioGroup("PropertyType", 190, 480, 6, 70, obj.CreditData.PropertyOwned ? obj.CreditData.PropertyType == "Residential" ? 0 : 1 : -1, contentByte);
                        if (obj.CreditData.PropertyOwned)
                        {
                            CreateTexBox("PropertyAddress", 18, 469, 570, $"{obj.CreditData.PropertyAddress1} {obj.CreditData.PropertyAddress2}, {obj.CreditData.PropertyCity}, {obj.CreditData.PropertyState}, {obj.CreditData.PropertyZipCode}", contentByte);
                        }
                        
                        CreateTexBox("BillingContactName", 45, 436, 540, obj.CreditData.BillingContactName, contentByte);

                        CreateTexBox("BillingContactPhone", 48, 417, 150, obj.CreditData.BillingContactPhone, contentByte);
                        CreateTexBox("BillingContactEmail", 225, 417, 360, obj.CreditData.BillingContactEmail, contentByte);

                        CreateTexBox("BillingContactAddress1", 55, 398, 220, obj.CreditData.BillingContactAddress1, contentByte);
                        CreateTexBox("BillingContactAddress2", 325, 398, 260, obj.CreditData.BillingContactAddress2, contentByte);

                        CreateTexBox("BillingContactCity", 38, 380, 235, obj.CreditData.BillingContactCity, contentByte);
                        CreateTexBox("BillingContactState", 305, 380, 140, obj.CreditData.BillingContactState, contentByte);
                        CreateTexBox("BillingContactZip", 468, 380, 120, obj.CreditData.BillingContactZipCode.ToString(), contentByte);

                        CreateTexBox("BankReferenceName", 45, 345, 290, obj.CreditData.BankReferenceName, contentByte);
                        CreateTexBox("BankReferencePhone", 382, 345, 205, obj.CreditData.BankReferencePhone, contentByte);

                        CreateTexBox("BankReferenceAccountType", 78, 326, 120, obj.CreditData.BankReferenceAccountType, contentByte);
                        CreateTexBox("BankReferenceAccountNumber", 273, 326, 135, obj.CreditData.BankReferenceAccountNumber, contentByte);
                        CreateTexBox("BankReferenceRoutingNumber", 485, 326, 100, obj.CreditData.BankReferenceRoutingNumber, contentByte);

                        CreateTexBox("BankReferenceAddress1", 56, 307, 220, obj.CreditData.BankReferenceAddress1, contentByte);
                        CreateTexBox("BankReferenceAddress2", 326, 307, 260, obj.CreditData.BankReferenceAddress2, contentByte);

                        CreateTexBox("BankReferenceCity", 38, 288, 240, obj.CreditData.BankReferenceCity, contentByte);
                        CreateTexBox("BankReferenceState", 305, 288, 140, obj.CreditData.BankReferenceState, contentByte);
                        CreateTexBox("BankReferenceZip", 468, 288, 118, obj.CreditData.BankReferenceZipCode.ToString(), contentByte);

                        //CreateTexBox("DistributorName2", 150, 254, 200, obj.CreditData.Distributor.DistributorName, contentByte);

                        //StatesBo priorBusinessState = new StatesBoService().GetByAbbreviation(obj.CreditData.PriorBusinessState);
                        //string priorBusinessStateName = priorBusinessState == null ? "" : priorBusinessState.State;
                        //string priorBusinessAddress = "";
                        //if (!string.IsNullOrEmpty(obj.CreditData.PriorBusinessAddress1) && !string.IsNullOrEmpty(obj.CreditData.PriorBusinessCity) && !string.IsNullOrEmpty(priorBusinessStateName) && !string.IsNullOrEmpty(obj.CreditData.PriorBusinessZipCode))
                        //    priorBusinessAddress = $"{obj.CreditData.PriorBusinessAddress1} {obj.CreditData.PriorBusinessAddress2}, {obj.CreditData.PriorBusinessCity}, {priorBusinessStateName}, {obj.CreditData.PriorBusinessZipCode}";
                        
                        //CreateTexBox("PriorBusinessAddress", 18, 249, 570, priorBusinessAddress, contentByte);

                        CreateTexBox("TradeReference1Name", 52, 203, 365, obj.CreditData.TradeReference1Name, contentByte);
                        CreateTexBox("TradeReference1AccountNumber", 494, 203, 80, obj.CreditData.TradeReference1AccountNumber, contentByte);

                        CreateTexBox("TradeReference1Address1", 63, 185, 175, obj.CreditData.TradeReference1Address1, contentByte);
                        CreateTexBox("TradeReference1Address2", 290, 185, 128, obj.CreditData.TradeReference1Address2, contentByte);
                        CreateTexBox("TradeReference1Phone", 485, 185, 90, obj.CreditData.TradeReference1Phone, contentByte);

                        CreateTexBox("TradeReference1City", 45, 166, 192, obj.CreditData.TradeReference1City, contentByte);
                        //StatesBo tradeReference1State = new StatesBoService().GetByAbbreviation(obj.CreditData.TradeReference1State);
                        //string tradeReference1StateName = tradeReference1State == null ? "" : tradeReference1State.State;
                        //CreateTexBox("TradeReference1State", 268, 166, 149, tradeReference1StateName, contentByte);
                        CreateTexBox("TradeReference1Zip", 438, 166, 136, obj.CreditData.TradeReference1ZipCode, contentByte);

                        CreateTexBox("TradeReference2Name", 52, 136, 365, obj.CreditData.TradeReference2Name, contentByte);
                        CreateTexBox("TradeReference2AccountNumber", 494, 136, 80, obj.CreditData.TradeReference2AccountNumber, contentByte);

                        CreateTexBox("TradeReference2Address1", 63, 118, 175, obj.CreditData.TradeReference2Address1, contentByte);
                        CreateTexBox("TradeReference2Address2", 290, 118, 128, obj.CreditData.TradeReference2Address2, contentByte);
                        CreateTexBox("TradeReference2Phone", 485, 118, 90, obj.CreditData.TradeReference2Phone, contentByte);

                        CreateTexBox("TradeReference2City", 45, 99, 192, obj.CreditData.TradeReference2City, contentByte);
                        //StatesBo tradeReference2State = new StatesBoService().GetByAbbreviation(obj.CreditData.TradeReference2State);
                        //string tradeReference2StateName = tradeReference2State == null ? "" : tradeReference2State.State;
                        //CreateTexBox("TradeReference2State", 268, 99, 149, tradeReference2StateName, contentByte);
                        CreateTexBox("TradeReference2Zip", 438, 99, 136, obj.CreditData.TradeReference2ZipCode, contentByte);

                        contentByte.AddTemplate(importedPage, 0, 0);
                    }

                    document.Close();
                    writer.Close();
                }
            }
            return true;
        }

        private bool CreateBiggerTexBox(string textBoxName, float left, float top, double width, string content, PdfContentByte contentByte)
        {
            bool success = false;
            try
            {
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                contentByte.BeginText();
                contentByte.SetFontAndSize(baseFont, 12);

                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, content, left, top, 0);
                contentByte.EndText();

                success = true;
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

        private bool CreateSmallerTexBox(string textBoxName, float left, float top, double width, string content, PdfContentByte contentByte)
        {
            bool success = false;
            try
            {
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                contentByte.BeginText();
                contentByte.SetFontAndSize(baseFont, 6);

                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, content, left, top, 0);
                contentByte.EndText();

                success = true;
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

        private bool CreateTexBox(string textBoxName, float left, float top, double width, string content, PdfContentByte contentByte)
        {
            bool success = false;
            try
            {
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //var baseFont = BaseFont.CreateFont("Arial", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                contentByte.BeginText();
                contentByte.SetFontAndSize(baseFont, 8);
                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, string.IsNullOrWhiteSpace(content)?string.Empty:content, left, top, 0);
                contentByte.EndText();

                success = true;
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

    }
}
