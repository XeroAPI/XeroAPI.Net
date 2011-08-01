<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Xero.ScreencastWeb.Models.Contact>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th>Contact Name</th>
            <th>Contact Person</th>
            <th>Email</th>
            <th>Telephone</th>
            <th>&nbsp;</th>

        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td><%=Html.Encode(item.Name) %></td>
            <td><%=Html.Encode(string.Format("{0} {1}", item.FirstName, item.LastName)) %></td>
            <td><%=Html.Encode(item.EmailAddress) %></td>
            <td><%=Html.Encode(item.Phones.Where(phone => !phone.IsEmpty).FirstOrDefault())%></td>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.ContactID }) %> |
                <%= Html.ActionLink("Details", "Details", new { id=item.ContactID })%>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

