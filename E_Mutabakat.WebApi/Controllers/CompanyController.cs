﻿using E_Mutabakat.Business.Abstract;
using E_Mutabakat.Entities.Concrete;
using E_Mutabakat.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace E_Mutabakat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyServices _companyServices;
        public CompanyController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }
        [HttpGet("GetCompanyList")]
        public IActionResult GetCompanyList()
        {
            var result=_companyServices.GetList();
            if(result.Success)
            {
                return Ok(result);
            }
            // veriler basarılı bir sekilde çekildi.
            return BadRequest(result.Message);
        }
        [HttpGet("GetCompany")]
        public IActionResult GetById(int id)
        {
            var result = _companyServices.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            // veriler basarılı bir sekilde çekildi.
            return BadRequest(result.Message);
        }
        [HttpPost("addCompanyAndUserCompany")]
        public IActionResult AddCompanyaAndUserCompany (CompanyDto companyDto)
        {
            var result = _companyServices.AddCompanyAndUserCompany(companyDto);
            if(result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost("UpdateCompany")]
        public IActionResult UpdateCompanyAndUserCompany(Company company)
        {
            var result = _companyServices.Update(company);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
