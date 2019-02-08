namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderStatusHistoryToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderStatusHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        UserId = c.Int(),
                        Date = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .ForeignKey("dbo.UserProfiles", t => t.UserId)
                .Index(t => t.OrderId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderStatusHistories", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.OrderStatusHistories", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderStatusHistories", new[] { "UserId" });
            DropIndex("dbo.OrderStatusHistories", new[] { "OrderId" });
            DropTable("dbo.OrderStatusHistories");
        }
    }
}
