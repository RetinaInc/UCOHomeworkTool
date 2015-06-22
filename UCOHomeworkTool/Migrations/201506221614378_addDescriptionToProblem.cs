namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDescriptionToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Problems", "Description");
        }
    }
}
