using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Salon.Application;
using Salon.Infra;
using Salon.Infra.CollectionDefinitions;
using VideoStore.Api.Middlewares;

namespace SalonApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            services.AddTransient<ILogger, Logger<object>>();
            services.AddMongoDbDependencyInjection(Configuration);
            services.AddInfraDependencyInjection();
            services.AddCollectionDefinitions();
            services.AddAppDependencyInjection();
            services.AuthenticationConfiguration(Configuration);
            services.BuildIndexes();

            //services.AddCors(
            //    options => options.AddPolicy("AllowCors",
            //    builder => 
            //    {
            //        //builder
            //        //.WithOrigins(Configuration["Front:Url"])
            //        //.AllowAnyMethod()
            //        //.WithHeaders("Authorization", "Access-Control-Allow-Origin", "Content-type");
            //        builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //    }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
