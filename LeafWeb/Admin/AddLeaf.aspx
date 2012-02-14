<%@ Page Title="Add leaf" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AddLeaf.aspx.cs" Inherits="Leaf.Web.Admin.AddLeaf" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Add new leaf to database
    </h2>
    <table>
        <tr>
            <td>
                Image:
            </td>
            <td>
                <asp:FileUpload ID="fuLeafFile" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                Leaf name:
            </td>
            <td>
                <asp:TextBox ID="txtLeafName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="ButtonSaveLeaf" runat="server" Text="Save" />
            </td>
        </tr>
    </table>
</asp:Content>
