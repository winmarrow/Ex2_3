using Ex2.Infrastructure.Interfaces.Builders;

namespace Ex2.Infrastructure.Abstractions
{
    public abstract class Builder<T> : IBuilder<T>
    {
        public abstract T Build();
    }
}