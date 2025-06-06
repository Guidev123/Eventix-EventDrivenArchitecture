namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public record TicketTypeSpecification
    {
        public TicketTypeSpecification(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; }
        public int Quantity { get; }

        public static implicit operator TicketTypeSpecification((string name, int quantity) value)
            => new(value.name, value.quantity);

        public override string ToString() => $"{Name} ({Quantity})";
    }
}