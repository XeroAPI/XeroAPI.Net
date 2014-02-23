Imports System.IO
Imports XeroApi.Integration
Imports XeroApi.Model

Public Class StubIntegrationProxy
    Implements IIntegrationProxy

    
    Public Function FindElements(apiQueryDescription As IApiQueryDescription) As String Implements IIntegrationProxy.FindElements
        LastQueryDescription = apiQueryDescription
        Return GenerateSampleResponseXml(apiQueryDescription.ElementName)
    End Function


    Public Function FindAttachments(endpointName As String, itemId As String) As String Implements IIntegrationProxy.FindAttachments
        Return GenerateSampleResponseXml(endpointName)
    End Function


    Public Function FindOne(endpointName As String, itemId As String, acceptMimeType As String) As Byte() Implements IIntegrationProxy.FindOne
        Return Nothing
    End Function


    Public Function FindOneAttachment(endpointName As String, itemId As String, attachmentIdOrFileName As String) As Stream Implements IIntegrationProxy.FindOneAttachment
        Return Stream.Null
    End Function


    Public Function UpdateOrCreateElements(endpointName As String, body As String) As String Implements IIntegrationProxy.UpdateOrCreateElements
        Return GenerateSampleResponseXml(endpointName)
    End Function


    Public Function UpdateOrCreateAttachment(endpointName As String, itemId As String, attachment As Attachment) As String Implements IIntegrationProxy.UpdateOrCreateAttachment
        Return GenerateSampleResponseXml(endpointName)
    End Function


    Public Function CreateElements(endpointName As String, body As String) As String Implements IIntegrationProxy.CreateElements
        Return GenerateSampleResponseXml(endpointName)
    End Function


    Public Function CreateAttachment(endpointName As String, itemId As String, attachment As Attachment) As String Implements IIntegrationProxy.CreateAttachment
        Return GenerateSampleResponseXml(endpointName)
    End Function

    Private Function GenerateSampleResponseXml(elementName As String) As String

        Return "<Response><Id>" & Guid.NewGuid().ToString & "</Id><Status>OK</Status><ProviderName>NullIntegrationProxy</ProviderName><DateTimeUTC>" & DateTime.UtcNow.ToString("s") & "</DateTimeUTC><" & elementName & "s" & " /></Response>"

    End Function

    Public LastQueryDescription As IApiQueryDescription = Nothing

    Public Function ApplyAllocation(creditNote As CreditNote, xml As String) As String Implements IIntegrationProxy.ApplyAllocation
        Return String.Empty
    End Function

End Class
