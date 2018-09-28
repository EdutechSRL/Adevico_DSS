<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DialogProjectAttachments.ascx.vb" Inherits="Comunita_OnLine.UC_DialogProjectAttachments" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayUrl" Src="~/Modules/Common/UC/UC_DisplayUrlItem.ascx" %>
<div class="dialog<%=CssClass%>" title="<%=GetDialogTitle() %>">
    <div class="fieldobject maincontent attachments">
        <div class="fieldrow group project first last">
            <div class="groupcontent">
                <ul class="filelist">
            <asp:Repeater ID="RPTattachments" runat="server">
                <ItemTemplate>
                    <li>
                        <span class="objectRender file">
                        <CTRL:DisplayUrl ID="CTRLdisplayUrl" runat="server" Visible="false" />
                        <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="false" DisplayExtraInfo="true" DisplayLinkedBy="true" Visible="false" />
                        </span>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>
    <asp:HyperLink ID="HYPallattachments" runat="server" CssClass="linkMenu close" Visible="false" Text="*All Attachments"></asp:HyperLink>
</div><asp:Literal ID="LTdefaultCssClass" runat="server" Text="dlgprojectattachments" Visible="false"></asp:Literal>
