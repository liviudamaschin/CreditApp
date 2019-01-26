namespace CreditAppBMG.Models
{
    public class AddCommentModel
    {
        public int CreditDataId { get; set; }
        public string CreditDataStatus { get; set; }
        public string Comments { get; set; }
        public string Token { get; set; }
    }
}
