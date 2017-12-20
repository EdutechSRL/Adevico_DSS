<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AssignerSelector.ascx.vb" Inherits="Comunita_OnLine.UC_AssignerSelector" %>
<%--Nomi Standard: OK--%>
<asp:Repeater ID="RPTtkUsers" runat="server">
	<HeaderTemplate>
		<table class="table light users fullwidth">
			<thead>
				<tr>
					<th class="select">
						<span class="headerselector leftside">
							
						</span>
					</th>
										
					<th class="lastname">
						<asp:Label ID="LBsname_t" runat="server">*Cognome</asp:Label>
					</th>
					<th class="firstname">
						<asp:Label ID="LBname_t" runat="server">*Nome</asp:Label>
					</th>
					<th class="usermail">
						<asp:Label ID="LBmail_t" runat="server">*Mail</asp:Label>
					</th>
				</tr>
			</thead>
			<tbody>     
	</HeaderTemplate>
	<ItemTemplate>
				<tr>
					<td class="select">
						<asp:LinkButton ID="LNBselect" runat="server" CssClass="submitterselect">*Select</asp:LinkButton>
					</td>
					<td class="lastname"><asp:Literal ID="LTsname" runat="server">#sname</asp:Literal></td>
					<td class="firstname"><asp:Literal ID="LTname" runat="server">#name</asp:Literal></td>
					<td class="usermail"><asp:Literal ID="LTmail" runat="server">#mail</asp:Literal></td>
				</tr>
	</ItemTemplate>	
	<FooterTemplate>
					<asp:PlaceHolder ID="PLHfooterVoid" runat="server">
						<tr class="empty norecordrow">
							<td colspan="4" data-colspan="0">
								<asp:Literal ID="LTempty" runat="server">Nessun manager/resolver diponibile nella comunità corrente.</asp:Literal>
							</td>
						</tr>
				</asp:PlaceHolder>
			</tbody> 
		</table>
	</FooterTemplate>
	
</asp:Repeater>

	<div class="fieldobject commands" id="DVcommands" runat="server">
		<div class="fieldrow buttons right">
			<asp:LinkButton id="LNBcloseAssignersWindow" runat="server" CssClass="linkMenu close">#close</asp:LinkButton>
		</div>
	</div>