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


namespace CreditAppBMG.Controllers
{
    [Route("api/[controller]")]
    public class CreditDataController : Controller
    {
        private readonly CreditAppContext _context;
        private readonly IMapper _mapper;

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
            return Ok(creditDataEntity);
        }

        [HttpGet]
        [Route("/Distributor")]
        public IActionResult GetDistributorView(string token)
        {
            string baseUrl = "https://webservice.bevmedia.com/BMGOrderWebService/api";

            BevMediaService bevMediaService = new BevMediaService(baseUrl);
            TokenInfo tokenInfo= bevMediaService.VerifyToken(token, out var err);
            RetailerInfo retailer = bevMediaService.GetRetailerInfo(tokenInfo, out var errMsg);
            var creditDataList=_context.CreditData
                .Where(x => x.DistributorId == tokenInfo.DistribuitorID)
                .Include(files=>files.CreditDataFiles)
                .ToList();
            var creditDataListModel = _mapper.Map<List<CreditData>>(creditDataList);
            var distributorViewModel = new DistributorViewModel();
            distributorViewModel.CreditDataList = creditDataListModel;

            distributorViewModel.Distributor = new Distributor();
            distributorViewModel.Distributor.DistributorLogoURL = retailer.DistributorLogoURL;
            distributorViewModel.Distributor.DistributorName = retailer.DistributorName;
                      
            return View("DistributorView", distributorViewModel);
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
                    creditDataEntity.Comments = model.Comments;
                    _context.SaveChanges();
                }
            }
            var retUrl = Url.Action("GetDistributorView", "CreditData", new { token = model.Token });
            return Json(new { url = retUrl });
        }

    }
}