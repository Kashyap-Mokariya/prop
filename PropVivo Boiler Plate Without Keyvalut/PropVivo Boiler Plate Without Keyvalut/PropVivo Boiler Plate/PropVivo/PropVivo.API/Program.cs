using PropVivo.API;
using PropVivo.Application.Repositories;
using PropVivo.Infrastructure.DataSeeders;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var startup = new Startup();
startup.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
app.Logger.LogInformation($"Current environment: {app.Environment.EnvironmentName}");

if (app.Environment.IsDevelopment())
{
    // Seed sample data
    using (var scope = app.Services.CreateScope())
    {
        var customerRepo = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        var seeder = new CustomerDataSeeder(customerRepo);
        await seeder.SeedDataAsync();
    }
}

startup.Configure(app);
app.Run();
