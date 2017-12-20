<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_OtherModuleItemFiles.ascx.vb"
    Inherits="Comunita_OnLine.UC_OtherModuleItemFiles" %>
<%@ Register TagPrefix="CTRL" TagName="FileToDisplay" Src="~/Modules/Repository/UC/UC_ModuleToRepositoryDisplay.ascx" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<asp:Repeater ID="RPTitemFiles" runat="server">
    <HeaderTemplate>
        <table width="100%" border="1" cellspacing="0" cellpadding="0">
            <tr class="ROW_header_Small_Center">
                <th runat="server" id="THaction">
                    <asp:Label ID="LBaction" runat="server">E</asp:Label>
                </th>
                <th runat="server" id="THpublish">
                    <asp:Label ID="LBpublish" runat="server">&nbsp;</asp:Label>
                </th>
                <th>
                    &nbsp;
                </th>
                <th colspan="3">
                    <asp:Label ID="LBproperty" runat="server">&nbsp;</asp:Label>
                </th>
                <th>
                    <asp:Label ID="LBaddedAt" runat="server">Added At</asp:Label>
                    <asp:Label ID="LBaddedBy" runat="server" Visible="false">Added By</asp:Label>
                </th>
                <th runat="server" id="THvisible">

                    <asp:Label ID="LBvisible" runat="server">V</asp:Label>
                </th>
                <th runat="server" id="THstatus" visible="false">
                    <asp:Label ID="LBstatus" runat="server">Status</asp:Label>
                </th>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="<%#BackGroundCss(Container.ItemType,Container.DataItem) %>">
            <td runat="server" id="TDaction">
                &nbsp;
                <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                    CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"></asp:LinkButton>
                <col:dialoglinkbutton runat="server" id="LNBdelete" visible="false" causesvalidation="false"
                    commandname="confirmDelete" cssclass="ROW_ItemLink_Small"></col:dialoglinkbutton>
                <asp:LinkButton ID="LNBunlink" runat="server" CommandName="unlink" CausesValidation="false"></asp:LinkButton>&nbsp;
            </td>
            <td runat="server" id="TDpublish">
                <asp:Literal ID="LTitemLinkID" runat="server" Visible="false"></asp:Literal>
                &nbsp;<asp:HyperLink ID="HYPpublishItem" runat="server" Target="_self"></asp:HyperLink>&nbsp;
            </td>
            <CTRL:FileToDisplay ID="FileDisplay" runat="server" DisplayAsTable="true" IconSize="Medium" />
            <td>
                <asp:Label ID="LBdata" runat="server"></asp:Label><br /><asp:Label ID="LBauthor" runat="server"></asp:Label>
            </td>
            <td runat="server" id="TDvisible">
                &nbsp;
                <col:dialoglinkbutton runat="server" id="LNBhide" visible="false" text="Edit" causesvalidation="false"
                    commandname="editvisibility" cssclass="ROW_ItemLink_Small"></col:dialoglinkbutton>
                &nbsp;
            </td>
            <td runat="server" id="TDstatus" visible="false">
                <asp:DropDownList ID="DDLstatus" runat="server" CssClass="Testo_CampoSmall">
                </asp:DropDownList>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<COL:Dialog runat="server" id="DLGmoduleFileItemVisibility" dialogclass="moduleItemVisibility"
    serversidecancel="false"></COL:Dialog>
<COL:Dialog runat="server" id="DLGrepositoryFileItemVisibility" dialogclass="repositoryItemVisibility"
    serversidecancel="false"></COL:Dialog>
<COL:Dialog runat="server" id="DLGremoveFile" dialogclass="mandatoryDial" serversidecancel="false"></COL:Dialog>
