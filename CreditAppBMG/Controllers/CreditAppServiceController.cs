using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreditAppBMG.Entities;
using CreditAppBMG.Enums;
using CreditAppBMG.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CreditAppBMG.Controllers
{

    [Produces("application/json")]
    [Route("api/CreditAppService")]
    [ApiController]
    public class CreditAppServiceController : ControllerBase
    {
        private readonly CreditAppRepository repository = new CreditAppRepository();

        [HttpPost("DocEvents")]
        public Task<bool> UpdateCreditApplicationStatus([FromBody]WebHookInfo webHookInfo)
        {
            using (var context = new CreditAppContext())
            {
                var creditDataEntity = context.CreditData.SingleOrDefault(x =>
                    x.AdobeSignAgreementId == webHookInfo.agreement.id);
                if (creditDataEntity != null)
                {
                    creditDataEntity.Status = webHookInfo.Event;

                    context.Update(creditDataEntity);
                    context.SaveChanges();
                }
            }
            //log
            repository.AddAdobeSignLog("UpdateCreditApplicationStatus", $"AgreementId={webHookInfo.agreement.id}", webHookInfo.ToJson());

            return Task.FromResult(true);
        }
    }
}