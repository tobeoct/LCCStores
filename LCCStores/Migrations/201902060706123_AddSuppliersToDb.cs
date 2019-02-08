namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSuppliersToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonalInfoId = c.Int(nullable: false),
                        CompanyName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        AddedById = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                
                .Index(t => t.PersonalInfoId)
                .Index(t => t.AddedById);
           
            AddForeignKey("dbo.Suppliers", "PersonalInfoId", "dbo.PersonalInfos", "Id");
            AddForeignKey("dbo.Suppliers", "AddedById", "dbo.Admins", "Id");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Suppliers", "PersonalInfoId", "dbo.PersonalInfos");
            DropForeignKey("dbo.Suppliers", "AddedById", "dbo.Admins");
            DropIndex("dbo.Suppliers", new[] { "AddedById" });
            DropIndex("dbo.Suppliers", new[] { "PersonalInfoId" });
            DropTable("dbo.Suppliers");
        }
    }
}
