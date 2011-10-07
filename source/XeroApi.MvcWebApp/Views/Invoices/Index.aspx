<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<XeroApi.Model.Invoice>>" %>

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
            <td><%=item.Date.HasValue ? item.Date.Value.ToString("dd-MMM-yyyy") : string.Empty %></td>
            <td><%=item.Total.HasValue ? item.Total.Value.ToString("c") : string.Empty %></td>
            <td><%=item.AmountDue.HasValue ? item.AmountDue.Value.ToString("c") : string.Empty %></td>
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

