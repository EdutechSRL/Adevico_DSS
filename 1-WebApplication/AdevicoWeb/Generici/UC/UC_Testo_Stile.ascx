<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Testo_Stile.ascx.vb" Inherits="Comunita_OnLine.UC_Testo_Stile" %>

<div>
    <div id="DVstile" runat="server">
	   <div style=" float:left; width:40px;">
		  <asp:Label ID="LBfontFace" Runat=server CssClass="Testo_campoSmall">Font:</asp:Label>
	   </div>
	   <div style=" float:left; padding-left: 5px;">
		  <asp:DropDownList ID="DDLfontFace" Runat=server CssClass="Testo_campoSmall">
			 <asp:ListItem>Arial</asp:ListItem>
             <asp:ListItem>Courier New</asp:ListItem>
             <asp:ListItem>Helvetica</asp:ListItem>
             <asp:ListItem>Lucida</asp:ListItem>
             <asp:ListItem >Palatino Linotype</asp:ListItem>
             <asp:ListItem Selected="True">Verdana</asp:ListItem>
		     <asp:ListItem>Tahoma</asp:ListItem>
             <asp:ListItem>Times New Roman</asp:ListItem>
		   </asp:DropDownList>
	   </div>
	    <div style=" float:left; width:40px; padding-left: 5px;">
		  <asp:Label ID="LBfontSize" Runat=server CssClass="Testo_campoSmall">Size:</asp:Label>
	    </div>
	    <div style=" float:left;">
		  <asp:DropDownList ID="DDLfontSize" Runat=server CssClass="Testo_campoSmall">
			    <asp:ListItem Selected=True Value=16>20px</asp:ListItem>
			    <asp:ListItem Value=14>16px</asp:ListItem>
			    <asp:ListItem Value=12>14px</asp:ListItem>
			    <asp:ListItem Value=10>12px</asp:ListItem>
		    </asp:DropDownList>
	    </div>
	    <div style=" float:left; width:40px; padding-left: 5px;">
		  <asp:Label ID="LBfontColor" Runat=server CssClass="Testo_campoSmall">Color:</asp:Label>
	    </div>
	    <div style=" float:left;">
		  <asp:DropDownList ID="DDLfontColor" Runat=server CssClass="Testo_campoSmall">
			   <asp:ListItem Value=black Selected=True >Black</asp:ListItem>
			   <asp:ListItem Value=gray >Gray</asp:ListItem>
			   <asp:ListItem Value=Blue>Blue</asp:ListItem>
			   <asp:ListItem Value=Red>Red</asp:ListItem>
			   <asp:ListItem Value=navy></asp:ListItem>
		   </asp:DropDownList>
	    </div>
	    <div style=" float:left; padding-left: 5px;">
		  <asp:CheckBox ID="CBXbold" Runat=server Text="Bold" CssClass="Testo_campoSmall"></asp:CheckBox>
	    </div>
	    <div style="padding-left: 5px;">
		  <asp:CheckBox ID="CBXitalic" Runat=server Text="Italic" CssClass="Testo_campoSmall"></asp:CheckBox>
	    </div>
    </div>
    <div style="float:none; text-align:left;  ">
	    <asp:textbox id="TXBcampo" Runat="server" CssClass="Testo_campoSmall_obbligatorio" Columns="65" Rows=3 TextMode=MultiLine 
						MaxLength="300"></asp:textbox>	
	   <asp:requiredfieldvalidator id="RFVcampo" runat="server" CssClass="Validatori" ControlToValidate="TXBcampo"
		Display="static" EnableClientScript="true" Visible="false">*</asp:requiredfieldvalidator>
    </div>
</div>