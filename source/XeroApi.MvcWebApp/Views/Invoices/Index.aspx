<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Xero.ScreencastWeb.Models.Invoice>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th>Number</th>
            <th>Invoice Date</th>
            <th>Total</th>
            <th>Amount Due</th>
        </tr>

    <% if (Model != null) { 

     foreach (var item in Model) { %>
    
        <tr>
            <td><%=item.InvoiceNumber%></td>
            <td><%=item.Date.ToString("dd-MMM-yyyy")%></td>
            <td><%=item.Total.ToString("c")%></td>
            <td><%=item.AmountDue.ToString("c")%></td>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id = item.InvoiceID })%> |
                <%= Html.ActionLink("Details", "Details", new { id = item.InvoiceID })%>
            </td>
        </tr>
    
    <% }
       } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

