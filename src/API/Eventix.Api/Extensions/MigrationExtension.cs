using Eventix.Modules.Events.Infrastructure.Database;
using Eventix.Modules.Ticketing.Infrastructure.Database;
using Eventix.Modules.Users.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Eventix.Api.Extensions
{
    internal static class MigrationExtension
    {
        internal static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            ApplyMigration<EventsDbContext>(scope);
            ApplyMigration<UsersDbContext>(scope);
            ApplyMigration<TicketingDbContext>(scope);
        }

        private static void ApplyMigration<TDb>(this IServiceScope scope)
            where TDb : DbContext
        {
            using TDb context = scope.ServiceProvider.GetRequiredService<TDb>();

            context.Database.Migrate();
        }
    }
}