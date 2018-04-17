namespace CarDealership.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPurchaseDateAndUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purchase", "PurchaseDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Purchase", "PurchaseUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purchase", "PurchaseUser");
            DropColumn("dbo.Purchase", "PurchaseDate");
        }
    }
}
