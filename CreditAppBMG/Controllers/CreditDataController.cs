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

            //var creditDataFiles = _context.CreditDataFiles.FirstOrDefault(x => x.CreditDataId == creditDataEntity.Id);
            //if (creditDataFiles != null)
            //{
            //    viewModel.CreditDataFiles = _mapper.Map<CreditDataFiles>(creditDataFiles);
            //}

            return View("DistributorView", distributorViewModel);
        }

        [HttpGet]
        [Route("/ShowDocument")]
        public IActionResult ShowDocument(int creditDataId, string agreementId)
        {
            AdobeSignWS ws = new AdobeSignWS();

            var documentUrl = ws.GetAgreementDocumentUrl(agreementId, creditDataId);
            //Response.Headers.Add("Content-Disposition", "inline; filename=Distributor_Credit_Application.pdf");
            return Redirect(documentUrl);
        }

        // GET: api/CreditData
        [HttpGet]
        public IEnumerable<CreditDataEntity> GetCreditData_1()
        {
            return _context.CreditData.ToList();
        }



        //// GET: api/CreditData/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetCreditData([FromRoute] int? id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var creditData = await _context.CreditData.FindAsync(id);

        //    if (creditData == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(creditData);
        //}

        //// PUT: api/CreditData/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCreditData([FromRoute] int? id, [FromBody] CreditDataEntity creditData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != creditData.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(creditData).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CreditDataExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/CreditData
        //[HttpPost]
        //public async Task<IActionResult> PostCreditData([FromBody] CreditDataEntity creditData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.CreditData.Add(creditData);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCreditData", new { id = creditData.Id }, creditData);
        //}

        //// DELETE: api/CreditData/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCreditData([FromRoute] int? id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var creditData = await _context.CreditData.FindAsync(id);
        //    if (creditData == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CreditData.Remove(creditData);
        //    await _context.SaveChangesAsync();

        //    return Ok(creditData);
        //}

        //private bool CreditDataExists(int? id)
        //{
        //    return _context.CreditData.Any(e => e.Id == id);
        //}
    }
}