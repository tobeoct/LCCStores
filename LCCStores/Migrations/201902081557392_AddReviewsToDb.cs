namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewsToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReviewDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        ReviewId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reviews", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfileId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserProfileId);
            
            CreateTable(
                "dbo.ReviewUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewDetails", "ReviewId", "dbo.Reviews");
            DropForeignKey("dbo.Reviews", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropIndex("dbo.Reviews", new[] { "UserProfileId" });
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropIndex("dbo.ReviewDetails", new[] { "ReviewId" });
            DropTable("dbo.ReviewUpdates");
            DropTable("dbo.Reviews");
            DropTable("dbo.ReviewDetails");
        }
    }
}
