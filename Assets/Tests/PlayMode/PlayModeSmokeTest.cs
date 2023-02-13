using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayModeSmokeTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SmokeTest()
        {
            // Use the Assert class to test conditions
            Assert.IsNotNull(42);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SmokeTestEnumerator()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            Assert.IsNotNull(42);
            yield return null;
        }
    }
}