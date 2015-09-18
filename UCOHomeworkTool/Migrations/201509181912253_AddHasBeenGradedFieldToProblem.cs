namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasBeenGradedFieldToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "HasBeenGraded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Problems", "HasBeenGraded");
        }
    }
}
