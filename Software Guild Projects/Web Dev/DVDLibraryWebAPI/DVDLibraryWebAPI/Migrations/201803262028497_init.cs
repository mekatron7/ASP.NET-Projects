namespace DVDLibraryWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DVDViews",
                c => new
                    {
                        DVDId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        Rating = c.String(maxLength: 5),
                        Genre = c.String(maxLength: 15),
                        Director = c.String(maxLength: 50),
                        ReleaseYear = c.Int(nullable: false),
                        Notes = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.DVDId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DVDViews");
        }
    }
}
