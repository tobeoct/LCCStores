namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTables2 : DbMigration
    {
        public override void Up()
        {
            RenameTable("BillingInformation", "BillingInformations");
            RenameTable("PersonalInformation", "PersonalInformations");
           
        }

        public override void Down()
        {
            RenameTable("BillingInformations", "BillingInformation");
            RenameTable("PersonalInformations", "PersonalInformation");
        }
    }
}
