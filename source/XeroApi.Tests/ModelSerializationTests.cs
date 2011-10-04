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
        [Test]
        [Ignore("Github is replacing CrLf with Cr only. This is causing problems on the build sever.")]
        public void TestManualJournalCanBeSerialized()
        {
            ManualJournal manualJournal = new ManualJournal
            {
                Narration = "Test Manual Journal",
                Status = "ACTIVE",
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

            string xml = ModelSerializer.Serialize(manualJournal);

            Assert.AreEqual(@"<ManualJournal>
  <ManualJournalID>00000000-0000-0000-0000-000000000000</ManualJournalID>
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

    }
}
