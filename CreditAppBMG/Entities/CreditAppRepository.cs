using System;
using System.Collections.Generic;
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

        public string GetCreditAppComments(int creditDataId)
        {
            return this._context.DistributorLogs.Where(x => x.CreditDataId == creditDataId).LastOrDefault()?.Comments;
                //.SingleOrDefault(x => x.ConfigKey == key && x.IsActive).ConfigValue;
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

        public void AddDistributorLogWithComments(int creditDataId, string status, string comments)
        {
            using (var context = new CreditAppContext())
            {
                DistributorLogEntity entity = new DistributorLogEntity();
                entity.CreditDataId = creditDataId;
                entity.Status = status;
                entity.Comments = comments;
                entity.LastUpdate = DateTime.Now;
               
                context.DistributorLogs.Add(entity);
                context.SaveChanges();
            }
        }

        public List<CreditDataEntity> GetDistributorCreditData(string distributorId)
        {
            return this._context.CreditData.Where(x => x.DistributorId == distributorId).ToList();
        }

        public void UpdateAgreementStatus(int creditDataId, string status)
        {
            var creditDataEntity = this._context.CreditData.FirstOrDefault(x => x.Id == creditDataId);
            if (creditDataEntity != null)
            {
                creditDataEntity.Status = status;
                //creditDataEntity.LastUpdate
                //this._context.Update(creditDataEntity);
                this._context.SaveChanges();
            }
        }
    }
}