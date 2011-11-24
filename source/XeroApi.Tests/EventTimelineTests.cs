using System;
using NUnit.Framework;
using XeroApi.Integration;

namespace XeroApi.Tests
{
    public class EventTimelineTests
    {
        [Test]
        public void it_can_record_event_timestamps()
        {
            var timeline = new EventTimeline();
            
            DateTime eventDate = DateTime.Parse("07-Dec-2001");

            timeline.RecordEvent(eventDate);

            Assert.AreEqual(eventDate, timeline.GetLastEventDateAndTime(), "Unexpected event returned");
        }

        [Test]
        public void it_returns_only_the_most_recent_event_timestamp_no_matter_what_order_they_are_added()
        {
            var timeline = new EventTimeline();

            DateTime decemberSeventh = DateTime.Parse("07-Dec-2001");
            DateTime decemberEighth = DateTime.Parse("08-Dec-2001");

            timeline.RecordEvent(decemberEighth);
            timeline.RecordEvent(decemberSeventh);

            Assert.AreEqual(decemberEighth, timeline.GetLastEventDateAndTime(), "Unexpected event returned. Expected the most recent timestamp.");
        }

        [Test]
        public void it_returns_null_if_no_events_have_been_recorded()
        {
            var timeline = new EventTimeline();

            Assert.IsNull(timeline.GetLastEventDateAndTime(), "Expected null returned since nothing has been added");
        }
    }
}
