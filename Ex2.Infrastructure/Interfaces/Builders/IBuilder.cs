using Ex2.Infrastructure.Abstractions;

namespace Ex2.Infrastructure.Interfaces.Builders
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}