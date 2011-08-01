<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Xero.ScreencastWeb.Models"%>
<%@ Import Namespace="Xero.ScreencastWeb.Services"%>
<div>
    <%
    HttpSessionAccessTokenRepository accessTokenRepository = new HttpSessionAccessTokenRepository(new HttpSessionStateWrapper(Session));
    var accessToken = accessTokenRepository.GetToken("");
    
    if (accessToken != null && !accessToken.HasExpired()) {
    %>
        <div>Access Token is valid</div>
        <div><%=Session["xero_organisation_name"]%></div>
    <% } else { %>
        <span>Not Connected</span>
    <% } %>
    
</div>