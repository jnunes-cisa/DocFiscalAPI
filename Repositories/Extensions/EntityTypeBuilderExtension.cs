using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repositories.Entities;

namespace Repositories.Extensions
{
    public static class EntityTypeBuilderExtension
    {
        public static void CreateAuditProperties(EntityTypeBuilder builder)
        {            
            builder.Property("DataCriacao").HasColumnName("DataCriacao").HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property("DataAlteracao").HasColumnName("DataAlteracao").HasDefaultValueSql("GETDATE()").IsRequired();            
        }
    }
}
