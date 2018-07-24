using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.Infrastructure.Interfaces.Factories
{
    public interface IFileSystemWatcherFactory
    {
        IFileSystemWatcher Create();
    }
}