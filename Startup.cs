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
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

            //����ע�빫˾����������
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddDbContext<WebApiDbContext>(options =>
            {
                //�������ݿ�����,���EF��Ǩ����� �������ĿĿ¼������һ��DB�ļ�
                options.UseSqlite("Data Source=company.db");
            });

            //��ȡToken��֤��Secret
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"]));
            services.AddAuthentication("Bearer").AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    //�Ƿ�����Կ��֤��keyֵ
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    //�Ƿ�����������֤�ͷ�����
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Audience:Issuer"],

                    //�Ƿ�����������֤�Ͷ�����
                    ValidateAudience = true,
                    ValidAudience = Configuration["Audience:Audience"],

                    //��֤ʱ���ƫ����
                    ClockSkew = TimeSpan.Zero,
                    //�Ƿ���ʱ����֤
                    ValidateLifetime = true,
                    //�Ƿ�����Ʊ�����й���ʱ��
                    RequireExpirationTime = true
                };
            });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                option.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                option.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "JinXinApi", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });

                // ���س��򼯵�xml�����ĵ�
                // 1.��ȡ����ʱ��Ŀ¼
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                // 2.������Ǹո����õ�xml�ļ���
                var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "127.0.0.1:6379";//��Configuration������redis�����ӡ�
                option.InstanceName = "WebApiDemoRedis";//InstaceName��ʵ����������redis��keyǰ��ġ�
            });
        }

        /// <summary>
        /// ��ConfigureServices֮����ã���������ܵ�����Ӹ����м��
        /// �ؼ��㣺HTTP�ܵ������Ⱥ�˳���
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
            else
            {
                app.UseHsts();
            }

            //            app.UseAuthorization();

            //ʹ�þ�̬�ļ�
            app.UseStaticFiles();

            //·���м��
            app.UseRouting();

            #region ��֤����Ȩ

            //��Ҫ��½��̳�������û�������������1234��������ȷ��֤��������ȷʵ������������� authentication����һcheck�û������Ǹ�������������Ȩ�޼Ӿ�ɾ������������� authorization��
            //��֤�м��
            app.UseAuthentication();
            //��Ȩ�м��
            app.UseAuthorization();

            #endregion ��֤����Ȩ

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            //��������䵽�����ض���Controller��Action����
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
