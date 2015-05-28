namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImagesAndGeneratedFromKey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProblemDiagrams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProblemId = c.Int(nullable: false),
                        ImageContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Problems", "GeneratedFrom", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Problems", "GeneratedFrom");
            DropTable("dbo.ProblemDiagrams");
        }
    }
}
