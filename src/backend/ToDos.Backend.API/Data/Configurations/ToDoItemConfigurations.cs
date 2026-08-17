using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDos.Backend.API.Models;

namespace ToDos.Backend.API.Data.Configurations;

public class ToDoItemConfigurations : IEntityTypeConfiguration<ToDoItem>
{
    public const string SchemaName = "Core";
    public const string TableName = "ToDoItems";

    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable(TableName, SchemaName);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Priority)
            .HasConversion<string>();
    }
}
