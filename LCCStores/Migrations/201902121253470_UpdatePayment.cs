namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePayment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "AuthCode", c => c.String());
            AlterColumn("dbo.Payments", "Type", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Payments", "AuthCode");
        }
    }
}
