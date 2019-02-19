namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMigrationInDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Payments", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Payments", "PaymentReference", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "PaymentReference", c => c.Guid(nullable: false));
            DropColumn("dbo.Payments", "Status");
            DropColumn("dbo.Orders", "TotalPrice");
        }
    }
}
