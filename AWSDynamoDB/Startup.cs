using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using AWSProductListDynamoDb.AWSProductListDynamoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AWSDynamoDB
{
    public class Startup
    {
        public Startup()
        {
            //Configuration = configuration;
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AWS service config
            services.AddMvcCore();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);
            Environment.SetEnvironmentVariable("AWS_CONTENT", Configuration["VAR:TableName"]);

            services.AddAWSService<IAmazonDynamoDB>();

            services.AddSingleton<IAWSProductListDynamoDbExamples, AWSProductListDynamoDbExamples>();
            services.AddSingleton<IInsertItem, InsertItem>();
            services.AddSingleton<IQueryItem, QueryItem>();
            services.AddSingleton<IDeleteItem, DeleteItem>();
            services.AddSingleton<IUpdateItem, UpdateItem>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
