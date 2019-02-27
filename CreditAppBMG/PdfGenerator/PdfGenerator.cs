using CreditAppBMG.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace CreditAppBMG.Pdf
{
    public class PdfGenerator
    {
        private CreditAppModel obj;
        private string FontPath;

        public PdfGenerator()
        {
           
        }
            public PdfGenerator(CreditAppModel creditAppModel)
        {
            this.obj = creditAppModel;
        }

        public bool GeneratePdf2(string templatePath, string outputFile)
        {

            //Document document = new Document();
            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFile, FileMode.Create, FileAccess.Write));
            //document.Open();

            //document.Add(new Paragraph("Hello World!"));
            //PdfFormField field = PdfFormField.CreateSignature(writer);
            //field.FieldName="SIGNAME";
            //field.SetPage();
            //field.SetWidget(new iTextSharp.text.Rectangle(72, 732, 144, 780), PdfAnnotation.HIGHLIGHT_INVERT);
            //field.SetFieldFlags(PdfAnnotation.FLAGS_PRINT);
            //writer.AddAnnotation(field);
            //PdfAppearance tp = PdfAppearance.CreateAppearance(writer, 72, 48);
            //tp.SetColorStroke(BaseColor.BLUE);
            //tp.SetColorFill(BaseColor.LIGHT_GRAY);
            //tp.Rectangle(0.5f, 0.5f, 71.5f, 47.5f);
            //tp.FillStroke();
            //tp.SetColorFill(BaseColor.BLUE);
            //ColumnText.ShowTextAligned(tp, Element.ALIGN_CENTER,
            //new Phrase("SIGN HERE"), 36, 24, 25);
            //field.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, tp);
            //document.Close();

            PdfReader reader = new PdfReader(templatePath);

            using (var fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                PdfStamper stamp = new PdfStamper(reader, fileStream);

                //TextField field = new TextField(stamp.Writer, new Rectangle(40, 500, 360, 530), "some_text");

                // add the field here, the second param is the page you want it on         
                //stamp.AddAnnotation(field.GetTextField(), 1);


                var content = stamp.GetOverContent(1);

                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                content.SetFontAndSize(baseFont, 8);

                content.BeginText();
                content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "QQQQQQQQQQQQQQQQQQQ", 45, 557, 0);
                content.EndText();

                //PdfFormField field = PdfFormField.CreateSignature(stamp.Writer);

                //field.FieldName = "SIGNAME";
                //field.SetPage();
                //field.SetWidget(new iTextSharp.text.Rectangle(72, 732, 144, 780), PdfAnnotation.HIGHLIGHT_INVERT);
                //field.SetFieldFlags(PdfAnnotation.FLAGS_PRINT);


                //PdfAppearance tp = PdfAppearance.CreateAppearance(stamp.Writer, 72, 48);
                //tp.SetColorStroke(BaseColor.BLUE);
                //tp.SetColorFill(BaseColor.LIGHT_GRAY);
                //tp.Rectangle(0.5f, 0.5f, 71.5f, 47.5f);
                //tp.FillStroke();
                //tp.SetColorFill(BaseColor.BLUE);
                //ColumnText.ShowTextAligned(tp, Element.ALIGN_CENTER,
                //new Phrase("SIGN HERE"), 36, 24, 25);
                //field.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, tp);


                stamp.FormFlattening = true; // lock fields and prevent further edits.
                //stamp.AddSignature("SIG_NAME", 1, 73, 705, 149, 759);
                stamp.AddSignature("SIGNATURE_NAME", 1, 75, 30, 275, 50);
                stamp.Close();
            }



            return true;
        }
        public bool GeneratePdf(string templatePath, string fontPath, string outputFile)
        {
            //generate pdf
            var fileTemplatePath = templatePath;
            this.FontPath = fontPath;
            Stream inputImageStream = null;
            using (var reader = new PdfReader(fileTemplatePath))
            {
                if (this.obj.LocalLogo != null)
                    inputImageStream = new FileStream(this.obj.LocalLogo, FileMode.Open, FileAccess.Read, FileShare.Read);

                using (var fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                {
                    var stamper = new PdfStamper(reader, fileStream);
                    var contentByte = stamper.GetOverContent(1);

                    if (this.obj.LocalLogo != null)
                    {
                        Image image = Image.GetInstance(inputImageStream);
                        image.ScaleAbsoluteHeight(image.Height / (float)3.5);
                        image.ScaleAbsoluteWidth(image.Width / (float)3.5);
                        image.SetAbsolutePosition(20, 730);
                        contentByte.AddImage(image);
                    }
                   
                    //appearance.setReason(reason);
                    //appearance.setLocation(location);
                    //appearance.setVisibleSignature(new Rectangle(36, 748, 144, 780), 1, "sig");


                    CreateBiggerTexBox("DistributorName", 88, 770, 300, obj.Distributor.DistributorName, contentByte);
                    CreateSmallerTexBox("DistributorAddress", 88, 760, 800, obj.Distributor.DistributorAddress, contentByte);
                    CreateSmallerTexBox("DistributorAddress2", 88, 752, 800,  obj.Distributor.DistributorCity + ", " + obj.Distributor.DistributorState + ", " + obj.Distributor.DistributorZip, contentByte);
                    CreateSmallerTexBox("DistributorPhone", 88, 744, 400, "Phone: " + obj.Distributor.DistributorPhone, contentByte);
                    if (!string.IsNullOrWhiteSpace(obj.Distributor.DistributorWebSiteURL))
                        CreateSmallerTexBox("DistributorWebsite", 88, 736, 400, "Web: " + obj.Distributor.DistributorWebSiteURL.Replace("Http://",""), contentByte);

                    //CreteTextField("BusinessName", 88, 663, 215, obj.CreditData.BusinessName, stamper);
                    CreateTexBox("BusinessName", 88, 665, 215, obj.CreditData.BusinessName, contentByte);
                    CreateTexBox("LicenseNumber", 352, 665, 90, obj.CreditData.LicenseNumber, contentByte);
                    CreateTexBox("LicenseExpirationDate", 530, 665, 60, obj.CreditData.LicenseExpirationDate.ToString("MM/dd/yyyy"), contentByte);

                    CreateTexBox("TradeName", 72, 646, 125, obj.CreditData.TradeName, contentByte);
                    CreateTexBox("CompanyType", 265, 646, 180, obj.CreditData.CompanyTypeName, contentByte);
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

                    CreateTexBox("PrincipalContactPhone", 48, 539, 148, obj.CreditData.PrincipalPhone, contentByte);
                    CreateTexBox("PrincipalContactEmail", 225, 539, 220, obj.CreditData.PrincipalEmail, contentByte);
                    CreateTexBox("PrincipalContactSSN", 478, 539, 110, obj.CreditData.PrincipalSSN, contentByte);

                    CreateTexBox("PrincipalContactAddress1", 58, 520, 220, obj.CreditData.PrincipalAddress1, contentByte);
                    CreateTexBox("PrincipalContactAddress2", 327, 520, 260, obj.CreditData.PrincipalAddress2, contentByte);

                    CreateTexBox("PrincipalContactCity", 38, 502, 240, obj.CreditData.PrincipalCity, contentByte);
                    CreateTexBox("PrincipalContactState", 305, 502, 140, obj.CreditData.PrincipalState, contentByte);
                    CreateTexBox("PrincipalContactZip", 468, 502, 120, obj.CreditData.PrincipalZipCode, contentByte);

                    //CreateRadioGroup("OwnProperty", 113, 480, 6, 30, obj.CreditData.PropertyOwned ? 0 : 1, contentByte);
                    //CreateRadioGroup("PropertyType", 190, 480, 6, 70, obj.CreditData.PropertyOwned ? obj.CreditData.PropertyType == "Residential" ? 0 : 1 : -1, contentByte);
                    CreateTexBox("OwnProperty", 113, 482, 6, obj.CreditData.PropertyOwned ? "X" : "", contentByte);
                    CreateTexBox("OwnProperty", 143, 482, 6, !obj.CreditData.PropertyOwned ? "X" : "", contentByte);
                    CreateTexBox("PropertyType", 190, 482, 6, obj.CreditData.PropertyOwned ? obj.CreditData.PropertyType=="Residential" ? "X" : "":"", contentByte);
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

                    //CreateTexBox("DistributorName2", 150, 262, 200, obj.Distributor.DistributorName, contentByte);

                    //StatesBo priorBusinessState = new StatesBoService().GetByAbbreviation(obj.CreditData.PriorBusinessState);
                    string priorBusinessStateName = obj.CreditData.PriorBusinessState == null ? "" : obj.CreditData.PriorBusinessState;
                    string priorBusinessAddress = "";

                    CreateTexBox("PriorBusiness", (float)144, (float)261.3, 570, $"{obj.Distributor.DistributorName} before? If yes, provide location address:", contentByte);

                    if (!obj.CreditData.PriorBusiness)
                    {
                        priorBusinessAddress = "No";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(obj.CreditData.PriorBusinessAddress1) && !string.IsNullOrEmpty(obj.CreditData.PriorBusinessCity) && !string.IsNullOrEmpty(priorBusinessStateName) && !string.IsNullOrEmpty(obj.CreditData.PriorBusinessZipCode))
                            priorBusinessAddress = $"{obj.CreditData.PriorBusinessAddress1} {obj.CreditData.PriorBusinessAddress2}, {obj.CreditData.PriorBusinessCity}, {priorBusinessStateName}, {obj.CreditData.PriorBusinessZipCode}";
                    }

                    DrawHorizontalLine(18, (float) 246.4, 576, contentByte);

                    CreateTexBox("PriorBusinessAddress", 20, 249, 570, priorBusinessAddress, contentByte);

                    CreateTexBox("TradeReference1Name", 52, 203, 365, obj.CreditData.TradeReference1Name, contentByte);
                    CreateTexBox("TradeReference1AccountNumber", 494, 203, 80, obj.CreditData.TradeReference1AccountNumber, contentByte);

                    CreateTexBox("TradeReference1Address1", 63, 185, 175, obj.CreditData.TradeReference1Address1, contentByte);
                    CreateTexBox("TradeReference1Address2", 290, 185, 128, obj.CreditData.TradeReference1Address2, contentByte);
                    CreateTexBox("TradeReference1Phone", 485, 185, 90, obj.CreditData.TradeReference1Phone, contentByte);

                    CreateTexBox("TradeReference1City", 45, 166, 192, obj.CreditData.TradeReference1City, contentByte);
                    //StatesBo tradeReference1State = new StatesBoService().GetByAbbreviation(obj.CreditData.TradeReference1State);
                    string tradeReference1StateName = obj.CreditData.TradeReference1State == null ? "" : obj.CreditData.TradeReference1State;
                    CreateTexBox("TradeReference1State", 268, 166, 149, tradeReference1StateName, contentByte);
                    CreateTexBox("TradeReference1Zip", 438, 166, 136, obj.CreditData.TradeReference1ZipCode, contentByte);

                    CreateTexBox("TradeReference2Name", 52, 136, 365, obj.CreditData.TradeReference2Name, contentByte);
                    CreateTexBox("TradeReference2AccountNumber", 494, 136, 80, obj.CreditData.TradeReference2AccountNumber, contentByte);

                    CreateTexBox("TradeReference2Address1", 63, 118, 175, obj.CreditData.TradeReference2Address1, contentByte);
                    CreateTexBox("TradeReference2Address2", 290, 118, 128, obj.CreditData.TradeReference2Address2, contentByte);
                    CreateTexBox("TradeReference2Phone", 485, 118, 90, obj.CreditData.TradeReference2Phone, contentByte);

                    CreateTexBox("TradeReference2City", 45, 99, 192, obj.CreditData.TradeReference2City, contentByte);
                    //StatesBo tradeReference2State = new StatesBoService().GetByAbbreviation(obj.CreditData.TradeReference2State);
                    string tradeReference2StateName = obj.CreditData.TradeReference2State == null ? "" : obj.CreditData.TradeReference2State;
                    CreateTexBox("TradeReference2State", 268, 99, 149, tradeReference2StateName, contentByte);
                    CreateTexBox("TradeReference2Zip", 438, 99, 136, obj.CreditData.TradeReference2ZipCode, contentByte);

                    //contentByte.AddTemplate(importedPage, 0, 0);
                    stamper.FormFlattening = true;

                    stamper.FormFlattening = true; // lock fields and prevent further edits.
                    stamper.AddSignature("SIGNATURE_NAME", 1, 75, 30, 275, 50);
                    stamper.Close();

                    stamper.Close();

                }
            }
            if (inputImageStream != null)
            {
                inputImageStream.Close();
                inputImageStream.Dispose();
            }

            ////signature
            ////stamper.SignatureAppearance.SetVisibleSignature(new Rectangle(36, 748, 144, 780), 1, "sig");
            //var reader2 = new PdfReader(outputFile);

            //FileInfo fi = new FileInfo(outputFile);

            //var fileStream2 = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            //        PdfStamper signature = PdfStamper.CreateSignature(reader2, fileStream2, '\0');
            //        PdfSignatureAppearance sap = signature.SignatureAppearance;

            //        sap.Reason = "Reason";
            //        sap.Contact = "Contact";
            //        sap.Location = "Location";
            //        sap.SetVisibleSignature(new iTextSharp.text.Rectangle(100, 100, 250, 150), 1, null);
            //        //sap.Close();
            //        //signature.Close();

            //var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", filename);
            return true;
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        private bool CreateCheckBoxField()
        {
            //checkBox = new RadioCheckField(writer, new Rectangle(20, 20), "noDelete", "Yes");
            //field = checkBox.CheckField;
            //field.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, "Off", checkbox[0]);
            //field.SetAppearance(PdfAnnotation.APPEARANCE_NORMAL, "Yes", checkbox[1]);
            //writer.AddAnnotation(field);

            return true;
        }

        private bool CreteTextField(string fieldName, float x, float y, float width, string fieldText, PdfStamper stamper)
        {
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            PdfFormField field = PdfFormField.CreateTextField(stamper.Writer, false, false, 50);
            TextField textField = new TextField(stamper.Writer, new Rectangle(x, y, x + width, y+10), fieldName);
            textField.Text = fieldText;
            textField.FontSize = 8;
            textField.Font = baseFont;
            //field.SetWidget(new Rectangle(x, y - 10, x + width, y), PdfAnnotation.HIGHLIGHT_INVERT);
            //field.Flags = PdfAnnotation.FLAGS_PRINT;
            //field.RichValue = fieldText;
            //field.FieldName = fieldName;

            stamper.AddAnnotation(textField.GetTextField(), 1);

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
                BaseFont baseFont = BaseFont.CreateFont(this.FontPath, BaseFont.CP1250, BaseFont.EMBEDDED);
                //contentByte.SetColorFill(new BaseColor(255, 0, 0));
                contentByte.BeginText();
                contentByte.SetFontAndSize(baseFont, 9);

                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, string.IsNullOrWhiteSpace(content) ? string.Empty : content, left, top, 0);
                //contentByte.
                contentByte.EndText();

                success = true;
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

        private bool CreateLabel(string label, float left, float top, string content, PdfContentByte contentByte)
        {
            bool success = false;
            try
            {
                var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //var baseFont = BaseFont.CreateFont("Arial", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                contentByte.BeginText();
                contentByte.SetFontAndSize(baseFont, 8);

                contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, string.IsNullOrWhiteSpace(content) ? string.Empty : content, left, top, 0);
                contentByte.EndText();

                success = true;
            }
            catch (Exception)
            {
                throw;
            }
            return success;
        }

        private void DrawHorizontalLine(float left, float top, float length, PdfContentByte contentByte)
        {
            contentByte.SetLineWidth(0.70);
            //contentByte.SetColorStroke(new BaseColor(255,0,0));
            contentByte.MoveTo(left, top);
            contentByte.LineTo(left+length, top);
            contentByte.Stroke();
        }
    }
}
