using CreditAppBMG.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class CreditData
    {
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DistributorId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int RetailerId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Token { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? LastUpdate { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Business name:*")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Trade name:*")]
        public string TradeName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "License number:*")]
        [MaxLength(40)]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Date)]
        [Display(Name = "License expiration date:*")]
        public DateTime LicenseExpirationDate { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "EIN:*")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^[1-9]\d?-\d{7}$", ErrorMessage = "Not a valid EIN number")]
        public string EIN { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "NY State Tax Id: *")]
        public string NYStateTaxId { get; set; }

        [Display(Name = "Delivery time:")]
        [Required(ErrorMessage = "Required field")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Company type:*")]
        public string CompanyType { get; set; }

        public string CompanyTypeName => this.GetCompanyTypeName(this.CompanyType);

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Display(Name = "Phone:*")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Address 1:*")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2:")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "City:*")]
        public string City { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "State:*")]
        public string State { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Zip:*")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Name:*")]
        public string PrincipalName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Title:*")]
        public string PrincipalTitle { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Email:*")]
        [EmailAddress]
        public string PrincipalEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Phone:*")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PrincipalPhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Social security number: *")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Not a valid SSN number")]
        public string PrincipalSSN { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Address 1:*")]
        public string PrincipalAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string PrincipalAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "City:*")]
        public string PrincipalCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "State:*")]
        public string PrincipalState { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Zip:*")]
        public string PrincipalZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Do you own property?")]
        public bool PropertyOwned { get; set; }

        [Display(Name = "Property type:*")]
        [Required(ErrorMessage = "Required field")]
        public string PropertyType { get; set; }

        [Display(Name = "Address 1:*")]
        [Required(ErrorMessage = "Required field")]
        public string PropertyAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string PropertyAddress2 { get; set; }

        [Display(Name = "City:*")]
        [Required(ErrorMessage = "Required field")]
        public string PropertyCity { get; set; }

        [Display(Name = "State:*")]
        [Required(ErrorMessage = "Required field")]
        public string PropertyState { get; set; }

        [Display(Name = "Zip:*")]
        [Required(ErrorMessage = "Required field")]
        public string PropertyZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Have you done business before with {0}?")]
        public bool PriorBusiness { get; set; }

        [Display(Name = "Address 1:*")]
        [Required(ErrorMessage = "Required field")]
        public string PriorBusinessAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string PriorBusinessAddress2 { get; set; }

        [Display(Name = "City:*")]
        [Required(ErrorMessage = "Required field")]
        public string PriorBusinessCity { get; set; }

        [Display(Name = "State:*")]
        [Required(ErrorMessage = "Required field")]
        public string PriorBusinessState { get; set; }

        [Display(Name = "Zip:*")]
        [Required(ErrorMessage = "Required field")]
        public string PriorBusinessZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Name:*")]
        public string BillingContactName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Email:*")]
        [EmailAddress]
        public string BillingContactEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Phone:*")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string BillingContactPhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Address 1:*")]
        public string BillingContactAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string BillingContactAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "City:*")]
        public string BillingContactCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "State:*")]
        public string BillingContactState { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Zip:*")]
        public string BillingContactZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Name:*")]
        public string BankReferenceName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Phone:*")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string BankReferencePhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Account type:*")]
        public string BankReferenceAccountType { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Account number:*")]
        public string BankReferenceAccountNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Routing number:*")]
        public string BankReferenceRoutingNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Address 1:*")]
        public string BankReferenceAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string BankReferenceAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "City:*")]
        public string BankReferenceCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "State:*")]
        public string BankReferenceState { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Zip:*")]
        public string BankReferenceZipCode { get; set; }

        [Display(Name = "Name:")]
        public string TradeReference1Name { get; set; }

        [Display(Name = "Phone:")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string TradeReference1Phone { get; set; }

        [Display(Name = "Account number:")]
        public string TradeReference1AccountNumber { get; set; }

        [Display(Name = "Address 1:")]
        public string TradeReference1Address1 { get; set; }

        [Display(Name = "Address 2:")]
        public string TradeReference1Address2 { get; set; }

        [Display(Name = "City:")]
        public string TradeReference1City { get; set; }

        [Display(Name = "State:")]
        public string TradeReference1State { get; set; }

        [Display(Name = "Zip:")]
        public string TradeReference1ZipCode { get; set; }

        [Display(Name = "Name:")]
        public string TradeReference2Name { get; set; }

        [Display(Name = "Phone:")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string TradeReference2Phone { get; set; }

        [Display(Name = "Account number:")]
        public string TradeReference2AccountNumber { get; set; }

        [Display(Name = "Address 1:")]
        public string TradeReference2Address1 { get; set; }

        [Display(Name = "Address 2:")]
        public string TradeReference2Address2 { get; set; }

        [Display(Name = "City:")]
        public string TradeReference2City { get; set; }

        [Display(Name = "State:")]
        public string TradeReference2State { get; set; }

        [Display(Name = "Zip:")]
        public string TradeReference2ZipCode { get; set; }

        public string SigningUrl { get; set; }

        public string AdobeSignAgreementId { get; set; }

        public string Status { get; set; }

        public string Comments => this.GetComments(this.Id);

        public bool CanAddComments { get; set; }

        public string DistributorStatus { get; set; }

        public ICollection<CreditDataFiles> CreditFiles { get; set; }

        public Dictionary<string, string> CompanyTypes = new Dictionary<string, string> {
            { "MX", "Limited liability company" },
            { "CA", "S Corporation" },
            { "US", "Sole proprietor" },
            { "PS", "Partnership" }
        };

        public string GetCompanyTypeName(string companyType)
        {
            string companyTypeName = string.Empty;
            if (!String.IsNullOrEmpty(companyType) && CompanyTypes.ContainsKey(companyType))
            {
                companyTypeName = CompanyTypes[companyType];
            }
            return companyTypeName;
        }

        private string GetComments(int? creditDataId)
        {
            string retVal = string.Empty;
            if (creditDataId.HasValue)
            {
                CreditAppRepository repository = new CreditAppRepository();
                retVal= repository.GetCreditAppComments(creditDataId.Value);
            }
            return retVal;
        }
    }
}