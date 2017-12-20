<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryItemError.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryItemError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <style type="text/css">
        UL LI
        {
            list-style-type: none;
        }
    </style>
    <asp:MultiView ID="MLVerrors" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstandard" runat="server">
            <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                clear: both;" runat="server">
                <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Text="Retry" Height="18px"></asp:HyperLink>
                <asp:HyperLink ID="HYPbackToList" runat="server" CssClass="Link_Menu" Text="Back to list"
                    Height="18px"></asp:HyperLink>
            </div>
            <div id="Div2" style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;
                clear: both;">
                <asp:Label ID="LBfileError" runat="server"></asp:Label>
                <div style="padding-top: 20px;">
                    <asp:Button ID="BTNlogin" runat="server" Visible="false" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWrename" runat="server">
            <div id="Div3" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                clear: both;" runat="server">
                <asp:HyperLink ID="HYPbackToUploader" runat="server" CssClass="Link_Menu" Text="Retry" Height="18px"></asp:HyperLink>
                 <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Rename items" ID="LNBrenameItems"
                CausesValidation="false"></asp:LinkButton>
            </div>
            <div style="height: 400px; padding: 20px, auto; margin: 0, auto; background-color: White;">
                <asp:Label ID="LBrenameFiles_t" runat="server"></asp:Label>
                <asp:Repeater ID="RPTfileName" runat="server">
                    <HeaderTemplate>
                        <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div>
                                    <asp:Label ID="LBfileNameToReplace" runat="server" AssociatedControlID="TXBfileName"></asp:Label>
                                </div>
                                <div style="padding-left: 50px;">
                                    <asp:Literal ID="LTfileID" runat="server" Visible="false"></asp:Literal>
                                    <asp:Literal ID="LTimageFile" runat="server"></asp:Literal>
                                    <asp:TextBox ID="TXBfileName" runat="server" Columns="60" MaxLength="150"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RFVfileName" runat="server" ControlToValidate="TXBfileName"
                                        Text="*"></asp:RequiredFieldValidator>
                                    <asp:Literal ID="LTfileType" runat="server"></asp:Literal>
                                    <asp:Literal ID="LTfileOldName" runat="server" Visible="false"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <br />
                <br />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
