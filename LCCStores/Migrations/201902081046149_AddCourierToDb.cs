namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourierToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Couriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.Int(nullable: false),
                        CompanyName = c.String(),
                        AddedById = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admins", t => t.AddedById, cascadeDelete: true)
                .Index(t => t.AddedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Couriers", "AddedById", "dbo.Admins");
            DropIndex("dbo.Couriers", new[] { "AddedById" });
            DropTable("dbo.Couriers");
        }
    }
}
