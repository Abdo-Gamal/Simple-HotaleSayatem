namespace HotalSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoomCategories", "CategoryImage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoomCategories", "CategoryImage");
        }
    }
}
