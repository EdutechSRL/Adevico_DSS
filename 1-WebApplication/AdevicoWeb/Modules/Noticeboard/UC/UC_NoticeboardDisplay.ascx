<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_NoticeboardDisplay.ascx.vb" Inherits="Comunita_OnLine.UC_NoticeboardDisplay" %>
<div style="width: <%=ContenitoreWidth %>; margin-top: 5px;"  >
    <div style="width: <%=ContenitoreWidth %>; height: 35px; line-height: 35px; color: #000099; text-align: right; font-weight: bold; padding: 10px 0px 0px 0px;" runat="server" id="DIVmenu">
        <asp:Button ID="BTNsetActive" runat="server" Text="Set active" CommandName="setactive" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
        <asp:HyperLink ID="HYPeditADV" runat="server" Text="Edit Advanced" CssClass="linkMenu"></asp:HyperLink>
        <asp:HyperLink ID="HYPeditHTML" runat="server" Text="Edit HTML" CssClass="linkMenu"></asp:HyperLink>
	    <asp:Button ID="BTNclear" runat="server" Text="Clear Whiteboard" CommandName="ClearNoticeBoard" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
	    <asp:Button ID="BTNvirtualDelete" runat="server" Text="Delete" CommandName="virtualdelete" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
        <asp:Button ID="BTNvirtualUndelete" runat="server" Text="Undelete" CommandName="virtualundelete" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
        <asp:Button ID="BTNvirtualUndeleteAndActivate" runat="server" Text="Undelete and set active" CommandName="virtualundeleteactivate" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
        <asp:Button ID="BTNdelete" runat="server" Text="Remove from basket" CommandName="delete" OnClick="BTNmanage_Click" Visible="false"></asp:Button>
        <asp:Button ID="BTNstampa" runat="server" Text="Print" CommandName="Print" Visible="false"></asp:Button>
    </div>
    <div style="width: <%=ContenitoreWidth %>; padding: 0px 0px 0px 0px;" runat="server" id="DIVmenuDashboard">
        <div class="fakeFieldsetNoticeboard">
            <span class="left"><asp:Literal ID="LTdashBoard" runat="server">Bacheca Portale</asp:Literal></span>
            <span class="right">
                <asp:HyperLink ID="HYPmoreMessagesDS" runat="server" Text="Management" CssClass="linkMenu" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPeditDS" runat="server" Text="Management" CssClass="linkMenu" Visible="false"></asp:HyperLink>
                <asp:HyperLink ID="HYPstampaDS" runat="server" Text="Print" CssClass="linkMenu" Visible="false" Target="_blank"></asp:HyperLink>
            </span>
            <hr/> 
        </div>
    </div>
    <div style="width: <%=ContenitoreWidth %>; height: 35px; line-height: 35px; color: #000099; text-align: left; font-weight: bold; padding: 0px 0px 0px 0px;" runat="server" id="DVinfo">
        <b>
            <asp:Literal ID="LTinfoOpen" runat="server"></asp:Literal>&nbsp;
            <span><asp:Literal ID="LTinfoByUser" runat="server"></asp:Literal></span>&nbsp;
            <span><asp:Literal ID="LTinfoOnDate" runat="server"></asp:Literal></span>
        </b>
    </div>
    <div style="width: <%=ContenitoreBachecaWidth %>; border: 1px solid #333; height: <%=ContenitoreBachecaHeight%>; color: #000099; text-align: left; font-weight: bold; margin:0 0 0 0; padding: 0px 0px 0px 0px; float: left;">
        <asp:Literal ID="LTRBacheca" runat="Server" Text="" visible="true"/>
    </div>
 </div>