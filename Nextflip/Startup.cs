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
using System.Threading.Tasks;
using Nextflip.utils;
using Nextflip.Services.Interfaces;
using Nextflip.Services.Implementations;
using Nextflip.Models.category;
using Nextflip.Models.episode;
using Nextflip.Models.favoriteList;
using Nextflip.Models.media;
using Nextflip.Models.mediaCategory;
using Nextflip.Models.mediaFavorite;
using Nextflip.Models.season;
using Nextflip.Models.subtitle;
using Nextflip.Models.supportTopic;
using Nextflip.Models.supportTicket;

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

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IEpisodeService, EpisodeService>();
            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<ISubtitleService, SubtitleService>();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
