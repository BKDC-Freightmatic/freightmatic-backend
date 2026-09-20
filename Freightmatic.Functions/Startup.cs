using Freightmatic.Application.Files;
using Freightmatic.Application.News;
using Freightmatic.Application.Notifications;
using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Freightmatic.Domain.Deliveries;
using Freightmatic.Domain.Files;
using Freightmatic.Domain.News;
using Freightmatic.Domain.Notifications;
using Freightmatic.Domain.Users;
using Freightmatic.Functions.Shared;
using Freightmatic.Infrastructure.Deliveries;
using Freightmatic.Infrastructure.Files;
using Freightmatic.Infrastructure.News;
using Freightmatic.Infrastructure.Notifications;
using Freightmatic.Infrastructure.Shared;
using Freightmatic.Infrastructure.Users;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Freightmatic.Functions.Startup))]
namespace Freightmatic.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddAuthorization();

        builder.Services.AddIdentityCore<User>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<INewsRepository, NewsRepository>();
        builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
        builder.Services.AddTransient<IDeliveryRepository, DeliveryRepository>();
        builder.Services.AddTransient<IFileRepository, FileRepository>();
        builder.Services.AddTransient<UserAppService>();
        builder.Services.AddTransient<NewsAppService>();
        builder.Services.AddTransient<NotificationAppService>();
        builder.Services.AddTransient<DeliveryAppService>();
        builder.Services.AddSingleton<FileAppService>();
        builder.Services.AddSingleton<CosmosDbClient>();
        builder.Services.AddSingleton<JwtValidation>();
    }
}