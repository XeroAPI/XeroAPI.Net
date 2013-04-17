using System;
using NUnit.Framework;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi.Tests
{
    [TestFixture]
    public class JsonModelSerializationTests
    {
        private readonly IModelSerializer _serializer = new JsonModelSerializer();

        private static readonly ManualJournal SampleManualJournal = new ManualJournal
        {
            ManualJournalID = new Guid("c8eee1d2-ef1c-401a-8788-2e97d5f34aba"),
            Narration = "Test Manual Journal",
            Date = new DateTime(2012, 01, 30),
            Status = "ACTIVE",
            UpdatedDateUTC = null,
            JournalLines = new ManualJournalLineItems
                {
                    new ManualJournalLineItem
                    {
                        AccountCode = "200",
                        Description = "Description",
                        LineAmount = 10.0m
                    }
                }
        };

        private const string SampleManualJournalJson = "{\"ManualJournalID\":\"c8eee1d2ef1c401a87882e97d5f34aba\",\"Date\":\"\\/Date(1327834800000+1300)\\/\",\"Status\":\"ACTIVE\",\"LineAmountTypes\":\"NoTax\",\"Narration\":\"Test Manual Journal\",\"JournalLines\":[{\"Description\":\"Description\",\"LineAmount\":10.0,\"AccountCode\":\"200\",\"ValidationStatus\":\"OK\"}],\"ValidationStatus\":\"OK\"}";

        [Test]
        public void serialize_method_can_serialize_a_sample_manual_journal()
        {
            string actualjson = _serializer.Serialize(SampleManualJournal);
            Assert.AreEqual(SampleManualJournalJson, actualjson);
        }

        [Test]
        public void serialize_method_can_omit_string_properties_that_are_null()
        {
            var contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = null } } };

            var json = _serializer.Serialize(contact);
            Assert.IsFalse(json.Contains("PhoneAreaCode"));
        }

        [Test]
        public void serialize_method_can_serialise_properties_that_are_empty_strings()
        {
            var contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = "" } } };

            var json = _serializer.Serialize(contact);
            Assert.IsTrue(json.Contains("PhoneAreaCode"));
        }

        [Test]
        public void serialize_method_can_serialize_properties_that_are_populated()
        {
            var contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = "04" } } };

            var json = _serializer.Serialize(contact);
            Assert.IsTrue(json.Contains("\"PhoneAreaCode\":\"04\""));
        }
    }
}
