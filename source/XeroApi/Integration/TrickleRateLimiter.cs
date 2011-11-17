using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;

namespace XeroApi.Integration
{
    public interface IEventTimeline
    {
        void RecordEvent(DateTime eventDate);
        DateTime? GetLastEventDateAndTime();
    }

    public class EventTimeline
    {
        
    }


    public interface IXXX
    {
        void PauseBeforeEvent(TimeSpan pauseTime);
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
        private readonly IXXX _xxx;

        public TrickleRateLimiter(IEventTimeline timeline)
        {
            Timeline = timeline;
        }

        public TrickleRateLimiter(IEventTimeline timeline, IXXX xxx)
        {
            Timeline = timeline;
            _xxx = xxx;
        }

        public void CheckAndEnforceRateLimit(DateTime eventTimestamp)
        {
            DateTime? lastEventTimestamp = Timeline.GetLastEventDateAndTime();

            if (lastEventTimestamp.HasValue)
            {
                TimeSpan timeSinceLastEvent = eventTimestamp - lastEventTimestamp.Value;

                PauseMaybe(timeSinceLastEvent);
            }

            Timeline.RecordEvent(eventTimestamp);
        }

        private void PauseMaybe(TimeSpan timeSinceLastEvent)
        {
            if (timeSinceLastEvent < TimeSpan.FromSeconds(1))
            {
                TimeSpan timeToPause = TimeSpan.FromSeconds(1).Subtract(timeSinceLastEvent);
                PauseFor(timeToPause);
            }
        }

        private void PauseFor(TimeSpan timeToPause)
        {
            _xxx.PauseBeforeEvent(timeToPause);
        }
    }

    public class RateLimitExceededException : Exception
    {
    }
}
