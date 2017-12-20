<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ItemPermissions.ascx.vb" Inherits="Comunita_OnLine.UC_ItemPermissions" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsectionOpener" Src="~/Modules/Common/UC/UC_ExpandAndCollapse.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<div class="section box permissions collapsable grid_12<%=IIf(HasPendingChanges, " warning", "")%>">
    <div class="sectioninner">
        <div class="cisectionheader clearfix">
            <div class="left">
                <h3><asp:Literal ID="LTpermissionsSection" runat="server">*Permissions</asp:Literal>
                    <CTRL:CTRLsectionOpener ID="CTRLopenDescription" runat="server" />
                </h3>
                <a name="action_editPermission" href="#"></a><a name="action_viewPermission" href="#"></a>
            </div>
            <div class="right hideme">
                <div class="hideme ddbuttonlist enabled" id="DVcommands" runat="server"><!--
                    --><asp:LinkButton ID="LNBaddRoles" runat="server" Text="*Add roles" CssClass="linkMenu openpermissionrolesdlg"/><!--
                    --><asp:LinkButton ID="LNBaddUsers" runat="server" Text="*Add users" CssClass="linkMenu openpermissionusersdlg"  /><!--
                --></div>
            </div>
        </div>
        <div class="cisectioncontent">
            <div class="messages" id="DVpendingChanges" runat="server" visible="false">
                <div class="message alert">
                    <span>
                        <asp:Literal ID="LTpendingChangesOnPermissions" runat="server"></asp:Literal>
                        <asp:LinkButton ID="LNBreloadPermissionsToItem" runat="server" Text="*Undo"></asp:LinkButton>
                        <asp:LinkButton ID="LNBapplyPermissionsToItem" runat="server" Text="*Apply"></asp:LinkButton>
                        <asp:LinkButton ID="LNBapplyPermissionsToContent" runat="server" Text="*Apply to content"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="tablewrapper">
                <table class="table minimal permissions fullwidth">
                    <thead>
                        <tr>
                            <th class="userrole"><asp:literal id="LTpermissionsRolePerson_t" runat="server">*Role/Person</asp:literal></th>
                            <th class="permission"><asp:literal id="LTpermissionsPermissions_t" runat="server">Permissions</asp:literal></th>
                            <th class="actions" id="THactions" runat="server" visible="false" >
                                <span class="icons"><span class="icon actions"></span></span>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="RPTpermissions" runat="server">
                            <ItemTemplate>
                                <tr class="<%#Container.DataItem.Type.ToString().ToLower() & " " & Replace(Container.DataItem.TranslatedPermissions.Type.ToString(), ",", " ")%>">
                                    <td class="userrole"><asp:Literal ID="LTdisplayName" runat="server" Text="<%#Container.DataItem.DisplayName%>"/></td>
                                    <td class="permission">
                                        <asp:Literal ID="LTidAssignment" runat="server" Text="<%#Container.DataItem.Id%>" Visible="false"></asp:Literal>
                                        <asp:Literal ID="LTassignmentType" runat="server" Text="<%#CInt(Container.DataItem.Type)%>" Visible="false"></asp:Literal>
                                        <asp:Literal ID="LTassignments" runat="server" Visible="false"></asp:Literal>
                                        <asp:Literal ID="LTinherited" runat="server" Text="<%#Container.DataItem.Inherited%>" Visible="false"></asp:Literal>
                                        <asp:Literal ID="LToldValue" runat="server" Text="<%#Container.DataItem.Denyed%>" Visible="false"></asp:Literal>
                                        <asp:Label ID="LBpermissions" runat="server" CssClass="readonly">
                                            <%#Container.DataItem.TranslatedPermissions.Value %>
                                        </asp:Label>
                                        <span class="checkboxlist"><span class="item"><asp:CheckBox ID="CBXallowAccess" runat="server" /></span></span>
                                    </td>
                                    <td class="actions" id="TDactions" runat="server" visible="false" >
                                        <span class="icons">
                                            <asp:Button ID="BTNdeleteItemPermission" runat="server" CommandName="delete" CommandArgument="<%#Container.DataItem.Id %>" CssClass="icon delete needconfirm" Visible="false" />
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="cisectionfooter"></div>
    </div>
</div>
<div class="dialog permissionrolesdlg" id="DVselectRoles" runat="server" visible="false">
    <div class="fieldobject fielddescription">
        <div class="fieldrow">
            <asp:Label ID="LBselectRolesDescription" runat="server" CssClass="description">*Seleziona a quali ruoli della comunità personalizzare i permessi:</asp:Label>
        </div>
    </div>
    <asp:CheckBoxList ID="CBLroles" runat="server" CssClass="checkboxlist" RepeatLayout="Flow" RepeatColumns="3" RepeatDirection="Horizontal">
    </asp:CheckBoxList>
    <div class="fieldobject clearfix">
        <div class="fieldrow right">
            <asp:HyperLink ID="HYPcloseAddRoleDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
            <asp:Button ID="BTNsaveRoleAssignments" runat="server" CssClass="Link_Menu" />
        </div>
    </div>
</div>
<div class="dialog permissionusersdlg" id="DVselectUsers" runat="server" visible="false">
        <CTRL:SelectUsers ID="CTRLselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
        DefaultPageSize="10" ShowSubscriptionsProfileTypeColumn="false" DefaultMaxPreviewItems="10"  MultipleSelection="true"
        ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false"/>
</div>
<div class="dialog dlgapplyfolder" id="DVaskForApply" runat="server" visible="false">
    <div class="fieldobject">
        <div class="fieldrow title">
            <div class="description">
                <asp:Label ID="LBfolderToApplyOptions" runat="server">*</asp:Label>
            </div>
        </div>
        <div class="fieldrow commandoptions clearfix">
            <div class="commandoption left">
                <asp:Button Text="* Applica solo alla cartella" runat="server" CssClass="commandbutton editoption1 closedlgeditoptions" ID="BTNapplyToFolder" />
                <asp:Label ID="LBapplyToFolder" runat="server" CssClass="commanddescription">* Applica solo alla cartella</asp:Label>
            </div>
            <div class="commandoption right">
                <asp:Button Text="* Applica anche al contenuto" runat="server" CssClass="commandbutton editoption2" ID="BTNapplyToFolderContent" />
                <asp:Label ID="LBapplyToFolderContent" runat="server" CssClass="commanddescription">* Applica alla cartella ed al suo contenuto</asp:Label>
            </div>
        </div>
    </div>
</div>