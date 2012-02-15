<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Leaf.Web._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Recognize leaf from photo
    </h2>
    <p>
        Leaf recognition service, just upload photo of leaf and submit.
    </p>
    <asp:PlaceHolder ID="flashMessageBox" runat="server" Visible="false">
        <div style="margin: 20px; border: 1px solid #476bd6; background: #A7D4EB; padding: 10px;
            text-align: center">
            <asp:Label ID="flashMessageText" runat="server" Text=""></asp:Label>
        </div>
    </asp:PlaceHolder>
    <fieldset>
        <legend>Recognize</legend>
        <table>
            <tr>
                <td>
                    <strong>Photo:</strong>
                </td>
                <td>
                    <asp:FileUpload ID="LeafPhotoUpload" runat="server" />
                </td>
                <!--/tr>
        <tr-->
                <td>
                </td>
                <td>
                    <asp:Button ID="ProcessUpload" runat="server" OnClientClick="ShowLoader();" Text="Recognize it!"
                        OnClick="ProcessUpload_Click" />
                </td>
            </tr>
        </table>
    </fieldset>
    <script type="text/javascript">
        function ShowLoader() {
            var cvr = document.getElementById("cover");
            var loader = document.getElementById("loading");
            cvr.style.display = "block";
            loader.style.display = "block";
        }
    </script>
    <div id="loading">
        Please wait, recognizing...</div>
    <asp:PlaceHolder ID="ResponsePlaceHolder" runat="server" Visible="false">
        <fieldset>
            <legend>Results</legend>
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
                                    <%# ((LeafWeb.LeafService.Tree)(Container.DataItem)).RodoveCzech %>
                                    <%# ((LeafWeb.LeafService.Tree)(Container.DataItem)).DruhoveCzech %>
                                </b>(<%# ((LeafWeb.LeafService.Tree)(Container.DataItem)).RodoveLatin %>
                                    <%# ((LeafWeb.LeafService.Tree)(Container.DataItem)).DruhoveLatin %>) <span style="color: silver">
                                        <%# Math.Round(((LeafWeb.LeafService.Tree)(Container.DataItem)).Confidence,5) %>
                                    </span></li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ol>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                    <td valign="middle" style="text-align: center">
                        <asp:Image ID="ImageUploaded" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:PlaceHolder>
</asp:Content>
