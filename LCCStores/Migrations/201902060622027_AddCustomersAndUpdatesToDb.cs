namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomersAndUpdatesToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PersonalInfoId = c.Int(nullable: false),
                        BillingInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                //.ForeignKey("dbo.BillingInfoes", t => t.BillingInfoId, cascadeDelete: true)
                //.ForeignKey("dbo.PersonalInfoes", t => t.PersonalInfoId, cascadeDelete: true)
                .Index(t => t.PersonalInfoId)
                .Index(t => t.BillingInfoId);
            AddForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos", "Id");
            AddForeignKey("dbo.Customers", "PersonalInfoId", "dbo.PersonalInfos", "Id");
            CreateTable(
                "dbo.CustomerUpdates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "PersonalInfoId", "dbo.PersonalInfos");
            DropForeignKey("dbo.Customers", "BillingInfoId", "dbo.BillingInfos");
            DropIndex("dbo.Customers", new[] { "BillingInfoId" });
            DropIndex("dbo.Customers", new[] { "PersonalInfoId" });
            DropTable("dbo.CustomerUpdates");
            DropTable("dbo.Customers");
        }
    }
}
