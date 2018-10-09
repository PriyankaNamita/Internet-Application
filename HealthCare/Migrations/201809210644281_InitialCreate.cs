namespace HealthCare.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        DoctorID = c.Int(nullable: false),
                        OnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.DoctorID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Weight = c.Decimal(nullable: true, precision: 18, scale: 2),
                        Age = c.Decimal(nullable: true, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        Path = c.String(),
                        OnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Specialization = c.String(),
                        Capacity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AppointmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Appointments", t => t.AppointmentID, cascadeDelete: true)
                .Index(t => t.AppointmentID);
            
            CreateTable(
                "dbo.PresMeds",
                c => new
                    {
                        PrescriptionID = c.Int(nullable: false),
                        MedicineID = c.Int(nullable: false),
                        Dosage = c.String(),
                    })
                .PrimaryKey(t => new { t.PrescriptionID, t.MedicineID })
                .ForeignKey("dbo.Medicines", t => t.MedicineID, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionID, cascadeDelete: true)
                .Index(t => t.PrescriptionID)
                .Index(t => t.MedicineID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        DoctorID = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        ReviewText = c.String(),
                        OnDate = c.DateTime(nullable: false),
                        IsArchive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.DoctorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Reviews", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.PresMeds", "PrescriptionID", "dbo.Prescriptions");
            DropForeignKey("dbo.PresMeds", "MedicineID", "dbo.Medicines");
            DropForeignKey("dbo.Prescriptions", "AppointmentID", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Reports", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Reviews", new[] { "DoctorID" });
            DropIndex("dbo.Reviews", new[] { "CustomerID" });
            DropIndex("dbo.PresMeds", new[] { "MedicineID" });
            DropIndex("dbo.PresMeds", new[] { "PrescriptionID" });
            DropIndex("dbo.Prescriptions", new[] { "AppointmentID" });
            DropIndex("dbo.Reports", new[] { "CustomerID" });
            DropIndex("dbo.Appointments", new[] { "DoctorID" });
            DropIndex("dbo.Appointments", new[] { "CustomerID" });
            DropTable("dbo.Reviews");
            DropTable("dbo.PresMeds");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Medicines");
            DropTable("dbo.Doctors");
            DropTable("dbo.Reports");
            DropTable("dbo.Customers");
            DropTable("dbo.Appointments");
        }
    }
}
