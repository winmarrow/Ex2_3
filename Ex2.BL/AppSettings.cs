using System;
using System.IO;
using System.Xml.Serialization;
using AspectInjector.Broker;
using Ex2.BL.Aspects;

namespace Ex2.BL
{
    [Serializable]
    [Inject(typeof(TraceAspect))]
    public class AppSettings
    {
        private const string SettingsFileName = "Settings.xml";

        public MailSettings Mail { get; set; } = new MailSettings();

        public ClientSettings Client { get; set; } = new ClientSettings();

        public DirSettings Dir { get; set; } = new DirSettings();

        public int LogLevel { get; set; } = 5; // Log Levels 0-5

        public static AppSettings Load()
        {
            if (!File.Exists(SettingsFileName))
            {
                new AppSettings().Save();
            }

            using (var stream = File.OpenRead(SettingsFileName))
            {
                var serializer = new XmlSerializer(typeof(AppSettings));
                return serializer.Deserialize(stream) as AppSettings;
            }
        }

        private void Save()
        {
            using (var writer = new StreamWriter(SettingsFileName))
            {
                var serializer = new XmlSerializer(GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        public class MailSettings
        {
            public string Body { get; set; } = "Mail Body";

            public string SenderEmailAddres { get; set; } = "winmarrow@gmail.com";

            public string Subject { get; set; } = "Mail subject";

            public string[] ToEmailAddreses { get; set; } = { "aleksandr.golgovskii@outlook.com" };
        }

        public class ClientSettings
        {
            public string CredentialMailAddress { get; set; } = "winmarrow@gmail.com";

            public string CredentialMailPassword { get; set; } = "01091994";

            public int SendTimeOut { get; set; } = 5000;

            public string SmptHost { get; set; } = "smtp.gmail.com";

            public int SmptPort { get; set; } = 587;
        }

        public class DirSettings
        {
            public string DirectoryWithFilesToSend { get; set; } = @"C:\TestDir";

            public string FilesFilter { get; set; } = "*.txt";
        }
    }
}