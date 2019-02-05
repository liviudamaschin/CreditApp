using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreditAppBMG.Entities;
using CreditAppBMG.BL;
using CreditAppBMG.Models;
using AutoMapper;
using CreditAppBMG.ViewModels;
using CreditAppBMG.Extensions;
using CreditAppBMG.Enums;

namespace CreditAppBMG.Controllers
{
    [Route("api/[controller]")]
    public class CreditDataController : Controller
    {
        private readonly CreditAppContext _context;
        private readonly IMapper _mapper;
        private readonly CreditAppRepository repository = new CreditAppRepository();

        public CreditDataController(IMapper mapper, CreditAppContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        [Route("/GetComment")]
        public IActionResult GetComment(int creditDataId)
        {
            var creditDataEntity = _context.CreditData
                    .Where(x => x.Id == creditDataId).SingleOrDefault();
            //if (creditDataEntity != null)
            //{ }
            var creditDataModel = _mapper.Map<CreditDataEntity, CreditData>(creditDataEntity);
            creditDataModel.Status = creditDataEntity.Status== "DECLINED"|| creditDataEntity.Status == "APPROVED"? creditDataEntity.Status : "";
            return Ok(creditDataModel);
        }

        [HttpGet]
        [Route("/Distributor")]
        public IActionResult GetDistributorView(string token)
        {
            string baseUrl = repository.GetKeyValue("BMGBaseUrl");// "https://webservice.bevmedia.com/BMGOrderWebService/api";
            ErrorModel errorModel = new ErrorModel();
            BevMediaService bevMediaService = new BevMediaService(baseUrl);
            TokenInfo tokenInfo= bevMediaService.VerifyToken(token, out var err);
            if (string.IsNullOrWhiteSpace(err))
            {
                DistributorInfo distributor = bevMediaService.GetDistributorInfo(tokenInfo, out var errMsg);
                //RetailerInfo distributor = bevMediaService.GetRetailerInfo(tokenInfo, out var errMsg);
                if (string.IsNullOrWhiteSpace(errMsg))
                {
                    var creditDataList = _context.CreditData
                        .Where(x => x.DistributorId == tokenInfo.DistributorID)
                        .Include(files => files.CreditDataFiles)
                        .ToList();
                    var ws = new AdobeSignWS();
                    foreach (var creditDataEntity in creditDataList)
                    {
                        if (creditDataEntity.Status == CreditAppStatusEnum.OUT_FOR_SIGNATURE.ToString())
                        {
                            var agreement = ws.GetAgreement(creditDataEntity.AdobeSignAgreementId, creditDataEntity.Id.Value);
                            if (agreement.status != creditDataEntity.Status)
                            {
                                creditDataEntity.Status = agreement.status;
                                _context.SaveChanges();
                            }
                        }
                    }
                    var creditDataListModel = _mapper.Map<List<CreditData>>(creditDataList);
                    var distributorViewModel = new DistributorViewModel();
                    distributorViewModel.CreditDataList = creditDataListModel;

                    distributorViewModel.Distributor = new Distributor();
                    distributorViewModel.Distributor.DistributorLogoURL = distributor.DistributorLogoURL;
                    distributorViewModel.Distributor.DistributorName = distributor.DistributorName;

                    return View("DistributorView", distributorViewModel);
                }
                else
                {
                    errorModel.Message = errMsg;
                }
            }
            else
            {
                errorModel.Message = err;
            }
            return View("ErrorView", errorModel);
        }

        [HttpGet]
        //[Route("/ShowDocument")]
        public IActionResult ShowDocument(int creditDataId, string agreementId)
        {
            AdobeSignWS ws = new AdobeSignWS();

            var documentUrl = ws.GetAgreementDocumentUrl(agreementId, creditDataId);
            //Response.Headers.Add("Content-Disposition", "inline; filename=Distributor_Credit_Application.pdf");
            return Redirect(documentUrl);
        }

        [HttpPost]
        [Route("/Distributor/AddComment")]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment([FromBody]AddCommentModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.CreditDataStatus))
            {
                var creditDataEntity = _context.CreditData
                    .Where(x => x.Id == model.CreditDataId).SingleOrDefault();
                if (creditDataEntity != null)
                {
                    creditDataEntity.Status = model.CreditDataStatus;
                    //creditDataEntity.Comments = model.Comments;
                    _context.SaveChanges();

                    repository.AddDistributorLogWithComments(model.CreditDataId, model.CreditDataStatus, model.Comments);
                }
            }
            var retUrl = Url.Action("GetDistributorView", "CreditData", new { token = model.Token });
            return Json(new { url = retUrl });
        }

    }
}