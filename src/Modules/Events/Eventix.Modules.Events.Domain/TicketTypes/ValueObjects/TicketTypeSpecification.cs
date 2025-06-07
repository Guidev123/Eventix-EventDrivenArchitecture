namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public record TicketTypeSpecification
    {
        public TicketTypeSpecification(string name, decimal quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; }
        public decimal Quantity { get; }

        public static implicit operator TicketTypeSpecification((string name, decimal quantity) value)
            => new(value.name, value.quantity);

        public override string ToString() => $"{Name} ({Quantity})";
    }
}