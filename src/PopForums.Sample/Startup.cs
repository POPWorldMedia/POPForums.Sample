﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PopForums.Configuration;
using PopForums.Extensions;
using PopForums.Mvc.Areas.Forums.Authorization;
using PopForums.Mvc.Areas.Forums.Extensions;
using PopForums.Sql;

namespace PopForums.Sample
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			// Setup configuration sources.
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();
			builder.Build();

			// setup PopForums.json config file
			Config.SetPopForumsAppEnvironment(env.ContentRootPath);
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// needed for social logins in POP Forums
			services.AddAuthentication(options => options.DefaultSignInScheme = PopForumsAuthorizationDefaults.AuthenticationScheme);

			services.Configure<AuthorizationOptions>(options =>
			{
				// sets claims policies for admin and moderator functions in POP Forums
				options.AddPopForumsPolicies();
			});

			services.AddMvc(options =>
			{
				// identifies users on POP Forums actions
				options.Filters.Add(typeof(PopForumsUserAttribute));
			});

			// It's unfortunately necessary to use the Json.NET serializer for API requests because System.Text.Json doesn't handler enums correctly
			services.AddControllers().AddNewtonsoftJson();

			// sets up the dependencies for the base, SQL and web libraries in POP Forums
			services.AddPopForumsBase();
			services.AddPopForumsSql();
			// this adds dependencies from the MVC project and sets up authentication for the forum
			services.AddPopForumsMvc();

			// use Redis cache for POP Forums using AzureKit
			//services.AddPopForumsRedisCache();

			// required for real-time updating of POP Forums
			services.AddSignalR();
			// use this instead of previous line if you need to route SignalR messages
			// over a Redis backplane for multi-instance host
			//services.AddSignalR().AddRedisBackplaneForPopForums();

			// use Azure Search for POP Forums using AzureKit
			//services.AddPopForumsAzureSearch();

			// use ElasticSearch for POP Forums using AwsKit
			//services.AddPopForumsElasticSearch();

			// use Azure Functions queues for POP Forums using AzureKit for background tasks...
			// do NOT call AddPopForumsBackgroundServices()
			//services.AddPopForumsAzureFunctionsAndQueues();

			// creates an instance of the background services for POP Forums... call this last in forum setup,
			// but don't use if you're running these in functions
			services.AddPopForumsBackgroundServices();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			// Records exceptions and info to the POP Forums database.
			loggerFactory.AddPopForumsLogger(app);

			// Enables languages
			app.UsePopForumsCultures();

			app.UseStaticFiles();

			// Not unique to POP Forums, but required.
			app.UseAuthentication();

			// Populate the POP Forums identity in every request.
			app.UsePopForumsAuth();

			app.UseDeveloperExceptionPage();
			
			// Add MVC to the request pipeline. The order of the next three lines matters:
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				// POP Forums routes
				endpoints.AddPopForumsEndpoints(app);

				// need this if you have lots of routing and/or areas
				endpoints.MapAreaControllerRoute(
					"forumroutes", "forums",
					"Forums/{controller=Home}/{action=Index}/{id?}");

				// app routes
				endpoints.MapControllerRoute(
					"areaRoute",
					"{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
