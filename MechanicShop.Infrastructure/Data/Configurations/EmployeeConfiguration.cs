namespace MechanicShop.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");


        builder.HasKey(e => e.Id).IsClustered(false);

        builder.Property(e => e.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.LastName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.Role).HasConversion<string>().IsRequired();


        builder.HasIndex(e => e.FirstName)
            .IsClustered(true);

        builder.HasIndex(e => e.LastName);
    }
}