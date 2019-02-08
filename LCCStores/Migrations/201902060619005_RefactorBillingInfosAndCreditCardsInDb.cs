namespace LCCStores.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactorBillingInfosAndCreditCardsInDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BillingInfos", "CreditCardId", "dbo.CreditCards");
            DropIndex("dbo.BillingInfos", new[] { "CreditCardId" });
            AddColumn("dbo.CreditCards", "BillingInfoId", c => c.Int(nullable: false));
            CreateIndex("dbo.CreditCards", "BillingInfoId");
            AddForeignKey("dbo.CreditCards", "BillingInfoId", "dbo.BillingInfos", "Id", cascadeDelete: true);
            DropColumn("dbo.BillingInfos", "CreditCardId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BillingInfos", "CreditCardId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CreditCards", "BillingInfoId", "dbo.BillingInfos");
            DropIndex("dbo.CreditCards", new[] { "BillingInfoId" });
            DropColumn("dbo.CreditCards", "BillingInfoId");
            CreateIndex("dbo.BillingInfos", "CreditCardId");
            AddForeignKey("dbo.BillingInfos", "CreditCardId", "dbo.CreditCards", "Id", cascadeDelete: true);
        }
    }
}
