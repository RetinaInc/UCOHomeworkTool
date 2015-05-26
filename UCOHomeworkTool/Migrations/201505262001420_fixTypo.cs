namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixTypo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "ProblemNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Problems", "ProblemNumeber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Problems", "ProblemNumeber", c => c.Int(nullable: false));
            DropColumn("dbo.Problems", "ProblemNumber");
        }
    }
}
