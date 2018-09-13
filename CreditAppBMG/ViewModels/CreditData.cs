using CreditAppBMG.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class CreditData
    {
        [Required(ErrorMessage = "Required field")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int DistributorId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string RetailerId { get; set; }

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
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "License expiration date:*")]
        //[DisplayFormat(DataFormatString = "mm/dd/yyyy", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LicenseExpirationDate { get; set; }

        [Required(ErrorMessage = "Required field")]
        [HTMLMaskAttribute("mask", "99-9999999")] //EIN format
        [Display(Name = "EIN:*")]
        public string EIN { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "NY State Tax Id: *")]
        public string NYStateTaxId { get; set; }

        [Display(Name = "Delivery time:")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Company type:*")]
        public string CompanyType { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Phone]
        [HTMLMaskAttribute("mask", "(999) 999-9999")] //phone format
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
        public string PrincipalEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Phone:*")]
        public string PrincipalPhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Social security number: *")]
        public string PrincipalSSN { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Address 1:*")]
        public string PrincipalAddress1 { get; set; }

        [Display(Name = "Address 2:*")]
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

        [Display(Name = "Property type:")]
        public string PropertyType { get; set; }

        [Display(Name = "Address 1:")]
        public string PropertyAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string PropertyAddress2 { get; set; }

        [Display(Name = "City:")]
        public string PropertyCity { get; set; }

        [Display(Name = "State:")]
        public string PropertyState { get; set; }

        [Display(Name = "Zip:")]
        public string PropertyZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Have you done business before with {0} Southern Glazers Wine & Spirits?")]
        public bool PriorBusiness { get; set; }

        [Display(Name = "Address 1:")]
        public string PriorBusinessAddress1 { get; set; }

        [Display(Name = "Address 2:")]
        public string PriorBusinessAddress2 { get; set; }

        [Display(Name = "City:")]
        public string PriorBusinessCity { get; set; }

        [Display(Name = "State:")]
        public string PriorBusinessState { get; set; }

        [Display(Name = "Zip:")]
        public string PriorBusinessZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Name:*")]
        public string BillingContactName { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Email:*")]
        public string BillingContactEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Phone:*")]
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
    }
}