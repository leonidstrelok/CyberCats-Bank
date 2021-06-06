using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RegistrationUser.Persistence
{
    public static class DependencyInjection
    {

        public static void AddRegistrationUserPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("IdentityConnection"));
            });
           
        }
    }
}