using Ex2.Infrastructure.Interfaces.Wrappers;

namespace Ex2.Infrastructure.Interfaces.Factories
{
    public interface IFileInfoFactory
    {
        IFileInfo Create(string fullName);
    }
}