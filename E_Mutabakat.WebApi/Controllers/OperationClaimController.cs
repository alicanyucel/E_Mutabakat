﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Mutabakat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [HttpPost("add")]
        public IActionResult Add(OperationClaim operationClaim)
        {
            var result = _operationClaimService.Add(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(OperationClaim operationClaim)
        {
            var result = _operationClaimService.Update(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(OperationClaim operationClaim)
        {
            var result = _operationClaimService.Delete(operationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var result = _operationClaimService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getList")]
        public IActionResult GetList()
        {
            var result = _operationClaimService.GetList();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
   