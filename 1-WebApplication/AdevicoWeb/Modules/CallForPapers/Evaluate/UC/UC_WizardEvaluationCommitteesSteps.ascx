<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_WizardEvaluationCommitteesSteps.ascx.vb" Inherits="Comunita_OnLine.UC_WizardEvaluationCommitteesSteps" %>
<asp:Repeater ID="RPTsteps" runat="server">
    <HeaderTemplate>
        <!--@Start Navigation Items -->
        <ul class="navigation">   
    </HeaderTemplate>
    <ItemTemplate>
        <!--@Start Navigation Item (separator) -->
        <li class="<%#Container.DataItem.Css %>">
            <asp:Literal ID="LTseparator" runat="server" Visible="false">&nbsp;</asp:Literal>
            <div class="navigationstep" runat="server" id="DVnavigationStep">
                <div class="title"><asp:Label ID="LBitem" runat="server" Visible="false"></asp:Label><asp:HyperLink ID="HTPlink" runat="server" Visible="false"></asp:HyperLink><asp:LinkButton ID="LNBlink" runat="server" Visible="false"></asp:LinkButton></div>
                <div class="stepinfo clearfix">
					<span class="icons left">
			    		<span class="icon status">&nbsp;</span>
					</span>
                    <asp:Label ID="LBmessage" runat="server" CssClass="statustext" Text="<%#Container.DataItem.Message%>"></asp:Label>
                    <span class="icons right"></span>
                </div>
            </div>
            <div class="downarrow">&nbsp;</div>
        </li>
        <!--@End Navigation Item -->
    </ItemTemplate>
    <FooterTemplate>
        </ul>
        <!--@End Navigation Items -->
    </FooterTemplate>
</asp:Repeater>