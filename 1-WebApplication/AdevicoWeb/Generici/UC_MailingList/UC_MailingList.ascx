<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MailingList.ascx.vb" Inherits="Comunita_OnLine.UC_MailingList" %>

<%@ Register Src="UC_GestioneListe.ascx" TagName="UC_GestioneListe" TagPrefix="uc1" %>
<%@ Register Src="UC_Lista.ascx" TagName="UC_Lista" TagPrefix="uc2" %>
<%@ Register Src="UC_Interni.ascx" TagName="UC_Interni" TagPrefix="uc3" %>

<table width="850px">
    <tr>
        <td>&nbsp;</td>
        <td align="right" >
            &nbsp;
           <asp:Button ID="Btn_BackLista" runat="server" CssClass="PulsanteFiltro" Text="Mostra liste" />
           <asp:Button ID="btn_Annulla" runat="server" CssClass="PulsanteFiltro" text="Annulla"/>
        </td>
    </tr><tr>
        <td colspan="2">
            <uc1:UC_GestioneListe id="UC_GestioneListe" runat="server"></uc1:UC_GestioneListe>
            <uc2:UC_Lista ID="UC_Lista" runat="server" />
            <uc3:UC_Interni id="UC_Interni1" runat="server"/>        
        </td>
    </tr>
</table>