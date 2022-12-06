using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Mapping
{
    internal class UserActivationMap : IEntityTypeConfiguration<UserActivation>
    {
        public void Configure(EntityTypeBuilder<UserActivation> builder)
        {
            builder.HasKey(t => t.UserActivationId);

            builder.ToTable("UserActivations");

            builder.Property(x => x.UserActivationId)
                .ValueGeneratedOnAdd();
        }
    }
}
