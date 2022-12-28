using CheesyTot.AzureTables.SimpleIndex.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReferenceSocialNetwork.Common.Data.Entities;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddLogging(options =>
        {
            options.AddDebug();
            options.AddConsole();
        });

        services.Configure<SimpleIndexRepositoryOptions>(options =>
        {
            options.StorageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            options.TablePrefix = Environment.GetEnvironmentVariable("TablePrefix");
            options.IndexTableSuffix = Environment.GetEnvironmentVariable("IndexTableSuffix");
        });

        services.AddScoped<ISimpleIndexRepository<FeedItem>, SimpleIndexRepository<FeedItem>>();
        services.AddScoped<ISimpleIndexRepository<Follow>, SimpleIndexRepository<Follow>>();
        services.AddScoped<ISimpleIndexRepository<Post>, SimpleIndexRepository<Post>>();
        services.AddScoped<ISimpleIndexRepository<Profile>, SimpleIndexRepository<Profile>>();
    })
    .Build();

await host.RunAsync();