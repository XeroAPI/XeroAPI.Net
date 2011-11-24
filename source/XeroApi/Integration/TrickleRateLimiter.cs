using System;
using System.Diagnostics;
using System.Threading;

namespace XeroApi.Integration
{
    public interface IEventTimeline
    {
        void RecordEvent(DateTime eventDate);
        DateTime? GetLastEventDateAndTime();
    }

    public class EventTimeline : IEventTimeline
    {
        private DateTime? _lastEventDate;

        #region Implementation of IEventTimeline

        public void RecordEvent(DateTime eventDate)
        {
            if (!_lastEventDate.HasValue || eventDate > _lastEventDate)
                _lastEventDate = eventDate;
        }

        public DateTime? GetLastEventDateAndTime()
        {
            return _lastEventDate;
        }

        #endregion
    }


    public interface IPauseTime
    {
        void Pause(TimeSpan pauseTime);
    }

    public class PauseTime : IPauseTime
    {
        public void Pause(TimeSpan pauseTime)
        {
            Thread.Sleep(Convert.ToInt32(pauseTime.TotalMilliseconds));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Defaults to 60 calls in a sliding 60 second time window.
    /// </remarks>
    public class TrickleRateLimiter : IRateLimiter
    {
        private IEventTimeline Timeline { get; set; }
        private readonly IPauseTime _pauseTime;

        public TrickleRateLimiter(IEventTimeline timeline)
        {
            Timeline = timeline;
        }

        public TrickleRateLimiter(IEventTimeline timeline, IPauseTime pauseTime)
        {
            Timeline = timeline;
            _pauseTime = pauseTime;
        }

        public void CheckAndEnforceRateLimit(DateTime expectedEventTimestamp)
        {
            DateTime? lastEventTimestamp = Timeline.GetLastEventDateAndTime();

            if (lastEventTimestamp.HasValue)
            {
                TimeSpan timeSinceLastEvent = expectedEventTimestamp - lastEventTimestamp.Value;

                TimeSpan pausedTime = PauseMaybe(timeSinceLastEvent);
                expectedEventTimestamp = expectedEventTimestamp.Add(pausedTime);
            }

            Timeline.RecordEvent(expectedEventTimestamp);
        }

        /// <summary>
        /// Pauses the maybe.
        /// </summary>
        /// <param name="timeSinceLastEvent">The time since last event.</param>
        /// <returns>Returns the timespan that was paused</returns>
        private TimeSpan PauseMaybe(TimeSpan timeSinceLastEvent)
        {
            Debug.WriteLine("Time since last event:" + timeSinceLastEvent.TotalMilliseconds + "ms");

            if (timeSinceLastEvent < TimeSpan.FromSeconds(1))
            {
                TimeSpan timeToPause = TimeSpan.FromSeconds(1).Subtract(timeSinceLastEvent);
                Debug.WriteLine("Pausing for " + timeToPause.TotalMilliseconds + "ms");
                PauseFor(timeToPause);

                return timeToPause;
            }

            return TimeSpan.Zero;
        }

        private void PauseFor(TimeSpan timeToPause)
        {
            _pauseTime.Pause(timeToPause);
        }
    }

    public class RateLimitExceededException : Exception
    {
    }
}
