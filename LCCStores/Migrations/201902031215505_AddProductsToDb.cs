namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductsToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductDetailId = c.Int(nullable: false),
                        BrandId = c.Int(nullable: false),
                        TaxId = c.Int(nullable: false),
                        AddedById = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AddedById, cascadeDelete: true)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: false)
                .ForeignKey("dbo.ProductDetails", t => t.ProductDetailId, cascadeDelete: true)
                .ForeignKey("dbo.Taxes", t => t.TaxId, cascadeDelete: true)
                .Index(t => t.ProductDetailId)
                .Index(t => t.BrandId)
                .Index(t => t.TaxId)
                .Index(t => t.AddedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "TaxId", "dbo.Taxes");
            DropForeignKey("dbo.Products", "ProductDetailId", "dbo.ProductDetails");
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Products", "AddedById", "dbo.Admins");
            DropIndex("dbo.Products", new[] { "AddedById" });
            DropIndex("dbo.Products", new[] { "TaxId" });
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropIndex("dbo.Products", new[] { "ProductDetailId" });
            DropTable("dbo.Products");
        }
    }
}
