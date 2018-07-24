using System;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.Infrastructure.Interfaces
{
    public interface IDirectoryWatcher: IDisposable
    {
        event EventHandler<IFileInfo> Changed;

        void Init();
    }
}