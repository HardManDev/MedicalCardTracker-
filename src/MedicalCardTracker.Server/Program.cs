// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

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

        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }

        _app.UseHttpsRedirection();

        _app.UseAuthorization();

        _app.MapControllers();

        _app.Run();
    }

    private static IServiceCollection ConfigureService(IServiceCollection services)
    {
        services.AddControllers();
        services.AddWindowsService();

        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();

        return services;
    }
}
