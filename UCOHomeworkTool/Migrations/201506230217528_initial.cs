namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignmentNumber = c.Int(nullable: false),
                        Grade = c.Double(nullable: false),
                        Course_Id = c.Int(),
                        Course_Id1 = c.Int(),
                        Course_Id2 = c.Int(),
                        Student_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id1)
                .ForeignKey("dbo.Courses", t => t.Course_Id2)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Course_Id1)
                .Index(t => t.Course_Id2)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Teacher_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Problems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProblemNumber = c.Int(nullable: false),
                        GeneratedFrom = c.Int(nullable: false),
                        TrysRemaining = c.Int(nullable: false),
                        Description = c.String(),
                        IsAssigned = c.Boolean(nullable: false),
                        Assignment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_Id)
                .Index(t => t.Assignment_Id);
            
            CreateTable(
                "dbo.Givens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        Value = c.Int(nullable: false),
                        minRange = c.Int(),
                        maxRange = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Problem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.Problem_Id)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        delegatePointer = c.Binary(),
                        Label = c.String(),
                        Expected = c.Double(nullable: false),
                        FirstAttempt = c.Double(nullable: false),
                        SecondAttempt = c.Double(nullable: false),
                        ThirdAttempt = c.Double(nullable: false),
                        FourthAttempt = c.Double(nullable: false),
                        FifthAttempt = c.Double(nullable: false),
                        Problem_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Problems", t => t.Problem_Id)
                .Index(t => t.Problem_Id);
            
            CreateTable(
                "dbo.ProblemDiagrams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProblemId = c.Int(nullable: false),
                        ImageContent = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_Id = c.String(nullable: false, maxLength: 128),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_Id, t.Course_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .Index(t => t.Student_Id)
                .Index(t => t.Course_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Assignments", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Problems", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "Course_Id1", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentCourses", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assignments", "Course_Id", "dbo.Courses");
            DropIndex("dbo.StudentCourses", new[] { "Course_Id" });
            DropIndex("dbo.StudentCourses", new[] { "Student_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Responses", new[] { "Problem_Id" });
            DropIndex("dbo.Givens", new[] { "Problem_Id" });
            DropIndex("dbo.Problems", new[] { "Assignment_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.Assignments", new[] { "Student_Id" });
            DropIndex("dbo.Assignments", new[] { "Course_Id2" });
            DropIndex("dbo.Assignments", new[] { "Course_Id1" });
            DropIndex("dbo.Assignments", new[] { "Course_Id" });
            DropTable("dbo.StudentCourses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProblemDiagrams");
            DropTable("dbo.Responses");
            DropTable("dbo.Givens");
            DropTable("dbo.Problems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Courses");
            DropTable("dbo.Assignments");
        }
    }
}
