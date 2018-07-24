using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.BL.Entities.Wrappers;
using Ex2.Infrastructure.Interfaces.Factories;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities.Factories
{
    [Inject(typeof(TraceAspect))]
    public class FileSystemWatcherFactory : IFileSystemWatcherFactory
    {
        public IFileSystemWatcher Create()
        {
            return new FileSystemWatcherWrapper();
        }
    }
}