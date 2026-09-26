using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SV.Numbers.Tests.Runtime
{
    public class MarbleBagTest
    {
        [Test]
        public void ExceptionTest()
        {
            IntMarbleBag intBag = new IntMarbleBag();

            // The bag hasn't been set up
            Assert.Throws<Exception>(()=>intBag.Reset());
            Assert.Throws<Exception>(()=>intBag.DrawOne());
            
            Assert.Throws<NullReferenceException>(()=>intBag.Init(null));
            Assert.Throws<NullReferenceException>(()=>intBag.Init(10,null));
            
            Dictionary<int, int> setup = new Dictionary<int, int>
            {
                { 0, 5 },
                { 1, 10 },
                { -1, 5 }
            };
            intBag.Init(setup);
            
            Assert.DoesNotThrow(() => intBag.DrawOne());
            Assert.DoesNotThrow(() => intBag.Reset());
        }

        [Test]
        public void DrawingTest()
        {
            IntMarbleBag intBag = new IntMarbleBag();
            Dictionary<int, int> setup = new Dictionary<int, int>
            {
                { 0, 5 },
                { 1, 10 },
                { -1, 5 }
            };
            intBag.Init(setup);

            List<int> draws = new List<int>();
            
            // All draws are in the setup
            for (int i = 0; i < 20; i++)
            {
                int m = intBag.DrawOne();
                Assert.Contains(m, setup.Keys);
                draws.Add(m);
            }
            
            // The total of each draw matches the setup
            foreach (int k in setup.Keys)
            {
                Assert.AreEqual(draws.Count((i) => k == i), setup[k]);
            }
            
            // Draws are out of order
            List<int> sorted = new List<int>(draws);
            sorted.Sort();
            Assert.AreNotEqual(sorted, draws);
            
            // Bag self resets
            Assert.DoesNotThrow(() => intBag.DrawOne());
        }
    }
}