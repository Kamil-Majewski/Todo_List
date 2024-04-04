using Microsoft.EntityFrameworkCore;
using Todo_List.Infrastructure;

namespace Todo_List
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var todoListConnectionString = builder.Configuration.GetConnectionString("TodoListConnectionString") ?? throw new InvalidOperationException($"Could not get database connection string");
            builder.Services.AddDbContext<TodoListDbContext>(options =>
                options.UseSqlServer(todoListConnectionString));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}