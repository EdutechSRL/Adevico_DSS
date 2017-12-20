<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_CommunityTreeMenu.ascx.vb" Inherits="Comunita_OnLine.UC_CommunityTreeMenu" %>
 <div class="groupedselector nolabel noactive" id="DBmenu" runat="server" visible="false">
    <!--<span class="description">description</span>-->
    <div class="selectorgroup">
        <span class="selectorlabel"></span>
        <span class="selectoricon">&nbsp;</span>
    </div>
    <div class="selectormenu">
        <div class="selectorinner">
            <div class="selectoritems">
                <div class="selectoritem first active">
                    <asp:hyperlink id="HYPnodeDetails" runat="server" Target="_blank"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:hyperlink>
                </div>
                <div class="selectoritem" id="DVnews" runat="server" visible="false">
                    <asp:hyperlink id="HYPnodeViewNews" runat="server"><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:hyperlink>
                </div>
                
                <div class="selectoritem" id="DVaccessTo" runat="server" visible="false">
                    <asp:LinkButton ID="LNBnodeAccessTo" runat="server" CausesValidation="false"  CommandName="access" ><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton>
                </div>
                <div class="selectoritem last" id="DVenrollTo" runat="server" visible="false">
                    <asp:LinkButton ID="LNBnodeEnrollTo" runat="server" CommandName="enroll" CausesValidation="false" ><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton>
                </div> 
                <div class="selectoritem last" id="DVunsubscribeFrom" runat="server" visible="false">
                   <asp:LinkButton ID="LNBnodeUnsubscribeFrom" runat="server" Text="*Unsubscribe" CausesValidation="false" CommandName="unsubscribe" CssClass="icon delete needconfirm-unsubscribefromcommunity" ><span class="icon activeicon">&nbsp;</span><span class="selectorlabel">{0}</span></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</div>