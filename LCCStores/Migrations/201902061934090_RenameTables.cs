namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                  "dbo.PersonalInformation",
                  c => new
                  {
                      Id = c.Int(nullable: false, identity: true),
                      Email = c.String(),
                      PostalCode = c.String(),
                      AddressId = c.Int(nullable: false),
                      PhoneNumberId = c.Int(nullable: false),
                  })
                  .PrimaryKey(t => t.Id)
                  .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                  .ForeignKey("dbo.PhoneNumbers", t => t.PhoneNumberId, cascadeDelete: true)
                  .Index(t => t.AddressId)
                  .Index(t => t.PhoneNumberId);

            
           
            CreateTable(
               "dbo.BillingInformation",
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
            DropForeignKey("dbo.PersonalInformation", "PhoneNumberId", "dbo.PhoneNumbers");
            DropForeignKey("dbo.PersonalInformation", "AddressId", "dbo.Addresses");
        
            DropIndex("dbo.PersonalInformation", new[] { "PhoneNumberId" });
            DropIndex("dbo.PersonalInformation", new[] { "AddressId" });
         
            DropTable("dbo.PersonalInformation");
            DropForeignKey("dbo.BillingInformation", "PhoneNumberId", "dbo.PhoneNumbers");
            DropForeignKey("dbo.BillingInformation", "CreditCardId", "dbo.CreditCards");
            DropForeignKey("dbo.BillingInformation", "AddressId", "dbo.Addresses");
            DropIndex("dbo.BillingInformation", new[] { "PhoneNumberId" });
            DropIndex("dbo.BillingInformation", new[] { "AddressId" });
            DropIndex("dbo.BillingInformation", new[] { "CreditCardId" });
            DropTable("dbo.BillingInformation");
        }
    
        
       
    }
}
