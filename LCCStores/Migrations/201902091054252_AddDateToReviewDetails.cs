namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateToReviewDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Date");
        }
    }
}
