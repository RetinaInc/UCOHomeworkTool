namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDelegateMapping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "delegatePointer", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Responses", "delegatePointer");
        }
    }
}
