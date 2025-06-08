namespace Eventix.Shared.Domain.DomainObjects
{
    public abstract record ValueObject
    {
        protected abstract void Validate();
    }
}