namespace Planner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        task_text = c.String(),
                        solved = c.Boolean(nullable: false),
                        date = c.DateTime(nullable: false),
                        Users_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.Users_id)
                .Index(t => t.Users_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        surname = c.String(),
                        email = c.String(),
                        password = c.String(),
                        role = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Users_id", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "Users_id" });
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
        }
    }
}
