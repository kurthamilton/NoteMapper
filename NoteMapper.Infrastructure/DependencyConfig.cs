using NoteMapper.Services;
using NoteMapper.Services.Web;

namespace NoteMapper.Infrastructure
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies(IDependencyContainer container)
        {
            container
                .AddScoped<IInstrumentFactory, InstrumentFactory>()
                .AddScoped<IMusicTheoryService, MusicTheoryService>();

            container
                .AddScoped<INoteMapViewModelService, NoteMapViewModelService>();
        }
    }
}
