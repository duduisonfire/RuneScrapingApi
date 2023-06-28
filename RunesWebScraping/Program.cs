using RunesWebScraping.services;
using dotenv.net;
using RunesWebScraping;
using RunesWebScraping.domain;
using CronNET;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<LolApi>();
builder.Services.AddSingleton<UggService>();
builder.Services.AddSingleton<UggRuneCacheSync>();
builder.Services.AddSingleton<UggCacheManager>();

var app = builder.Build();
var uggCacheManager = app.Services.GetRequiredService<UggCacheManager>();

var lifetime = app.Lifetime;
lifetime.ApplicationStarted.Register(async () =>
{
    var hasCache = await uggCacheManager.VerifyCache();

    if (!hasCache)
    {
        Console.WriteLine("Syncing the cache");
        await uggCacheManager.SyncCache();
    }
    else
    {
        Console.WriteLine("Have cache");
    }

    ThreadStart uggSyncCache =
        new(async () =>
        {
            await uggCacheManager.SyncCache();
        });

    var cron = new CronDaemon();
    cron.AddJob("* 22 * * *", uggSyncCache);
    cron.Start();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
