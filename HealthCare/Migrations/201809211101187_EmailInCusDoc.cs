namespace HealthCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailInCusDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Email", c => c.String());
            AddColumn("dbo.Doctors", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "Email");
            DropColumn("dbo.Customers", "Email");
        }
    }
}
