namespace SSISLibrary
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SSISDbModelContext : DbContext
    {
        public SSISDbModelContext()
            : base()
        {
        }

        public virtual DbSet<AdjustmentVoucher> AdjustmentVoucher { get; set; }
        public virtual DbSet<AdjustmentVoucherDetail> AdjustmentVoucherDetail { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CollectionPoint> CollectionPoint { get; set; }
        public virtual DbSet<Delegation> Delegation { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<DeptRequest> DeptRequest { get; set; }
        public virtual DbSet<DeptRequestDetail> DeptRequestDetail { get; set; }
        public virtual DbSet<DeptRequestStatus> DeptRequestStatus { get; set; }
        public virtual DbSet<Disbursement> Disbursement { get; set; }
        public virtual DbSet<DisbursementDetail> DisbursementDetail { get; set; }
        public virtual DbSet<Discrepancy> Discrepancy { get; set; }
        public virtual DbSet<DiscrepancyDetail> DiscrepancyDetail { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public virtual DbSet<Retrieval> Retrieval { get; set; }
        public virtual DbSet<RetrievalDetail> RetrievalDetail { get; set; }
        public virtual DbSet<SSISUser> SSISUser { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<UserRequest> UserRequest { get; set; }
        public virtual DbSet<UserRequestDetail> UserRequestDetail { get; set; }
        public virtual DbSet<UserRequestStatus> UserRequestStatus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdjustmentVoucher>()
                .HasMany(e => e.AdjustmentVoucherDetail)
                .WithRequired(e => e.AdjustmentVoucher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AdjustmentVoucherDetail>()
                .Property(e => e.AdjustmentComments)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionPoint>()
                .Property(e => e.CollectionPointName)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionPoint>()
                .Property(e => e.CollectionTime)
                .IsUnicode(false);

            modelBuilder.Entity<CollectionPoint>()
                .HasMany(e => e.Department)
                .WithRequired(e => e.CollectionPoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionPoint>()
                .HasMany(e => e.Disbursement)
                .WithRequired(e => e.CollectionPoint1)
                .HasForeignKey(e => e.CollectionPoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.DeptCode)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.DeptName)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.ContactFax)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.DeptRequest)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.UserRequest)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeptRequest>()
                .Property(e => e.DeptRequestStatusComment)
                .IsUnicode(false);

            modelBuilder.Entity<DeptRequest>()
                .HasMany(e => e.DeptRequestDetail)
                .WithRequired(e => e.DeptRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeptRequestStatus>()
                .Property(e => e.DeptRequestStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<DeptRequestStatus>()
                .HasMany(e => e.DeptRequest)
                .WithRequired(e => e.DeptRequestStatus1)
                .HasForeignKey(e => e.DeptRequestStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disbursement>()
                .Property(e => e.DisbursementStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Disbursement>()
                .HasMany(e => e.DisbursementDetail)
                .WithRequired(e => e.Disbursement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Discrepancy>()
                .Property(e => e.DiscrepancyStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Discrepancy>()
                .HasMany(e => e.DiscrepancyDetail)
                .WithRequired(e => e.Discrepancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscrepancyDetail>()
                .Property(e => e.InventoryAmount)
                .HasPrecision(29, 2);

            modelBuilder.Entity<DiscrepancyDetail>()
                .Property(e => e.ActualAmount)
                .HasPrecision(29, 2);

            modelBuilder.Entity<DiscrepancyDetail>()
                .Property(e => e.DiscrepancyAmount)
                .HasPrecision(30, 2);

            modelBuilder.Entity<DiscrepancyDetail>()
                .Property(e => e.DiscrepancyReason)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.ItemNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .Property(e => e.UnitOfMeasure)
                .IsUnicode(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.AdjustmentVoucherDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.DeptRequestDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.DisbursementDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.DiscrepancyDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.PurchaseOrderDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.RetrievalDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Inventory>()
                .HasMany(e => e.UserRequestDetail)
                .WithRequired(e => e.Inventory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(e => e.DeliveryStatus)
                .IsUnicode(false);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(e => e.PurchaseOrderDetail)
                .WithRequired(e => e.PurchaseOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Retrieval>()
                .HasMany(e => e.RetrievalDetail)
                .WithRequired(e => e.Retrieval)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SSISUser>()
                .HasMany(e => e.CollectionPoint)
                .WithOptional(e => e.SSISUser)
                .HasForeignKey(e => e.ClerkEmail);

            modelBuilder.Entity<SSISUser>()
                .HasMany(e => e.Department)
                .WithOptional(e => e.SSISUser)
                .HasForeignKey(e => e.HeadEmail);

            modelBuilder.Entity<SSISUser>()
                .HasMany(e => e.Disbursement)
                .WithOptional(e => e.SSISUser)
                .HasForeignKey(e => e.CollectedByUserEmail);

            modelBuilder.Entity<SSISUser>()
                .HasMany(e => e.Retrieval)
                .WithOptional(e => e.SSISUser)
                .HasForeignKey(e => e.RetrieverEmail);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierCode)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierName)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.SupplierContactFax)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.GSTRegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Inventory)
                .WithOptional(e => e.Supplier)
                .HasForeignKey(e => e.Supplier1ID);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.PurchaseOrder)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRequest>()
                .Property(e => e.RequestStatusComment)
                .IsUnicode(false);

            modelBuilder.Entity<UserRequest>()
                .HasMany(e => e.UserRequestDetail)
                .WithRequired(e => e.UserRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRequestStatus>()
                .Property(e => e.UserRequestStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<UserRequestStatus>()
                .HasMany(e => e.UserRequest)
                .WithRequired(e => e.UserRequestStatus)
                .HasForeignKey(e => e.RequestStatus)
                .WillCascadeOnDelete(false);
        }
    }
}
