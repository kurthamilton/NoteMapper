using NoteMapper.Infrastructure;

namespace NoteMapper.Web.Api
{
    public class DependencyContainer : IDependencyContainer
    {
        private readonly IServiceCollection _services;

        public DependencyContainer(IServiceCollection services)
        {
            _services = services;
        }

        public IDependencyContainer AddScoped<TInterface, TImplementation>() 
            where TInterface : class 
            where TImplementation : class, TInterface
        {
            _services.AddScoped<TInterface, TImplementation>();
            return this;
        }
    }
}
