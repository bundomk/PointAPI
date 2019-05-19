using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Point.Data.Models
{
    public partial class PointAdvisorContext : DbContext
    {
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<InfoPost> InfoPost { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<VotePost> VotePost { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }
        public virtual DbSet<ZonePoint> ZonePoint { get; set; }

        public PointAdvisorContext(DbContextOptions<PointAdvisorContext> options)
                  : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image", "dbo");

                entity.Property(e => e.CameraMaker).HasColumnType("varchar(32)");

                entity.Property(e => e.CameraModel).HasColumnType("varchar(32)");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.Latitude).HasColumnType("decimal");

                entity.Property(e => e.Longitude).HasColumnType("decimal");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.TakenTime).HasColumnType("datetime");

                entity.Property(e => e.Type).HasColumnType("varchar(16)");

                entity.HasOne(d => d.InfoPost)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.InfoPostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Image_InfoPost");
            });

            modelBuilder.Entity<InfoPost>(entity =>
            {
                entity.ToTable("InfoPost", "dbo");

                entity.Property(e => e.ApprovedTime).HasColumnType("datetime");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.FixedTime).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasColumnType("decimal");

                entity.Property(e => e.Longitude).HasColumnType("decimal");

                entity.HasOne(d => d.BelongToNavigation)
                    .WithMany(p => p.InfoPostBelongToNavigation)
                    .HasForeignKey(d => d.BelongTo)
                    .HasConstraintName("FK_InfoPost_BelongToInstitution");

                entity.HasOne(d => d.FixedByNavigation)
                    .WithMany(p => p.InfoPostFixedByNavigation)
                    .HasForeignKey(d => d.FixedBy)
                    .HasConstraintName("FK_InfoPost_FixedInstitution");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.InfoPost)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_InfoPost_User");
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.ToTable("Institution", "dbo");

                entity.Property(e => e.Address).HasColumnType("varchar(32)");

                entity.Property(e => e.City).HasColumnType("varchar(16)");

                entity.Property(e => e.Country).HasColumnType("varchar(16)");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description).HasColumnType("varchar(512)");

                entity.Property(e => e.Email).HasColumnType("varchar(32)");

                entity.Property(e => e.Latitude).HasColumnType("decimal");

                entity.Property(e => e.Longitude).HasColumnType("decimal");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Number).HasColumnType("varchar(64)");

                entity.Property(e => e.Phone).HasColumnType("varchar(32)");

                entity.Property(e => e.ResponsiblePersonName).HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "dbo");

                entity.HasIndex(e => e.DeviceId)
                    .HasName("IX_User_DeviceId")
                    .IsUnique();

                entity.HasIndex(e => e.Key)
                    .HasName("IX_User_Key");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description).HasColumnType("varchar(512)");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Key).HasDefaultValueSql("newid()");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<VotePost>(entity =>
            {
                entity.HasKey(e => new { e.InfoPostId, e.UserId })
                    .HasName("PK_VotePost");

                entity.ToTable("VotePost", "dbo");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.HasOne(d => d.InfoPost)
                    .WithMany(p => p.VotePost)
                    .HasForeignKey(d => d.InfoPostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_VotePost_InfoPost");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.VotePost)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_VotePost_User");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.ToTable("Zone", "dbo");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("getdate()");

                entity.Property(e => e.Description).HasColumnType("varchar(128)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(32)");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Zone)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Zone_Institution");
            });

            modelBuilder.Entity<ZonePoint>(entity =>
            {
                entity.ToTable("ZonePoint", "dbo");

                entity.Property(e => e.Latitude).HasColumnType("decimal(9,6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.ZonePoint)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ZonePoint_Zone");
            });
        }
    }
}