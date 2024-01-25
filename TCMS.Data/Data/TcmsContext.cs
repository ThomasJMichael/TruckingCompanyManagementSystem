using Microsoft.EntityFrameworkCore;
using TCMS.Data.Models;

namespace TCMS.Data.Data;

public class TcmsContext : DbContext
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

    // Constructor
    public TcmsContext() { }
    public TcmsContext(DbContextOptions<TcmsContext> options) : base(options) { }
    
    // OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Assignment
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Driver)
            .WithMany(d => d.Assignments)
            .HasForeignKey(a => a.DriverId)
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
            .HasForeignKey(a => a.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Driver>()
            .HasMany(d => d.IncidentReports)
            .WithOne(i => i.Driver)
            .HasForeignKey(i => i.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Driver>()
            .HasMany(d => d.DrugAndAlcoholTests)
            .WithOne(t => t.Driver)
            .HasForeignKey(t => t.DriverId)
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
            .HasForeignKey(i => i.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        // IncidentReport and DrugAndAlcoholTest
        modelBuilder.Entity<IncidentReport>()
            .HasOne(i => i.DrugAndAlcoholTest)
            .WithOne(t => t.IncidentReport)
            .HasForeignKey<DrugAndAlcoholTest>(t => t.IncidentReportID)
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

    }
}