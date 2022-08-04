using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManager.Auth.Authentication;
using UserManager.Data;
using UserManager.Data.Dapper;
using UserManager.Data.EntityFramework;
using UserManager.Filters;

namespace UserManager
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, DapperRepository>();
            services.AddScoped<IDatabaseConnection, SqlServerDatabaseConnection>();
            services.AddScoped<MailService>();
            services.AddScoped<TokenService>();
            
            services.AddControllers();
            /*options => 
                options.Filters.Add(new ExceptionFilter())*/

            services.AddCors();
            
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("Auth")));
            /*services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("Users")));*/

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    
                    options.User.RequireUniqueEmail = true;
                    
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<IdentityContext>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true
                    };
                });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Market", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader());
 
            app.UseRouting();
 
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","Market API"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}