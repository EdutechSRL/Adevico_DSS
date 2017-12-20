<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ItemMenu.ascx.vb" Inherits="Comunita_OnLine.UC_ItemMenu" %>
<asp:MultiView ID="MLVmenu" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server"></asp:View>
    <asp:View ID="VIWmenu" runat="server">
        <div class="groupedselector nolabel noactive" >
            <div class="selectorgroup">
                <span class="selectorlabel">&nbsp;</span>
                <span class="selectoricon actions">&nbsp;</span>
            </div>
            <div class="selectormenu">
                <div class="selectorinner">
                    <div class="selectoritems">
                        <asp:Repeater ID="RPTactions" runat="server">
                            <ItemTemplate>
                                <div class="selectoritem" id="DVitemAction" runat="server">
                                    <asp:Literal ID="LTpreviewItemAction" runat="server" Visible="false"></asp:Literal>
                                    <asp:LinkButton ID="LNBaction" CommandName="" runat="server" Visible="false" CausesValidation="false"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton> 
                                    <asp:Hyperlink ID="HYPaction" runat="server" Visible="false"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:Hyperlink> 
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView><asp:Literal ID="LTpreviewImages" runat="server" Visible="false"><a href="#imageurl#" rel="image-preview" class="lightbox image"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">#text#</span></a></asp:Literal><asp:Literal ID="LTcssClassItem" runat="server" Visible="false">selectoritem</asp:Literal><asp:Literal ID="LTclientScriptmove" runat="server" Visible="false">return onMoveUpdating();</asp:Literal><asp:Literal ID="LTmodalCssClass" runat="server" Visible="false">openmodal </asp:Literal>
