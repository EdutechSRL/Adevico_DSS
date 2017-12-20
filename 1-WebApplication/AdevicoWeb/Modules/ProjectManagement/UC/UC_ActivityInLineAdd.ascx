<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActivityInLineAdd.ascx.vb"
    Inherits="Comunita_OnLine.UC_ActivityInLineAdd" %>
<span class="iconbuttons">
    <span class="buttonwrapper">
        <span class="icons floating">
            <span class="icon new"></span>
            <span class="listwrapper">
                <span class="arrow"></span>
                <asp:label id="LBgroupAddSummary" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
                <span class="group graph" runat="server" visible="false" id="SPNaddSummary">
                    <asp:LinkButton ID="LNBaddSummary1" runat="server" CssClass="item" CommandName="addsummary" CommandArgument="1" CausesValidation="false" OnClick="AddActivity_Click">1</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddSummary2" runat="server" CssClass="item" CommandName="addsummary" CommandArgument="2" CausesValidation="false" OnClick="AddActivity_Click">2</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddSummary3" runat="server" CssClass="item" CommandName="addsummary" CommandArgument="3" CausesValidation="false" OnClick="AddActivity_Click">3</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddSummary4" runat="server" CssClass="item" CommandName="addsummary" CommandArgument="4" CausesValidation="false" OnClick="AddActivity_Click">4</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddSummary5" runat="server" CssClass="item" CommandName="addsummary" CommandArgument="5" CausesValidation="false" OnClick="AddActivity_Click">5</asp:LinkButton>
                </span>
                <asp:Literal ID="LTaddLinkedActivitiesSeparator" runat="server" Visible="false"><hr /></asp:Literal>
                <asp:label id="LBgroupAddLinkedActivities" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
                <span class="group graph" runat="server" visible="false" id="SPNaddLinkedActivities">
                    <asp:LinkButton ID="LNBaddalinkedctivities1" runat="server" CssClass="item" CommandName="addlinked" CommandArgument="1" CausesValidation="false" OnClick="AddActivity_Click">1</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddalinkedctivities2" runat="server" CssClass="item" CommandName="addlinked" CommandArgument="2" CausesValidation="false" OnClick="AddActivity_Click">2</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddalinkedctivities3" runat="server" CssClass="item" CommandName="addlinked" CommandArgument="3" CausesValidation="false" OnClick="AddActivity_Click">3</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddalinkedctivities4" runat="server" CssClass="item" CommandName="addlinked" CommandArgument="4" CausesValidation="false" OnClick="AddActivity_Click">4</asp:LinkButton>
                    <asp:LinkButton ID="LNBaddalinkedctivities5" runat="server" CssClass="item" CommandName="addlinked" CommandArgument="5" CausesValidation="false" OnClick="AddActivity_Click">5</asp:LinkButton>
                </span>
                <asp:Literal ID="LTaddActivitiesSeparator" runat="server" Visible="false"><hr /></asp:Literal>
                <asp:label id="LBgroupAddActivities" runat="server" Visible="false" CssClass="grouptitle"></asp:label>
                <span class="group graph" runat="server" visible="false" id="SPNaddActivities">
                    <asp:LinkButton ID="LNKaddactivities1" runat="server" CssClass="item" CommandName="addactivities" CommandArgument="1" CausesValidation="false" OnClick="AddActivity_Click">1</asp:LinkButton>
                    <asp:LinkButton ID="LNKaddactivities2" runat="server" CssClass="item" CommandName="addactivities" CommandArgument="2" CausesValidation="false" OnClick="AddActivity_Click">2</asp:LinkButton>
                    <asp:LinkButton ID="LNKaddactivities3" runat="server" CssClass="item" CommandName="addactivities" CommandArgument="3" CausesValidation="false" OnClick="AddActivity_Click">3</asp:LinkButton>
                    <asp:LinkButton ID="LNKaddactivities4" runat="server" CssClass="item" CommandName="addactivities" CommandArgument="4" CausesValidation="false" OnClick="AddActivity_Click">4</asp:LinkButton>
                    <asp:LinkButton ID="LNKaddactivities5" runat="server" CssClass="item" CommandName="addactivities" CommandArgument="5" CausesValidation="false" OnClick="AddActivity_Click">5</asp:LinkButton>
                </span>
                <asp:Literal ID="LTaddActivityToMap" runat="server" Visible="false"><hr /></asp:Literal>
                <asp:LinkButton ID="LNBaddActivityToMap" runat="server" CssClass="item" CommandName="addactivity"  CommandArgument="1" Visible="false" CausesValidation="false" OnClick="AddActivity_Click"></asp:LinkButton>
            </span>
        </span>
    </span>
</span>
<asp:Literal ID="LTaddGroupTitle" runat="server" Visible="false"><strong>{n}</strong></asp:Literal>