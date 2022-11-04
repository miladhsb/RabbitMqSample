using MassTransit;
using MassTransit.Transports.Fabric;
using RabbitMqMassTransit.Consumer;
using RabbitMqMassTransit.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMassTransit(x =>
{
  //  x.AddConsumer<PersonConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.Publish<Person>(p =>
        //{
        //    p.Durable = true;
        //    p.ExchangeType = "direct";
        //    p.AutoDelete = false;
        //    p.Exclude = false;
        //});
        cfg.ReceiveEndpoint("input-queue", e =>
        {
            e.Bind("exchange-name", x =>
            {
                x.Durable = false;
                x.AutoDelete = true;
                x.ExchangeType = "direct";
                x.RoutingKey = "8675309";
            });
        });

        cfg.Host("localhost", "/", h => {
          
                h.Username("admin");
            h.Password("admin");
           
        });

        cfg.ConfigureEndpoints(context);
    });
});
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
