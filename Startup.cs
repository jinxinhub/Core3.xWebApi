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
        /// 像服务容器中 注册服务
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
        /// 在ConfigureServices之后调用，配置请求管道，添加各种中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //判断当前环境是不是Development
            if (env.IsDevelopment())
            {
                //如果是则返回开发异常页面
                app.UseDeveloperExceptionPage();
            }

            //路由中间件
            app.UseRouting();

            //授权
            app.UseAuthorization();

            //把请求分配到各个特定的Controller和Action上面
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
