using CinemaApp.Domain;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using CinemaApp.Repository.Implementation;
using CinemaApp.Repository.Interface;
using CinemaApp.Service;
using CinemaApp.Service.Implementation;
using CinemaApp.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<CinemaAppApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<IdentityRole>() // Add this line to enable roles
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IUserOrderRepository), typeof(UserOrderRepository));
            services.AddScoped(typeof(IMoviesRepository), typeof(MoviesRepository));
            services.AddScoped(typeof(IMovieDatesRepository), typeof(MovieDatesRepository));
            services.AddScoped(typeof(ICartsRepository), typeof(CartsRepository));
            services.AddScoped(typeof(ICartItemsRepository), typeof(CartItemsRepository));
            services.AddScoped(typeof(IUserOrderRepository), typeof(UserOrderRepository));
            services.AddScoped(typeof(IOrderItemsRepository), typeof(OrderItemsRepository));

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddTransient<IMoviesService, Service.Implementation.MoviesService>();
            services.AddTransient<ICartsService, Service.Implementation.CartsService>();
            services.AddTransient<IUserOrderService, Service.Implementation.UserOrderService>();
            services.AddScoped<IMovieDatesService, Service.Implementation.MovieDatesService>();
            services.AddScoped<ICartsService, Service.Implementation.CartsService>();
            services.AddScoped<ICartItemsService, Service.Implementation.CartItemsService>();
            services.AddScoped<IUserOrderService, Service.Implementation.UserOrderService>();
            services.AddScoped<IOrderItemsService, Service.Implementation.OrderItemsService>();
            services.AddScoped<AccountController>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddRazorPages();

            services.ConfigureApplicationCookie(options =>
            {
                // Location for your Custom Access Denied Page
                options.AccessDeniedPath = "/Account/AccessDenied";

                // Location for your Custom Login Page
                options.LoginPath = "/Account/Login";
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
