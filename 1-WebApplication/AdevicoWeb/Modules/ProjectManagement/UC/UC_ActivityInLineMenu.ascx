<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActivityInLineMenu.ascx.vb"
    Inherits="Comunita_OnLine.UC_ActivityInLineMenu" %>
<span class="floatingwrapper">
    <span class="icons floating">
        <asp:Label ID="LBeditActivityIcon" runat="server" cssclass="icon edit"></asp:Label>
        <span class="listwrapper">
            <span class="arrow"></span>
            <asp:LinkButton ID="LNBeditActivity" runat="server" Text="*Edit" CssClass="edit first" CommandName="edit" CausesValidation="false"></asp:LinkButton>
            <asp:Literal ID="LTindentSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:LinkButton ID="LNBindentActivityToLeft" runat="server" Text="*Indent Left" Visible="false" CommandName="indentleft" CausesValidation="false"></asp:LinkButton>
            <asp:LinkButton ID="LNBindentActivityToRight" runat="server" Text="*Indent Right" Visible="false" CommandName="indentright" CausesValidation="false"></asp:LinkButton>
            <asp:Literal ID="LTaddChildrenSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:label id="LBgroupAddChildren" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
            <span class="group graph" runat="server" visible="false" id="SPNaddChildren">
                <asp:LinkButton ID="LNBaddChildren1" runat="server" CssClass="item" CommandName="addchildren" CausesValidation="false" >1</asp:LinkButton>
                <asp:LinkButton ID="LNBaddChildren2" runat="server" CssClass="item" CommandName="addchildren" CausesValidation="false">2</asp:LinkButton>
                <asp:LinkButton ID="LNBaddChildren3" runat="server" CssClass="item" CommandName="addchildren" CausesValidation="false">3</asp:LinkButton>
                <asp:LinkButton ID="LNBaddChildren4" runat="server" CssClass="item" CommandName="addchildren" CausesValidation="false">4</asp:LinkButton>
                <asp:LinkButton ID="LNBaddChildren5" runat="server" CssClass="item" CommandName="addchildren" CausesValidation="false">5</asp:LinkButton>
            </span>
            <asp:Literal ID="LTaddLinkedChildrenSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:label id="LBgroupAddLinkedChildren" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
            <span class="group graph" runat="server" visible="false" id="SPNaddLinkedChildren">
                <asp:LinkButton ID="LNBaddLinkedChildren1" runat="server" CssClass="item" CommandName="addlinkedchildren" CausesValidation="false" >1</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedChildren2" runat="server" CssClass="item" CommandName="addlinkedchildren" CausesValidation="false">2</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedChildren3" runat="server" CssClass="item" CommandName="addlinkedchildren" CausesValidation="false">3</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedChildren4" runat="server" CssClass="item" CommandName="addlinkedchildren" CausesValidation="false">4</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedChildren5" runat="server" CssClass="item" CommandName="addlinkedchildren" CausesValidation="false">5</asp:LinkButton>
            </span>
            <asp:Literal ID="LTaddAfterSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:label id="LBgroupAddAfter" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
            <span class="group graph" runat="server" visible="false" id="SPNaddAfter">
                <asp:LinkButton ID="LNBaddAfter1" runat="server" CssClass="item" CommandName="addafter" CausesValidation="false">1</asp:LinkButton>
                <asp:LinkButton ID="LNBaddAfter2" runat="server" CssClass="item" CommandName="addafter" CausesValidation="false">2</asp:LinkButton>
                <asp:LinkButton ID="LNBaddAfter3" runat="server" CssClass="item" CommandName="addafter" CausesValidation="false">3</asp:LinkButton>
                <asp:LinkButton ID="LNBaddAfter4" runat="server" CssClass="item" CommandName="addafter" CausesValidation="false">4</asp:LinkButton>
                <asp:LinkButton ID="LNBaddAfter5" runat="server" CssClass="item" CommandName="addafter" CausesValidation="false">5</asp:LinkButton>
            </span>
            <asp:Literal ID="LTaddLinkedAfterSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:label id="LBgroupAddLinkedAfter" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
            <span class="group graph" runat="server" visible="false" id="SPNaddLinkedAfter">
                <asp:LinkButton ID="LNBaddLinkedAfter1" runat="server" CssClass="item" CommandName="addlinkedafter" CausesValidation="false">1</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedAfter2" runat="server" CssClass="item" CommandName="addlinkedafter" CausesValidation="false">2</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedAfter3" runat="server" CssClass="item" CommandName="addlinkedafter" CausesValidation="false">3</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedAfter4" runat="server" CssClass="item" CommandName="addlinkedafter" CausesValidation="false">4</asp:LinkButton>
                <asp:LinkButton ID="LNBaddLinkedAfter5" runat="server" CssClass="item" CommandName="addlinkedafter" CausesValidation="false">5</asp:LinkButton>
            </span>
            <asp:Literal ID="LTaddBeforeSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:label id="LBgroupAddBefore" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
            <span class="group graph" runat="server" visible="false" id="SPNaddBefore">
                <asp:LinkButton ID="LNBaddBefore1" runat="server" CssClass="item" CommandName="addbefore" CausesValidation="false">1</asp:LinkButton>
                <asp:LinkButton ID="LNBaddBefore2" runat="server" CssClass="item" CommandName="addbefore" CausesValidation="false">2</asp:LinkButton>
                <asp:LinkButton ID="LNBaddBefore3" runat="server" CssClass="item" CommandName="addbefore" CausesValidation="false">3</asp:LinkButton>
                <asp:LinkButton ID="LNBaddBefore4" runat="server" CssClass="item" CommandName="addbefore" CausesValidation="false">4</asp:LinkButton>
                <asp:LinkButton ID="LNBaddBefore5" runat="server" CssClass="item" CommandName="addbefore" CausesValidation="false">5</asp:LinkButton>
            </span>
            <asp:Literal ID="LTaddVirtualDeleteSeparator" runat="server" Visible="false"><hr /></asp:Literal>
            <asp:LinkButton ID="LNBvirtualDeleteActivity" runat="server" CssClass="delete needconfirm" CommandName="virtualdelete" Visible="false" CausesValidation="false"></asp:LinkButton>
            <asp:LinkButton ID="LNBvirtualDeleteActivityWithChildren" runat="server" CssClass="delete needconfirm" CommandName="virtualdelete" Visible="false" CausesValidation="false"></asp:LinkButton>
        </span>
    </span>
</span>
<asp:Literal ID="LTaddGroupTitle" runat="server" Visible="false"><strong>{n}</strong></asp:Literal>
