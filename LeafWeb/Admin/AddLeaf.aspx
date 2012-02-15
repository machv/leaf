<%@ Page Title="Add leaf" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="AddLeaf.aspx.cs" Inherits="Leaf.Web.Admin.AddLeaf" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script language="javascript" type="text/javascript">
        function OnSelectedIndexChange(select) {
            document.getElementById("newRow").style.display = (select[select.selectedIndex].value == 0) ? "" : "none";
        }
    </script>
    <h2>
        Add new leaf to database
    </h2>
    <asp:PlaceHolder ID="flashMessageBox" runat="server" Visible="false">
        <div style="margin: 20px; border: 1px solid #476bd6; background: #A7D4EB; padding: 10px;
            text-align: center">
            <asp:Label ID="flashMessageText" runat="server" Text=""></asp:Label>
        </div>
    </asp:PlaceHolder>
    <table>
        <asp:PlaceHolder ID="PlaceHolderUpload" runat="server" Visible="true">
            <tr>
                <td style="font-weight: bold">
                    Image:
                </td>
                <td>
                    <asp:FileUpload ID="fuLeafFile" runat="server" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr>
            <td style="font-weight: bold">
                Tree name:
            </td>
            <td>
                <asp:DropDownList ID="DropDownListTrees" runat="server" DataTextField="Name" DataValueField="ID">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <i>If tree is not listed in selectbox, you can add new one selecting <strong>-- New
                    tree --</strong> in dropdown list.</i>
            </td>
        </tr>
        <tr style="display: none" id="newRow">
            <td valign="top" style="font-weight: bold">
                New tree:
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            Name (Czech):
                        </td>
                        <td>
                            <asp:TextBox ID="txtTreeNameCzechRodove" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTreeNameCzechDruhove" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Name (Latin):
                        </td>
                        <td>
                            <asp:TextBox ID="txtTreeNameLatinRodove" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTreeNameLatinDruhove" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <!--tr>
                    <td>Photo:</td>
                    <td colspan="2"><asp:TextBox ID="txtTreePhoto" runat="server"></asp:TextBox></td>
                    </tr-->
                </table>
            </td>
        </tr>
        <asp:PlaceHolder ID="PlaceHolderPreview" runat="server" Visible="true">
            <tr>
                <td>
                    <strong>Preview: </strong>
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxPreview" runat="server" Text="Preview descriptor before saving to database" />
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="PlaceHolderImage" runat="server" Visible="false">
            <tr>
                <td>
                    <strong>Descriptor:</strong>
                </td>
                <td>
                    <p>
                        <i>Preview of computed descriptor.</i></p>
                    <asp:Image ID="ImageUploaded" runat="server" AlternateText="Visualised descriptor" />
                </td>
            </tr>
            <script type="text/javascript">
                var logo = document.getElementById("<%=ImageUploaded.ClientID %>");

                $(document).ready(function () {
                    ShowLoaderImage();
                });

                logo.onload = function () {
                    HideLoader();
                };
            </script>
        </asp:PlaceHolder>
        <tr>
            <td>
                <asp:HiddenField ID="HiddenFieldIsPreview" Value="0" runat="server" />
            </td>
            <td>
                <asp:Button ID="ButtonSaveLeaf" runat="server" Text="Save leaf" OnClientClick="ShowLoader();"
                    OnClick="ButtonSaveLeaf_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        var IsPreview = <%= IsNewTreeInPreview ? 1 : 0 %>;

        if(IsPreview)
        {
            document.getElementById("newRow").style.display="";
        }

        var CheckBoxPreview = document.getElementById("<%= CheckBoxPreview.ClientID %>");

        function ShowLoader() {
            var loader = document.getElementById("loading");

            if (CheckBoxPreview != null && CheckBoxPreview.checked) {
                loader.innerHTML = "Generating preview...";
            }
            else {
                loader.innerHTML = "Generating descriptor...";
            }

            var cvr = document.getElementById("cover");

            cvr.style.display = "block";
            loader.style.display = "block";
        }

        function ShowLoaderImage() {
            var loader = document.getElementById("loading");
            loader.innerHTML = "Generating preview...";

            var cvr = document.getElementById("cover");

            cvr.style.display = "block";
            loader.style.display = "block";
        }

        function HideLoader()
        {
            var loader = document.getElementById("loading");

            loader.innerHTML = "Generating preview...";

            var cvr = document.getElementById("cover");

            cvr.style.display = "none";
            loader.style.display = "none";
        }
    </script>
    <div id="loading">
        Generating descriptor...</div>
</asp:Content>
