namespace Planner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tasksrelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "User_Id", "dbo.User");
            DropIndex("dbo.Tasks", new[] { "User_Id" });
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Solved = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("dbo.Tasks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        task_text = c.String(),
                        solved = c.Boolean(nullable: false),
                        date = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("dbo.Task", "UserId", "dbo.User");
            DropIndex("dbo.Task", new[] { "UserId" });
            DropTable("dbo.Task");
            CreateIndex("dbo.Tasks", "User_Id");
            AddForeignKey("dbo.Tasks", "User_Id", "dbo.User", "Id");
        }
    }
}
