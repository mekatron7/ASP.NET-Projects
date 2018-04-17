namespace DVDLibraryWebAPI.Migrations
{
    using DVDLibraryWebAPI.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DVDLibraryWebAPI.Models.DVDCatalogEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DVDLibraryWebAPI.Models.DVDCatalogEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.DVDs.AddOrUpdate(
                d => d.Title,
                new DVDView
                {
                    Title = "Django Unchained",
                    Rating = "R",
                    Director = "Quentin Tarentino",
                    ReleaseYear = 2012,
                    Notes = "A nigga beating up some bitchass crackkkas."
                },
                new DVDView
                {
                    Title = "Jumanji: Welcome to the Jungle",
                    Rating = "PG-13",
                    Director = "Jake Kasdan",
                    ReleaseYear = 2017,
                    Notes = "Kevin Hart and The Rock have really got themselves in a predicament this time."
                },
                new DVDView
                {
                    Title = "Wall-E",
                    Rating = "PG",
                    Director = "Andrew Stanton",
                    ReleaseYear = 2008,
                    Notes = "A robot who discovers the meaning of life."
                });
        }
    }
}
