using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Mapping
{
    internal class UserLoginTokenMap : IEntityTypeConfiguration<UserLoginToken>
    {
        public void Configure(EntityTypeBuilder<UserLoginToken> builder)
        {
            builder.HasKey(t => t.UserLoginTokenId);

            builder.ToTable("UserLoginTokens");

            builder.Property(x => x.UserLoginTokenId)
                .ValueGeneratedOnAdd();
        }
    }
}
