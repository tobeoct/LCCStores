namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourierUpdateToDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couriers", "PlateNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Couriers", "PlateNumber");
        }
    }
}
