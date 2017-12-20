<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_NoticeboardBlock.ascx.vb" Inherits="Comunita_OnLine.UC_NoticeboardBlock" %>
<div class="asidecontent grid_<%=GetItemColspan()%>">
    <div class="homeboard clearfix">
        <div class="sectionheader clearfix">
            <div class="icon comtype_48 noticeboard"></div>
            <h3 class="sectiontitle clearifx"><asp:Literal ID="LTnoticeboardTitle" runat="server">*Noticeboard</asp:Literal></h3>
            <span class="icons">
                <asp:HyperLink ID="HYPnoticeboard" runat="server" CssClass="noticeboard"></asp:HyperLink>
            </span>
        </div>
        <div class="innerwrapper">
            <asp:Literal ID="LTrenderNoticeboard" runat="server"></asp:Literal>
        </div>
        <div class="viewbuttons bottom">
            <asp:HyperLink ID ="HYPmanageNoticeboard" runat="server" CssClass="linkMenu" Visible="false">*Manage</asp:HyperLink>
            <asp:HyperLink ID ="HYPprintNoticeboard" runat="server" CssClass="linkMenu print" Visible="false">*Print</asp:HyperLink>
        </div>
    </div>
</div>
<asp:Literal ID="LTexpandNoticeboardTemplate" Visible="false" runat="server"><span class="icon expand" title="{0}">{0}</span></asp:Literal>
<asp:Literal ID="LTrenderNoticeboardTemplate" Visible="false" runat="server"><iframe class="noticeboardiframe" src="{0}" width="100%" height="450px" frameborder="0"></iframe></asp:Literal>