using System.Net.Mail;
using AspectInjector.Broker;
using Ex2.BL.Aspects;
using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.BL.Entities.Wrappers
{
    [Inject(typeof(TraceAspect))]
    public class SmtpClientWrapper : SmtpClient, ISmtpClient
    {
        public SmtpClientWrapper()
        {
        }

        public SmtpClientWrapper(string host)
            : base(host)
        {
        }

        public SmtpClientWrapper(string host, int port)
            : base(host, port)
        {
        }
    }
}