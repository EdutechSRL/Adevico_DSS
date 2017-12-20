<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityRepositoryItemPermission.aspx.vb" Inherits="Comunita_OnLine.CommunityRepositoryItemPermission" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="~/UC/UC_SearchUserByCommunities.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            $('#selectRole').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 700,
                height: 400,
                minHeight: 400,
                //                minWidth: 700,
                zIndex: 1000,
                open: function(type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                   //$(this).parent().appendTo("form");
                }

            });

            $('#applyToSubItems').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 500,
                height: 200,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 1000,
                open: function(type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                   // $(this).parent().appendTo("form");
                }

            });
        });

        function showDialog(id) {
            $('#' + id).dialog("open");
            return false;
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>

    <asp:UpdatePanel ID="UPdpermissionContainer" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div id="DVmenu" runat="server" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                padding-bottom: 5px;">
                <div style="float: left; text-align: left; width: 420px">
                    &nbsp;
                </div>
                <div style="float: left; text-align: right; width: 480px;">
                    <span style="vertical-align: text-bottom;">
                        <asp:HyperLink ID="HYPbackToDownloads" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Back to downloads" Height="18px" Target="_self"></asp:HyperLink>
                        <asp:HyperLink ID="HYPbackToManagement" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Back to management" Height="18px" Target="_self"></asp:HyperLink>
                        <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu" Visible="false"
                            Text="Undo" Height="18px" Target="_self"></asp:HyperLink>
                        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Upload" ID="LNBsave" CausesValidation="false"
                            Visible="false"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <asp:MultiView ID="MLVdata" runat="server" ActiveViewIndex="1">
                <asp:View ID="VIWdata" runat="server">
                    <div style="text-align: center; width: 900px;">
                        <div style="text-align: left">
                            <asp:Label ID="LBinfoItem_t" runat="server">You want to modify permission for file:{0}</asp:Label>
                        </div>
                        <div style="text-align: left; padding-top: 15px;">
                            <asp:Label ID="LBinfoItem" runat="server">This file is downlodable by:</asp:Label>
                            <%--   <asp:UpdatePanel ID="UDPpermission" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                            <div style="width: 900px; padding-top: 10px; padding-left: 10px;">
                                <asp:RadioButtonList ID="RBLallowTo" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Vertical"
                                    RepeatLayout="Flow" AutoPostBack="true">
                                    <asp:ListItem Selected="true" Value="True">All community members</asp:ListItem>
                                    <asp:ListItem Value="False">Some users/role</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <asp:MultiView ID="MLVpermission" runat="server" ActiveViewIndex="1">
                                <asp:View ID="VIWdefinePermission" runat="server">
                                    <div style="width: 900px; padding-top: 15px;">
                                        <div style="float: left; width: 80%">
                                            <asp:Label ID="LBrolesInfo" runat="server">ROLES</asp:Label>
                                        </div>
                                        <div style="float: left; text-align: right; width: 20%">
                                            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Add roles" ID="LNBroles"
                                                CausesValidation="false" OnClientClick="showDialog('selectRole');"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <hr />
                                    <div style="width: 890px; padding-left: 10px;">
                                        <asp:CheckBoxList ID="CBLselectedRole" runat="server" CssClass="Testo_campoSmall"
                                            RepeatLayout="table" RepeatColumns="4" RepeatDirection="vertical">
                                        </asp:CheckBoxList>
                                    </div>
                                    <div style="width: 900px; padding-top: 15px;">
                                        <div style="float: left; width: 80%">
                                            <asp:Label ID="LBpersonInfo" runat="server">USERS</asp:Label></div>
                                        <div style="float: left; text-align: right; width: 20%">
                                            <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Add user" ID="LNBaddPerson"
                                                CausesValidation="false"></asp:LinkButton></div>
                                    </div>
                                    <hr />
                                    <div style="width: 890px; padding-left: 10px;">
                                        <asp:Repeater ID="RPTperson" runat="server">
                                            <ItemTemplate>
                                                <div style="width: 200px; float: left; font-size: x-small;">
                                                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="remove"
                                                        CommandArgument='<%#Container.DataItem.Id%>'></asp:LinkButton>
                                                    <%#Container.DataItem.Name%></div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </asp:View>
                                <asp:View ID="VIWnone" runat="server">
                                </asp:View>
                            </asp:MultiView>
                            <%--  </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="VIWnoPermissionToEdit" runat="server">
                    <div style="padding-top: 100px; padding-bottom: 100px;">
                        <asp:Label ID="LBnoPermissionToEdit" runat="server" CssClass=""></asp:Label></div>
                </asp:View>
                <asp:View ID="VIWusers" runat="server">
                    <div style="clear: both;">
                        <div style="clear: both;">
                            <CTRL:USERlist ID="CTRLuserList" runat="server" AjaxEnabled="true"></CTRL:USERlist>
                        </div>
                        <div style="clear: both; text-align: right;">
                            <asp:LinkButton ID="LNBunSavePerson" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('selectRole')"></asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="LNBsavePerson" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('selectRole')"></asp:LinkButton>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="selectRole" style="display: none;">
        <img src="../../images/blanck.jpg" style="height: 450px; width: 1px; float: left;" alt="" />
        <asp:UpdatePanel ID="UDPselectRoles" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="float: left;">
                    <div style="clear: both;">
                        <div style="float: left;">
                            <asp:Label ID="LBselectRole" runat="server" CssClass="Titolo_campoSmall"></asp:Label></div>
                        <div style="float: left; padding-left: 5px;">
                            <asp:CheckBoxList ID="CBLselectRole" runat="server" CssClass="Testo_campoSmall" RepeatLayout="table"
                                RepeatColumns="3" RepeatDirection="vertical">
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div style="clear: both; text-align: right;">
                        <asp:LinkButton ID="LNBunSaveRole" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('selectRole');return false;"></asp:LinkButton>&nbsp;
                        <asp:LinkButton ID="LNBsaveRole" runat="server" CssClass="Link_Menu"></asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="applyToSubItems" style="display: none;">
        <div style="clear: both;">
            <asp:Label ID="LBapplyTo" runat="server" CssClass="Titolo_campoSmall"></asp:Label>
            <br /><br />
        </div>
        <div style="clear: both; text-align: right;">
            <asp:LinkButton ID="LNBundo" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return false;"></asp:LinkButton>&nbsp;
            <asp:LinkButton ID="LNBapplyToThis" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return true;"></asp:LinkButton>&nbsp;
            <asp:LinkButton ID="LNBapplyToSubItems" runat="server" CssClass="Link_Menu" OnClientClick="closeDialog('applyToSubItems');return true;"></asp:LinkButton>
        </div>
    </div>
</asp:Content>
