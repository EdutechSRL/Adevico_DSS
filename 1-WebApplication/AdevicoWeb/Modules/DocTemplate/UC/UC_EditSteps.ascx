<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditSteps.ascx.vb" Inherits="Comunita_OnLine.UC_EditSteps" %>

<!-- LIKE: ~/Modules/CallForPapers/UC/UC_WizardSteps.ascx -->
<div class="column left persist-header copyThis _floatingHeader">
    <asp:Repeater ID="RPTsteps" runat="server">
        <HeaderTemplate>
            <ul class="navigation">   
        </HeaderTemplate>
        <ItemTemplate>
            <li class="<%# Container.DataItem.css %>">
                <div class="navigationstep" runat="server" id="DVnavigationStep">
                    <div class="title">
                        <asp:Label ID="LBLitem" runat="server" Visible="false"></asp:Label>
                        <asp:LinkButton ID="LNBlink" runat="server" Visible="false"></asp:LinkButton>
                    </div>
                    <div class="stepinfo clearfix">
					    <span class="icons left">
			    		    <span class="icon status" id="SPNstatus" runat="server">&nbsp;</span>
					    </span>
                        <asp:Label ID="LBLmessage" runat="server" CssClass="statustext"></asp:Label>
                        <span class="icons right"></span>
                    </div>
                </div>
                <div class="downarrow">&nbsp;</div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>