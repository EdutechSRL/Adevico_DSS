<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_NoticeboardTile.ascx.vb" Inherits="Comunita_OnLine.UC_NoticeboardTile" %>
<div id="tile-0" class="tile <%=GetItemCssClass %> noticeboard">
    <div class="innerwrapper">
        <div class="tileheader clearfix">
            <div class="left">
                <h3><asp:Literal ID="LTnoticeboardTitle" runat="server">*Noticeboard</asp:Literal></h3>
                <span class=" icons">
                    <asp:HyperLink ID="HYPnoticeboard" runat="server" CssClass="noticeboard"></asp:HyperLink>
                </span>
            </div>
            <div class="right">
            </div>
        </div>
        <div class="tilecontent clearfix">
            <div class="noticeboardthumb">
                <asp:HyperLink ID="HYPnoticeboardImage" runat="server" CssClass="noticeboard">
                    <asp:Image ID="IMBthumbnail"  runat="server" CssClass="thumb" />
                </asp:HyperLink>
            </div>
        </div>
        <div class="tilefooter">
            <div class="left">
                <div class="info">
                    <div class="message" id="DVmessage" runat="server"><asp:literal id="LTmessage" runat="server"></asp:literal></div>
                </div>
            </div>
            <div class="right"></div>
        </div>
    </div>
</div>
<asp:Literal ID="LTexpandNoticeboardTemplate" Visible="false" runat="server"><span class="icon expand" title="{0}">{0}</span></asp:Literal>
<asp:Literal ID="LTdefaultImg" Visible="false" runat="server">Graphics/Modules/Dashboard/Img/noticeboard_thumb_1.png</asp:Literal>
<asp:Literal ID="LTrecentCssClass" Visible="false" runat="server">isrecent</asp:Literal>
<asp:Literal ID="LTisNewCssClass" Visible="false" runat="server">isnew</asp:Literal>
<asp:Literal ID="LTmessageInfoCssClass" Visible="false" runat="server">message</asp:Literal>