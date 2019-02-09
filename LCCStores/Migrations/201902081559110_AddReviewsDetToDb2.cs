namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewsDetToDb2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Reviews", "UserProfileId", "dbo.UserProfiles");
            DropForeignKey("dbo.ReviewDetails", "ReviewId", "dbo.Reviews");
            DropIndex("dbo.ReviewDetails", new[] { "ReviewId" });
            DropIndex("dbo.Reviews", new[] { "ProductId" });
            DropIndex("dbo.Reviews", new[] { "UserProfileId" });
            DropTable("dbo.ReviewDetails");
            DropTable("dbo.Reviews");
            DropTable("dbo.ReviewUpdates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReviewUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        UserProfileId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReviewDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        ReviewId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Reviews", "UserProfileId");
            CreateIndex("dbo.Reviews", "ProductId");
            CreateIndex("dbo.ReviewDetails", "ReviewId");
            AddForeignKey("dbo.ReviewDetails", "ReviewId", "dbo.Reviews", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "UserProfileId", "dbo.UserProfiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
