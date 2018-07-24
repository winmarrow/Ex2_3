using System.IO;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities.Wrappers
{
    [Inject(typeof(TraceAspect))]
    public class FileSystemWatcherWrapper : FileSystemWatcher, IFileSystemWatcher
    {
        public FileSystemWatcherWrapper()
        {
        }

        public FileSystemWatcherWrapper(string path)
            : base(path)
        {
        }

        public FileSystemWatcherWrapper(string path, string filter)
            : base(path, filter)
        {
        }
    }
}