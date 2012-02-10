using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Status = "ACTIVE",
            Date = new DateTime(2012, 01, 30),
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


        [Test]
        [Ignore("Github is replacing CrLf with Cr only. This is causing problems on the build sever.")]
        public void TestManualJournalCanBeSerialized()
        {
            string xml = ModelSerializer.Serialize(SampleManualJournal);

            Assert.AreEqual(@"<ManualJournal>
  <ManualJournalID>c8eee1d2-ef1c-401a-8788-2e97d5f34aba</ManualJournalID>
  <Status>ACTIVE</Status>
  <LineAmountTypes>NoTax</LineAmountTypes>
  <Narration>Test Manual Journal</Narration>
  <JournalLines>
    <JournalLine>
      <Description>Description</Description>
      <LineAmount>10.0</LineAmount>
      <AccountCode>200</AccountCode>
    </JournalLine>
  </JournalLines>
</ManualJournal>".Replace("'", "\""), xml);
        }


        [Test]
        public void Test_ModelSerializer_Serialize_and_Serializer2_method_output_identical_xml()
        {
            string xml1 = ModelSerializer.Serialize(SampleManualJournal);
            string xml2 = ModelSerializer.Serialize2(SampleManualJournal);
            
            Assert.IsNotEmpty(xml1);
            Assert.IsNotEmpty(xml2);
            
            Assert.AreEqual(xml1, xml2);
        }

    }
}
