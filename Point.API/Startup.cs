using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Point.API.Customizations.ExceptionFilters;
using Point.Common.LoggerProvider;
using Point.Common.Storage.Common.Options;
using Point.Data.Models;
using Point.Data.Repositories;
using Point.Data.Repositories.Contracts;
using Point.Services.ImageService;
using Point.Services.ImageService.Model;
using Point.Services.InfoPostService;
using Point.Services.InfoPostService.Model;
using Point.Services.InstitutionService;
using Point.Services.InstitutionService.Model;
using Point.Services.RegisterService;
using Point.Services.RegisterService.Model;
using Point.Services.ReturnImageService;
using Point.Services.ReturnImageService.Model;
using Swashbuckle.AspNetCore.Swagger;

namespace Point
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config => config.Filters.Add(typeof(GlobalExceptionFilter)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Point.API", Version = "v1" });
            });

            services.AddDbContext<PointAdvisorContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PointConnection")));
            services.Configure<AzureStorageOptions>(Configuration.GetSection("AzureStorage"));

            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IInfoPostRepository, InfoPostRepository>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVotePostRepository, VotePostRepository>();
            services.AddScoped<IZoneRepository, ZoneRepository>();
            services.AddScoped<IZonePointRepository, ZonePointRepository>();

            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IInfoPostService, InfoPostService>();
            services.AddScoped<IStorageImageService, FtpStorageImageService>();
            services.AddScoped<IInstitutionService, InstitutionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory
                    .WithFilter(new FilterLoggerSettings
                    {
                        { "Microsoft", LogLevel.Warning },
                        { "System", LogLevel.Warning }
                    })
                    .AddDebug();

            loggerFactory.AddCustomLogger(LogLevel.Information);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseRequestLogging();
            //    app.UseResponseLogging();
            //}

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Point.API V1");
            });
        }
    }
}
