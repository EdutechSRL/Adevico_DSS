<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Cover.aspx.vb" Inherits="Comunita_OnLine.Cover" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLgestioneCover" Src="./UC/UC_DatiCover.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="Javascript" src="../jscript/generali.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
		  <div id="DVmenu" align=right style="width: 900px;" runat="server">
			 <asp:linkbutton id="LNBgestione" Runat="server" Text="Gestione Cover" CausesValidation=False  CssClass="LINK_MENU"  ></asp:linkbutton>
			 <asp:linkbutton id="LNBvisualizza" Runat="server" Text="Visualizza Cover" CausesValidation=False  CssClass="LINK_MENU" Visible=False ></asp:linkbutton>
			 <asp:linkbutton id="LNBsalvaImpostazioni" Runat="server" Text="Salva Impostazioni" CssClass="LINK_MENU" Visible=False></asp:linkbutton> 
		  </div>
		  <div  align="center">
			 <asp:MultiView ID="MLVcontenuto" runat="server">
				<asp:View ID="VIWpermessi" runat="server">
				     <div id="DVpermessi" align="center">
					   <div style="height: 50px;"></div>
					   <div align="center">
						  <asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
					   </div>
					   <div style="height: 50px;"></div>
				    </div>
				</asp:View>
				<asp:View ID="VIWcover" runat="server">
				     
				    <div id="DVcover" align="center">
					   <div id="DVtitoloCover" runat="server" align="center">
						  <asp:Label ID="LBTitolo" Runat="server" Visible=False >Cover</asp:Label>
						   <asp:LinkButton ID=LNBtitolo Runat=server></asp:LinkButton>
					   </div>
					   <div id="DVannoAccademico" runat="server" align="center">
						  <asp:Label ID="LBannoAccademico" Runat=server></asp:Label>
					   </div>
					   <div>&nbsp;</div>
					   <div id="DVimmagineCover" runat="server" align="center">
						  <asp:ImageButton ID="IMBcover" Runat=server ></asp:ImageButton>
					   </div>
					   <div id="DVcommentiCover" runat="server" align="center">
						   <asp:Label ID="LBcommenti" Runat=server></asp:Label>
					   </div>
					   <div id="DVskip" runat="server" align="center">
						  <asp:CheckBox ID="CBXskip" Runat=server AutoPostBack=True Text=skip></asp:CheckBox>
					   </div>
				    </div>
				</asp:View>
				<asp:View ID="VIWedit" runat="server">
				    <div id="DVgestione" align="center">
					   <CTRL:CTRLgestioneCover id="CTRLgestioneCover" runat="server" ></CTRL:CTRLgestioneCover>
				    </div>
				</asp:View>
				<asp:View ID="VIWnonDefinita" runat="server">
				    <div id="DVnonDefinita" align="center">
					   <asp:Label ID="LBnonDefinita" Runat=server ></asp:Label>
				    </div>
				</asp:View>
				<asp:View ID="VIWnonAttivata" runat="server">
				    <div id="DVnonAttivata" align="center">
					   <asp:Label ID="LBnoattivata" Runat=server ></asp:Label>
				    </div>
				</asp:View>
			 </asp:MultiView>
		  </div>
</asp:Content>













<%--

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Comunità On Line - Cover</title>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
	<meta name=vs_defaultClientScript content="JavaScript"/>
	<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
	
	<LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="aspnetForm" method="post" runat="server">
    <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
	   <div id="DVcontenitore" align="center">
		  <div id="DVheader" align="center" style="width: 900px; text-align:left;">
			 <HEADER:CTRLheader id="Intestazione" runat="server" HeaderNewsMemoHeight="85px"></HEADER:CTRLheader>
		  </div>

		 
	   </div>
	   <FOOTER:CtrLFOOTER id="CtrLFooter" runat="server"></FOOTER:CtrLFOOTER>
	</form>
</body>
</html>--%>
