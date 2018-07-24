using System.IO;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities.Wrappers
{
    [Inject(typeof(TraceAspect))]
    public class FileInfoWrapper : IFileInfo
    {
        private readonly FileInfo _fileInfo;

        public FileInfoWrapper(string fullName)
        {
            _fileInfo = new FileInfo(fullName);
        }

        public bool Exists => _fileInfo.Exists;

        public string FullName => _fileInfo.FullName;

        public string Name => _fileInfo.Name;

        public long Length => _fileInfo.Length;

        public FileStream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return _fileInfo.Open(fileMode, fileAccess, fileShare);
        }

        public void Delete()
        {
            _fileInfo.Delete();
        }
    }
}