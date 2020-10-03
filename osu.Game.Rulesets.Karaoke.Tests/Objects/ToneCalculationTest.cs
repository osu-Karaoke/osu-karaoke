﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using System;

namespace osu.Game.Rulesets.Karaoke.Tests.Objects
{
    [TestFixture]
    public class ToneCalculationTest
    {
        [TestCase(1, 1, 2)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, -1, 0)]
        [TestCase(1.5, 2.5, 4)]
        [TestCase(-1.5, 2.5, 1)]
        [TestCase(1.5, -2.5, -1)]
        public void TestOperatorPlus(double tone1, double tone2, double tone)
        {
            Assert.AreEqual(numberToTone(tone1) + numberToTone(tone2), numberToTone(tone));
        }

        [TestCase(1, 1, 2)]
        [TestCase(-1, 1, 0)]
        [TestCase(1, -1, 0)]
        public void TestOperatorPlusWithInt(double tone1, int scale1, double tone)
        {
            Assert.AreEqual(numberToTone(tone1) + scale1, numberToTone(tone));
        }

        [TestCase(1, 1, 0)]
        [TestCase(-1, 1, -2)]
        [TestCase(1, -1, 2)]
        [TestCase(1.5, 2.5, -1)]
        [TestCase(-1.5, 2.5, -4)]
        [TestCase(1.5, -2.5, 4)]
        public void TestOperatorMinus(double tone1, double tone2, double tone)
        {
            Assert.AreEqual(numberToTone(tone1) - numberToTone(tone2), numberToTone(tone));
        }

        [TestCase(1, 1, 0)]
        [TestCase(-1, 1, -2)]
        [TestCase(1, -1, 2)]
        public void TestOperatorMinusWithInt(double tone1, int scale1, double tone)
        {
            Assert.AreEqual(numberToTone(tone1) - scale1, numberToTone(tone));
        }

        [TestCase(1, 1)]
        [TestCase(1.5, 1.5)]
        [TestCase(-1.5, -1.5)]
        public void TestOperatorEqual(double tone1, double tone2)
        {
            Assert.AreEqual(numberToTone(tone1), numberToTone(tone2));
        }

        [TestCase(1, 1)]
        [TestCase(-1, -1)]
        public void TestOperatorEqualWithInt(double tone1, int scale1)
        {
            Assert.IsTrue(numberToTone(tone1) == scale1);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1.5, -1.5)]
        [TestCase(1.5, 1)]
        [TestCase(-1.5, -1)]
        [TestCase(-1.5, -2)]
        public void TestOperatorNotEqual(double tone1, double tone2)
        {
            Assert.AreNotEqual(numberToTone(tone1), numberToTone(tone2));
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1.5, -1)]
        [TestCase(-1.5, -2)]
        public void TestOperatorNotEqualWithInt(double tone1, int scale1)
        {
            Assert.IsTrue(numberToTone(tone1) != scale1);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, false)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreater(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) > numberToTone(tone2), match);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 1, false)]
        public void TestOperatorGreaterWithInt(double tone1, int scale1, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) > scale1, match);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, true)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreaterOrEqual(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) >= numberToTone(tone2), match);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 1, true)]
        public void TestOperatorGreaterOrEqualWithInt(double tone1, int scale1, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) >= scale1, match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, false)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLess(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) < numberToTone(tone2), match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -1, false)]
        public void TestOperatorLessWithInt(double tone1, int scale1, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) < scale1, match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, true)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLessOrEqual(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) <= numberToTone(tone2), match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -1, true)]
        public void TestOperatorLessOrEqualWithInt(double tone1, int scale1, bool match)
        {
            Assert.AreEqual(numberToTone(tone1) <= scale1, match);
        }

        private Tone numberToTone(double tone)
        {
            var half = Math.Abs(tone) % 1 == 0.5;
            var scale = tone < 0 ? (((int)tone) - (half ? 1 : 0)) : (int)tone;
            return new Tone(scale, half);
        }
    }
}
