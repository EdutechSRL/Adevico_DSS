<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddLinks.ascx.vb" Inherits="Comunita_OnLine.UC_AddLinks" %>
<%@ Register TagPrefix="CTRL" TagName="Selector" Src="~/Modules/Repository/UC_New/UC_FolderSelector.ascx" %>
<div class="dialog createnewlink" title="<%=AddLinksDialogTitle()%>">
    <div class="fieldobject upload">
        <div class="fieldrow title">
            <div class="description">
                <asp:Literal ID="LTaddLinksDescription" runat="server" Text="*"></asp:Literal>
            </div>
        </div>
        <div class="fieldrow community" id="DVcommunity" runat="server" visible="false">
            <asp:Label id="LBaddLinksToCommunity_t" runat="server" Text="*Community:" CssClass="fieldlabel" AssociatedControlID="LBaddLinksToCommunity"></asp:Label>
            <asp:Label id="LBaddLinksToCommunity" runat="server" Text="*Community:" CssClass="text"></asp:Label>
        </div>
        <div class="fieldrow path" id="DVcurrentPath" runat="server">
            <asp:Label ID="LBcurrentPath_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBcurrentPath" Text="*Current Path"></asp:Label>
            <asp:Label ID="LBcurrentPath" runat="server" CssClass="text" ></asp:Label>
        </div>
        <div class="fieldrow path" id="DVselectFolder" runat="server">
            <asp:Label ID="LBselectDestinationFolderPath" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLfolderSelector" Text="*Current Path"></asp:Label>
            <CTRL:Selector ID="CTRLfolderSelector" runat="server" /> 
        </div>
    </div>
    <div class="fieldobject upload">
         <asp:Repeater id="RPTitems" runat="server">
            <ItemTemplate>
                <div class="fieldgroup<%#GetCssClass(Container.ItemIndex ) %>">
                    <div class="fieldrow newlinkinput url">
                       <asp:Label ID="LBnewUrl_t" runat="server" AssociatedControlID="TXBurl" CssClass="fieldlabel">*Url:</asp:Label>
                        <asp:TextBox ID="TXBurl" runat="server" MaxLength="2000" CssClass="inputtext" ></asp:TextBox>
                    </div>
                    <div class="fieldrow newlinkinput name">
                        <asp:Label ID="LBnewUrlName_t" runat="server" AssociatedControlID="TXBurlName" CssClass="fieldlabel">Display name:</asp:Label>
                        <asp:TextBox ID="TXBurlName" runat="server" MaxLength="2000" CssClass="inputtext" ></asp:TextBox>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="fieldobject upload">
        <div class="fieldrow">
            <asp:Label ID="LBhideLinks_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXhideLinks" Text="*Hide links:"></asp:Label>
            <div class="inlinewrapper">
                <span class="inputgroup">
                    <asp:CheckBox ID="CBXhideLinks" runat="server" /><asp:Label ID="LBhideLinks" runat="server" AssociatedControlID="CBXhideLinks">*hide links</asp:Label>
                </span>
            </div>
        </div>
    </div>
    <div class="fieldobject upload">
        <div class="fieldrow clearfix commands">
            <div class="left">&nbsp;</div>
            <div class="right">
                <asp:HyperLink ID="HYPcloseAddLinkDialog" runat="server" CssClass="linkMenu close" Text="*Close"></asp:HyperLink>
                <asp:Button ID="BTNaddLinks" runat="server" CssClass="linkMenu" Text="*Add"  CausesValidation="false" />
            </div>
        </div>
    </div>
</div>