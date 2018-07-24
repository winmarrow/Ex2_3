using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex2.BL;
using Ex2.BL.Entities;
using Ex2.BL.Entities.Builders;
using Ex2.BL.Entities.Factories;
using Ex2.BL.Entities.PipeLineHandlers;
using Ex2.BL.Entities.Validators;
using Ex2.Infrastructure.Interfaces;
using Ex2.Infrastructure.Interfaces.Builders;
using Ex2.Infrastructure.Interfaces.Factories;
using Ex2.Infrastructure.Interfaces.Validators;
using Ex2.Infrastructure.Interfaces.Wrappers;
using Ex3.DI;
using NLog;

namespace Ex2.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = DiContainer.Instance;
            ConfigureDi(container);

            var app = container.GetInstance<App>();
            app.Init();

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {

            }
        }

        private static void ConfigureDi(DiContainer container)
        {
            container.Register<App>();
            container.Register<AppSettings>(AppSettings.Load());

            container.Register<ILogger>(LogManager.GetCurrentClassLogger());
            container.Register<IPipeLineBuilder<IFileInfo>, PipeLineBuilder<IFileInfo>>();
            container.Register<IMailMessageBuilder, MailMessageBuilder>();

            container.Register<IFileInfoFactory, FileInfoFactory>();
            container.Register<IFileSystemWatcherFactory, FileSystemWatcherFactory>();
            container.Register<ISmtpClientFactory, SmtpClientFactory>();

            container.Register<IMailMessageValidator, MailMessageValidator>();

            container.Register<CriticalAndCancelExceptionHandler, CriticalAndCancelExceptionHandler>();
            container.Register<FileDeleteHandler, FileDeleteHandler>();
            container.Register<FileSendHandler, FileSendHandler>();

            container.Register<IDirectoryWatcher, DirectoryWatcher>();
            container.Register<IMailSender, MailSender>();
        }
    }
}
