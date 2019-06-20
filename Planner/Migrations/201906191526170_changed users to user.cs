namespace Planner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeduserstouser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "Users_id", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "Users_id" });
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "User_Id", c => c.Int());
            CreateIndex("dbo.Tasks", "User_Id");
            AddForeignKey("dbo.Tasks", "User_Id", "dbo.User", "Id");
            DropColumn("dbo.Tasks", "Users_id");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.Tasks", "Users_id", c => c.Int());
            DropForeignKey("dbo.Tasks", "User_Id", "dbo.User");
            DropIndex("dbo.Tasks", new[] { "User_Id" });
            DropColumn("dbo.Tasks", "User_Id");
            DropTable("dbo.User");
            CreateIndex("dbo.Tasks", "Users_id");
            AddForeignKey("dbo.Tasks", "Users_id", "dbo.Users", "id");
        }
    }
}
