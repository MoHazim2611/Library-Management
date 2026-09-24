using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> b)
    {
        b.ToTable("Payments", t =>
        {
            t.HasCheckConstraint("CK_Payments_Amount", "[Amount] > 0");
            t.HasCheckConstraint("CK_Payments_Method", "[PaymentMethod] IN ('Cash','Card','Online')");
        });

        b.HasKey(x => x.Id);
        b.Property(x => x.Amount).HasColumnType("decimal(10,2)");
        b.Property(x => x.PaymentMethod).HasMaxLength(20).IsRequired();
        b.Property(x => x.PaidAt).HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();

        b.HasIndex(x => x.FineId).HasDatabaseName("IX_Payments_FineId");
    }
}
