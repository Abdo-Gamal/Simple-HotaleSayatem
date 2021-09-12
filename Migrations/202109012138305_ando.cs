namespace HotalSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ando : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserPassword", c => c.String(nullable: false));
            DropColumn("dbo.Users", "UserPassward");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserPassward", c => c.String(nullable: false));
            DropColumn("dbo.Users", "UserPassword");
        }
    }
}
