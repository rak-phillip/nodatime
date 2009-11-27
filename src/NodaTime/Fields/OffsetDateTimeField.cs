﻿using System;

namespace NodaTime.Fields
{
    /// <summary>
    /// Porting status: Needs AddWrapField
    /// </summary>
    internal sealed class OffsetDateTimeField : DecoratedDateTimeField
    {
        private readonly int offset;
        private readonly int min;
        private readonly int max;

        internal OffsetDateTimeField(IDateTimeField field, int offset)
            // If the field is null, we want to let the 
            // base constructor throw the exception, rather than
            // fail to dereference it properly here.
            : this(field, field == null ? null : field.FieldType,
                offset, int.MinValue, int.MaxValue)
        {
        }

        internal OffsetDateTimeField(IDateTimeField field, 
            DateTimeFieldType fieldType, int offset)
            : this(field, fieldType, offset, int.MinValue, int.MaxValue)
        {
        }

        public OffsetDateTimeField(IDateTimeField field, 
            DateTimeFieldType fieldType, int offset, int minValue, int maxValue)
            : base(field, fieldType)
        {
            if (offset == 0)
            {
                throw new ArgumentOutOfRangeException("offset", "The offset cannot be zero");
            }

            this.offset = offset;
            // This field is only really used for weeks etc - not ticks -
            // so casting the min and max to int should be fine.
            this.min = Math.Max(minValue, (int) field.GetMinimumValue() + offset);
            this.max = Math.Min(maxValue, (int) field.GetMaximumValue() + offset);
        }

        public override int GetValue(LocalInstant localInstant)
        {
            return base.GetValue(localInstant) + offset;
        }

        public override long GetInt64Value(LocalInstant localInstant)
        {
            return base.GetValue(localInstant) + offset;
        }

        public override LocalInstant Add(LocalInstant localInstant, int value)
        {
            localInstant = base.Add(localInstant, value);
            FieldUtils.VerifyValueBounds(this, GetInt64Value(localInstant), min, max);
            return localInstant;
        }

        public override LocalInstant Add(LocalInstant localInstant, long value)
        {
            localInstant = base.Add(localInstant, value);
            FieldUtils.VerifyValueBounds(this, GetInt64Value(localInstant), min, max);
            return localInstant;
        }

        public override LocalInstant SetValue(LocalInstant localInstant, long value)
        {
            FieldUtils.VerifyValueBounds(this, value, min, max);
            return base.SetValue(localInstant, value - offset);
        }

        public override bool IsLeap(LocalInstant localInstant)
        {
            return WrappedField.IsLeap(localInstant);
        }

        public override int GetLeapAmount(LocalInstant localInstant)
        {
            return WrappedField.GetLeapAmount(localInstant);
        }

        public override DurationField LeapDurationField { get { return WrappedField.LeapDurationField; } }

        public override long GetMinimumValue()
        {
            return min;
        }

        public override long GetMaximumValue()
        {
            return max;
        }

        // No need to override RoundFloor again - it already just delegates.

        public override LocalInstant RoundCeiling(LocalInstant localInstant)
        {
            return WrappedField.RoundCeiling(localInstant);
        }

        public override LocalInstant RoundHalfFloor(LocalInstant localInstant)
        {
            return WrappedField.RoundHalfFloor(localInstant);
        }

        public override LocalInstant RoundHalfCeiling(LocalInstant localInstant)
        {
            return WrappedField.RoundHalfCeiling(localInstant);
        }

        public override LocalInstant RoundHalfEven(LocalInstant localInstant)
        {
            return WrappedField.RoundHalfEven(localInstant);
        }

        public override Duration Remainder(LocalInstant localInstant)
        {
            return WrappedField.Remainder(localInstant);
        }
    }
}
