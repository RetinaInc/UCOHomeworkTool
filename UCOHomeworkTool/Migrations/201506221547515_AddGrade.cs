namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "Grade", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assignments", "Grade");
        }
    }
}
