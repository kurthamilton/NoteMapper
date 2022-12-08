using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class RegistrationCodeRepository : SqlRepositoryBase<RegistrationCode>, IRegistrationCodeRepository
    {
        public RegistrationCodeRepository(NoteMapperContext context) 
            : base(context)
        {
        }

        public Task<RegistrationCode?> FindAsync(string code)
        {
            return FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}
