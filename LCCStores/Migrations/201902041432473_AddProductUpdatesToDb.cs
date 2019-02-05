namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductUpdatesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductUpdates");
        }
    }
}
