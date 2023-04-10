// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Application.Mappings;
using MedicalCardTracker.Database;
using MedicalCardTracker.Server.Application;
using MedicalCardTracker.Server.Hubs;
using MedicalCardTracker.Server.Middlewares;
using Serilog;
using Serilog.Events;

namespace MedicalCardTracker.Server;

internal static class Program
{
    private static WebApplication _app = null!;
    private static WebApplicationBuilder _builder = null!;

    public static void Main(string[] args) => Startup(args);

    private static void Startup(string[] args)
    {
        ConfigureLogger();

        _builder = WebApplication.CreateBuilder(args);
        _builder.Host.UseSerilog();

        ConfigureService(_builder.Services);


        _app = _builder.Build();

        _app.MapHub<NotificationHub>("/notification");
        _app.MapControllers();
        _app.UseAuthorization();
        _app.UseHttpsRedirection();

        _app.UseMiddleware<MachineFingerprintMiddleware>();

        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();

            _app.Run();
        }
        else
        {
            _app.Run(_builder.Configuration.GetValue<string>("Url"));
        }
    }

    private static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
                LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor",
                LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                LogEventLevel.Warning)
            .WriteTo.Console(outputTemplate:
                "{Timestamp:dd.MM.yyyy HH:mm:ss} [{Level:u4}] {Message}{NewLine}{Exception}")
#if !DEBUG
            .WriteTo.File($"logs/{DateTime.UtcNow:yyyy-MM-dd}.log",
                outputTemplate: "{Timestamp:dd.MM.yyyy HH:mm:ss} [{Level:u4}] ({SourceContext}) {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                shared: true,
                retainedFileCountLimit: 15)
#endif
            .CreateLogger();
    }

    private static void ConfigureService(IServiceCollection services)
    {
        services.AddDatabase(_builder.Configuration);
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly));
        });
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(BaseRequestHandler).Assembly));

        services.AddSignalR();
        services.AddControllers();
        services.AddWindowsService();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
    }
}
