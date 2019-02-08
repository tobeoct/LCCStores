namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfilesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        CustomerId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.UserProfiles", "CustomerId", "dbo.Customers");
            DropIndex("dbo.UserProfiles", new[] { "SupplierId" });
            DropIndex("dbo.UserProfiles", new[] { "CustomerId" });
            DropTable("dbo.UserProfiles");
        }
    }
}
