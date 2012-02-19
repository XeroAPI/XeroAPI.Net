Imports NUnit.Framework
Imports XeroApi.Model

<TestFixture()>
Public Class ApiQueryTests

    <Test()>
    Public Sub it_can_query_a_root_endpoint()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim organisations As List(Of Organisation) = repository.Organisations.ToList()


        Assert.AreEqual("Organisation", integrationProxy.LastQueryDescription.ElementName)

    End Sub


    'https://community.xero.com/developer/discussion/48341/
    <Test()>
    Public Sub it_can_filter_using_inline_string_equals_comparison_Q39411()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim organisations As List(Of Organisation) = repository.Organisations.Where(Function(o) o.Name = "Demo Company").ToList()

        Dim linqQueryDescription As Linq.LinqQueryDescription = integrationProxy.LastQueryDescription

        Assert.AreEqual("(Name == ""Demo Company"")", linqQueryDescription.Where)
        Assert.AreEqual("Organisation", linqQueryDescription.ElementName)

    End Sub


    'https://community.xero.com/developer/discussion/48341/
    <Test()>
    Public Sub it_can_filter_using_variable_string_equals_comparison_Q48341()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim contactname As String = "Fresh Start Bakeries NZ"
        Dim myContact2 As Contact = repository.Contacts.FirstOrDefault(Function(c) c.Name = contactname)

        Dim linqQueryDescription As Linq.LinqQueryDescription = integrationProxy.LastQueryDescription

        Assert.AreEqual("(Name == ""Fresh Start Bakeries NZ"")", linqQueryDescription.Where)
        Assert.AreEqual("Contact", linqQueryDescription.ElementName)

    End Sub


    'https://community.xero.com/developer/discussion/48341/
    <Test()>
    Public Sub it_can_filter_using_variable_string_not_equals_comparison_Q48341()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim contactname As String = "Fresh Start Bakeries NZ"
        Dim contact As Contact = repository.Contacts.FirstOrDefault(Function(c) c.Name <> contactname)

        Dim linqQueryDescription As Linq.LinqQueryDescription = integrationProxy.LastQueryDescription

        Assert.AreEqual("(Name <> ""Fresh Start Bakeries NZ"")", linqQueryDescription.Where)
        Assert.AreEqual("Contact", linqQueryDescription.ElementName)

    End Sub


    <Test()>
    Public Sub it_can_filter_using_inline_date_equals_comparison()

        Dim integrationProxy As New StubIntegrationProxy()
        Dim repository As New Repository(integrationProxy)

        Dim organisations As List(Of Organisation) = repository.Organisations.Where(Function(o) o.CreatedDateUTC = New DateTime(2000, 4, 1)).ToList()

        Dim linqQueryDescription As Linq.LinqQueryDescription = integrationProxy.LastQueryDescription

        Assert.AreEqual("(CreatedDateUTC == DateTime(2000,4,1))", linqQueryDescription.Where)
        Assert.AreEqual("Organisation", linqQueryDescription.ElementName)

    End Sub

End Class
