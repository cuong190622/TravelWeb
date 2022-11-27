namespace TravelWeb.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Price");
        }
    }
}
