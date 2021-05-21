namespace DepositsComparisonDomainLogicAPI
{
    using DepositsComparison.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Services;
    using Services.DataCollecting;
    using Services.Interfaces;

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
                .AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DepositsComparisonDomainLogicAPI", Version = "v1"});
            });
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddTransient<IWebScraper, MoitePariScraper>();
            services.AddTransient<IBankProductsService, BankProductsService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IDepositsService, DepositsService>();
            services.AddTransient<IInterestService, InterestService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* ToDo:
            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }
            */
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DepositsComparisonDomainLogicAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}