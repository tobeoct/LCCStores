namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactorPhoneNumberPropertyType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PhoneNumbers", "NumberOne", c => c.String());
            AlterColumn("dbo.PhoneNumbers", "NumberTwo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PhoneNumbers", "NumberTwo", c => c.Int(nullable: false));
            AlterColumn("dbo.PhoneNumbers", "NumberOne", c => c.Int(nullable: false));
        }
    }
}
