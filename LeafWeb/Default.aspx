<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="LeafWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Recognize leaf from photo
    </h2>
    <p>
        Simple test interface, just upload photo of leaf and submit.
    </p>
    <asp:PlaceHolder ID="flashMessageBox" runat="server" Visible="false">
        <div style="margin: 20px; border: 1px solid #476bd6; background: #A7D4EB; padding: 10px;
            text-align: center">
            <asp:Label ID="flashMessageText" runat="server" Text=""></asp:Label>
        </div>
    </asp:PlaceHolder>
    <p>
        <table>
            <tr>
                <td>
                    Photo:
                </td>
                <td>
                    <asp:FileUpload ID="LeafPhotoUpload" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="ProcessUpload" runat="server" Text="Recognize" OnClick="ProcessUpload_Click" />
                </td>
            </tr>
        </table>
    </p>
    <asp:PlaceHolder ID="ResponsePlaceHolder" runat="server" Visible="false">
        <asp:Label ID="LabelResults" runat="server" Text=""></asp:Label>
        <table style="width: 100%">
            <tr>
                <td valign="top" style="width: 50%">
                    <asp:Repeater ID="ResultsRepeater" runat="server">
                        <HeaderTemplate>
                            <ol>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li><b>
                                <%# ((LeafWeb.LeafService.RecognizedLeaf)(Container.DataItem)).Name %>
                            </b>(<%# ((LeafWeb.LeafService.RecognizedLeaf)(Container.DataItem)).LatinName %>) <span
                                style="color: silver">
                                <%# ((LeafWeb.LeafService.RecognizedLeaf)(Container.DataItem)).Confidency %>
                            </span></li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ol>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
                <td valign="middle">
                    <asp:Image ID="ImageUploaded" runat="server" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
