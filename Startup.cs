using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using URLService.Services;
using URLService.Services.HealthCheck;
using Newtonsoft.Json;

namespace URLService
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",

                new OpenApiInfo
                {

                    Title = "URLService",
                    Version = "v1",
                    Description = ".Net Service Academic",
                    TermsOfService = new Uri("https://github.com/Gomes-Gomes/URLService"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contact",
                        Email = "",
                        Url = new Uri("https://github.com/Gomes-Gomes/URLService")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://github.com/Gomes-Gomes/URLService/blob/main/LICENSE")
                    }

                });


                c.DocumentFilter<JsonPatchDocumentFilter>();

                foreach (var filePath in System.IO.Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
                {
                    try
                    {
                        c.IncludeXmlComments(filePath);
                    }
                    catch (Exception e)
                    {
                        //colocar aqui um log
                        Console.WriteLine(e);
                    }
                }
            });
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //comentamos o modo de dev para testes
            //if (env.IsDevelopment())
            //{
                app.UseStaticFiles();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "URLService v1");
                    c.RoutePrefix = string.Empty;
                    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");

                });
            //}

            //Tiveos de Remover para correr o Cntainer no Docker
            //warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
            //Failed to determine the https port for redirect.
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //QuickHealthCheck
            app.UseHealthChecks("/quickhealth", new HealthCheckOptions
            {
                Predicate = _ => false
            });

            //HealthCheck
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        checks = report.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),

                        Duration = report.TotalDuration
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }

            });
        }
    }
}
