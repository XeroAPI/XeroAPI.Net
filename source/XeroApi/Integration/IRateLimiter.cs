using System;
using System.Net;

namespace XeroApi.Integration
{
    public interface IRateLimiter
    {
        /// <summary>
        /// Checks and enforces the current rate limit.
        /// </summary>
        /// <param name="eventTimestamp">The event timestamp.</param>
        void CheckAndEnforceRateLimit(DateTime eventTimestamp);

    }
}
