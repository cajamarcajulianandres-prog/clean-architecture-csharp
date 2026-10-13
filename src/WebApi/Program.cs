using Application.Abstractions;
using Application.UseCases.CreateOrder;
using Application.UseCases.GetLastOrders;
using Infrastructure.Logging;
using Infrastructure.Persistence;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options => options.AddPolicy("default", policy =>
    policy.WithOrigins(allowedOrigins).WithMethods("GET", "POST").WithHeaders("Content-Type")));

builder.Services.AddSingleton<IAppLogger, ConsoleAppLogger>();

var connectionString = builder.Configuration.GetConnectionString("Sql");
if (string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddSingleton<InMemoryOrderRepository>();
    builder.Services.AddSingleton<IOrderWriter>(sp => sp.GetRequiredService<InMemoryOrderRepository>());
    builder.Services.AddSingleton<IOrderReader>(sp => sp.GetRequiredService<InMemoryOrderRepository>());
}
else
{
    builder.Services.AddSingleton(new SqlServerOptions { ConnectionString = connectionString });
    builder.Services.AddScoped<SqlOrderRepository>();
    builder.Services.AddScoped<IOrderWriter>(sp => sp.GetRequiredService<SqlOrderRepository>());
    builder.Services.AddScoped<IOrderReader>(sp => sp.GetRequiredService<SqlOrderRepository>());
}

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<GetLastOrdersUseCase>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("default");

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

await app.RunAsync();
