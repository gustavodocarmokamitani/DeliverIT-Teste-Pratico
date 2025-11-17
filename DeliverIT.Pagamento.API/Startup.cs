using DeliverIT.Pagamento.API.Filters;
using DeliverIT.Pagamento.Application.Interfaces;
using DeliverIT.Pagamento.Application.Services;
using DeliverIT.Pagamento.Domain.Interfaces;
using DeliverIT.Pagamento.Infrastructure.Data;
using DeliverIT.Pagamento.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger; 
using Swashbuckle.AspNetCore.Newtonsoft;

namespace DeliverIT.Pagamento.API
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

            services.AddDbContext<PagamentoDbContext>(options =>
               options.UseNpgsql(
                   Configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly("DeliverIT.Pagamento.Infrastructure")
               )
           );

            services.AddScoped<IContaPagarRepository, ContaPagarRepository>();

            services.AddScoped<IContaPagarService, ContaPagarService>();


            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilter));
            })

            .AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DeliverIT.Pagamento.API", Version = "v1" });
            })
            .AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                 
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeliverIT.Pagamento.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
