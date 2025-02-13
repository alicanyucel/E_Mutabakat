﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Mutabakat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountReconciliationController : ControllerBase
    {
        public readonly IAccountReconcliationService _accountReconcliationService;
        public AccountReconciliationController(IAccountReconcliationService accountReconcliationService)
        {
            _accountReconcliationService = accountReconcliationService;
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(AccountReconclition accountReconclition)
        {
            var result = _accountReconcliationService.Delete(accountReconclition);
            if(result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result.Message);
        }
        [HttpGet("GetList")]
        public IActionResult GetList(int companyid)
        {
            var result = _accountReconcliationService.GetList(companyid);
            if (result.Success)
            {
                return Ok(result);

            }

            return BadRequest(result.Message);
        }
            [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _accountReconcliationService.GetById(id);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPut("Update")]
        public IActionResult Update(AccountReconclition accountReconclition)
        {

            var result = _accountReconcliationService.Update(accountReconclition);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("addFromExcels")]
        public IActionResult AddFromExcels(IFormFile file, int companyId)
        {
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + ".xlsx";
                var filePath = $"{Directory.GetCurrentDirectory()}/Content/{fileName}";
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                    stream.Flush();
                }
                var result = _accountReconcliationService.AddExcel(filePath, companyId);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
            return BadRequest("Dosya secimi yapmadiniz");
        }
        [HttpPost("add")]
        public IActionResult Add(AccountReconclition accountReconclition)
        {
            var result = _accountReconcliationService.Add(accountReconclition);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
