
namespace MechanicShop.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id).IsClustered(false);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

        builder.Property(c => c.Email)
               .HasMaxLength(150);

        builder.HasMany(c => c.Vehicles).WithOne().HasForeignKey(v => v.CustomerId);

        builder.Navigation(c => c.Vehicles)
       .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.HasIndex(c => c.Name)
            .IsClustered(true);

        builder.HasIndex(c => c.PhoneNumber);
    }
}