namespace Nixon.ValueUnion
{
    public class ValueOr<T, TOther>
    {
        private readonly T _value;

        private readonly TOther _other;

        public T Value => 
            HasValue ? 
                _value : 
                throw new InvalidOperationException("Attempted to access a non-specified value");

        public bool HasValue { get; }

        protected ValueOr(T value)
        {
            HasValue = true;

            _value = value;
            _other = default!;
        }

        protected ValueOr(TOther problem)
        {
            _other = problem;
            _value = default!;
        }

        public bool TryGetValue(out T value, out TOther other)
        {
            value = _value;
            other = _other;

            return HasValue;
        }

        public static ValueOr<T, TOther> From(T value)
        {
            return new ValueOr<T, TOther>(value);
        }

        public static ValueOr<T, TOther> FromOther(TOther value)
        {
            return new ValueOr<T, TOther>(value);
        }


        public static implicit operator ValueOr<T, TOther>(T value)
        {
            return new ValueOr<T, TOther>(value);
        }


        public static implicit operator ValueOr<T, TOther>(TOther problem)
        {
            return new ValueOr<T, TOther>(problem);
        }
    }
}