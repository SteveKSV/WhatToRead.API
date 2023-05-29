namespace Core.Domain.ValueObjects
{
    public class Money : ValueObject<Money>
    {
        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(_amount), "Amount cannot be negative.");

                _amount = value;
            }
        }

        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public static Money operator +(Money money1, Money money2)
        {
            if (money1.Currency != money2.Currency)
                throw new InvalidOperationException("Cannot add money of different currencies.");

            return new Money(money1.Amount + money2.Amount, money1.Currency);
        }

        public static Money operator -(Money money1, Money money2)
        {
            if (money1.Currency != money2.Currency)
                throw new InvalidOperationException("Cannot subtract money of different currencies.");

            return new Money(money1.Amount - money2.Amount, money1.Currency);
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }

        public static Money operator /(Money money, decimal divisor)
        {
            return new Money(money.Amount / divisor, money.Currency);
        }
    }

    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (T)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
