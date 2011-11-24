using System;
using System.Diagnostics;
using NUnit.Framework;
using XeroApi.Integration;

namespace XeroApi.Tests
{
    public class PauseTimeTests
    {

        [Test]
        public void it_can_pause_time()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            PauseTime pauseTime = new PauseTime();
            pauseTime.Pause(TimeSpan.FromMilliseconds(1000));

            stopwatch.Stop();

            Assert.AreEqual(1000, stopwatch.ElapsedMilliseconds, 10, "Actual elapsed timespan is not within 50ms of the expected timespan");
        }

    }
}
