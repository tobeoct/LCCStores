namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePhoneNumberProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhoneNumbers", "NumberOne", c => c.Int(nullable: false));
            AddColumn("dbo.PhoneNumbers", "NumberTwo", c => c.Int(nullable: false));
            DropColumn("dbo.PhoneNumbers", "PhoneNumber1");
            DropColumn("dbo.PhoneNumbers", "PhoneNumber2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhoneNumbers", "PhoneNumber2", c => c.Int(nullable: false));
            AddColumn("dbo.PhoneNumbers", "PhoneNumber1", c => c.Int(nullable: false));
            DropColumn("dbo.PhoneNumbers", "NumberTwo");
            DropColumn("dbo.PhoneNumbers", "NumberOne");
        }
    }
}
