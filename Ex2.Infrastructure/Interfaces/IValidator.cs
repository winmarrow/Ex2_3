namespace Ex2.Infrastructure.Interfaces
{
    public interface IValidator<in T>
    {
        void ValidateIsNotNull(T item);
        void Validate(T item);
    }
}