// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Application.Mappings;
using MedicalCardTracker.Database;

namespace MedicalCardTracker.Server;

internal static class Program
{
    private static WebApplication _app = null!;
    private static WebApplicationBuilder _builder = null!;

    public static void Main(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);

        ConfigureService(_builder.Services);

        _app = _builder.Build();

        _app.UseHttpsRedirection();

        _app.UseAuthorization();

        _app.MapControllers();


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

    private static IServiceCollection ConfigureService(IServiceCollection services)
    {
        services.AddApplication();
        services.AddDatabase(_builder.Configuration);
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly));
        });

        services.AddControllers();
        services.AddWindowsService();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();

        return services;
    }
}
