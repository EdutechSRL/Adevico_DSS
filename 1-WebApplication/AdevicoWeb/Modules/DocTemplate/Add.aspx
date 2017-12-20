<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Add.aspx.vb" ValidateRequest="false" Inherits="Comunita_OnLine.AddDocTemplate" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	
	<link href="../../Graphics/Modules/DocTemplate/css/certificates.css" rel="Stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">Certificate Management</asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="view">
		<div class="TemplateName">
			<span class="TemplateName">
				<asp:Label ID="LBLname_t" runat="server">#Template name:</asp:Label>
				<asp:TextBox ID="TXBname" runat="server" Columns="100"></asp:TextBox>
			</span>
		</div>
		<fieldset class="section">
			<legend>
				<asp:Literal ID="LTtype_t" runat="server">#Template Type</asp:Literal>
			</legend>
			<div class="sectiondescription">
				<asp:Label ID="LBLtypeDescription_t" runat="server">
					#Selezionare la tipologia di template.<br/>Non sar&agrave; pi&ugrave; possibile modificare la tipologia dopo la creazione.
				</asp:Label>
			</div>
			
			<div class="fieldrow">
				<asp:RadioButtonList ID="RBLtype" runat="server" RepeatLayout="Flow" CssClass="rbldl">
					<asp:ListItem Value="0" Selected="True">
						<dl>
							<dt>#Default Template></dt>
							<dd>#Template di default. Tutti i parametri saranno gestiti internamente. Sar&agrave; possibile importare tutti i dati da una skin, ma rimarranno indipendenti dalla stessa.</dd>
						</dl>
					</asp:ListItem>
					<asp:ListItem Value="1" Enabled="false">
						<dl>
							<dt>#System Template</dt>
							<dd>#Template di sistema, solo per <strong>template definitivi</strong>.<br/>#Non sar&agrave; possibile modificarli in futuro.</dd>
						</dl>
					</asp:ListItem>
					<asp:ListItem Value="2" Enabled="true">
						<dl>
							<dt>#Skin Template</dt>
							<dd>#Il template utilizza i date di una skin. Se questa viene modificata o cancellata, il templete subisce le variazioni o potrà essere non più valido.</dd>
						</dl>
					</asp:ListItem>
					<%--<asp:ListItem Value="3" Enabled="false">
						<dl><dt>#External Template</dt><dd>#Utilizza un pdf come base.</dd></dl>
					</asp:ListItem>--%>
				</asp:RadioButtonList>
	 
			</div>
		</fieldset>

		<fieldset class="section">
			<legend>
				<asp:Literal ID="LTservices_t" runat="server">Services</asp:Literal>
			</legend>
			<div class="sectiondescription">
				<asp:Label ID="LBLservicesDescription_t" runat="server">
					Selezionare i servizi associati.<br/>Sono in servizi per i quali sar&agrave; possibile inserire parametri specifici all'interno del corpo del tamplate. Saranno trasformati nei relativi dati del servizio nel momento dell'esportazione/creazione del template stesso.
				</asp:Label>
			</div>
			<div class="fieldrow">
				<asp:RadioButtonList ID="RBLservices" runat="server" RepeatLayout="Flow" AutoPostBack="true" CssClass="rbldl">
					<asp:ListItem Selected="True" Value="0">
						<dl>
							<dt>System</dt>
							<dd>Potranno essere inseriti esclusivamente tag comuni a livello di sistema.</dd>
						</dl>
					</asp:ListItem>
					<asp:ListItem  Value="1">
						<dl>
							<dt>Service</dt>
							<dd>Associando un template ad uno o pià servizi, questo sar&agrave; visibile solamente ai servizi selezionati, e sar&agrave; possibile utilizzare etichette specifiche del servizio stesso.</dd>
						</dl>
					</asp:ListItem>
				</asp:RadioButtonList>
				<div class="services">
					<asp:CheckBoxList runat="server" ID="CBLservices" RepeatLayout="Flow" RepeatColumns="50">
						<asp:ListItem Text="Service 1"></asp:ListItem>
						<asp:ListItem Text="Service 2"></asp:ListItem>
						<asp:ListItem Text="Service 3"></asp:ListItem>
					</asp:CheckBoxList>
				</div>
			</div>
		</fieldset>

		<div class="buttonwrapper big">
			<asp:LinkButton ID="LKBcrea" runat="server" class="linkMenu">*Crea Template</asp:LinkButton>
			<asp:HyperLink ID="HYPundo" runat="server" CssClass="linkMenu">*Undo</asp:HyperLink>
		</div>
	</div>
</asp:Content>
