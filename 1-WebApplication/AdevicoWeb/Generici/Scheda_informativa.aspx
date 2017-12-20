<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Scheda_Informativa.aspx.vb" Inherits="Comunita_OnLine.Scheda_Informativa" %>

<%--<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../uc/UC_Footer.ascx" %>
<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_Header.ascx" %>--%>
<%@ Register TagPrefix="DETTAGLI" TagName="CTRLDettagli" Src="../UC/UC_dettaglicomunita.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
	   <table cellspacing="0" cellpadding="0"  align="center" border="0" width="900px">
            <tr class="contenitore">
                <td colSpan="3">
                    <table cellSpacing="0" cellPadding="0" width="900px" border="0">
<%--	                    <tr>
		                    <td class="RigaTitolo" align="left">
			                    <asp:Label ID="LBTitolo" Runat="server"></asp:Label>
		                    </td>
	                    </tr>--%>
		                <tr>
		                    <td>
                                <DETTAGLI:CTRLDettagli id="CTRLDettagli" runat="server"></DETTAGLI:CTRLDettagli>
                            </td>                            
                        </tr>
                        <tr>
                            <td align="right" style="padding-top:5px; ">
                                <asp:Button ID="BTNindietro" Runat=server CssClass ="LINK_MENU" Visible =False Text =Indietro></asp:Button>
                            </td>       
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

</asp:Content>


<%--<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Comunità On Line - Scheda Informativa</title>
    <script language="Javascript" src="./../jscript/generali.js" type="text/javascript"></script>
    <link href="./../Styles.css" type="text/css" rel="stylesheet"/>
</head>
<body>
     <form id="form2" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
                 <tr class="contenitore">
                <td>
                    <div>
                        <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>	
                    </div>
                    <br style="clear:both;" />
                </td>
            </tr>


	    <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
</body>
</html>--%>