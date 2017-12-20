<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="GenericMailSender.aspx.vb" Inherits="Comunita_OnLine.GenericMailSender" Theme="Materiale" MaintainScrollPositionOnPostback="true"%>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>--%>
<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">

		  <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
			 <asp:Label ID="LBtitoloServizio" Runat="server">Servizio Mail</asp:Label>
		  </div>
		  <div align="center">
			 <asp:Wizard runat="server" id="WZRmail" ActiveStepIndex="0" DisplaySideBar="False" Width="900px" StepStyle-HorizontalAlign="Center">
				<WizardSteps>
				    <asp:WizardStep ID="WZScommunity" runat="server" StepType="Start" 
					   Title="Scelta delle comunità">
					   <br /><CTRL:CTRLsearch  id="CTRLsearchCommunity" runat="server" SelectionMode="Multiple" AllowMultipleOrganizationSelection="true"  AllowCommunityChangedEvent="false"/><br />
				    </asp:WizardStep>
				    <asp:WizardStep ID="WZSaddresses" runat="server" StepType="Step" 
					   Title="Scelta dei destinatari">
				    </asp:WizardStep>
				</WizardSteps>
				 <StepStyle HorizontalAlign="Center" />
				 <StartNavigationTemplate>
						  <asp:Button ID="BTNgoToDetailsView" runat="server" CommandName="Cancel" 
							 Text="Torna alla visualizzazione" />
						  <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" 
							 Text="Avanti" />
					   </StartNavigationTemplate>
					   <StepNavigationTemplate>
						  <asp:Button ID="BTNgoToDetailsView" runat="server" CommandName="Cancel" 
							 Text="Torna alla visualizzazione" />
						  <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" 
							 CommandName="MovePrevious" Text="Precedente" />
						  <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" 
							 Text="Avanti" />
					   </StepNavigationTemplate>
					   <FinishNavigationTemplate>
						  <asp:Button ID="BTNgoToDetailsView" runat="server" CommandName="Cancel" 
							 Text="Torna alla visualizzazione" />
						  <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" 
							 CommandName="MovePrevious" Text="Precedente" />
						  <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" 
							 Text="Fine" />
					   </FinishNavigationTemplate>
			 </asp:Wizard>
			 <asp:MultiView ID="MLVmenu" runat="server">
			 </asp:MultiView>
			 <asp:MultiView ID="MLVbacheca" runat="server">
			 </asp:MultiView>
		  </div>


</asp:Content>



<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server">Comunità On Line - Bacheca</title>
     <LINK href="./../styles.css" type="text/css" rel="stylesheet"/>
</head>
<body>
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	     <div id="DVcontenitore" align="center" >
		  <div id="DVheader" style="width: 900px;" align="center">
			 <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
		  </div>

	   </div>
	   <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
</body>
</html>--%>
