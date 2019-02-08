namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonalInfoToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonalInfos",
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
            
            CreateIndex("dbo.Addresses", "CityId");
            AddForeignKey("dbo.Addresses", "CityId", "dbo.Cities", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonalInfos", "PhoneNumberId", "dbo.PhoneNumbers");
            DropForeignKey("dbo.PersonalInfos", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Addresses", "CityId", "dbo.Cities");
            DropIndex("dbo.PersonalInfos", new[] { "PhoneNumberId" });
            DropIndex("dbo.PersonalInfos", new[] { "AddressId" });
            DropIndex("dbo.Addresses", new[] { "CityId" });
            DropTable("dbo.PersonalInfos");
        }
    }
}
