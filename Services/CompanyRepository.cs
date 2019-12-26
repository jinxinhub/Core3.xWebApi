using Core3.xWebApi.Data;
using Core3.xWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core3.xWebApi.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly WebApiDbContext _context;
        private RedisCacheHelper _redisCacheHelper;

        public CompanyRepository(WebApiDbContext context)
        {
            _context = context;
            _redisCacheHelper = new RedisCacheHelper();
        }
        public async Task<List<Company>> GetCompany()
        {
           var redisValueList =  await _redisCacheHelper.GetCacheListRight("Company");
           if (string.IsNullOrEmpty(redisValueList))
           {
               var listCompany = await _context.Companies.ToListAsync();
               var jsonCompany = JsonConvert.SerializeObject(listCompany.ToList());
               await _redisCacheHelper.SetCacheListRightPush("Company", jsonCompany);
               return listCompany;
           }
           var resList = JsonHelper.JsonToObj<List<Company>>(redisValueList);
            return resList;
        }

        public async Task<bool> InsertNewCompany(Company company)
        {
            var newId = await _context.Companies.AddAsync(company);
            if (newId == null)
            {
                return false;
            }
            return true;
        }
    }
}
