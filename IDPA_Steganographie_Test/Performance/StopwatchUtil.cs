using System;
using System.Diagnostics;

namespace Steganographie.Tests.Performance
{
    public static class StopwatchUtil
    {
        public static TimeSpan Time(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
