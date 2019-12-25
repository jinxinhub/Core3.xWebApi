using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core3.xWebApi.Data;
using Core3.xWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Core3.xWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����������� ע�����
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddDbContext<WebApiDbContext>(options =>
            {
                options.UseSqlite("Data Source=company.db");
            });
        }

        /// <summary>
        /// ��ConfigureServices֮����ã���������ܵ�����Ӹ����м��
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //�жϵ�ǰ�����ǲ���Development
            if (env.IsDevelopment())
            {
                //������򷵻ؿ����쳣ҳ��
                app.UseDeveloperExceptionPage();
            }

            //·���м��
            app.UseRouting();

            //��Ȩ
            app.UseAuthorization();

            //��������䵽�����ض���Controller��Action����
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
