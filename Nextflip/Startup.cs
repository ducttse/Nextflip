using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nextflip.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Nextflip.utils;
using Nextflip.Models.account;
using Nextflip.Services.Implementations;
using Nextflip.Services.Interfaces;
using Nextflip.Models.mediaEditRequest;
using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaCategory;
using Nextflip.Models.mediaFavorite;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;
using Nextflip.Models.notification;
using Nextflip.Models.supportTopic;
using Nextflip.Models.supportTicket;
using Nextflip.Models.role;
using Microsoft.AspNetCore.Http;
using Nextflip.Models.paymentPlan;
using Nextflip.Models.subscription;
using Nextflip.Models.filmType;

namespace Nextflip
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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddTransient<ICategoryDAO, CategoryDAO>();
            services.AddTransient<IEpisodeDAO, EpisodeDAO>();
            services.AddTransient<IMediaDAO, MediaDAO>();
            services.AddTransient<IFavoriteListDAO, FavoriteListDAO>();
            services.AddTransient<IMediaCategoryDAO, MediaCategoryDAO>();
            services.AddTransient<IMediaFavoriteDAO, MediaFavoriteDAO>();
            services.AddTransient<ISeasonDAO, SeasonDAO>();
            services.AddTransient<ISubtitleDAO, SubtitleDAO>();
            services.AddTransient<INotificationDAO, NotificationDAO>();
            services.AddTransient<IFilmTypeDAO, FilmTypeDAO>();

            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IRoleDAO, RoleDAO>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISubscriptionDAO, SubscriptionDAO>();
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson();
            services.AddTransient<IAccountDAO, AccountDAO>();
            services.AddTransient<IUserManagerManagementService, UserManagerManagementService>();
            services.AddTransient<IMediaEditRequestDAO, MediaEditRequestDAO>();
            services.AddTransient<IMediaManagerManagementService, MediaManagerManagementService>();
            services.AddTransient<ISubscribedUserService, SubscribedUserService>();
            services.AddTransient<IEditorService, EditorService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IFilmTypeService, FilmTypeService>();


            ///get connection string
            DbUtil.ConnectionString = Configuration.GetConnectionString("MySql");
            //get mail settings
            services.AddOptions();
            var mailsettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsettings);

            services.AddTransient<ISendMailService, SendMailService>();

            //add SupportTiket, SupportTopic, SupportTicketResponse DAOs to service
            services.AddTransient<ISupportTicketDAO, SupportTicketDAO>();
            services.AddTransient<ISupportTopicDAO, SupportTopicDAO>();

            //add supportTicket, SupportTopic services to service
            services.AddTransient<ISupportTicketService, SupportTicketService>();
            services.AddTransient<ISupportTopicService, SupportTopicService>();

            //add session
            services.AddDistributedMemoryCache();

            //add payment plan
            services.AddTransient<IPaymentPlanService, PaymentPlanService>();
            services.AddTransient<IPaymentPlanDAO, PaymentPlanDAO>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //cooki author
            services.AddScoped<CookieUtil>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                options.AccessDeniedPath = "/Common/AccessDenied";
                options.LoginPath = "/Login/Index";
                options.EventsType = typeof(CookieUtil);
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("user manager", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "user manager"))
                        .Build();
                });
                options.AddPolicy("subscribed user", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "subscribed user"))
                        .Build();
                });
                options.AddPolicy("customer supporter", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "customer supporter"))
                        .Build();
                });
                options.AddPolicy("media editor", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "media editor"))
                        .Build();
                });
                options.AddPolicy("media manager", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "media manager"))
                        .Build();
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");

                endpoints.MapGet("/testmail", async context =>
                {

                    // Lấy dịch vụ sendmailservice
                    var sendmailservice = context.RequestServices.GetService<ISendMailService>();

                    MailContent content = new MailContent
                    {
                        To = "technical.nextflipcompany@gmail.com",
                        Subject = "Kiểm tra thử",
                        Body = "Test"
                    };

                    await sendmailservice.SendMail(content);
                    await context.Response.WriteAsync("Send mail");
                });

            });

        }
    }
}
