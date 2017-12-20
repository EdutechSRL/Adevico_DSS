<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Materiale_VisualizzaScorm.aspx.vb" Inherits="Comunita_OnLine.Materiale_VisualizzaScorm" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CtrLFooter" Src="../UC/UC_Footer.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="expires" content="0"/>
    <style>
        hr {
            margin:0em;
	        clear:both;
	        visibility:hidden;
        } 
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVcontenitore" align="center">
        <div id="DVtitle" style="width: 900px; text-align:left;" class="RigaTitolo" align="center">
			 <asp:Label ID="LBtitoloServizio" Runat="server">Servizio Bacheca</asp:Label>
		  </div>
		  <div id="DVmenu" align=right style="width: 900px;" runat="server">
			 <asp:linkbutton id="LNBindietro" Visible="true" Runat="server" Text="Indietro" CssClass="Link_Menu"></asp:linkbutton>
			 <asp:linkbutton id="LNBdownload" Visible="true" Runat="server" Text="Download" CssClass="Link_Menu"></asp:linkbutton>
		  </div>
		  <div style="width: 900px;" align="center">
			 <asp:MultiView ID="MLVcontenuto" runat="server" ActiveViewIndex=0>
				<asp:View ID="VIWscorm" runat="server">
				   <div id="DIVriga1">
					   <div style="width: 500px; float: left; text-align: left;">
						  <asp:TreeView ID="TRVpackageScorm" runat="server" nodeindent="10" ExpandDepth="1" ShowLines="True" >
							<LeafNodeStyle forecolor="#000099" />
							<ParentNodeStyle ForeColor="#000099" />
							<HoverNodeStyle ForeColor="#000099" Font-Bold="True" />
							<SelectedNodeStyle ForeColor="#640000" Font-Bold="True" />
							<NodeStyle Font-Size="X-Small" ForeColor="Black" />
							<RootNodeStyle ForeColor="#000099" Font-Bold="True" Height="25px" />
						  </asp:TreeView>
					  </div>
					   <div style="float: right; width: 400px;">
						 <asp:MultiView id="MLVmateriale" runat="server" ActiveViewIndex=0>
							<asp:View ID="VIWmateriale" runat="server">
								<asp:DataList ID="DTLmateriale" runat="server" Width="390px">
								    <HeaderStyle  BackColor="#000099" Font-Bold="True" ForeColor="#ffffff" Height="20"/>
								    <HeaderTemplate>
									   <asp:Label ID="LBtitoloElencoMateriale" runat="server">Elenco materiale</asp:Label>
								    </HeaderTemplate>
								    <ItemTemplate>
									   <asp:Label ID="LBnomeFile" Runat=server></asp:Label>
									    <asp:HyperLink Runat=server ID="HYPfile" CssClass="ROW_ItemLink_Small" Visible="False" Target="_blank"></asp:HyperLink>
									    <asp:Label ID="LBdimensione" Runat="server"></asp:Label>&nbsp;
								    </ItemTemplate>
							    </asp:DataList>
							</asp:View>
							<asp:View ID="VIWdettagli" runat="server">
							
							</asp:View>
						 </asp:MultiView>
					  </div>
					  <hr />
				   </div>
				</asp:View>
				<asp:View runat="server" ID="VIWpermessi">
				     <div id="DVpermessi" align="center" style=" margin: 50px 0 0 50px;">
					   <asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio">Errore nell'accesso alla pagina</asp:Label>
				    </div>
				</asp:View>
			 </asp:MultiView>
		  </div>
	   </div>
</asp:Content>


<%--
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Comunità On Line - Visualizzazione Pacchetto Scorm</title>
	
    <script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
    

</head>
<body>
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
     	<div id="DVheader" style="width: 900px;" align="center">
			 <HEADER:CtrLHEADER id="Intestazione" runat="server"></HEADER:CtrLHEADER>
		</div>

	   <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
    </form>
</body>
</html>
--%>