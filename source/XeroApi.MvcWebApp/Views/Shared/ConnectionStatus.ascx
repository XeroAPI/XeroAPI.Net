<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Xero.ScreencastWeb.Services"%>
<div>
    <%
    var tokenRepository = ServiceProvider.CurrentTokenRepository;
    var accessToken = tokenRepository.GetAccessToken();
    
    if (accessToken != null && (!accessToken.HasExpired() ?? false)) {
    %>
        <div>Access Token is valid</div>
        <div><%=Session["xero_organisation_name"]%></div>
    <% } else { %>
        <span>Not Connected</span>
    <% } %>
    
</div>