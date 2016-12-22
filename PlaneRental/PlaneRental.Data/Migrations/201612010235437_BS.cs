namespace PlaneRental.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BS : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rental", "AccountId", c => c.String());
            AlterColumn("dbo.Reservation", "AccountId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservation", "AccountId", c => c.Int(nullable: false));
            AlterColumn("dbo.Rental", "AccountId", c => c.Int(nullable: false));
        }
    }
}
