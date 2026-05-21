
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using WebAPI_DAY3.Models;

namespace WebAPI_DAY3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            //   builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen();

            // Adding Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<EmpContext>();
            builder.Services.AddDbContext<EmpContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            #region JWT Bearer Validation
            builder.Services.AddAuthentication(
      op =>
      {
          op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             op.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
          // op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(
      op =>
      {
          #region  key
          string key = builder.Configuration["JWT:key"];
          var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));

          #endregion
          op.TokenValidationParameters = new TokenValidationParameters()
          {
              ValidateAudience = false,
              ValidateIssuer = false,
              IssuerSigningKey = secretkey,
              ValidateLifetime = true,
          };





      });
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/openapi/v1.json","v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
