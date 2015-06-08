namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsAssignedToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "IsAssigned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Problems", "IsAssigned");
        }
    }
}
