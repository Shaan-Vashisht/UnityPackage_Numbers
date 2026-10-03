using System;
using UnityEngine;
using UnityEngine.Events;

namespace SV.Numbers
{
    [Serializable]
    public enum BoundReachedType
    {
        None,
        Max,
        Min
    }
    
    /// A value that is bound to a specific range and has various event callbacks when it changes.
    [Serializable]
    public class BoundValue<T> where T : IComparable
    {
        [SerializeField]
        protected T min;
        /// The minimum value of the range.
        public T Min { get => min; set => min = value; }
        
        [SerializeField]
        protected T max;
        /// The maximum value of the range.
        public T Max { get => max; set => max = value; }
        
        [SerializeField]
        protected T value;
        
        /// <para>@get The current value of the field.</para>
        /// <para>@set The value will be clamped between Min and Max and onValueChanged will be invoked.</para>
        public T CurrentValue
        {
            get => value;
            set
            {
                T final = value;
                BoundReachedType reached = BoundReachedType.None;
                if (value.CompareTo(Min) < 0)
                {
                    final = Min;
                    reached = BoundReachedType.Min;
                }
                else if (value.CompareTo(Max) > 0)
                {
                    final = Max;
                    reached = BoundReachedType.Max;
                }
                
                this.value = final;
                onValueChange?.Invoke(this.value, reached);
            }
        }
        
        /// Event invoked when the value of the field is changed.
        /// Passes the new value and which, if any, of the bounds was reached.
        public UnityEvent<T, BoundReachedType> onValueChange = new UnityEvent<T, BoundReachedType>();
        
        /// <param name="min">The minimum bound of the field.</param>
        /// <param name="max">The maximum bound of the field.</param>
        /// <param name="value">The starting value of the field.</param>
        /// <exception cref="ArgumentOutOfRangeException">If max &lt; min</exception>
        public BoundValue(T min, T max, T value)
        {
            if (max.CompareTo(min) < 0)
                throw new ArgumentOutOfRangeException(nameof(max));
            
            Min = min;
            Max = max;
            CurrentValue = value;
        }
    }

    /// <inheritdoc/>
    [Serializable]
    public class BoundInt : BoundValue<int>
    {
        [SerializeField, Tooltip("If true, the field value will wrap around the interval when going beyond one of the bounds.")]
        private bool doOverflow;
        private int Interval => Max - Min;
        
        public new int Min { 
            get => min; 
            set
            {
                base.Min = value;
                onValueChangePercent?.Invoke(Percent);
            }
        }
        public new int Max { 
            get => max; 
            set
            {
                base.Max = value;
                onValueChangePercent?.Invoke(Percent);
            }
        }
        
        /// <para>@get The current value of the field.</para>
        /// <para>@set The value will be clamped between Min and Max and onValueChanged will be invoked.
        /// If doOverflow is true, will check for overflows and invoke onOverflow once for each time the new value went beyond the interval.</para>
        public new int CurrentValue
        {
            get => value;
            set
            {
                int final = value;
                BoundReachedType reached = BoundReachedType.None;
                
                if (doOverflow)
                {
                    while (final < Min)
                    {
                        final += Interval;
                        onOverflow?.Invoke(BoundReachedType.Min);
                    }

                    while (final > Max)
                    {
                        final -= Interval;
                        onOverflow?.Invoke(BoundReachedType.Max);
                    }
                }
                
                if (final < Min)
                {
                    final = Min;
                    reached = BoundReachedType.Min;
                }
                else if (final > Max)
                {
                    final = Max;
                    reached = BoundReachedType.Max;
                }
                
                this.value = final;
                onValueChange?.Invoke(this.value, reached);
                onValueChangePercent?.Invoke(Percent);
            }
        }
        
        /// The field's current value normalized to the interval, where 0 = Min and 1 = Max.
        public float Percent => (float)(value - Min) / Interval;
        
        /// Event invoked when the field's value goes beyond the interval.
        /// Passes the bound the value flowed over.
        public UnityEvent<BoundReachedType> onOverflow = new UnityEvent<BoundReachedType>();
        /// Event invoked when the field's value or its bounds change.
        /// Passes the new normalized value. 
        public UnityEvent<float> onValueChangePercent = new UnityEvent<float>();
        
        /// <inheritdoc/>
        /// <param name="doOverflow">Whether the overflow behavior should be wrap-around or clamp.</param>
        public BoundInt(int min, int max, int value, bool doOverflow) : base(min, max, value)
        {
            this.doOverflow = doOverflow;
        }
    }
    
    /// <inheritdoc/>
    [Serializable]
    public class BoundFloat : BoundValue<float>
    {
        [SerializeField, Tooltip("If true, the field value will wrap around the interval when going beyond one of the bounds.")]
        private bool doOverflow;
        private float Interval => Max - Min;
        
        public new float Min { 
            get => min; 
            set
            {
                base.Min = value;
                onValueChangePercent?.Invoke(Percent);
            }
        }
        public new float Max { 
            get => max; 
            set
            {
                base.Max = value;
                onValueChangePercent?.Invoke(Percent);
            }
        }
        
        /// <para>@get The current value of the field.</para>
        /// <para>@set The value will be clamped between Min and Max and onValueChanged will be invoked.
        /// If doOverflow is true, will check for overflows and invoke onOverflow once for each time the new value went beyond the interval.</para>
        public new float CurrentValue
        {
            get => value;
            set
            {
                float final = value;
                BoundReachedType reached = BoundReachedType.None;
                
                if (doOverflow)
                {
                    while (final < Min)
                    {
                        final += Interval;
                        onOverflow?.Invoke(BoundReachedType.Min);
                    }

                    while (final > Max)
                    {
                        final -= Interval;
                        onOverflow?.Invoke(BoundReachedType.Max);
                    }
                }
                
                if (final < Min)
                {
                    final = Min;
                    reached = BoundReachedType.Min;
                }
                else if (final > Max)
                {
                    final = Max;
                    reached = BoundReachedType.Max;
                }
                
                this.value = final;
                onValueChange?.Invoke(this.value, reached);
                onValueChangePercent?.Invoke(Percent);
            }
        }
        
        /// The field's current value normalized to the interval, where 0 = Min and 1 = Max.
        public float Percent => (value - Min) / Interval;
        
        /// Event invoked when the field's value goes beyond the interval.
        /// Passes the bound the value flowed over.
        public UnityEvent<BoundReachedType> onOverflow = new UnityEvent<BoundReachedType>();
        /// Event invoked when the field's value or its bounds change.
        /// Passes the new normalized value.
        public UnityEvent<float> onValueChangePercent = new UnityEvent<float>();
        
        /// <inheritdoc/>
        /// <param name="doOverflow">Whether the overflow behavior should be wrap-around or clamp.</param>
        public BoundFloat(float min, float max, float value, bool doOverflow) : base(min, max, value)
        {
            this.doOverflow = doOverflow;
        }
    }
}
