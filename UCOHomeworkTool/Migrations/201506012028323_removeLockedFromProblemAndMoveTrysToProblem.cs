namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeLockedFromProblemAndMoveTrysToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "TrysRemaining", c => c.Int(nullable: false));
            DropColumn("dbo.Problems", "Locked");
            DropColumn("dbo.Responses", "TrysRemaining");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Responses", "TrysRemaining", c => c.Int(nullable: false));
            AddColumn("dbo.Problems", "Locked", c => c.Boolean(nullable: false));
            DropColumn("dbo.Problems", "TrysRemaining");
        }
    }
}
