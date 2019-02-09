namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewToDb4 : DbMigration
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
                .ForeignKey("dbo.Reviews", t => t.ReviewId, cascadeDelete: false)
                .Index(t => t.ReviewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewDetails", "ReviewId", "dbo.Reviews");
            DropIndex("dbo.ReviewDetails", new[] { "ReviewId" });
            DropTable("dbo.ReviewDetails");
        }
    }
}
