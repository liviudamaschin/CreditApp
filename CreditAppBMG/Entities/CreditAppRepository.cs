using System.Linq;

namespace CreditAppBMG.Entities
{
    public class CreditAppRepository
    {
        private CreditAppContext _context;

        public CreditAppRepository()
        {
            this._context = new CreditAppContext();
        }

        public string GetKeyValue(string key)
        {
            return this._context.ApplicationConfigurations.SingleOrDefault(x => x.ConfigKey == key && x.IsActive).ConfigValue;
        }

        public void AddAdobeSignLog(string action, string request, string response)
        {
            using (var context = new CreditAppContext())
            {
                AdobeSignLogEntity entity = new AdobeSignLogEntity();
                entity.Action = action;
                entity.Request = request;
                entity.Response = response;

                context.AdobeSignLogs.Add(entity);
                context.SaveChanges();
            }
        }
    }
}