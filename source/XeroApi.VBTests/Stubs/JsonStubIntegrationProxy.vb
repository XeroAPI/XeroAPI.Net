Imports System.IO
Imports XeroApi.Integration
Imports XeroApi.Model

Public Class JsonStubIntegrationProxy
    Implements IIntegrationProxy


    Public Function FindElements(apiQueryDescription As IApiQueryDescription) As String Implements IIntegrationProxy.FindElements
        LastQueryDescription = apiQueryDescription
        Return GenerateSampleResponse(apiQueryDescription.ElementName)
    End Function


    Public Function FindAttachments(endpointName As String, itemId As String) As String Implements IIntegrationProxy.FindAttachments
        Return GenerateSampleResponse(endpointName)
    End Function


    Public Function FindOne(endpointName As String, itemId As String, acceptMimeType As String) As Byte() Implements IIntegrationProxy.FindOne
        Return Nothing
    End Function


    Public Function FindOneAttachment(endpointName As String, itemId As String, attachmentIdOrFileName As String) As Stream Implements IIntegrationProxy.FindOneAttachment
        Return Stream.Null
    End Function


    Public Function UpdateOrCreateElements(endpointName As String, body As String) As String Implements IIntegrationProxy.UpdateOrCreateElements
        Return GenerateSampleResponse(endpointName)
    End Function


    Public Function UpdateOrCreateAttachment(endpointName As String, itemId As String, attachment As Attachment) As String Implements IIntegrationProxy.UpdateOrCreateAttachment
        Return GenerateSampleResponse(endpointName)
    End Function


    Public Function CreateElements(endpointName As String, body As String) As String Implements IIntegrationProxy.CreateElements
        Return GenerateSampleResponse(endpointName)
    End Function


    Public Function CreateAttachment(endpointName As String, itemId As String, attachment As Attachment) As String Implements IIntegrationProxy.CreateAttachment
        Return GenerateSampleResponse(endpointName)
    End Function

    Private Function GenerateSampleResponse(elementName As String) As String

        Return "{""Id"":" & Guid.NewGuid.ToString & """,""Status"":""OK"",""ProviderName"":""NullIntegrationProxy"",""DateTimeUTC"":""" & DateTime.UtcNow.ToString("s") + """," & elementName + "s"":[{}]"

    End Function

    Public LastQueryDescription As IApiQueryDescription = Nothing

End Class
