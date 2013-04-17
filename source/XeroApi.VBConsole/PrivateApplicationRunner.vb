Imports DevDefined.OAuth.Storage.Basic
Imports DevDefined.OAuth.Logging
Imports DevDefined.OAuth.Consumer
Imports XeroApi.OAuth
Imports DevDefined.OAuth.Framework
Imports System.Security.Cryptography.X509Certificates

Public Class PrivateApplicationRunner

    Const UserAgent As String = "Xero.API.Console.VB (Private App Testing)"
    Const ConsumerKey As String = "A34K6MSHPFUKVVMLLFLBB7VAJ5MBH6"

    Public Shared Function CreateRepository() As Repository

        Dim privateCertificate As X509Certificate2 = New X509Certificate2("D:\Stevie-Cert.pfx", "xero")
        Dim consumerSession As IOAuthSession = New XeroApiPrivateSession(UserAgent, ConsumerKey, privateCertificate)

        consumerSession.MessageLogger = New DebugMessageLogger()

        ' Wrap the authenticated consumerSession in the repository...
        Return New Repository(consumerSession)

    End Function

End Class
