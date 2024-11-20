using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Web;
using ManagementSystem.Web.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices (this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddScoped<IUser, CurrentUserService>();
        services.AddHttpContextAccessor();
        //builder.Services.Configure<ApiBehaviorOptions>(options =>
        //options.SuppressModelStateInvalidFilter = true);

        return services;
    }
}
