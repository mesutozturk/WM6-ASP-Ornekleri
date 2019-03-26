namespace Admin.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthOperationRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Id2 = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Id2 })
                .ForeignKey("dbo.AuthOperations", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.Id2, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Id2);
            
            CreateTable(
                "dbo.AuthOperations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ControllerName = c.String(),
                        ActionName = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthOperationRoles", "Id2", "dbo.AspNetRoles");
            DropForeignKey("dbo.AuthOperationRoles", "Id", "dbo.AuthOperations");
            DropIndex("dbo.AuthOperationRoles", new[] { "Id2" });
            DropIndex("dbo.AuthOperationRoles", new[] { "Id" });
            DropTable("dbo.AuthOperations");
            DropTable("dbo.AuthOperationRoles");
        }
    }
}
