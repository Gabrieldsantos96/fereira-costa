using Fereira.Costa.Application;
using Fereira.Costa.Infra;
using Fereira.Costa.Infra.Adapters;
using Fereira.Costa.Server;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(8080);
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddResponseCompression();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("Fereira.Costa.Client")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });
}
else
{
    Console.WriteLine(builder.Configuration.GetValue<string>("WEBSITE_CORS_ALLOWED_ORIGINS"));
    Console.WriteLine(builder.Configuration.GetValue<string>("DATABASE_URI"));

    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration.GetValue<string>("WEBSITE_CORS_ALLOWED_ORIGINS")?.Split(",");
        options.AddDefaultPolicy(policy =>
        {
            if (allowedOrigins != null && allowedOrigins.Length > 0)
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            }
        });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    try
    {
        Console.WriteLine($"Iniciando migrações às {DateTime.Now} com Connection String: {dbContext.Database.GetConnectionString()}");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrações aplicadas com sucesso às {DateTime.Now}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao aplicar migrações às {DateTime.Now}: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
        throw;
    }
}

app.UseStatusCodePages();
app.UseExceptionHandler();

if (builder.Environment.IsProduction())
{
    app.UseResponseCompression();
}

app.UseRouting();
app.UseCors();

app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures("pt-BR")
    .AddSupportedUICultures("pt-BR"));

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseSwaggerGen();
app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

app.Run();