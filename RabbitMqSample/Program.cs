using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMqSample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHostedService<StartupConsumer>();
builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
builder.Services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMqPooledObjectPolicy>();

builder.Services.AddSingleton<IRabbitManager, RabbitManager>();

builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitOptions"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
