using IF.ContactManagement.Application.Interfaces.Services;
using IF.ContactManagement.Domain.Common;
using IF.ContactManagement.Domain.Constants;
using IF.ContactManagement.Domain.Entities;
using IF.ContactManagement.Domain.Enums;
using IF.ContactManagement.Infrastructure.Persistence.ValueGenerators;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace IF.ContactManagement.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IConfiguration configuration;

        private readonly string connectionString;
        public string? UserId { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions,
                                    ITenantService tenantService,
                                    IConfiguration configuration) : base(dbContextOptions)
        {
            UserId = tenantService.GetUserId();
            this.configuration = configuration;
        }

        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Fund> Funds => Set<Fund>();
        public DbSet<FundContact> FundContacts => Set<FundContact>();
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<RoleModulePermission> RoleModulePermissions { get; set; }
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<SystemModuleCategory> SystemModuleCategories { get; set; }
        public DbSet<UserPermissionsAssignment> UserPermissionsAssignments { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public IDbConnection DapperConnection => new SqlConnection(connectionString);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fund
            builder.Entity<Fund>(entity =>
            {
                entity.HasKey(f => f.Id); // Primary key

                // Relationships
                entity.HasMany(f => f.FundContacts)
                      .WithOne(fc => fc.Fund)
                      .HasForeignKey(fc => fc.FundId);

                // Audit properties
                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime2")
                      .ValueGeneratedNever(); // Automatically set UTC timestamp on insert

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime2"); // Nullable, update manually

                entity.Property(e => e.DeletedAt)
                      .HasColumnType("datetime2"); // Nullable, set when soft-deleting

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(36); // User who created the record

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(36); // User who updated the record

                entity.Property(e => e.DeletedBy)
                      .HasMaxLength(36); // User who deleted the record
            });

            // Contact
            builder.Entity<Contact>(entity =>
            {
                entity.HasKey(c => c.Id); // Primary key

                // Relationships
                entity.HasMany(c => c.FundContacts)
                      .WithOne(fc => fc.Contact)
                      .HasForeignKey(fc => fc.ContactId);

                // Audit properties
                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime2")
                      .ValueGeneratedNever();

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                      .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(36);

                entity.Property(e => e.UpdatedBy)
                     .HasMaxLength(36); // User who updated the record

                entity.Property(e => e.DeletedBy)
                      .HasMaxLength(36); // User who deleted the record
            });

            // FundContact
            builder.Entity<FundContact>(entity =>
            {
                entity.HasKey(fc => new { fc.FundId, fc.ContactId }); // Composite key

                // Relationships
                entity.HasOne(fc => fc.Fund)
                      .WithMany(f => f.FundContacts)
                      .HasForeignKey(fc => fc.FundId);

                entity.HasOne(fc => fc.Contact)
                      .WithMany(c => c.FundContacts)
                      .HasForeignKey(fc => fc.ContactId);

                // Audit properties
                entity.Property(e => e.CreatedAt)
                      .HasColumnType("datetime2")
                      .ValueGeneratedNever();

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                      .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(36);

                entity.Property(e => e.UpdatedBy)
                     .HasMaxLength(36); // User who updated the record

                entity.Property(e => e.DeletedBy)
                      .HasMaxLength(36); // User who deleted the record
            });


            #region Identity
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "ContactManagement");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasMaxLength(36)
                      .IsRequired();

                entity.HasIndex(u => u.NormalizedUserName, "UserNameIndex").IsUnique();
                entity.HasIndex(u => u.NormalizedEmail, "EmailIndex");

                entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                entity.Property(u => u.FullName).HasMaxLength(256);
                entity.Property(u => u.FirstName).HasMaxLength(256);
                entity.Property(u => u.LastName).HasMaxLength(256);
                entity.Property(u => u.Email).HasMaxLength(256);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Gender)
                    .HasConversion(
                        v => v.ToString(),
                        v => (Gender)Enum.Parse(typeof(Gender), v));
                entity.Property(u => u.UserName).HasMaxLength(256);
                entity.Property(u => u.NormalizedUserName).HasMaxLength(256);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(256);

                entity.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                entity.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                entity.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                entity.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("UserClaims", "ContactManagement");

                entity.HasKey(uc => uc.Id);
            });

            builder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("UserLogins", "ContactManagement");

                entity.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                entity.Property(l => l.LoginProvider).HasMaxLength(128);
                entity.Property(l => l.ProviderKey).HasMaxLength(128);
            });

            builder.Entity<UserToken>(entity =>
            {
                entity.ToTable("UserTokens", "ContactManagement");

                entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                entity.Property(t => t.LoginProvider).HasMaxLength(128);
                entity.Property(t => t.Name).HasMaxLength(128);
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "ContactManagement");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasMaxLength(36)
                      .IsRequired();

                entity.HasIndex(r => r.NormalizedName, "RoleNameIndex").IsUnique();

                entity.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                entity.Property(u => u.Name).HasMaxLength(256);
                entity.Property(u => u.NormalizedName).HasMaxLength(256);
                entity.Property(u => u.Description).HasMaxLength(500);
                entity.Property(u => u.Icon);

                entity.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

                entity.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable("RoleClaims", "ContactManagement");

                entity.HasKey(e => e.Id);
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles", "ContactManagement");

                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            builder.Entity<UserRefreshToken>(entity =>
            {
                entity.ToTable("UserRefreshTokens", "ContactManagement");

                entity.HasKey(e => e.UserRefreshTokenId);

                entity.Property(e => e.UserRefreshTokenId)
                      .HasMaxLength(36)
                      .HasDefaultValueSql("NEWID()");

                entity.Property(e => e.UserId)
                      .HasMaxLength(36)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);

                entity.Property(e => e.UserId).HasMaxLength(36);

                entity.HasOne(e => e.User)
                         .WithMany(u => u.RefreshTokens)
                         .HasForeignKey(e => e.UserId)
                         .HasConstraintName("UserRefreshToken_User_FK");
            });

            #endregion
            #region Catalogues

            builder.Entity<RoleModulePermission>(entity =>
            {
                entity.ToTable("RoleModulePermissions", "ContactManagement");

                entity.HasKey(e => new
                {
                    e.RoleId,
                    e.SystemModuleId,
                    e.PermissionTypeId
                }).HasName("RoleModulePermission_PK");

                entity.Property(e => e.RoleId).HasMaxLength(36);
                entity.Property(e => e.SystemModuleId).HasMaxLength(36);
                entity.Property(e => e.PermissionTypeId).HasMaxLength(36);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);


                // create the foreign key for the Role
                entity.HasOne(e => e.Role)
                                  .WithMany(r => r.RoleModulePermissions)
                                  .HasForeignKey(e => e.RoleId)
                                  .HasConstraintName("RoleModulePermission_Role_FK");

                // create the foreign key for the SystemModule
                entity.HasOne(e => e.SystemModule)
                                  .WithMany(s => s.RoleModulePermissions)
                                  .HasForeignKey(e => e.SystemModuleId)
                                  .HasConstraintName("RoleModulePermission_SystemModule_FK");

                // create the foreign key for the PermissionType
                entity.HasOne(e => e.PermissionType)
                                  .WithMany(p => p.RoleModulePermissions)
                                  .HasForeignKey(e => e.PermissionTypeId)
                                  .HasConstraintName("RoleModulePermission_PermissionType_FK");
            });

            builder.Entity<UserPermissionsAssignment>(entity =>
            {
                entity.ToTable("UserPermissionsAssignment", "ContactManagement");
                entity.HasKey(e => new
                {
                    e.UserId,
                    e.RoleId
                }).HasName("UserPermissionsAssignment_PK");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);


                entity.HasOne(e => e.User)
                                  .WithMany(p => p.UserPermissionsAssignments)
                                  .HasForeignKey(e => e.UserId)
                                  .HasConstraintName("UserPermissionsAssignment_User_FK");

                entity.HasOne(e => e.Role)
                                  .WithMany(p => p.UserPermissionsAssignments)
                                  .HasForeignKey(e => e.RoleId)
                                  .HasConstraintName("UserPermissionsAssignment_Role_FK");
            });
            #endregion
            #region Modules

            builder.Entity<SystemModule>(entity =>
            {
                entity.ToTable("SystemModules", "ContactManagement");

                entity.HasKey(e => e.SystemModuleId).HasName("SystemModule_PK");

                entity.Property(e => e.SystemModuleId)
                     .HasMaxLength(36)
                     .HasDefaultValueSql("NEWID()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);

                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Url).HasMaxLength(1000);
                entity.Property(e => e.ParentId).HasMaxLength(36);
                entity.Property(e => e.SystemModuleCategorieId).HasMaxLength(36);

                // create the foreign key for the SystemModule
                entity.HasOne(e => e.Parent)
                      .WithMany(s => s.Children)
                      .HasForeignKey(e => e.ParentId)
                      .HasConstraintName("SystemModule_Parent_FK");

                // create the foreign key for the SystemModuleCategorie
                entity.HasOne(e => e.SystemModuleCategory)
                      .WithMany(s => s.SystemModules)
                      .HasForeignKey(e => e.SystemModuleCategorieId)
                      .HasConstraintName("SystemModule_SystemModuleCategorie_FK");
            });

            builder.Entity<SystemModuleCategory>(entity =>
            {
                entity.ToTable("SystemModuleCategories", "ContactManagement");

                entity.HasKey(e => e.SystemModuleCategoryId).HasName("SystemModuleCategorie_PK");

                entity.Property(e => e.SystemModuleCategoryId).HasMaxLength(36).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);

                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            builder.Entity<PermissionType>(entity =>
            {
                entity.ToTable("PermissionTypes", "ContactManagement");
                entity.HasKey(e => e.PermissionTypeId).HasName("PermissionType_PK");
                entity.Property(e => e.PermissionTypeId).HasMaxLength(36).HasValueGenerator<GuidStringValueGenerator>();
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.DeletedAt)
                    .HasColumnType("datetime2");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(36);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
            });
            #endregion

            var systemModules = Seed.SystemModule.GetDataSeed();
            var systemModuleCategories = Seed.SystemModuleCategorie.GetDataSeed();
            var roleModulePermission = Seed.RoleModulePermission.GetDataSeed();
            var permissionType = Seed.PermissionType.GetDataSeed();
            var role = Seed.Role.GetDataSeed();
            var userPermissionAsignment = Seed.UserPermissionsAssignment.GetDataSeed();

            builder.Entity<SystemModule>().HasData(systemModules);
            builder.Entity<SystemModuleCategory>().HasData(systemModuleCategories);
            builder.Entity<RoleModulePermission>().HasData(roleModulePermission);
            builder.Entity<PermissionType>().HasData(permissionType);
            builder.Entity<Role>().HasData(role);
            builder.Entity<UserPermissionsAssignment>().HasData(userPermissionAsignment);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Create the validations for the entities that inherit from the class BaseAuditableEntity
            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added &&
                                                                    e.Entity is BaseAuditableEntity))
            {
                // TODO: Uncomment this in production
                //if (string.IsNullOrEmpty(UserId))
                //{
                //    throw new Exception("UserId was not found at the moment of create the record");
                //}
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = ConstantsCreatedBy.System;
                }

                var entity = item.Entity as BaseAuditableEntity;
                entity!.CreatedBy = UserId;
            }


            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
