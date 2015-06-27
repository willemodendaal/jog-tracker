namespace JogTracker.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JogTracker.Data.JogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JogTracker.Data.JogDbContext context)
        {
            // Logic here to seed with initial data.
        }
    }
}
