using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SV.Numbers.Tests.Runtime
{
    public class BoundValueTest
    {
        [Test]
        public void ValueTest()
        {
            BoundInt test = new BoundInt(0, 100, 50, false);
            
            Assert.AreEqual(0, test.Min);
            Assert.AreEqual(100, test.Max);
            Assert.AreEqual(50, test.CurrentValue);
            Assert.AreEqual(0.5f, test.Percent);

            test.CurrentValue += 10;
            Assert.AreEqual(60, test.CurrentValue);
            Assert.AreEqual(0.6f, test.Percent);

            test.CurrentValue = 30;
            Assert.AreEqual(30, test.CurrentValue);
            Assert.AreEqual(0.3f, test.Percent);

            test.CurrentValue = 5000;
            Assert.AreEqual(100, test.CurrentValue);
            Assert.AreEqual(1f, test.Percent);

            test.CurrentValue = -400;
            Assert.AreEqual(0, test.CurrentValue);
            Assert.AreEqual(0f, test.Percent);
        }

        [Test]
        public void CallbackTest()
        {
            BoundFloat test = new BoundFloat(0, 100, 50, true);
            BoundReachedType reached = BoundReachedType.None;
            BoundReachedType overflow = BoundReachedType.None;
            float newVal = 0;
            float percent = 0;
            int count = 0;

            test.onValueChange.AddListener((v, b) =>
            {
                newVal = v;
                reached = b;
            });
            test.onValueChangePercent.AddListener((p) => percent = p);
            test.onOverflow.AddListener((b) =>
            {
                overflow = b;
                count++;
            });

            test.CurrentValue += 10;
            Assert.AreEqual(60, newVal);
            Assert.AreEqual(BoundReachedType.None, reached);
            Assert.AreEqual(0.6f, percent);
            Assert.AreEqual(BoundReachedType.None, overflow);
            Assert.AreEqual(0, count);
            count = 0;
            
            test.CurrentValue = 250;
            Assert.AreEqual(50, newVal);
            Assert.AreEqual(BoundReachedType.None, reached);
            Assert.AreEqual(0.5f, percent);
            Assert.AreEqual(BoundReachedType.Max, overflow);
            Assert.AreEqual(2, count);
            count = 0;
            
            test.CurrentValue -= 70;
            Assert.AreEqual(80, newVal);
            Assert.AreEqual(BoundReachedType.None, reached);
            Assert.AreEqual(0.8f, percent);
            Assert.AreEqual(BoundReachedType.Min, overflow);
            Assert.AreEqual(1, count);
            count = 0;
        }
    }
}
