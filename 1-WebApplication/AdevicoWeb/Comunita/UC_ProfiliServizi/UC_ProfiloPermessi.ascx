
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_ProfiloPermessi.ascx.vb" Inherits="Comunita_OnLine.UC_ProfiloPermessi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript" type="text/javascript">
	function SelectFromNameAndAssocia(Nome,value){
		var HIDcheckbox;
		eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];
			if ( e.type=='checkbox' && e.name ==Nome) {//"CBXassocia"
				if (e.checked==true){
					if (HIDcheckbox.value == "")
						HIDcheckbox.value = ',' + value +','
					else{
						pos1 = HIDcheckbox.value.indexOf(',' + value+',')
						if (pos1==-1)
						HIDcheckbox.value = HIDcheckbox.value + value +','
						}
					}
				else{
					valore = HIDcheckbox.value;
					pos1 = HIDcheckbox.value.indexOf(',' + value+',')
					if (pos1!=-1){
						stringa = ',' + value+','
						HIDcheckbox.value = HIDcheckbox.value.substring(0,pos1)
						HIDcheckbox.value = HIDcheckbox.value + valore.substring(pos1+value.length+1,valore.length)
						}
					}
				}
		}

		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
	}
</script>

<input type=hidden id="HIDcheckbox" runat=server NAME="HIDcheckbox"/>
<input type=hidden id="HDN_profiloID" runat=server NAME="HDN_profiloID"/>
<input type=hidden id="HDN_definito" runat=server NAME="HDN_definito"/>
<input type=hidden id="HDN_TPCM_ID" runat=server NAME="HDN_TPCM_ID"/>

<div id="layer1">
</div>
			
<asp:Table ID="TBLpermessi" Runat=server HorizontalAlign=left Width=800px>
	<asp:TableRow>
		<asp:TableCell ColumnSpan=2 Height=40px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:Label ID="LBserviziPermessi_t" Runat=server CssClass="Titolo_campoSmall">Servizi:</asp:Label>
			&nbsp;&nbsp;
			<asp:DropDownList ID="DDLserviziPermessi" AutoPostBack=True CssClass="Testo_campoSmall" Runat=server ></asp:DropDownList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell CssClass="nosize0" Height=10px>&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell>
			<asp:Table Runat=server HorizontalAlign=left ID="TBLpermessiRuoli" GridLines=Both CellSpacing=0 CellPadding=2>
																		
			</asp:Table>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>
