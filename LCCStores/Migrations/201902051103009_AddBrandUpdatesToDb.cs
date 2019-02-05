namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBrandUpdatesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BrandUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BrandUpdates");
        }
    }
}
