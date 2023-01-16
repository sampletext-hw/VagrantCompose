using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Models.Configs;
using Serilog;
using Serilog.Events;
using Services;
using Services.ExternalServices;
using WebAPI.HostedServices;
using WebAPI.Middlewares;

namespace WebAPI
{
    public class Startup
    {
        // NOTE: In startup constructor is Automatically called first, then ConfigureService, and then Configure

        private readonly IWebHostEnvironment _env;

        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services
                .AddRouting(options => options.LowercaseUrls = true);
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Configure();

                        options.SerializerSettings.Error =
                            (sender, args) => _logger.LogCritical(args.ErrorContext.Error.Message);

                        // options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                        // options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        // options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                        // options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    }
                );

            services
                .AddAkianaDependencies(Configuration);

            services.AddHostedService<OrderPostProcessingHostedService>();

            services.Configure<StaticConfig>(Configuration.GetSection(nameof(StaticConfig)));
            
            var staticConfig = Configuration.GetSection(nameof(StaticConfig)).Get<StaticConfig>();
            
            _env.WebRootFileProvider = new PhysicalFileProvider(staticConfig.StaticFilesPath);

            services.AddSpaStaticFiles(options =>
                options.RootPath = staticConfig.StaticFilesPath
            );

            services
                .AddSwaggerGen(swagger =>
                {
                    swagger.EnableAnnotations();
                    swagger.SwaggerDoc("v1", new OpenApiInfo() {Title = "Akiana API Docs"});
                    swagger.AddSecurityDefinition("basic", new OpenApiSecurityScheme()
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert auth-token here",
                        Name = "auth-token",
                        Type = SecuritySchemeType.ApiKey
                    });
                });

            services.Configure<EmailServiceConfig>(Configuration.GetSection(nameof(EmailServiceConfig)));
            services.Configure<PaymentServiceConfig>(Configuration.GetSection(nameof(PaymentServiceConfig)));
            services.Configure<EmailSenderConfig>(Configuration.GetSection(nameof(EmailSenderConfig)));
            services.Configure<ClientAccountServiceConfig>(Configuration.GetSection(nameof(ClientAccountServiceConfig)));

            var telegramConfig = Configuration.GetSection(nameof(TelegramConfig)).Get<TelegramConfig>();

            TelegramAPI.Configure(telegramConfig);

            TelegramAPI.Send($"{_env.EnvironmentName}: Server started");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IMapper mapper, IOptions<StaticConfig> staticConfig)
        {
            _logger = loggerFactory.CreateLogger<Startup>();
            _logger.LogInformation("Startup.Configure");

            app.UseSerilogRequestLogging(
                options =>
                {
                    options.GetLevel = (context, d, arg3) => LogEventLevel.Debug;
                });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // mapper.ConfigurationProvider.AssertConfigurationIsValid();

            if (!_env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionCatcherMiddleware>();
            app.UseMiddleware<RequestCounterMiddleware>();

            // Enable this if someone fucks up the payment
            // app.UseMiddleware<PaymentMiddleware>();

            if (!_env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Akiana API Docs");
                    c.RoutePrefix = "docs";
                });
            }

            // app.UseHttpsRedirection();

            // TODO: Extract to middleware class
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/index.html" && context.Request.Method == "POST")
                {
                    context.Request.Method = "GET";
                }
            
                await next();
            });

            app.UseDefaultFiles(); // Serve index.html for route "/"

            var staticFileOptions = new StaticFileOptions
            {
                FileProvider = _env.WebRootFileProvider,
                ServeUnknownFileTypes = true
            };
            // app.UseStaticFiles(staticFileOptions);
            app.UseSpaStaticFiles(staticFileOptions);

            app.UseMiddleware<IspValidatorMiddleware>();

            app.UseRouting();

            app.UseCors(builder => builder
                .WithOrigins(
                    "http://localhost",
                    "http://localhost:4200",
                    "https://localhost",
                    "https://localhost:4200",
                    "http://akiana.io",
                    "https://akiana.io",
                    "http://www.akiana.io",
                    "https://akiana.io",
                    "http://akiantr.ru",
                    "https://akiantr.ru")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "callcenter_area",
                    areaName: "CallCenter",
                    pattern: "callcenter/{controller}/{action}");

                endpoints.MapAreaControllerRoute(
                    name: "franchisee_area",
                    areaName: "Franchisee",
                    pattern: "franchisee/{controller}/{action}");

                endpoints.MapAreaControllerRoute(
                    name: "mobile_area",
                    areaName: "Mobile",
                    pattern: "m/{controller}/{action}");

                endpoints.MapAreaControllerRoute(
                    name: "shared_area",
                    areaName: "Shared",
                    pattern: "shared/{controller}/{action}");

                endpoints.MapAreaControllerRoute(
                    name: "superuser_area",
                    areaName: "Superuser",
                    pattern: "superuser/{controller}/{action}");

                endpoints.MapDefaultControllerRoute();

                endpoints.MapFallback(FallbackDelegate);
            });

            // serve index.html for everything not mapped
            // app.UseSpa(builder => { builder.Options.DefaultPageStaticFileOptions = staticFileOptions; });
        }

        private async Task FallbackDelegate(HttpContext context, IOptions<StaticConfig> options)
        {
            // var webClient = new WebClient();
            var remoteIp = context.Connection.RemoteIpAddress!.ToString().Split(':').Last();
            // var ipLocation = await webClient.DownloadStringTaskAsync($"https://api.iplocation.net/?ip={remoteIp}");

            // var deserializeObject = JsonConvert.DeserializeObject<dynamic>(ipLocation);
            // var isp = deserializeObject.isp;
            // var country_name = deserializeObject.country_name;

            // https://api.iplocation.net/?ip=XX.XX.XX.XX
            if (context.Request.Method.ToUpper() != "GET")
            {
                var content = Encoding.UTF8.GetString((await context.Request.BodyReader.ReadAsync()).Buffer);

                await TelegramAPI.Send($"Unknown endpoint Fallback!\n" +
                                       $"{context.Request.Path}\n" +
                                       $"Method: {context.Request.Method}\n" +
                                       $"IP: {remoteIp}\n" +
                                       $"Body: {content}\n" +
                                       $"Query: {context.Request.QueryString.Value}"
                    // + $"\nISP: {isp}\nCountry: {country_name}"
                );
                await context.Response.WriteAsync("What are you doing bro?\nPlease use existing endpoints :)");
            }
            else
            {
                await context.Response.SendFileAsync(options.Value.StaticFilesPath + "/index.html");
            }
        }
    }
}