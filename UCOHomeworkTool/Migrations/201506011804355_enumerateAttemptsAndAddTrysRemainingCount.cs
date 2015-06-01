namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enumerateAttemptsAndAddTrysRemainingCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "FirstAttempt", c => c.Double(nullable: false));
            AddColumn("dbo.Responses", "SecondAttempt", c => c.Double(nullable: false));
            AddColumn("dbo.Responses", "ThirdAttempt", c => c.Double(nullable: false));
            AddColumn("dbo.Responses", "TrysRemaining", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Responses", "TrysRemaining");
            DropColumn("dbo.Responses", "ThirdAttempt");
            DropColumn("dbo.Responses", "SecondAttempt");
            DropColumn("dbo.Responses", "FirstAttempt");
        }
    }
}
