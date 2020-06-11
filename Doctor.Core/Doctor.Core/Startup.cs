using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Doctor.Core.AuthHelper;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Doctor.Core.AOP;
using Doctor.Core.Common.MemoryCache;
using Doctor.Core.Common.Helper;
using Doctor.Core.Repository;
using Doctor.Core.Common.Redis;
using AutoMapper;
using log4net;
using log4net.Config;
using Doctor.Core.Log;
using log4net.Repository;

namespace Doctor.Core
{
    public class Startup
    {
        /// <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //log4net
            repository = LogManager.CreateRepository("");//需要获取日志的仓库名，也就是你的当然项目名

            //指定配置文件，如果这里你遇到问题，应该是使用了InProcess模式，请查看Blog.Core.csproj,并删之 
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));//配置文件
        }

        public IConfiguration Configuration { get; }

        public string ApiName { get; set; } = "Doctor.Core";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddScoped<ICaching, MemoryCaching>();//记得把缓存注入！！！
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();//这里说下，如果是自己的项目，个人更建议使用单例模式 
            //log日志注入                                                              
            services.AddSingleton<ILoggerHelper, LogHelper>();
            services.AddAutoMapper(typeof(Startup));

            BaseDBConfig.ConnectionString = Configuration.GetSection("AppSettings:SqlServer:SqlServerConnection").Value;

            var basePath = AppContext.BaseDirectory;

            var appsettings = new Appsettings(basePath);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} 定义成全局变量，方便修改
                    Version = "V1",
                    Title = $"{ApiName} 接口文档——Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                    Contact = new OpenApiContact { Name = ApiName, Email = "Doctor.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                    License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                });
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(basePath, "Doctor.Core.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "Doctor.Core.Model.xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);

                //c.OperationFilter<AddResponseHeadersFilter>();
                //c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //c.OperationFilter<SecurityRequirementsOperationFilter>();

                //#region Token绑定到ConfigureServices
                //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                //    Name = "Authorization",//jwt默认的参数名称
                //    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                //    Type = SecuritySchemeType.ApiKey
                //});
                //#endregion
            });

            // 1【授权】、这个和上边的异曲同工，好处就是不用在controller中，写多个 roles 。
            // 然后这么写 [Authorize(Policy = "Admin")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));
            //});
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;

            //直接注册某一个类和接口
            //左边的是实现类，右边的As是接口
            // builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();

            builder.RegisterType<DoctorLogAOP>();//可以直接替换其他拦截器！一定要把拦截器进行注册
            // builder.RegisterType<DoctorCacheAOP>();
            builder.RegisterType<DcotorRedisCacheAOP>();

            //注册要通过反射创建的组件
            var servicesDllFile = Path.Combine(basePath, "Doctor.Core.Services.dll");
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);

            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope()
                      .EnableInterfaceInterceptors()
                      .InterceptedBy(typeof(DoctorLogAOP), typeof(DcotorRedisCacheAOP));//可以放一个AOP拦截器集合

            //  builder.RegisterAssemblyTypes(assemblysServices)
            //.AsImplementedInterfaces()
            //.InstancePerLifetimeScope()
            //.EnableInterfaceInterceptors();//可以放一个AOP拦截器集合

            var repositoryDllFile = Path.Combine(basePath, "Doctor.Core.Repository.dll");
            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
            });

            //自定义认证中间件
            // app.UseJwtTokenAuth(); //也可以app.UseMiddleware<JwtTokenAuth>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
