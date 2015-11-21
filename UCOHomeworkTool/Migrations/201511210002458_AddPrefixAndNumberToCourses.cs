namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrefixAndNumberToCourses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "CoursePrefix", c => c.String());
            AddColumn("dbo.Courses", "CourseNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CourseNumber");
            DropColumn("dbo.Courses", "CoursePrefix");
        }
    }
}
