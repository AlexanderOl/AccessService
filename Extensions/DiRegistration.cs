using AccessService.Services;

namespace AccessService.Extensions
{
    public static class DiRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<CacheService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<AuthorizeService>();
        }
    }
}
