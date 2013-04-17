using NUnit.Framework;
using XeroApi.Model;
using XeroApi.Model.Serialize;

namespace XeroApi.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void it_can_UpdateOrCreate_a_model_that_represents_a_valid_endpoint()
        {
            var integrationProxy = new Stubs.XmlStubIntegrationProxy();

            var repository = new CoreRepository(integrationProxy, new XmlModelSerializer());

            repository.UpdateOrCreate(new Invoice());

            Assert.AreEqual("Invoice", integrationProxy.LastEndpointName);
        }

        [Test]
        public void it_can_UpdateOrCreate_a_model_that_represents_a_valid_endpoint_with_json()
        {
            var integrationProxy = new Stubs.JsonStubIntegrationProxy();

            var repository = new CoreRepository(integrationProxy, new JsonModelSerializer());

            repository.UpdateOrCreate(new Invoice());

            Assert.AreEqual("Invoice", integrationProxy.LastEndpointName);
        }
    }
}
