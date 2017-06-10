
namespace SmartStock.Core.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartStock.Core.Context.SSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SmartStock.Core.Context.SSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //initial Role
            context.Role.AddOrUpdate(
                new Entities.Role { ID = 1, RoleCode = "client", Description = "This is client role." },
                new Entities.Role { ID = 2, RoleCode = "operator", Description = "This is operator role." },
                new Entities.Role { ID = 3, RoleCode = "Leader", Description = "This is leader role." },
                new Entities.Role { ID = 4, RoleCode = "Supervisor", Description = "This is supervisor role." },
                new Entities.Role { ID = 5, RoleCode = "manager", Description = "This is manager role." }
            );
        }
    }
}
