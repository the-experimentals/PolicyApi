using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PolicyApi.Data;
using PolicyApi.Policy;
using PolicyApi.Services.gRPC.Services;
using PolicyApi.Services.SQLServer;
using PolicyApi.StartupTasks;
using PolicyApi.StartupTasks.Tasks;
using PolicyApi.Utilities;

namespace PolicyApi
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
            services.AddGrpc();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddDbContext<PolicyStore>(options => options.UseSqlServer(Configuration.GetConnectionString("PolicyStoreConnectionString")));

            // configure strongly typed settings objects
            var JwtSecretKeySection = Configuration.GetSection("JwtSecretKey");
            services.Configure<JwtSecretKey>(JwtSecretKeySection);

            // configure jwt authentication.
            var jwtSettings = JwtSecretKeySection.Get<JwtSecretKey>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SECRET);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });

            services.AddScoped<IPolicyManager, PolicyManager>();
            services.AddScoped<DBInitializer>();
            services.AddSingleton<TMCache>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddStartupTask<DBMigrator>();
            services.AddStartupTask<WarmupServices>().TryAddSingleton(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBInitializer dBInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dBInitializer.Initialize();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<PolicyApiService>().RequireHost("*:6900").EnableGrpcWeb();
                endpoints.MapControllers();
            });
        }
    }
}
