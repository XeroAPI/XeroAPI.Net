Imports DevDefined.OAuth.Storage.Basic
Imports DevDefined.OAuth.Logging
Imports DevDefined.OAuth.Consumer
Imports XeroApi.OAuth
Imports DevDefined.OAuth.Framework

Public Class PublicApplicationRunner

    Const UserAgent As String = "Xero.API.Console.VB (Public App Testing)"
    Const ConsumerKey As String = "ZGIXM2M1Y2RIZJGYNGY1Y2EWZGYZMW"
    Const ConsumerSecret As String = "RZRCMBRPK57EAG6GRO4GPLYDH9REPX"

    Public Shared Function CreateRepository() As Repository

        Dim consumerSession As IOAuthSession = New XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret, New InMemoryTokenRepository())

        consumerSession.MessageLogger = New DebugMessageLogger()

        ' 1. Get a request token
        Dim requestToken As RequestToken = consumerSession.GetRequestToken()

        Console.WriteLine("Request Token Key: {0}", requestToken.Token)
        Console.WriteLine("Request Token Secret: {0}", requestToken.TokenSecret)


        ' 2. Get the user to log into Xero using the request token in the querystring
        Dim authorisationUrl As String = consumerSession.GetUserAuthorizationUrl()
        Process.Start(authorisationUrl)


        ' 3. Get the use to enter the authorisation code from Xero (4-7 digit number)
        Console.WriteLine("Please input the code you were given in Xero:")
        Dim verificationCode As String = Console.ReadLine()

        If (String.IsNullOrEmpty(verificationCode)) Then

            Console.WriteLine("You didn't type a verification code!")
            Return Nothing

        End If


        ' 4. Use the request token and verification code to get an access token
        Dim accessToken As AccessToken


        Try
            accessToken = consumerSession.ExchangeRequestTokenForAccessToken(verificationCode.Trim())

        Catch ex As OAuthException

            Console.WriteLine("An OAuthException was caught:")
            Console.WriteLine(ex.Report)
            Return Nothing

        End Try

        Console.WriteLine("Access Token Key: {0}", accessToken.Token)
        Console.WriteLine("Access Token Secret: {0}", accessToken.TokenSecret)
        Console.WriteLine("Access Token Lasts for: {0}", accessToken.TokenTimespan)

        ' Wrap the authenticated consumerSession in the repository...
        Return New Repository(consumerSession)

    End Function

End Class
