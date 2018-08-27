using System;
using System.ComponentModel.DataAnnotations;

namespace CreditAppBMG.ViewModels
{
    public class CreditData
    {
        [Key]
        [Required(ErrorMessage = "Required field")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int DistributorId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int RetailerId { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Token { get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? LastUpdate { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BusinessName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string TradeName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        public DateTime LicenseExpirationDate { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string EIN { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string NYStateTaxId { get; set; }

        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string CompanyType { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string City { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string State { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalTitle { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalPhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalSSN { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalAddress1 { get; set; }

        public string PrincipalAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalState { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string PrincipalZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        public bool PropertyOwned { get; set; }

        public string PropertyType { get; set; }

        public string PropertyAddress1 { get; set; }

        public string PropertyAddress2 { get; set; }

        public string PropertyCity { get; set; }

        public string PropertyState { get; set; }

        public string PropertyZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        public bool PriorBusiness { get; set; }

        public string PriorBusinessAddress1 { get; set; }

        public string PriorBusinessAddress2 { get; set; }

        public string PriorBusinessCity { get; set; }

        public string PriorBusinessState { get; set; }

        public string PriorBusinessZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactEmail { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactPhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactAddress1 { get; set; }

        public string BillingContactAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactState { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BillingContactZipCode { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferencePhone { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceAccountType { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceAccountNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceRoutingNumber { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceAddress1 { get; set; }

        public string BankReferenceAddress2 { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceCity { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceState { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string BankReferenceZipCode { get; set; }

        public string TradeReference1Name { get; set; }

        public string TradeReference1Phone { get; set; }

        public string TradeReference1AccountNumber { get; set; }

        public string TradeReference1Address1 { get; set; }

        public string TradeReference1Address2 { get; set; }

        public string TradeReference1City { get; set; }

        public string TradeReference1State { get; set; }

        public string TradeReference1ZipCode { get; set; }

        public string TradeReference2Name { get; set; }

        public string TradeReference2Phone { get; set; }

        public string TradeReference2AccountNumber { get; set; }

        public string TradeReference2Address1 { get; set; }

        public string TradeReference2Address2 { get; set; }

        public string TradeReference2City { get; set; }

        public string TradeReference2State { get; set; }

        public string TradeReference2ZipCode { get; set; }
    }
}