namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBillingInfosToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingInfos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreditCardId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        AddressId = c.Int(nullable: false),
                        PhoneNumberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardId, cascadeDelete: true)
                .ForeignKey("dbo.PhoneNumbers", t => t.PhoneNumberId, cascadeDelete: true)
                .Index(t => t.CreditCardId)
                .Index(t => t.AddressId)
                .Index(t => t.PhoneNumberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BillingInfos", "PhoneNumberId", "dbo.PhoneNumbers");
            DropForeignKey("dbo.BillingInfos", "CreditCardId", "dbo.CreditCards");
            DropForeignKey("dbo.BillingInfos", "AddressId", "dbo.Addresses");
            DropIndex("dbo.BillingInfos", new[] { "PhoneNumberId" });
            DropIndex("dbo.BillingInfos", new[] { "AddressId" });
            DropIndex("dbo.BillingInfos", new[] { "CreditCardId" });
            DropTable("dbo.BillingInfos");
        }
    }
}
