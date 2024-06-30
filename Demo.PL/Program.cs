using Demo.BLL.Interfaces;
using Demo.BLL.Repostories;
using Demo.BLL;
using Demo.PL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Demo.PL.Helper;
using Demo.DAL.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Demo.DAL.Models;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

			#region Configure Service That is Allow Dependenci Injection	
			Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			//services.AddScoped<AppDbContext>();
			//services.AddAutoMapper(typeof(MappingProfiles));
			Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			Builder.Services.AddScoped<IScopedService, ScopedService>();
			Builder.Services.AddTransient<ITransientService, TransientService>();
			Builder.Services.AddSingleton<ISingletoneService, SingletoneService>();



			Builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			Builder.Services.AddControllersWithViews();
			//services.AddScoped<UserManager<ApplicationUser>>();
			//services.AddScoped<SignInManager<ApplicationUser>>();
			Builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();
			Builder.Services.ConfigureApplicationCookie(config =>
			{
				config.LoginPath = "/Account/SignIn";
				config.AccessDeniedPath = "/Account/AccessDenied";

			});
			#endregion

			var app = Builder.Build();

			#region Configure HTTP Request pipline Or Middlewares
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			#endregion

			app.Run();
		}


    }
}
