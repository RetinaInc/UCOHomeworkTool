namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeGivensToDoubles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Givens", "Value", c => c.Double(nullable: false));
            AlterColumn("dbo.Givens", "minRange", c => c.Double());
            AlterColumn("dbo.Givens", "maxRange", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Givens", "maxRange", c => c.Int());
            AlterColumn("dbo.Givens", "minRange", c => c.Int());
            AlterColumn("dbo.Givens", "Value", c => c.Int(nullable: false));
        }
    }
}
