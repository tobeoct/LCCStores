namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        OrderNumber = c.Guid(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        ShippedDate = c.DateTime(),
                        ShipVia = c.Int(nullable: false),
                        CourierId = c.Int(),
                        Freight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingInfoId = c.Int(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillingInformations", t => t.BillingInfoId, cascadeDelete: false)
                .ForeignKey("dbo.Couriers", t => t.CourierId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: false)
                .Index(t => t.CustomerId)
                .Index(t => t.CourierId)
                .Index(t => t.BillingInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "CourierId", "dbo.Couriers");
            DropForeignKey("dbo.Orders", "BillingInfoId", "dbo.BillingInformations");
            DropIndex("dbo.Orders", new[] { "BillingInfoId" });
            DropIndex("dbo.Orders", new[] { "CourierId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropTable("dbo.Orders");
        }
    }
}
