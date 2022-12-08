using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Mapping
{
    internal class RegistrationCodeMap : IEntityTypeConfiguration<RegistrationCode>
    {
        public void Configure(EntityTypeBuilder<RegistrationCode> builder)
        {
            builder.HasKey(t => t.RegistrationCodeId);

            builder.ToTable("RegistrationCodes");
            
            builder.Property(x => x.RegistrationCodeId)
                .ValueGeneratedOnAdd();
        }
    }
}
