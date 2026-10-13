using System;

namespace Domain.Exceptions;

/// Se lanza si se viola una regla de negocio
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
