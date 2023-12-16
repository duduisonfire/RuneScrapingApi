using CronNET;
using dotenv.net;
using RunesWebScraping.cases;
using RunesWebScraping.domain;
using RunesWebScraping.repository;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddSingleton<IUggRepository, UggRepository>();
builder.Services.AddSingleton<IChampionsRepository, ChampionsRepository>();
builder.Services.AddTransient<IRuneCacheSync, RuneCacheSync>();
builder.Services.AddTransient<IChampionsListCacheSync, ChampionsListCacheSync>();
builder.Services.AddTransient<ILolApi, LolApi>();
builder.Services.AddTransient<ICacheManager, CacheManager>();

var app = builder.Build();
var cacheManager = app.Services.GetRequiredService<ICacheManager>();

app.Lifetime
    .ApplicationStarted
    .Register(async () =>
    {
        await cacheManager.UpdateAllCaches();

        ThreadStart thread =
            new(async () =>
            {
                await cacheManager.SyncRunesCache();
                await cacheManager.SyncChampionsCache();
            });

        var cron = new CronDaemon();
        cron.AddJob("* 22 * * *", thread);
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
