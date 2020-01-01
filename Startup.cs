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
        /// 像服务容器中 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //依赖注入公司服务类数据
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddDbContext<WebApiDbContext>(options =>
            {
                //新增数据库链接,如果EF的迁移完成 则会再项目目录下生产一个DB文件
                options.UseSqlite("Data Source=company.db");
            });

            //获取Token认证的Secret
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Audience:Secret"]));
            services.AddAuthentication("Bearer").AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    //是否开启密钥认证和key值
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    //是否开启发行人认证和发行人
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Audience:Issuer"],

                    //是否开启订阅人认证和订阅人
                    ValidateAudience = true,
                    ValidAudience = Configuration["Audience:Audience"],

                    //认证时间的偏移量
                    ClockSkew = TimeSpan.Zero,
                    //是否开启时间认证
                    ValidateLifetime = true,
                    //是否该令牌必须带有过期时间
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
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
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

                // 加载程序集的xml描述文档
                // 1.获取运行时根目录
                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                // 2.这个就是刚刚配置的xml文件名
                var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "127.0.0.1:6379";//　Configuration：连接redis的链接。
                option.InstanceName = "WebApiDemoRedis";//InstaceName：实例名，加在redis的key前面的。
            });
        }

        /// <summary>
        /// 在ConfigureServices之后调用，配置请求管道，添加各种中间件
        /// 关键点：HTTP管道是有先后顺序的
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
            else
            {
                app.UseHsts();
            }

            //            app.UseAuthorization();

            //使用静态文件
            app.UseStaticFiles();

            //路由中间件
            app.UseRouting();

            #region 认证和授权

            //你要登陆论坛，输入用户名张三，密码1234，密码正确，证明你张三确实是张三，这就是 authentication；再一check用户张三是个版主，所以有权限加精删别人帖，这就是 authorization。
            //认证中间件
            app.UseAuthentication();
            //授权中间件
            app.UseAuthorization();

            #endregion 认证和授权

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            //把请求分配到各个特定的Controller和Action上面
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
