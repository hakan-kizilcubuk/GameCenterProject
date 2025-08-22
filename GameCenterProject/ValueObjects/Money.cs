using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenterProject.ValueObjects
{
    public sealed record Money
    {
        public string Currency { get; } = "USD"; // Default currency
        public decimal Amount { get; }
        public static Money Zero => new(0m, "USD");

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public Money Add(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return new Money(this.Amount + other.Amount, "USD");
        }

        public Money Subtract(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return new Money(this.Amount - other.Amount, "USD");
        }

        public void EnsureSameCurrency(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (this.Currency != other.Currency)
                throw new InvalidOperationException("Currency Mismatch");
        }

        public int CompareTo(Money other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            EnsureSameCurrency(other);
            return this.Amount.CompareTo(other.Amount);
        }
    }
}