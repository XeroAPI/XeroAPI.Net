using System;
using NUnit.Framework;
using Rhino.Mocks;
using XeroApi.Integration;

namespace XeroApi.Tests
{
    public class TrickleRateLimiterTests
    {
    
        [Test]
        public void first_event_doesnt_require_rate_limiting()
        {
            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(MockRepository.GenerateStub<IEventTimeline>());
            Assert.DoesNotThrow(() => rateLimiter.CheckAndEnforceRateLimit(DateTime.UtcNow));
        }

        [Test]
        public void first_event_is_recorded_in_timeline()
        {
            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline);
            
            Assert.DoesNotThrow(() => rateLimiter.CheckAndEnforceRateLimit(DateTime.UtcNow));

            timeline.AssertWasCalled(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
        }

        [Test]
        public void it_asks_the_timeline_for_the_date_and_time_of_the_last_event()
        {
            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline);

            rateLimiter.CheckAndEnforceRateLimit(DateTime.UtcNow);

            timeline.AssertWasCalled(it => it.GetLastEventDateAndTime());
        }

        [Test]
        public void it_records_the_event_AFTER_asking_the_timeline_for_the_date_and_time_of_the_last_event()
        {
            var mocks = new MockRepository();

            var timeline = mocks.StrictMock<IEventTimeline>();
            
            using (mocks.Ordered())
            {
                Expect.Call(timeline.GetLastEventDateAndTime()).Return(DateTime.Parse("19-Nov-1976"));
                Expect.Call(() => timeline.RecordEvent(Arg<DateTime>.Is.Anything));
            }

            mocks.ReplayAll();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline);
            rateLimiter.CheckAndEnforceRateLimit(DateTime.UtcNow);
            
            mocks.VerifyAll();
        }

        [Test]
        public void it_doesnt_trigger_the_rate_limit_when_the_timeline_is_empty()
        {
            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
            timeline.Stub(it => it.GetLastEventDateAndTime()).Return(null);

            IXXX xxx = MockRepository.GenerateMock<IXXX>();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(DateTime.UtcNow);
            
            xxx.AssertWasNotCalled(it => it.PauseBeforeEvent(Arg<TimeSpan>.Is.Anything));
        }

        [Test]
        public void it_doesnt_trigger_the_rate_limit_when_the_last_event_date_is_more_than_1_second_ago()
        {
            DateTime firstEventDateTime  = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime secondEventDateTime = new DateTime(2000, 1, 1, 12, 0, 2);

            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
            timeline.Stub(it => it.GetLastEventDateAndTime()).Return(firstEventDateTime);

            IXXX xxx = MockRepository.GenerateMock<IXXX>();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(secondEventDateTime);

            xxx.AssertWasNotCalled(it => it.PauseBeforeEvent(Arg<TimeSpan>.Is.Anything));
        }

        [Test]
        public void it_doesnt_trigger_the_rate_limit_when_the_last_event_date_is_exactly_1_second_ago()
        {
            DateTime firstEventDateTime = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime secondEventDateTime = firstEventDateTime.AddSeconds(1); 

            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
            timeline.Stub(it => it.GetLastEventDateAndTime()).Return(firstEventDateTime);

            IXXX xxx = MockRepository.GenerateMock<IXXX>();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(secondEventDateTime);

            xxx.AssertWasNotCalled(it => it.PauseBeforeEvent(Arg<TimeSpan>.Is.Anything));
        }

        [Test]
        public void it_does_trigger_the_rate_limit_when_the_last_event_date_is_less_than_1_second_ago()
        {
            DateTime firstEventDateTime = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime secondEventDateTime = new DateTime(2000, 1, 1, 12, 0, 0, 500);

            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
            timeline.Stub(it => it.GetLastEventDateAndTime()).Return(firstEventDateTime);

            IXXX xxx = MockRepository.GenerateMock<IXXX>();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(secondEventDateTime);

            xxx.AssertWasCalled(it => it.PauseBeforeEvent(Arg<TimeSpan>.Is.Anything));
        }

        [Test]
        public void it_does_trigger_the_rate_limit_for_enough_time_to_maintain_one_event_per_second()
        {
            DateTime firstEventDateTime = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime secondEventDateTime = firstEventDateTime.AddMilliseconds(400); // <-- Second event is 400ms after the first event. This should trigger a pause of 600ms.

            var timeline = MockRepository.GenerateStub<IEventTimeline>();
            timeline.Stub(it => it.RecordEvent(Arg<DateTime>.Is.Anything));
            timeline.Stub(it => it.GetLastEventDateAndTime()).Return(firstEventDateTime);

            IXXX xxx = MockRepository.GenerateMock<IXXX>();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(secondEventDateTime);

            xxx.AssertWasCalled(it => it.PauseBeforeEvent(Arg<TimeSpan>.Is.Equal(TimeSpan.FromMilliseconds(600))));
        }

        [Test]
        public void it_records_the_event_AFTER_pausing_for_the_current_event()
        {
            DateTime firstEventDateTime = new DateTime(2000, 1, 1, 12, 0, 0);
            DateTime secondEventDateTime = firstEventDateTime.AddMilliseconds(400); // <-- Second event is 400ms after the first event. This should trigger a pause of 600ms.
            
            var mocks = new MockRepository();

            var timeline = mocks.StrictMock<IEventTimeline>();
            var xxx = mocks.StrictMock<IXXX>();

            using (mocks.Ordered())
            {
                Expect.Call(timeline.GetLastEventDateAndTime()).Return(firstEventDateTime);
                Expect.Call(() => xxx.PauseBeforeEvent(Arg<TimeSpan>.Is.Anything));
                Expect.Call(() => timeline.RecordEvent(Arg<DateTime>.Is.Anything));
            }

            mocks.ReplayAll();

            TrickleRateLimiter rateLimiter = new TrickleRateLimiter(timeline, xxx);
            rateLimiter.CheckAndEnforceRateLimit(secondEventDateTime);

            mocks.VerifyAll();
        }
    }
}