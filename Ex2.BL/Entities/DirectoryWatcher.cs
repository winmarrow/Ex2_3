using System;
using System.IO;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Factories;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities
{
    [Inject(typeof(TraceAspect))]
    public class DirectoryWatcher : IDirectoryWatcher
    {
        private readonly IFileInfoFactory _fileInfoFactory;
        private readonly AppSettings _settings;
        private readonly IFileSystemWatcherFactory _watcherFactory;

        private IFileSystemWatcher _watcher;
        private bool _disposedValue;

        public DirectoryWatcher(AppSettings settings, IFileSystemWatcherFactory watcherFactory, IFileInfoFactory fileInfoFactory)
        {
            _settings = settings;
            _watcherFactory = watcherFactory;
            _fileInfoFactory = fileInfoFactory;
        }

        ~DirectoryWatcher()
        {
            Dispose(false);
        }

        public void Init()
        {
            _watcher = _watcherFactory.Create();

            _watcher.Path = _settings.Dir.DirectoryWithFilesToSend;
            _watcher.EnableRaisingEvents = true;

            _watcher.Created += OnChanged;
            _watcher.Changed += OnChanged;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event EventHandler<IFileInfo> Changed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Changed = null;
                }

                _watcher?.Dispose();

                _disposedValue = true;
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var fileInfo = _fileInfoFactory.Create(e.FullPath);
            if (fileInfo.Exists && fileInfo.Length > 0)
            {
                Changed?.Invoke(this, fileInfo);
            }
        }
    }
}