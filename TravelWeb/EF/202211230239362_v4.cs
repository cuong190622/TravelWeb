namespace TravelWeb.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentType = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        UserId = c.String(),
                        Date = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comments");
        }
    }
}
