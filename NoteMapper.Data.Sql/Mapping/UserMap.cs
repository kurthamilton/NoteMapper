using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Mapping
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.UserId);

            builder.ToTable("Users");

            builder.Property(x => x.UserId)
                .ValueGeneratedOnAdd();
        }
    }
}
