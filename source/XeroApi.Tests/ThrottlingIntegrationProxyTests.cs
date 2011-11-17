using System;
using NUnit.Framework;
using Rhino.Mocks;
using XeroApi.Integration;

namespace XeroApi.Tests
{
    public class ThrottlingIntegrationProxyTests
    {
        [Test]
        public void it_enforces_the_rate_limiter_and_delegates_to_its_inner_integration_proxy_for_each_invocation()
        {
            var inner = MockRepository.GenerateStub<IIntegrationProxy>();
            
            var rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            var integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);

            integrationProxy.CreateAttachment(null, null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.CreateAttachment(null, null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.CreateElements(null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.CreateElements(null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.FindAttachments(null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.FindAttachments(null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.FindOneAttachment(null, null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.FindOneAttachment(null, null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.UpdateOrCreateAttachment(null, null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.UpdateOrCreateAttachment(null, null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.UpdateOrCreateElements(null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.UpdateOrCreateElements(null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.FindOne(null, null, null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.FindOne(null, null, null));

            rateLimiter = MockRepository.GenerateStub<IRateLimiter>();
            integrationProxy = new ThrottlingIntegrationProxy(inner, rateLimiter);
            integrationProxy.FindElements(null);
            rateLimiter.AssertWasCalled(it => it.CheckAndEnforceRateLimit(Arg<DateTime>.Is.Anything), opt => opt.Repeat.Once()); 
            inner.AssertWasCalled(it => it.FindElements(null));
        }
    }
}
