namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDatatypeOfGivens : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Givens", "Value", c => c.Int(nullable: false));
            AlterColumn("dbo.Givens", "minRange", c => c.Int());
            AlterColumn("dbo.Givens", "maxRange", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Givens", "maxRange", c => c.Double());
            AlterColumn("dbo.Givens", "minRange", c => c.Double());
            AlterColumn("dbo.Givens", "Value", c => c.Double(nullable: false));
        }
    }
}
