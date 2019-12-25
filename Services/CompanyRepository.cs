using Core3.xWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core3.xWebApi.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetCompany()
        {
            throw new NotImplementedException();
        }
    }
}
