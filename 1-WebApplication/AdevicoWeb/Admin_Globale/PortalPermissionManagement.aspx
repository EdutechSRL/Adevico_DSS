<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="PortalPermissionManagement.aspx.vb" Inherits="Comunita_OnLine.PortalPermissionManagement" Theme="Materiale" EnableTheming="true"%>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="Content2">

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
	<script type="text/javascript" language="javascript">
	
	    function SelezioneRiga(elementi){
		  var HIDcheckbox;
		  var totale, selezionati;

		  //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
		  HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
		  totale = 0
		  selezionati = 0
		  deselezionati=0
   		  for(i=0;i< document.forms[0].length; i++){
   		      e = document.forms[0].elements[i];
   		    /*  if (e.type == 'checkbox')
   		          alert(e.name);*/
			 if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
				totale = totale + 1
				if (e.checked==true)
				    selezionati = selezionati + 1
				else
				    deselezionati = deselezionati + 1
			 }
}
		  if (totale >0 && totale == selezionati){
			   for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
				    e.checked= false;
				    HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',',',')
				    } 
			 }
		  }
		  else{
			 for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if ( e.type=='checkbox' && elementi.indexOf(',' + e.name + ',') != -1 ) {
				    e.checked= true;
				    if (HIDcheckbox.value == "")
					   HIDcheckbox.value = ',' + e.value +','
				    else{
					   pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
					   if (pos1==-1)
						  HIDcheckbox.value = HIDcheckbox.value + e.value +','
					   }
				}
			 } 
		    }
		  
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";
		return false
	}
	
	  function SelezioneColonna(NomeColonna){
		  var HIDcheckbox;
		  var totale, selezionati;
		  //eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
		  HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
		  totale = 0
		  selezionati = 0
		  deselezionati=0

   		  for(i=0;i< document.forms[0].length; i++){ 
			 e=document.forms[0].elements[i];

			 if (e.type == 'checkbox' && e.name.indexOf('<%=me.Master.ClientID & me.IdSeparator.ToString & "CPHservice" & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1) {
				totale = totale + 1
				if (e.checked==true)
				    selezionati = selezionati + 1
				else
				    deselezionati = deselezionati + 1
			 } 
		  }
		  if (totale >0 && totale == selezionati){
			   for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if (e.type == 'checkbox' && e.name.indexOf('<%=me.Master.ClientID & me.IdSeparator.ToString & "CPHservice" & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1) {
				    e.checked= false;
				    HIDcheckbox.value = HIDcheckbox.value.replace(',' + e.value + ',',',')
				    } 
			 }
		  }
		  else{
			 for(i=0;i< document.forms[0].length; i++){ 
				e=document.forms[0].elements[i];
				if (e.type == 'checkbox' && e.name.indexOf('<%=me.Master.ClientID & me.IdSeparator.ToString & "CPHservice" & me.IdSeparator.ToString%>' + NomeColonna + '_') != -1) {
				    e.checked= true;
				    if (HIDcheckbox.value == "")
					   HIDcheckbox.value = ',' + e.value +','
				    else{
					   pos1 = HIDcheckbox.value.indexOf(',' + e.value + ',')
					   if (pos1==-1)
						  HIDcheckbox.value = HIDcheckbox.value + e.value +','
					   }
				}
			 } 
		    }
		  
		if (HIDcheckbox.value==",")
			HIDcheckbox.value = "";

		return false
	}
	
	
	 
	 function SelectFromNameAndAssocia(Nome,value){
		var HIDcheckbox;
		//eval('HIDcheckbox= this.document.forms[0].<%=Me.HIDcheckbox.clientID%>');
		HIDcheckbox = this.document.getElementById('<%=Me.HIDcheckbox.ClientID%>');
   		for(i=0;i< document.forms[0].length; i++){ 
			e=document.forms[0].elements[i];

			if (e.type == 'checkbox' && e.name == '<%=me.Master.ClientID & me.IdSeparator.ToString & "CPHservice" & me.IdSeparator.ToString%>CB_' + value) {//"CBXassocia"
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
	   <div style="text-align:right; padding-top:5px;" >
                 <asp:Button ID="BTNsave" runat="server" Text="Save" />
                  &nbsp;
                  <asp:Button ID="BTNsaveForAll" runat="server" Text="Save for all" />
                   &nbsp;
                  <asp:Button ID="BTBdefaultValue" runat="server" Text="Default value" />
                     &nbsp;
                  <asp:Button ID="BTNreplaceCommunityValues" runat="server" Text="Set to all community" ToolTip="Set permission to all community of selected community type" />
            </div>
    <div>
        <asp:Literal ID="LTcommunityType" runat="server" EnableViewState="false">Community Type:</asp:Literal>
        <asp:DropDownList ID="DDLcommunityType" AutoPostBack="true" Runat="server"></asp:DropDownList>
    </div>
    <div>
        <asp:Literal ID="LTorganization" runat="server" EnableViewState="false">Organization:</asp:Literal>
        <asp:DropDownList ID="DDLorganization" AutoPostBack="true" Runat="server"></asp:DropDownList>
    </div>
    <div>
        <asp:Literal ID="LTmodule" runat="server" EnableViewState="false">Module:</asp:Literal>
        <asp:DropDownList ID="DDLmodules" AutoPostBack="true" Runat="server"></asp:DropDownList>
    </div>
    <div>
        <asp:Table Runat="server" HorizontalAlign="left" ID="TBLpermessiRuoli" GridLines="Both" CellSpacing="0" CellPadding="2">
		
		</asp:Table>
    </div>
    <input type="hidden" runat="server" id="HIDcheckbox" />
</asp:Content>