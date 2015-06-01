namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLockedParamToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Problems", "Locked");
        }
    }
}
