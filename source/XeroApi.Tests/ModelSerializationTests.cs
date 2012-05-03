using System;
using NUnit.Framework;
using XeroApi.Model;

namespace XeroApi.Tests
{
    [TestFixture]
    public class ModelSerializationTests
    {
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

        private readonly string SampleManualJournalXml =
            "<ManualJournal status=\"OK\">" + Environment.NewLine +
                "  <ManualJournalID>c8eee1d2-ef1c-401a-8788-2e97d5f34aba</ManualJournalID>" + Environment.NewLine +
                "  <Date>2012-01-30T00:00:00</Date>" + Environment.NewLine +
                "  <Status>ACTIVE</Status>" + Environment.NewLine +
                "  <LineAmountTypes>NoTax</LineAmountTypes>" + Environment.NewLine +
                "  <Narration>Test Manual Journal</Narration>" + Environment.NewLine +
                "  <JournalLines>" + Environment.NewLine +
                "    <JournalLine status=\"OK\">" + Environment.NewLine +
                "      <Description>Description</Description>" + Environment.NewLine +
                "      <LineAmount>10.0</LineAmount>" + Environment.NewLine +
                "      <AccountCode>200</AccountCode>" + Environment.NewLine +
                "    </JournalLine>" + Environment.NewLine +
                "  </JournalLines>" + Environment.NewLine +
                "</ManualJournal>";

        [Test]
        public void serialize_method_can_serialize_a_sample_manual_journal()
        {
            string actualXml = ModelSerializer.Serialize(SampleManualJournal);
            Assert.AreEqual(SampleManualJournalXml, actualXml);
        }

        [Test]
        public void serialize2_method_can_serialize_a_sample_manual_journal()
        {
            string actualXml = ModelSerializer.Serialize2(SampleManualJournal);
            Assert.AreEqual(SampleManualJournalXml, actualXml);
        }
        

        [Test]
        public void serialize_method_can_omit_string_properties_that_are_null()
        {
            Contact contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = null } } };

            var xml = ModelSerializer.Serialize(contact);
            Assert.IsFalse(xml.Contains("PhoneAreaCode")); 
        }

        [Test]
        public void serialize_2_method_can_omit_string_properties_that_are_null()
        {
            Contact contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = null } } };
            
            var xml = ModelSerializer.Serialize2(contact);
            Assert.IsFalse(xml.Contains("PhoneAreaCode"));
        }

        [Test]
        public void serialize_method_can_serialise_properties_that_are_empty_strings()
        {
            Contact contact = new Contact { Name="Jason", Phones = new Phones { new Phone { PhoneAreaCode = "" } } };

            var xml = ModelSerializer.Serialize(contact);
            Assert.IsTrue(xml.Contains("<PhoneAreaCode />"));
        }

        [Test]
        public void serialize_2_method_can_serialise_properties_that_are_empty_strings()
        {
            Contact contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = "" } } };

            var xml = ModelSerializer.Serialize2(contact);
            Assert.IsTrue(xml.Contains("<PhoneAreaCode />"));
        }

        [Test]
        public void serialize_method_can_serialize_properties_that_are_populated()
        {
            Contact contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = "04" } } };

            var xml = ModelSerializer.Serialize(contact);
            Assert.IsTrue(xml.Contains("<PhoneAreaCode>04</PhoneAreaCode>"));
        }

        [Test]
        public void serialize_2_method_can_serialize_properties_that_are_populated()
        {
            Contact contact = new Contact { Name = "Jason", Phones = new Phones { new Phone { PhoneAreaCode = "04" } } };
            
            var xml = ModelSerializer.Serialize2(contact);
            Assert.IsTrue(xml.Contains("<PhoneAreaCode>04</PhoneAreaCode>"));
        }

    }
}
