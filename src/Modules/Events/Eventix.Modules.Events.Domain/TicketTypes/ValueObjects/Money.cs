namespace Eventix.Modules.Events.Domain.TicketTypes.ValueObjects
{
    public sealed record Money
    {
        public Money(decimal price, string currency)
        {
            Amount = price;
            Currency = currency;
        }
        private Money()
        {
        }

        public decimal Amount { get; }
        public string Currency { get; } = string.Empty;

        public static implicit operator Money((decimal price, string currency) value)
            => new(value.price, value.currency);

        public override string ToString() => $"{Amount} {Currency}";
    }
}