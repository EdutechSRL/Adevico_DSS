<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="WorkbookStatusEdit.aspx.vb" Inherits="Comunita_OnLine.WorkbookStatusEdit" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align: right;" align="center">
        <asp:HyperLink ID="HYPreturnToList" runat="server" CssClass="Link_Menu" 
            Text="Return to list" Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Save" ID="LNBsave" 
            Visible="false"></asp:LinkButton>
    </div>
    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;">
        <asp:MultiView ID="MLVworkbooks" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWlist" runat="server">
                <div style="text-align: left; padding-left: 20px; padding-top: 10px; clear: both;">
                    <div style="float: left; width: 100px; vertical-align: middle;">
                        <asp:Label ID="LBavailableFor_t" runat="server" CssClass="Titolo_campoSmall" AssociatedControlID="CBLpermission">Available for:</asp:Label>&nbsp;
                    </div>
                    <div style="float: left; vertical-align: middle;">
                        <asp:CheckBoxList ID="CBLpermission" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            runat="server" CssClass="testo_campoSmall">
                            <asp:ListItem Text="Workbook responsible" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Author" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Authors" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Workbooks administrators" Value="8"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div style="text-align: left; padding-left: 20px; padding-top: 10px; clear: both;">
                    <div style="float: left; width: 100px; vertical-align: middle;">
                        <asp:Label ID="LBdefault_t" runat="server" CssClass="Titolo_campoSmall">Default status:</asp:Label>
                    </div>
                    <div style="float: left; vertical-align: middle;">
                        <asp:CheckBox ID="CBXdefault" runat="server" CssClass="testo_campoSmall" />
                        <asp:Label ID="LBdefaultInfo" runat="server" CssClass="testo_campoSmall"></asp:Label>
                    </div>
                </div>
                <div style="text-align: left; padding-left: 20px; padding-top: 10px; clear: both;">
                    <div style="float: left; width: 100px; vertical-align: middle;">
                        <asp:Label ID="LBstatusNameFor_t" runat="server" CssClass="Titolo_campoSmall">Status name:</asp:Label>&nbsp;
                    </div>
                    <div style="float: left; vertical-align: middle;">
                        <table border="1" cellspacing="0">
                            <asp:Repeater ID="RPTstatus" runat="server">
                                <HeaderTemplate>
                                    <tr class="ROW_header_Small_Center">
                                        <td>
                                            <asp:Literal ID="LTheaderLanguage" runat="server" Visible="true" Text="Language"></asp:Literal>
                                        </td>
                                        <td>
                                            <asp:Literal ID="LTheaderTranslation" runat="server" Visible="true" Text="Translation"></asp:Literal>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="ROW_Normal_Small">
                                        <td valign="middle">
                                            <asp:Literal ID="LTlanguageName" runat="server" Visible="true" Text='<%#Container.DataItem.LanguageName%>'></asp:Literal>
                                            <asp:Literal ID="LTlanguageID" runat="server" Visible="false" Text='<%#Container.DataItem.LanguageID%>'></asp:Literal>
                                            <asp:Literal ID="LTid" runat="server" Visible="false" Text='<%#Container.DataItem.UniqueID%>'></asp:Literal>
                                        </td>
                                        <td valign="middle">
                                            <div>
                                                <asp:TextBox ID="TXBstatusName" Columns="70" runat="server" Width="600px" Text='<%#Container.DataItem.Translation%>'
                                                    SkinID="inputsmall"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFVstatustName" runat="server" ControlToValidate="TXBstatusName"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="ROW_Alternate_Small">
                                        <td valign="middle">
                                            <asp:Literal ID="LTlanguageName" runat="server" Visible="true" Text='<%#Container.DataItem.LanguageName%>'></asp:Literal>
                                            <asp:Literal ID="LTlanguageID" runat="server" Visible="false" Text='<%#Container.DataItem.LanguageID%>'></asp:Literal>
                                            <asp:Literal ID="LTid" runat="server" Visible="false" Text='<%#Container.DataItem.UniqueID%>'></asp:Literal>
                                        </td>
                                        <td valign="middle">
                                            <div>
                                                <asp:TextBox ID="TXBstatusName" Columns="70" runat="server" Width="600px" Text='<%#Container.DataItem.Translation%>'
                                                    SkinID="inputsmall"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWerrors" runat="server">
                <div style="padding-top: 180px; padding-bottom: 180px;">
                    <asp:Label ID="LBnoUpdate" runat="server"></asp:Label>
                </div>
                <div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
