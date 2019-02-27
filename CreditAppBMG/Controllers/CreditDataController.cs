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

            var creditDataModel = _mapper.Map<CreditDataEntity, CreditData>(creditDataEntity);
            creditDataModel.Status = creditDataEntity.Status == CreditAppStatusEnum.DENIED.ToString() || creditDataEntity.Status == CreditAppStatusEnum.APPROVED.ToString() ? creditDataEntity.Status : "";
            return Ok(creditDataModel);
        }

        [HttpGet]
        [Route("/Distributor")]
        public IActionResult GetDistributorView(string token)
        {
            string baseUrl = repository.GetKeyValue("BMGBaseUrl");// "https://webservice.bevmedia.com/BMGOrderWebService/api";
            ErrorModel errorModel = new ErrorModel();
            BevMediaService bevMediaService = new BevMediaService(baseUrl);
            TokenInfo tokenInfo = bevMediaService.VerifyToken(token, out var err);
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
                    List<string> acceptedStatuses = new List<string> {
                        CreditAppStatusEnum.OUT_FOR_SIGNATURE.ToString(),
                        CreditAppStatusEnum.SIGNED.ToString(),
                        CreditAppStatusEnum.APPROVED.ToString(),
                        CreditAppStatusEnum.DENIED.ToString(),
                    };
                    List<string> editStatuses = new List<string> {
                        CreditAppStatusEnum.SIGNED.ToString(),
                        CreditAppStatusEnum.APPROVED.ToString(),
                        CreditAppStatusEnum.DENIED.ToString(),
                    };
                    foreach (var item in creditDataListModel)
                    {
                        if (acceptedStatuses.Contains(item.Status))
                        {
                            item.DistributorStatus = item.Status;
                        }
                        else
                        {
                            item.DistributorStatus = CreditAppStatusEnum.IN_PROGRESS.ToString();
                        }
                        if (editStatuses.Contains(item.Status))
                        {
                            item.CanAddComments = true;
                        }
                    }
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
            var creditDataEntity = _context.CreditData
                   .Where(x => x.Id == model.CreditDataId).SingleOrDefault();
            if (creditDataEntity != null)
            {
                if (!string.IsNullOrWhiteSpace(model.CreditDataStatus))
                {
                    creditDataEntity.Status = model.CreditDataStatus;
                }
                //creditDataEntity.Comments = model.Comments;
                _context.SaveChanges();

                repository.AddDistributorLogWithComments(model.CreditDataId, model.CreditDataStatus, model.Comments);
            }

            var retUrl = Url.Action("GetDistributorView", "CreditData", new { token = model.Token });
            return Json(new { url = retUrl });
        }

    }
}