using System;

namespace Common
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public event Action<T> ValueChanged;
        
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(value);
            }
        }
    }
}