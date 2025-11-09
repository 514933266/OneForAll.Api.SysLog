using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using OneForAll.File;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SysLog.Host.Filters;
using SysLog.Host.Model;
using SysLog.Host.Models;
using SysLog.Host.Providers;
using SysLog.HttpService.Models;
using SysLog.Public.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace SysLog.Host
{
    public class Startup
    {
        const string CORS = "Cors";
        const string AUTH = "Auth";
        const string QUARTZ = "Quartz";
        const string BASE_HOST = "SysLog.Host";
        const string BASE_APPLICATION = "SysLog.Application";
        const string BASE_DOMAIN = "SysLog.Domain";
        const string BASE_REPOSITORY = "SysLog.Repository";
        const string HTTP_SERVICE_KEY = "HttpService";
        const string HTTP_SERVICE = "SysLog.HttpService";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public void ConfigureServices(IServiceCollection services)
        {
            #region Cors

            var corsConfig = new CorsConfig();
            Configuration.GetSection(CORS).Bind(corsConfig);
            if (corsConfig.Origins.Contains("*") || !corsConfig.Origins.Any())
            {
                // 不限制跨域
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                ));
            }
            else
            {
                services.AddCors(option => option.AddPolicy(CORS, policy => policy
                    .WithOrigins(corsConfig.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod().
                    AllowCredentials()
                ));
            }

            #endregion

            #region Http请求服务

            var serviceConfig = Configuration.GetSection(HTTP_SERVICE_KEY).Get<HttpServiceConfig>();
            var props = OneForAll.Core.Utility.ReflectionHelper.GetPropertys(serviceConfig);
            props.ForEach(e =>
            {
                services.AddHttpClient(e.Name, c =>
                {
                    c.BaseAddress = new Uri(e.GetValue(serviceConfig).ToString());
                    c.DefaultRequestHeaders.Add("ClientId", ClientClaimType.Id);
                });
            });

            #endregion

            #region Jwt

            var authConfig = Configuration.GetSection(AUTH).Get<AuthConfig>();
            // 1. 添加 JWT Bearer 认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // 指向你的 OAuth 服务（IdentityServer / Duende）
                options.Authority = authConfig.Authority; // 例如 https://oauth.yourcompany.com

                // 指定资源名称
                options.Audience = authConfig.ClientCode;

                // 可选：跳过 HTTPS（仅开发环境）
                options.RequireHttpsMetadata = false;

                // 可选：自定义 token 验证逻辑（如验证特定 claim）
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    RoleClaimType = ClaimTypes.Role
                    // 其他参数由 Authority 自动获取（通过 .well-known/openid-configuration）
                };
            });

            // 2. 添加授权
            services.AddAuthorization();

            #endregion

            #region AutoMapper
            services.AddAutoMapper(config =>
            {
                config.AllowNullDestinationValues = false;
            }, Assembly.Load(BASE_HOST));
            #endregion

            #region DI

            services.AddScoped<IUploader, Uploader>();
            services.AddScoped<ITenantProvider, TenantProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpServiceConfig>();
            services.AddSingleton(authConfig);
            #endregion

            #region Mvc
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ApiModelStateFilter>();
                options.Filters.Add<ExceptionFilter>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #endregion

            #region Redis

            var redisOption = Configuration.GetSection("Redis").Get<RedisOption>();
            if (redisOption?.IsEnabled == true)
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisOption.ConnectionString;
                    options.InstanceName = redisOption.InstanceName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            #endregion

            #region Quartz

            // 读取 Quartz 定时任务配置
            var quartzConfig = Configuration.GetSection(QUARTZ).Get<QuartzScheduleJobConfig>();
            services.AddSingleton(quartzConfig); // 注册配置为单例

            // 注册自定义 Job 工厂和调度器工厂
            services.AddSingleton<IJobFactory, ScheduleJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // 如果定时任务功能已启用
            if (quartzConfig != null && quartzConfig.IsEnabled)
            {
                // 添加后台服务来启动和管理 Quartz 调度器
                services.AddHostedService<QuartzJobHostedService>();

                // 构建任务所在的命名空间（例如：YourApp.QuartzJobs）
                var jobNamespace = BASE_HOST.Append(".QuartzJobs");

                // 遍历配置中列出的每个定时任务
                quartzConfig.ScheduleJobs.ForEach(e =>
                {
                    var typeName = jobNamespace + "." + e.TypeName;
                    // 通过反射加载任务类型
                    var jobType = Assembly.Load(BASE_HOST).GetType(typeName);
                    if (jobType != null)
                    {
                        e.JobType = jobType;
                        // 将任务类型注册为单例（供 JobFactory 创建实例）
                        services.AddSingleton(e.JobType);
                    }
                });
            }

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Http数据服务
            builder.RegisterAssemblyTypes(Assembly.Load(HTTP_SERVICE))
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // 应用层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_APPLICATION))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            // 领域层
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_DOMAIN))
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces();

            // 仓储层
            builder.Register(p =>
            {
                var optionBuilder = new DbContextOptionsBuilder<SysLogDbContext>();
                optionBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"]);
                return optionBuilder.Options;
            }).AsSelf();

            builder.RegisterType<SysLogDbContext>().Named<DbContext>("SysLogDbContext");
            builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("SysLogDbContext"))
               .AsImplementedInterfaces();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DirectoryHelper.Create(Path.Combine(Directory.GetCurrentDirectory(), @"upload"));
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"upload")),
                RequestPath = new PathString("/resources"),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                }
            });

            app.UseCors(CORS);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
