using System;
using NUnit.Framework;
using XeroApi.Integration;
using XeroApi.Model;

namespace XeroApi.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void it_can_UpdateOrCreate_a_model_that_represents_a_valid_endpoint()
        {
            var integrationProxy = new Stubs.StubIntegrationProxy();

            Repository repository = new Repository(integrationProxy);

            repository.UpdateOrCreate(new Invoice());

            Assert.AreEqual("Invoice", integrationProxy.LastEndpointName);
        }
    }
}
