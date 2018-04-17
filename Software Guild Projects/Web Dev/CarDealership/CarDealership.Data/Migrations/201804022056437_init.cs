namespace CarDealership.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BodyStyle",
                c => new
                    {
                        BSId = c.Int(nullable: false, identity: true),
                        BSName = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.BSId);
            
            CreateTable(
                "dbo.Car",
                c => new
                    {
                        VIN = c.String(name: "VIN#", nullable: false, maxLength: 17, unicode: false),
                        ModelId = c.Int(nullable: false),
                        ExtColorId = c.Int(nullable: false),
                        IntColorId = c.Int(nullable: false),
                        BSId = c.Int(nullable: false),
                        TransmissionId = c.Int(nullable: false),
                        CarType = c.String(nullable: false, maxLength: 5, unicode: false),
                        Purchased = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Featured = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CarYear = c.Int(nullable: false),
                        Mileage = c.Int(nullable: false),
                        MSRP = c.Decimal(nullable: false, precision: 18, scale: 0),
                        SalePrice = c.Decimal(nullable: false, precision: 18, scale: 0),
                        CarDescription = c.String(nullable: false, maxLength: 1000, unicode: false),
                        CarPicture = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.VIN)
                .ForeignKey("dbo.ExteriorColor", t => t.ExtColorId)
                .ForeignKey("dbo.InteriorColor", t => t.IntColorId)
                .ForeignKey("dbo.Model", t => t.ModelId)
                .ForeignKey("dbo.Transmission", t => t.TransmissionId)
                .ForeignKey("dbo.BodyStyle", t => t.BSId)
                .Index(t => t.ModelId)
                .Index(t => t.ExtColorId)
                .Index(t => t.IntColorId)
                .Index(t => t.BSId)
                .Index(t => t.TransmissionId);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        ContactDate = c.DateTime(nullable: false),
                        ContactName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactEmail = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactPhone = c.String(nullable: false, maxLength: 12, unicode: false),
                        ContactMessage = c.String(nullable: false, maxLength: 1000, unicode: false),
                        VIN = c.String(name: "VIN#", maxLength: 17, unicode: false),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.Car", t => t.VIN)
                .Index(t => t.VIN);
            
            CreateTable(
                "dbo.ExteriorColor",
                c => new
                    {
                        ExtColorId = c.Int(nullable: false, identity: true),
                        ExtColorName = c.String(nullable: false, maxLength: 25, unicode: false),
                        ExtColorType = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.ExtColorId);
            
            CreateTable(
                "dbo.InteriorColor",
                c => new
                    {
                        IntColorId = c.Int(nullable: false, identity: true),
                        IntColorName = c.String(nullable: false, maxLength: 25, unicode: false),
                        IntColorType = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.IntColorId);
            
            CreateTable(
                "dbo.Model",
                c => new
                    {
                        ModelId = c.Int(nullable: false, identity: true),
                        ModelName = c.String(nullable: false, maxLength: 25, unicode: false),
                        ModelEdition = c.String(nullable: false, maxLength: 10, unicode: false),
                        MakeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModelId)
                .ForeignKey("dbo.Make", t => t.MakeId)
                .Index(t => t.MakeId);
            
            CreateTable(
                "dbo.Make",
                c => new
                    {
                        MakeId = c.Int(nullable: false, identity: true),
                        MakeName = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.MakeId);
            
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        VIN = c.String(name: "VIN#", nullable: false, maxLength: 17, unicode: false),
                        PurchaseName = c.String(nullable: false, maxLength: 50, unicode: false),
                        PurchasePhone = c.String(maxLength: 12, unicode: false),
                        PurchaseEmail = c.String(maxLength: 50, unicode: false),
                        Street1 = c.String(nullable: false, maxLength: 50, unicode: false),
                        Street2 = c.String(maxLength: 50, unicode: false),
                        City = c.String(nullable: false, maxLength: 35, unicode: false),
                        PState = c.String(nullable: false, maxLength: 2, fixedLength: true, unicode: false),
                        ZipCode = c.Int(nullable: false),
                        PurchasePrice = c.Decimal(nullable: false, precision: 18, scale: 0),
                        PurchaseType = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.Car", t => t.VIN)
                .Index(t => t.VIN);
            
            CreateTable(
                "dbo.Transmission",
                c => new
                    {
                        TransmissionId = c.Int(nullable: false, identity: true),
                        TransmissionType = c.String(nullable: false, maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.TransmissionId);
            
            CreateTable(
                "dbo.Special",
                c => new
                    {
                        SpecialId = c.Int(nullable: false, identity: true),
                        SpecialName = c.String(nullable: false, maxLength: 50, unicode: false),
                        SpecialStartDate = c.DateTime(nullable: false),
                        SpecialEndDate = c.DateTime(nullable: false),
                        SpecialDescription = c.String(nullable: false, maxLength: 500, unicode: false),
                        SpecialJTronImage = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.SpecialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Car", "BSId", "dbo.BodyStyle");
            DropForeignKey("dbo.Car", "TransmissionId", "dbo.Transmission");
            DropForeignKey("dbo.Purchase", "VIN#", "dbo.Car");
            DropForeignKey("dbo.Model", "MakeId", "dbo.Make");
            DropForeignKey("dbo.Car", "ModelId", "dbo.Model");
            DropForeignKey("dbo.Car", "IntColorId", "dbo.InteriorColor");
            DropForeignKey("dbo.Car", "ExtColorId", "dbo.ExteriorColor");
            DropForeignKey("dbo.Contact", "VIN#", "dbo.Car");
            DropIndex("dbo.Purchase", new[] { "VIN#" });
            DropIndex("dbo.Model", new[] { "MakeId" });
            DropIndex("dbo.Contact", new[] { "VIN#" });
            DropIndex("dbo.Car", new[] { "TransmissionId" });
            DropIndex("dbo.Car", new[] { "BSId" });
            DropIndex("dbo.Car", new[] { "IntColorId" });
            DropIndex("dbo.Car", new[] { "ExtColorId" });
            DropIndex("dbo.Car", new[] { "ModelId" });
            DropTable("dbo.Special");
            DropTable("dbo.Transmission");
            DropTable("dbo.Purchase");
            DropTable("dbo.Make");
            DropTable("dbo.Model");
            DropTable("dbo.InteriorColor");
            DropTable("dbo.ExteriorColor");
            DropTable("dbo.Contact");
            DropTable("dbo.Car");
            DropTable("dbo.BodyStyle");
        }
    }
}
