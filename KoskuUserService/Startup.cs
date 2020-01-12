using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using FluentValidation.AspNetCore;
using KoskuUserService.AutoMappers;
using KoskuUserService.Filters;
using KoskuUserService.Managers;
using KoskuUserService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KoskuUserService
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
            services
                .AddMvc(c => c.Filters.Add(new ModelStateFilter()))
                .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(c => c.JsonSerializerOptions.IgnoreNullValues = true);

            // Versioning
            services.AddApiVersioning(c =>
            {
                c.ReportApiVersions = true;
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.DefaultApiVersion = new ApiVersion(1, 0);
                c.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddVersionedApiExplorer(c => c.GroupNameFormat = "'v'VVV");

            //Swagger
            services.AddSwaggerGen();

            // AutoMapper
            services.AddAutoMapper(c =>
            {
                c.AddProfile<UserProfile>();
            }
            , typeof(Startup));

            // AWS
            services.AddAWSService<IAmazonDynamoDB>();

            // Dependency Injections
            services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
