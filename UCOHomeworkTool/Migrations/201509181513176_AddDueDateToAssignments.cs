namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDueDateToAssignments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "DueDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "DueDate");
        }
    }
}
