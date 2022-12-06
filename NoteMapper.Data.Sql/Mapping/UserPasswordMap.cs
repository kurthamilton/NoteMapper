using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Mapping
{
    internal class UserPasswordMap : IEntityTypeConfiguration<UserPassword>
    {
        public void Configure(EntityTypeBuilder<UserPassword> builder)
        {
            builder.HasKey(t => t.UserPasswordId);

            builder.ToTable("UserPasswords");            
        }
    }
}
