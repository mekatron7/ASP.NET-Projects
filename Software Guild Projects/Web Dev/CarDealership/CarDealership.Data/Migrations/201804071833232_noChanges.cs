namespace CarDealership.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Model", "ModelUser", c => c.String());
            AddColumn("dbo.Model", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Make", "MakeUser", c => c.String());
            AddColumn("dbo.Make", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Make", "DateAdded");
            DropColumn("dbo.Make", "MakeUser");
            DropColumn("dbo.Model", "DateAdded");
            DropColumn("dbo.Model", "ModelUser");
        }
    }
}
