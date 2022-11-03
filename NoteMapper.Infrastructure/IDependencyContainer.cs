namespace NoteMapper.Infrastructure
{
    public interface IDependencyContainer
    {
        IDependencyContainer AddScoped<TInterface, TImplementation>() 
            where TInterface : class
            where TImplementation : class, TInterface;
    }
}
