<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Xero.ScreencastWeb.Services"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <%
        var accessTokenRepository = new HttpSessionAccessTokenRepository(new HttpSessionStateWrapper(Session));
        var accessToken = accessTokenRepository.GetToken("");
    %>

    <style type="text/css">
    div.paragraph { margin-bottom:10px; }
    </style>

    <% if (!string.IsNullOrEmpty(Request["message"])) { %>
    
    <div style="border:1px solid red;padding:5px;margin:1px;">
        <h3 style="color:red;margin:2px;"><%=Request["message"] %></h3>
    </div>
    
    <% } %>

    <h2>Welcome to the Xero API Sample Web Application!</h2>
    
    <%if (accessToken == null) {%>
    <p>
        You are not currently connected to the Xero API. To connect to the API, click <%=Html.ActionLink("here", "Index", "Connect") %>
    </p> 
        
    <% } else { %>   
    <p>
        You are currently connected to the Xero API! 
        Use the menu buttons at the top of this page to read/write data to the authorised organisation.
    </p>
    
    <div class="paragraph">
        <div>Consumer Key: <%=ConfigurationManager.AppSettings["XeroApiConsumerKey"]%></div>
        <div>Consumer Secret: <%=ConfigurationManager.AppSettings["XeroApiConsumerSecret"]%></div>
    </div>
    <div class="paragraph">
        <div>Access Key: <%=accessToken.Token%></div>
        <div>Access Secret: <%=accessToken.TokenSecret%></div>
    </div>
    <div class="paragraph">
        <div>Authorised Organisation: <%=Session["xero_organisation_name"]%></div>
        <div>The OAuth connection was made on <%=Session["xero_connection_time"]%></div>
        <div>The access token will expire <%=accessToken.ExpiresIn%> seconds since it was issued, which is roughly at <%=accessToken.ExpiryDateUtc.Value.ToLocalTime().ToString("dd MMM yyyy hh:mm:ss") %></div>
    </div>
    
        <% if (!string.IsNullOrEmpty(accessToken.SessionHandle)) { %>
        <div class="paragraph">
          <div>There is a session handle associated with this access token. This can be used to refresh the access token.</div>
          <div>Session Handle: <%=accessToken.SessionHandle%></div>
          <div><%=Html.ActionLink("Click here to refresh the access token", "RefreshAccessToken", "Connect") %></div>
        </div>
        <% } %>
    <% } %>
    
    <div class="paragraph">
        Note, This website is currently running under the user <strong><%=Environment.UserDomainName %>\<%=Environment.UserName %></strong>. 
        If this website needs to access certificates in the local machine store, this user (or a group containing this user) must have access 
        to any certificates that this website uses.
    </div>
    
    <div class="paragraph">
      <% if (CertificateRepository.GetOAuthSigningCertificate() == null) { %>
        No OAuth Signing Certificate has been specified, or the certificate details do not match a certificate in the local certificate store.
      <% } else { %>
      <div>OAuth Signing Certificate Friendly Name: <%=CertificateRepository.GetOAuthSigningCertificate().FriendlyName%></div>
      <div>OAuth Signing Certificate Subject: <%=CertificateRepository.GetOAuthSigningCertificate().SubjectName.Name%></div>
      <div>OAuth Signing Certificate Start Date: <%=CertificateRepository.GetOAuthSigningCertificate().NotBefore.ToString("dd MMM yyyy hh:mm:ss")%></div>
      <div>OAuth Signing Certificate End Date: <%=CertificateRepository.GetOAuthSigningCertificate().NotAfter.ToString("dd MMM yyyy hh:mm:ss")%></div>
      <% } %>
    </div>
    
    <div class="paragraph">
      <% if (CertificateRepository.GetClientSslCertificate() == null) { %>
        No Client SSL Certificate has been specified, or the certificate details do not match a certificate in the local certificate store.
      <% } else { %>
      <div>Client SSL Certificate Friendly Name: <%=CertificateRepository.GetClientSslCertificate().FriendlyName%></div>
      <div>Client SSL Certificate Subject: <%=CertificateRepository.GetClientSslCertificate().SubjectName.Name%></div>
      <div>Client SSL Certificate Start Date: <%=CertificateRepository.GetClientSslCertificate().NotBefore.ToString("dd MMM yyyy hh:mm:ss")%></div>
      <div>Client SSL Certificate End Date: <%=CertificateRepository.GetClientSslCertificate().NotAfter.ToString("dd MMM yyyy hh:mm:ss")%></div>
      <% } %>    
    </div>
    
</asp:Content>
