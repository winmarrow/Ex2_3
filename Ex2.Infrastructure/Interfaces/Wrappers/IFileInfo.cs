using System.IO;

namespace Ex2.Infrastructure.Interfaces.Wrappers
{
    public interface IFileInfo
    {
        bool Exists { get; }
        string FullName { get; }
        string Name { get; }
        long Length { get; }


        void Delete();
        FileStream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }
}