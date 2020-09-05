// -----------------------------------------------------------------
// <copyright>Copyright (C) 2020, David Ruiz.</copyright>
// Licensed under the Apache License, Version 2.0.
// You may not use this file except in compliance with the License:
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Software is distributed on an "AS IS", WITHOUT WARRANTIES
// OR CONDITIONS OF ANY KIND, either express or implied.
// -----------------------------------------------------------------

namespace Todos.WebApi
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Todos.WebApi.Formatters.Yaml;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Startup method")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddYamlFormatter(); // Adding Yaml Formatter
            services.AddSwaggerGen(options =>
            {
                var version = "v1";

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"Todos Web API {version}",
                    Version = version,
                    Description = "Todos API",
                    Contact = new OpenApiContact
                    {
                        Name = "ACME Company",
                        Email = string.Empty,
                        Url = new Uri("https://todos.acme.com/"),
                    }
                });
            });

            services.AddControllers();

            var databasemode = Configuration["Databasemode"].ToLowerInvariant();
            var databaseName = Configuration["DatabaseName"];

            if (databasemode == "sqlite")
            {
                services.AddDbContext<TodosContext>(opt => opt.UseSqlite(databaseName));
            }
            else
            {
                services.AddDbContext<TodosContext>(opt => opt.UseInMemoryDatabase(databaseName));
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Startup method")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TodosContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}