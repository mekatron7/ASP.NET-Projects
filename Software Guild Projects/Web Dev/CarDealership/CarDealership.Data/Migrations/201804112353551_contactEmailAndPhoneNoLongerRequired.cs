namespace CarDealership.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactEmailAndPhoneNoLongerRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contact", "ContactEmail", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Contact", "ContactPhone", c => c.String(maxLength: 12, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contact", "ContactPhone", c => c.String(nullable: false, maxLength: 12, unicode: false));
            AlterColumn("dbo.Contact", "ContactEmail", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
    }
}
