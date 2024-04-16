using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TCMS.Data.Models;

namespace TCMS.Data.Data;

public class TcmsContext : IdentityDbContext<UserAccount>
{
    // DbSets
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DrugAndAlcoholTest> DrugAndAlcoholTests { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<IncidentReport> IncidentReports { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<Manifest> Manifests { get; set; }
    public DbSet<ManifestItem> ManifestItems { get; set; }
    public DbSet<PartDetails> PartDetails { get; set; }
    public DbSet<Payroll> Payrolls { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<RepairRecord> RepairRecords { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<TimeSheet> TimeSheets { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    // Constructor
    public TcmsContext() { }
    public TcmsContext(DbContextOptions<TcmsContext> options) : base(options) { }
    
    // OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Inventory
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product) 
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductId); 

        // Assignment
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Driver)
            .WithMany(d => d.Assignments)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Shipment)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Driver
        modelBuilder.Entity<Driver>()
            .HasMany(d => d.Assignments)
            .WithOne(a => a.Driver)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Driver>()
            .HasMany(d => d.IncidentReports)
            .WithOne(i => i.Driver)
            .HasForeignKey(i => i.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Driver>()
            .HasMany(d => d.DrugAndAlcoholTests)
            .WithOne(t => t.Driver)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Employee
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.UserAccount)
            .WithOne(u => u.Employee)
            .HasForeignKey<UserAccount>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Manifest
        modelBuilder.Entity<Manifest>()
            .HasMany(m => m.ManifestItems)
            .WithOne(i => i.Manifest)
            .HasForeignKey(i => i.ManifestId)
            .OnDelete(DeleteBehavior.Restrict);

        // ManifestItem
        modelBuilder.Entity<ManifestItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // PartDetails
        modelBuilder.Entity<PartDetails>()
            .HasOne(p => p.Vehicle)
            .WithMany(v => v.Parts)
            .HasForeignKey(p => p.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payroll
        modelBuilder.Entity<Payroll>()
            .HasOne(p => p.Employee)
            .WithMany()
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseOrder
        modelBuilder.Entity<PurchaseOrder>()
            .HasMany(p => p.Manifests)
            .WithOne(m => m.PurchaseOrder)
            .HasForeignKey(m => m.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Restrict);
        // RepairRecord
        modelBuilder.Entity<RepairRecord>()
            .HasOne(r => r.Vehicle)
            .WithMany(v => v.RepairRecords)
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // TimeSheet
        modelBuilder.Entity<TimeSheet>()
            .HasOne(t => t.Employee)
            .WithMany(e => e.TimeSheets)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserAccount
        modelBuilder.Entity<UserAccount>()
            .HasOne(u => u.Employee)
            .WithOne(e => e.UserAccount)
            .HasForeignKey<Employee>(e => e.UserAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // IncidentReport
        modelBuilder.Entity<IncidentReport>()
            .HasOne(i => i.Driver)
            .WithMany(d => d.IncidentReports)
            .HasForeignKey(i => i.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // IncidentReport and DrugAndAlcoholTest
        modelBuilder.Entity<IncidentReport>()
            .HasOne(i => i.DrugAndAlcoholTest)
            .WithOne(t => t.IncidentReport)
            .HasForeignKey<DrugAndAlcoholTest>(t => t.IncidentReportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Shipment
        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Assignments)
            .WithOne(a => a.Shipment)
            .HasForeignKey(a => a.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Shipment and Vehicle
        modelBuilder.Entity<Shipment>()
            .HasOne(s => s.Vehicle)
            .WithMany(v => v.Shipments)
            .HasForeignKey(s => s.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // MaintenanceRecord
        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.Vehicle)
            .WithMany(v => v.MaintenanceRecords)
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasMany(m => m.PartDetails)
            .WithOne(p => p.MaintenanceRecord)
            .HasForeignKey(p => p.MaintenanceRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        // Inventory
        modelBuilder.Entity<Inventory>()
            .Property(i => i.InventoryId)
            .ValueGeneratedOnAdd();

        // Assignment
        modelBuilder.Entity<Assignment>()
            .Property(a => a.AssignmentId)
            .ValueGeneratedOnAdd();

        // Employee
        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeId)
            .ValueGeneratedOnAdd();

        // Manifest
        modelBuilder.Entity<Manifest>()
            .Property(m => m.ManifestId)
            .ValueGeneratedOnAdd();

        // ManifestItem
        modelBuilder.Entity<ManifestItem>()
            .Property(mi => mi.ManifestItemId)
            .ValueGeneratedOnAdd();

        // PartDetails
        modelBuilder.Entity<PartDetails>()
            .Property(p => p.PartDetailsId)
            .ValueGeneratedOnAdd();

        // Payroll
        modelBuilder.Entity<Payroll>()
            .Property(p => p.PayrollId)
            .ValueGeneratedOnAdd();

        // PurchaseOrder
        modelBuilder.Entity<PurchaseOrder>()
            .Property(po => po.PurchaseOrderId)
            .ValueGeneratedOnAdd();

        // RepairRecord
        modelBuilder.Entity<RepairRecord>()
            .Property(rr => rr.RepairRecordId)
            .ValueGeneratedOnAdd();

        // TimeSheet
        modelBuilder.Entity<TimeSheet>()
            .Property(t => t.TimeSheetId)
            .ValueGeneratedOnAdd();

        // IncidentReport
        modelBuilder.Entity<IncidentReport>()
            .Property(ir => ir.IncidentReportId)
            .ValueGeneratedOnAdd();

        // DrugAndAlcoholTest
        modelBuilder.Entity<DrugAndAlcoholTest>()
            .Property(dat => dat.DrugAndAlcoholTestId)
            .ValueGeneratedOnAdd();

        // Shipment
        modelBuilder.Entity<Shipment>()
            .Property(s => s.ShipmentId)
            .ValueGeneratedOnAdd();

        // MaintenanceRecord
        modelBuilder.Entity<MaintenanceRecord>()
            .Property(mr => mr.MaintenanceRecordId)
            .ValueGeneratedOnAdd();

        // Vehicle
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.VehicleId)
            .ValueGeneratedOnAdd();
    }
}