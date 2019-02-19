namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupplierUpdatesToDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuppliersUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Suppliers", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.Suppliers", "ProductId");
            AddForeignKey("dbo.Suppliers", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suppliers", "ProductId", "dbo.Products");
            DropIndex("dbo.Suppliers", new[] { "ProductId" });
            DropColumn("dbo.Suppliers", "ProductId");
            DropTable("dbo.SuppliersUpdates");
        }
    }
}
