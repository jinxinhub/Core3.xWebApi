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
using System.IO;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "JinXinApi", Version = "v1" });
                // ���س��򼯵�xml�����ĵ�
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
            services.AddStackExchangeRedisCache(option => {
                option.Configuration = "127.0.0.1:6379";//��Configuration������redis�����ӡ�
                option.InstanceName = "WebApiDemoRedis";//InstaceName��ʵ����������redis��keyǰ��ġ�
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

            //ʹ�þ�̬�ļ�
            app.UseStaticFiles();

            //·���м��
            app.UseRouting();

            //��Ȩ
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            //��������䵽�����ض���Controller��Action����
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
