using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TennisClub.Api.Extensions;

namespace TennisClub.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApplicationServices();

            services.AddMongoDbProvider(Configuration);

            services.AddJwtAuthentication(Configuration);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tennis Club Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    var exeptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if(exeptionHandlerPathFeature.Error is Exception ex)
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(ex.Message);
                    }
                });
            });

            var swaggerSection = Configuration.GetSection("SwaggerOptions");

            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerSection["JsonRoute"];
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerSection["UiEndpoint"], swaggerSection["Description"]);
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
