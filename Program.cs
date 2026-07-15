using KarisBrook.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace KarisBrook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Запуск приложения ===");

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    Console.WriteLine("=== Попытка получить контекст базы данных ===");
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    Console.WriteLine("=== Контекст получен. Запуск инициализатора ===");
                    DbInitializer.Initialize(context);
                    Console.WriteLine("=== Инициализация базы данных завершена успешно ===");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"=== ОШИБКА при инициализации базы данных: {ex.Message} ===");
                    Console.WriteLine($"=== Стек вызовов: {ex.StackTrace} ===");
                }
            }

            Console.WriteLine("=== Запуск веб-хоста ===");
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}