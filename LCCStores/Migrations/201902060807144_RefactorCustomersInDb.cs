namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactorCustomersInDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos");
            DropIndex("dbo.Customers", new[] { "BillingInfoId" });
            AlterColumn("dbo.Customers", "BillingInfoId", c => c.Int(nullable:true));
            CreateIndex("dbo.Customers", "BillingInfoId");
            AddForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos");
            DropIndex("dbo.Customers", new[] { "BillingInfoId" });
            AlterColumn("dbo.Customers", "BillingInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "BillingInfoId");
            AddForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos", "Id", cascadeDelete: true);
        }
    }
}
