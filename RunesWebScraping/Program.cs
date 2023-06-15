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
builder.Services.AddSingleton<UggService>();
builder.Services.AddSingleton<RuneCacheSync>();
builder.Services.AddSingleton<LolApi>();
builder.Services.AddSingleton<CacheManager>();

var app = builder.Build();
var cacheManager = app.Services.GetRequiredService<CacheManager>();

var lifetime = app.Lifetime;
lifetime.ApplicationStarted.Register(async () => {
    var hasCache = await cacheManager.VerifyCache();

    if (!hasCache)
    {
        await cacheManager.SyncCache();
        Console.WriteLine("Syncing the cache");
    } else {
        Console.WriteLine("Have cache");
    }

    ThreadStart thread = new(async () => {
        await cacheManager.SyncCache();
    });

    var cron = new CronDaemon();
    cron.AddJob("* 22 * * *", thread);
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