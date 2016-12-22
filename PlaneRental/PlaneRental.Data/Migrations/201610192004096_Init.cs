namespace PlaneRental.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        LoginEmail = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        CreditPlaned = c.String(),
                        ExpDate = c.String(),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Plane",
                c => new
                    {
                        PlaneId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Color = c.String(),
                        Year = c.Int(nullable: false),
                        RentalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PlaneId);
            
            CreateTable(
                "dbo.Rental",
                c => new
                    {
                        RentalId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        PlaneId = c.Int(nullable: false),
                        DateRented = c.DateTime(nullable: false),
                        DateDue = c.DateTime(nullable: false),
                        DateReturned = c.DateTime(),
                    })
                .PrimaryKey(t => t.RentalId);
            
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        PlaneId = c.Int(nullable: false),
                        RentalDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reservation");
            DropTable("dbo.Rental");
            DropTable("dbo.Plane");
            DropTable("dbo.Account");
        }
    }
}
