﻿namespace TravelWeb.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "CommentType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "CommentType", c => c.Int(nullable: false));
        }
    }
}
