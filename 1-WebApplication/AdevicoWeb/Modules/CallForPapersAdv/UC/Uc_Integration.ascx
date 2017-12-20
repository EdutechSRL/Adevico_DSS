<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Uc_Integration.ascx.vb" Inherits="Comunita_OnLine.Uc_Integration" %>


<%--<%@ Register TagPrefix="CTRL" TagName="FileUploader" Src="~/Modules/Repository/UC/UC_CompactInternalFileUploader.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="FileUploader" Src="~/Modules/Repository/UC/UC_CompactInternalFileUploader2.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>

<asp:HiddenField ID="HDNSubmissionId" runat="server" />
<asp:HiddenField ID="HDNSubmissionFieldId" runat="server" />
<asp:HiddenField ID="HDNCommissionId" runat="server" />
<asp:HiddenField ID="HDNSecretaryId" runat="server" />
<asp:HiddenField ID="HDNSubmitterId" runat="server" />

<asp:Panel ID="PNLAdd" runat="server">
 <div class="box-msg-secretary box-msg-secretary-add">
	<span class="rowcontainer">
		<asp:Label ID="LBLaddtext_t" runat="server">Testo richiesta:</asp:Label>
		<asp:TextBox ID="TXBaddtext" runat="server" TextMode="MultiLine"></asp:TextBox>
	</span>
	 
	<span class="rowcontainer">
		<asp:Label ID="LBLaddtype_t" runat="server">Tipo integrazione</asp:Label>
		<span class="RBLcontainer" style="margin-bottom: 8px;display: block;">
			<asp:RadioButtonList ID="RBLtype" runat="server" CssClass="box-Radio-btnList" RepeatLayout="Flow">
				<asp:ListItem Text="Solo testo" Value="text" Selected="True"></asp:ListItem>
				<asp:ListItem Text="Testo e file" Value="textfile"></asp:ListItem>
			</asp:RadioButtonList>
		</span>
	</span>
	 
	<span class="rowcontainer">
		<asp:Label ID="LBLaddreqsended_t" runat="server">Inviata: </asp:Label>
		<asp:Label ID="LBLaddreqsendedOn" runat="server">no</asp:Label>
	</span>
	<span class="buttonContainer" runat="server">
		<asp:LinkButton ID="LKBaddsave" runat="server" CssClass="linkMenu">Salva bozza</asp:LinkButton>
		<asp:LinkButton ID="LKBaddsend" runat="server" CssClass="linkMenu">Invia</asp:LinkButton>
	</span>
</div>
</asp:Panel>

<asp:Repeater ID="RPTintegrations" runat="server">
	<ItemTemplate>

		<asp:HiddenField ID="HDNintegrationId" runat="server" />
	
		<div class="box-msg-secretary box-msg-secretary-edit">
			<span class="rowcontainer">
				<asp:Label ID="LBLtext_t" runat="server">Testo richiesta:</asp:Label>
				<asp:TextBox ID="TXBrequest" runat="server" CssClass="request" TextMode="MultiLine"></asp:TextBox>
				<asp:Label ID="LBLrequest" runat="server" CssClass="request">Testo risposta:</asp:Label>
			</span>
			<span class="rowcontainer">
				<asp:Label ID="LBLtype_t" runat="server">Tipo integrazione:</asp:Label>
				<asp:RadioButtonList ID="RBLtype" runat="server" Enabled="false">
					<asp:ListItem Text="Solo testo" Value="text" Selected="True"></asp:ListItem>
					<asp:ListItem Text="Testo e file" Value="textfile"></asp:ListItem>
				</asp:RadioButtonList>
			</span>
			<span class="rowcontainer">
				<asp:Label ID="LBLreqsended_t" runat="server">Inviata: </asp:Label>
				<asp:Label ID="LBLreqsendedOn" runat="server">no</asp:Label>
			</span>
			<span class="buttonContainer" runat="server">
				<asp:LinkButton ID="LKBsave" runat="server" CssClass="linkMenu">Salva bozza</asp:LinkButton>
				<asp:LinkButton ID="LKBsend" runat="server" CssClass="linkMenu">Invia</asp:LinkButton>
			</span>
		</div>

		<asp:Panel ID="PNLsubmitter" runat="server" CssClass="box-msg-submitter">
			<span class="rowcontainer">
				<asp:Label ID="LBLanswer_t" runat="server" style="vertical-align: top;">Testo risposta:</asp:Label>
				<asp:TextBox ID="TXBanswer" runat="server" CssClass="answer" TextMode="MultiLine"></asp:TextBox>
				<asp:Label ID="LBLanswer" runat="server" CssClass="answer">Testo risposta:</asp:Label>
			</span>
		 
			<asp:Panel ID="PNLfile" runat="server" CssClass="rowcontainer">
				<CTRL:FileUploader ID="CTRLfileUploader" runat="server" ViewTypeSelector="false" />
				<%--<asp:linkbutton ID="LKBupload" runat="server" CssClass="linkMenu">Carica</asp:linkbutton>--%>

				<CTRL:DisplayFile ID="CTRLdisplayFile" runat="server"/>
			</asp:Panel>
			
			<span class="rowcontainer">
				<asp:Label ID="LBLansSended_t" runat="server">Inviata: </asp:Label>
				<asp:Label ID="LBLansSended" runat="server">no</asp:Label>
			</span>
			<span class="buttonContainer" runat="server">
				<asp:LinkButton ID="LKBansSave" runat="server" CssClass="linkMenu">Salva bozza</asp:LinkButton>
				<asp:LinkButton ID="LKBansSend" runat="server" CssClass="linkMenu">Invia</asp:LinkButton>
			</span>
		</asp:Panel>
	</ItemTemplate>

</asp:Repeater>
