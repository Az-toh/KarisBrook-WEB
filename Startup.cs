using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KarisBrook.Data;
using KarisBrook.Models;
using System.Globalization;

namespace KarisBrook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // 1. Подключение к базе данных
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // 2. НАСТРОЙКА IDENTITY С РУССКИМИ ТРЕБОВАНИЯМИ К ПАРОЛЮ
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Настройки пароля
                options.Password.RequireDigit = true;          // Требовать цифры
                options.Password.RequireLowercase = true;      // Требовать строчные буквы
                options.Password.RequireNonAlphanumeric = false; // Не требовать спецсимволы (!@#$)
                options.Password.RequireUppercase = true;      // Требовать заглавные буквы
                options.Password.RequiredLength = 6;           // Минимальная длина 6 символов
                options.Password.RequiredUniqueChars = 1;      // Минимальное количество уникальных символов

                // Настройки пользователя
                options.User.RequireUniqueEmail = true;        // Email должен быть уникальным
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // 3. Настройка cookies (опционально)
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // 4. Добавление контроллеров с представлениями
            services.AddControllersWithViews();

            // 5. Добавление сессий (для корзины)
            services.AddSession();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization(); // <-- ДОБАВЬТЕ ЭТУ СТРОЧКУ

            app.UseAuthentication();
            app.UseAuthorization();
            // ВАЖНО: аутентификация и авторизация должны быть именно в этом порядке
           

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}