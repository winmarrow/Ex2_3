using System;
using Ex2.Infrastructure.Interfaces;

namespace Ex2.Infrastructure.Abstractions
{
    public abstract class Validator<T> : IValidator<T>
    {
        public void ValidateIsNotNull(T item)
        {
            if (item == null)
            {
                throw new NullReferenceException($"{typeof(T).Name} cannot be null.");
            }
        }

        public abstract void Validate(T item);
    }
}