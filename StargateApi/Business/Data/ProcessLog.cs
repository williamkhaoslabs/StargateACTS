using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StargateAPI.Business.Data;

[Table("ProcessLog")]
public class ProcessLog
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string LogLevel { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Source { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
}
 
public class ProcessLogConfiguration : IEntityTypeConfiguration<ProcessLog>
{
    public void Configure(EntityTypeBuilder<ProcessLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.LogLevel).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Source).IsRequired().HasMaxLength(500);
        builder.Property(x => x.StackTrace).HasMaxLength(8000);
    }
}