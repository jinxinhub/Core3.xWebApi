using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core3.xWebApi.Entities;
using Core3.xWebApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core3.xWebApi.Controllers
{
    [ApiController]
    public class CompanysController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanysController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet]
        [Route("Companys/GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepository.GetCompany();
            return new JsonResult(companies);
        }

        [HttpPost]
        [Route("Companys/InsertNewCompany")]
        public async Task<IActionResult> InsertNewCompany(Company company)
        {
            var companies = await _companyRepository.InsertNewCompany(company);
            return new JsonResult(companies);
        }
    }
}
