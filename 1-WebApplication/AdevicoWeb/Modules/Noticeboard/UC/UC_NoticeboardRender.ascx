<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_NoticeboardRender.ascx.vb" Inherits="Comunita_OnLine.UC_NoticeboardRender" %>
<div class="noticeboardwrapper content clearfix <%=GetHistoryCssClass()%> <%=GetManagerCssClass()%>">
    <div class="column left noticeboard" id="DVnoticeboard" runat="server">
        <div class="header">
            <h2 class="title"><asp:Literal ID="LTnoticeboardTitle" runat="server">*Noticeboard</asp:Literal>:</h2>
        </div>
        <div class="content">
             <asp:Literal ID="LTrenderNoticeboard" runat="server"></asp:Literal>
        </div>
        <div class="footer clearfix" runat="server" id="DVfooterCommands" visible="false">
            <div class="left">
                &nbsp;
            </div>
            <div class="right">
                <asp:HyperLink ID ="HYPeditNoticeboard" runat="server" CssClass="linkMenu command" Visible="false">*Edit</asp:HyperLink>
                <asp:Label ID="LBseparator" runat="server" CssClass="sep">|</asp:Label>
                <asp:HyperLink ID ="HYPprintNoticeboard" runat="server" CssClass="linkMenu command print" Visible="false">*Print</asp:HyperLink>
            </div>
        </div>
    </div>
    <div class="column left history" id="DVcolumnHistory" runat="server" visible="false">
        <div class="header">
            <h2 class="title"><asp:Literal ID="LTnoticeboardHistoryTitle" runat="server">*History</asp:Literal>:</h2>
        </div>
        <div class="content">
            <ul class="historyitems">
                <li class="historyitem previous" runat="server" id="LIpreviousItems" visible="false">
                    <span class="icons">
                        <asp:LinkButton ID="LNBiconPreviousMessages" runat="server"></asp:LinkButton>
                    </span>
                    <asp:LinkButton ID="LNBpreviousMessages" runat="server"></asp:LinkButton>
                </li>
                <asp:Repeater ID="RPThistoryMessages" runat="server">
                    <ItemTemplate>
                        <li class="historyitem "  runat="server" id="LIitem">
                            <asp:LinkButton ID="LNBviewMessage" runat="server" CommandName="viewmessage" CommandArgument='<%#Container.DataItem.Id%>'></asp:LinkButton>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <li class="historyitem next" runat="server" id="LInextItems" visible="false">
                    <span class="icons">
                        <asp:LinkButton ID="LNBiconNextMessages" runat="server"></asp:LinkButton>
                    </span>
                    <asp:LinkButton ID="LNBnextMessages" runat="server"></asp:LinkButton>
                </li>
            </ul>
        </div>
        <div class="footer">&nbsp;</div>
    </div>
</div>
<asp:Literal ID="LTcssClassActive" Visible="false" runat="server">active</asp:Literal>
<asp:Literal ID="LTcssClassDeleted" Visible="false" runat="server">deleted</asp:Literal>
<asp:Literal ID="LTcssClassManager" runat="server" Visible="false">manager</asp:Literal>
<asp:Literal ID="LTcssClassHasHistory" runat="server" Visible="false">hashistory</asp:Literal>
<asp:Literal ID="LTcssClassNoHistory" runat="server" Visible="false">nohistory</asp:Literal>

<asp:Literal ID="LTnextIconTemplate" runat="server" Visible="false"><span class="icon arrowdown"></span></asp:Literal>
<asp:Literal ID="LTpreviousIconTemplate" runat="server" Visible="false"><span class="icon arrowup"></span></asp:Literal>
<asp:Literal ID="LTtextTemplate" runat="server" Visible="false"><span class="text">{0}</span></asp:Literal>
<asp:Literal ID="LTitemTemplate" runat="server" Visible="false"><span class="date">{1}</span><span class="by">{0}</span></asp:Literal>
<asp:Literal ID="LTrenderNoticeboardTemplate" Visible="false" runat="server"><iframe class="noticeboardiframe" src="{0}" width="100%" height="450px" frameborder="0"></iframe></asp:Literal>