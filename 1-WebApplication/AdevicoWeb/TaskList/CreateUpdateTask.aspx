<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateUpdateTask.aspx.vb" 
    Inherits="Comunita_OnLine.CreateUpdateTask" ValidateRequest="false" MasterPageFile="~/AjaxPortal.Master"%>


<%@ Register TagPrefix="CTRL" TagName="CTRLsearch" Src="../UC/UC_SearchCommunityByService.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AddAssignment" Src="UC/UC_AddTaskAssignment.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
<asp:Wizard ID="WZDprova"   runat="server">
    <WizardSteps >
        <asp:WizardStep id="WZSa" runat="server">
         noooo!!!!
        </asp:WizardStep>
        <asp:WizardStep id="WZSb" runat="server">
            <asp:Button ID="BTNa" runat="server"  Text="schiaccia" />
             <CTRL:AddAssignment ID="CTRLaddAss" runat="server" />
        </asp:WizardStep>
    </WizardSteps>
  
</asp:Wizard>

</asp:Content>