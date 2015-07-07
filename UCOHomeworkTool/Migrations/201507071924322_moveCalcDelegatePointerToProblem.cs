namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveCalcDelegatePointerToProblem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Problems", "DelegatePointer", c => c.Binary());
            DropColumn("dbo.Responses", "delegatePointer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Responses", "delegatePointer", c => c.Binary());
            DropColumn("dbo.Problems", "DelegatePointer");
        }
    }
}
