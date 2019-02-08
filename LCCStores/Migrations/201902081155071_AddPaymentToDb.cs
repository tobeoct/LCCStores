namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        BillingInfoId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PaymentReference = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillingInformations", t => t.BillingInfoId, cascadeDelete: false)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.CustomerId)
                .Index(t => t.BillingInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Payments", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Payments", "BillingInfoId", "dbo.BillingInformations");
            DropIndex("dbo.Payments", new[] { "BillingInfoId" });
            DropIndex("dbo.Payments", new[] { "CustomerId" });
            DropIndex("dbo.Payments", new[] { "OrderId" });
            DropTable("dbo.Payments");
        }
    }
}
