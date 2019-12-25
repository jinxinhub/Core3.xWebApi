using Core3.xWebApi.Data;
using Core3.xWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core3.xWebApi.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly WebApiDbContext _context;

        public CompanyRepository(WebApiDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetCompany()
        {
            return _context.Companies.ToList();
        }
    }
}
