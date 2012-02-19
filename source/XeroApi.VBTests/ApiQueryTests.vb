Imports NUnit.Framework
Imports XeroApi.Model

<TestFixture()>
Public Class ApiQueryTests

    <Test()>
    Public Sub it_can_query_root_endpoint()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim organisations As List(Of Organisation) = repository.Organisations.ToList()

        Assert.AreEqual(0, organisations.Count)
        Assert.AreEqual("Organisation", integrationProxy.LastQueryDescription.ElementName)

    End Sub

End Class
