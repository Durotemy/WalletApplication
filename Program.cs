using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Wallet")
        ?? throw new InvalidOperationException("Connection string 'Wallet' is missing.")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

