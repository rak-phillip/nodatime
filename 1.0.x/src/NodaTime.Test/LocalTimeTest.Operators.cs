﻿#region Copyright and license information
// Copyright 2001-2009 Stephen Colebourne
// Copyright 2009-2010 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using NUnit.Framework;

namespace NodaTime.Test
{
    public partial class LocalTimeTest
    {
        [Test]
        public void Addition_WithPeriod()
        {
            LocalTime start = new LocalTime(3, 30);
            Period period = Period.FromHours(2) + Period.FromSeconds(1);
            LocalTime expected = new LocalTime(5, 30, 1);
            Assert.AreEqual(expected, start + period);
        }

        [Test]
        public void Addition_WrapsAtMidnight()
        {
            LocalTime start = new LocalTime(22, 0);
            Period period = Period.FromHours(3);
            LocalTime expected = new LocalTime(1, 0);
            Assert.AreEqual(expected, start + period);
        }

        [Test]
        public void Addition_WithNullPeriod_ThrowsArgumentNullException()
        {
            LocalTime date = new LocalTime(12, 0);
            // Call to ToString just to make it a valid statement
            Assert.Throws<ArgumentNullException>(() => (date + (Period)null).ToString());
        }

        [Test]
        public void Subtraction_WithPeriod()
        {
            LocalTime start = new LocalTime(5, 30, 1);
            Period period = Period.FromHours(2) + Period.FromSeconds(1);
            LocalTime expected = new LocalTime(3, 30, 0);
            Assert.AreEqual(expected, start - period);
        }

        [Test]
        public void Subtraction_WrapsAtMidnight()
        {
            LocalTime start = new LocalTime(1, 0, 0);
            Period period = Period.FromHours(3);
            LocalTime expected = new LocalTime(22, 0, 0);
            Assert.AreEqual(expected, start - period);
        }

        [Test]
        public void Subtraction_WithNullPeriod_ThrowsArgumentNullException()
        {
            LocalTime date = new LocalTime(12, 0);
            // Call to ToString just to make it a valid statement
            Assert.Throws<ArgumentNullException>(() => (date - (Period)null).ToString());
        }

        [Test]
        public void Addition_PeriodWithDate()
        {
            LocalTime time = new LocalTime(20, 30);
            Period period = Period.FromDays(1);
            // Use method not operator here to form a valid statement
            Assert.Throws<ArgumentException>(() => LocalTime.Add(time, period));
        }

        [Test]
        public void Subtraction_PeriodWithTime()
        {
            LocalTime time = new LocalTime(20, 30);
            Period period = Period.FromDays(1);
            // Use method not operator here to form a valid statement
            Assert.Throws<ArgumentException>(() => LocalTime.Subtract(time, period));
        }

        [Test]
        public void PeriodAddition_MethodEquivalents()
        {
            LocalTime start = new LocalTime(20, 30);
            Period period = Period.FromHours(3) + Period.FromMinutes(10);
            Assert.AreEqual(start + period, LocalTime.Add(start, period));
            Assert.AreEqual(start + period, start.Plus(period));
        }

        [Test]
        public void PeriodSubtraction_MethodEquivalents()
        {
            LocalTime start = new LocalTime(20, 30);
            Period period = Period.FromHours(3) + Period.FromMinutes(10);
            Assert.AreEqual(start - period, LocalTime.Subtract(start, period));
            Assert.AreEqual(start - period, start.Minus(period));
        }

        [Test]
        public void ComparisonOperators()
        {
            LocalTime time1 = new LocalTime(10, 30, 45);
            LocalTime time2 = new LocalTime(10, 30, 45);
            LocalTime time3 = new LocalTime(10, 30, 50);

            Assert.IsTrue(time1 == time2);
            Assert.IsFalse(time1 == time3);
            Assert.IsFalse(time1 != time2);
            Assert.IsTrue(time1 != time3);

            Assert.IsFalse(time1 < time2);
            Assert.IsTrue(time1 < time3);
            Assert.IsFalse(time2 < time1);
            Assert.IsFalse(time3 < time1);

            Assert.IsTrue(time1 <= time2);
            Assert.IsTrue(time1 <= time3);
            Assert.IsTrue(time2 <= time1);
            Assert.IsFalse(time3 <= time1);

            Assert.IsFalse(time1 > time2);
            Assert.IsFalse(time1 > time3);
            Assert.IsFalse(time2 > time1);
            Assert.IsTrue(time3 > time1);

            Assert.IsTrue(time1 >= time2);
            Assert.IsFalse(time1 >= time3);
            Assert.IsTrue(time2 >= time1);
            Assert.IsTrue(time3 >= time1);
        }

        [Test]
        public void Comparison_IgnoresOriginalCalendar()
        {
            LocalDateTime dateTime1 = new LocalDateTime(1900, 1, 1, 10, 30, 0);
            LocalDateTime dateTime2 = dateTime1.WithCalendar(CalendarSystem.GetJulianCalendar(3));

            // Calendar information is propagated into LocalDate, but not into LocalTime
            Assert.IsFalse(dateTime1.Date == dateTime2.Date);
            Assert.IsTrue(dateTime1.TimeOfDay == dateTime2.TimeOfDay);
        }

        [Test]
        public void CompareTo()
        {
            LocalTime time1 = new LocalTime(10, 30, 45);
            LocalTime time2 = new LocalTime(10, 30, 45);
            LocalTime time3 = new LocalTime(10, 30, 50);

            Assert.That(time1.CompareTo(time2), Is.EqualTo(0));
            Assert.That(time1.CompareTo(time3), Is.LessThan(0));
            Assert.That(time3.CompareTo(time2), Is.GreaterThan(0));
        }
    }
}