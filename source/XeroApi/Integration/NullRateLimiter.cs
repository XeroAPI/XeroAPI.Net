using System;
using System.Net;

namespace XeroApi.Integration
{
    /// <summary>
    /// Null implementation of <c ref="IRateLimiter" />.
    /// </summary>
    public class NullRateLimiter : IRateLimiter
    {
        /// <summary>
        /// Checks and enforces the current rate limit.
        /// </summary>
        public void CheckAndEnforceRateLimit(DateTime eventDate)
        {
        }
    }
}
