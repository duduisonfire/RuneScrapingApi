using RunesWebScraping.repository;
using RunesWebScraping.cases;
using RunesWebScraping.domain;
using dotenv.net;
using CronNET;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<UggRepository>();
builder.Services.AddSingleton<ChampionsRepository>();
builder.Services.AddTransient<RuneCacheSync>();
builder.Services.AddTransient<ChampionsListCacheSync>();
builder.Services.AddTransient<LolApi>();
builder.Services.AddTransient<CacheManager>();

var app = builder.Build();
var cacheManager = app.Services.GetRequiredService<CacheManager>();

app.Lifetime.ApplicationStarted.Register(async () =>
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
