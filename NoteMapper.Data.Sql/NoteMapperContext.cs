using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NoteMapper.Data.Sql
{
    public class NoteMapperContext : DbContext
    {
        private readonly NoteMapperContextSettings _settings;

        public NoteMapperContext(NoteMapperContextSettings settings)
        {
            _settings = settings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_settings.ConnectionString))
            {
                return;
            }

            optionsBuilder
                .UseSqlServer(_settings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Assembly assembly = GetType().Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
