using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartSchool.WebAPI.Data;

namespace SmartSchool.WebAPI
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
            services.AddDbContext<SmartContext>(
                    context => context.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            
            //services.AddSingleton<IRepository, Repository>();
            //services.AddTransient<IRepository, Repository>();
            

            services.AddControllers()
                    .AddNewtonsoftJson(
                        opt => opt.SerializerSettings.ReferenceLoopHandling = 
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRepository, Repository>();

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                });

            var apiProviderDescription = services.BuildServiceProvider()
                                                 .GetService<IApiVersionDescriptionProvider>();
            
            services.AddSwaggerGen(options =>
                    {
                        foreach (var description in apiProviderDescription.ApiVersionDescriptions)
                        {
                            options.SwaggerDoc(
                                //"smartschoolapi",
                                description.GroupName,
                                new Microsoft.OpenApi.Models.OpenApiInfo()
                                {
                                    Title = "SmartSchool API",
                                    Version = description.ApiVersion.ToString(),
                                    TermsOfService = new Uri("https://www.google.com.br/"),
                                    Description = "A descri��o do web api SmartSchol",
                                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                                    {
                                        Name = "SmartSchool Licence",
                                        Url = new Uri("https://www.google.com.br/")
                                    },
                                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                                    {
                                        Name = "Cristiano Andrade",
                                        Email = "",
                                        Url = new Uri("https://www.google.com.br/")
                                    }
                                }
                            );
                        }

                        
                        var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                        options.IncludeXmlComments(xmlCommentsFullPath);
                    }             
                );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env,
                              IApiVersionDescriptionProvider apiProviderDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var descripton in apiProviderDescription.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{descripton.GroupName}/swagger.json", 
                            descripton.GroupName.ToUpperInvariant());
                    }

                    options.RoutePrefix = "";
                }
                
                );

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
