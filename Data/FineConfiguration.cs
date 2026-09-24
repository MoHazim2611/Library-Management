using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Data.Configurations;

public class FineConfiguration : IEntityTypeConfiguration<Fine>
{
    public void Configure(EntityTypeBuilder<Fine> b)
    {
        b.ToTable("Fines", t =>
        {
            t.HasCheckConstraint("CK_Fines_Amount", "[Amount] > 0");
            t.HasCheckConstraint("CK_Fines_Status", "[Status] IN ('Unpaid','PartiallyPaid','Paid')");
        });

        b.HasKey(x => x.Id);
        b.Property(x => x.Amount).HasColumnType("decimal(10,2)");
        b.Property(x => x.Reason).HasMaxLength(250).IsRequired();
        b.Property(x => x.Status).HasMaxLength(20).IsRequired().HasDefaultValue(FineStatus.Unpaid);
        b.Property(x => x.CreatedAt).HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();

        b.HasMany(x => x.Payments).WithOne(p => p.Fine)
            .HasForeignKey(p => p.FineId).OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.LoanId).HasDatabaseName("IX_Fines_LoanId");
        b.HasIndex(x => x.MemberId).HasDatabaseName("IX_Fines_MemberId");
        b.HasIndex(x => x.Status).HasDatabaseName("IX_Fines_Status");
    }
}
