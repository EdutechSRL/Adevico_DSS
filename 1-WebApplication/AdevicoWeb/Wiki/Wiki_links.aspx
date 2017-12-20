<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Wiki_links.aspx.vb" Inherits="Comunita_OnLine.Wiki_links" %>
<%@ Register TagPrefix="rade" Namespace="Telerik.WebControls" Assembly="RadEditor.Net2" %>

<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Wiki</title>
    <link href="./../Styles.css" type="text/css" rel="stylesheet"/>
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	
    <link href="../Graphics/Modules/Wiki/wiki.new.css?v=201508040900lm" rel="Stylesheet" />

    <link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />
    
</head>

 <script type="text/javascript"  language="javascript" src="./../Radcontrols/Editor/Scripts/7_0_0/RadWindow.js"></script>
    <script type="text/javascript" language="javascript">
	   function GetDialogArguments(){
		  if (window.radWindow) 
			return window.radWindow.Argument;
		  else
			 return null;
	   }

	   var isRadWindow = true;
	   var radWindow = GetEditorRadWindowManager().GetCurrentRadWindow(window);
	   if (radWindow){ 
		if (window.dialogArguments) { 
		  radWindow.Window = window;
		} 
		radWindow.OnLoad(); 
	   }
	</script>
    <script type="text/javascript">
	    InitializeRadWindow();
	</script>
<body>
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
    <asp:Panel ID="PNL_search" runat="server" BorderWidth="0" Width="100%">
        <div class="fieldobject search">
            <div class="fieldrow">
                <asp:Label
					ID="LBL_ricerca" 
						                              
					runat="server" 
					CssClass="Testo_campoSmall"></asp:label>
						                                			                                
                        <asp:DropDownList ID="DDL_ricerca" runat="server">
                            <asp:ListItem>(Topic)</asp:ListItem>
                            <asp:ListItem Selected="True">Inizia per</asp:ListItem>
                            <asp:ListItem>Finisce per</asp:ListItem>
                            <asp:ListItem>Contiene</asp:ListItem>
                        </asp:DropDownList> 
                                                        
				<asp:TextBox ID="TXB_search" runat="server" CssClass="Testo_campoSmall" MaxLength="60">
				</asp:TextBox>
                <asp:Button ID="BTN_search" runat="server"  CommandArgument="TXB_search.text" 
                    CommandName="Cerca" CssClass="PulsanteFiltro" Height="21px" Text="Cerca" />
            </div>
            <div class="fieldrow">
                <asp:CheckBox runat="server" ID="CBX_comunita"  Text="Questa comunità" Checked="true" />
                <asp:CheckBox runat="server" ID="CBX_pubblici"  Text="Pubblici" Checked="false" />
            </div>
        </div>

                                                </asp:Panel>
                                            
    	<table class="contenitore" align="center">
			
			<tr>
			    <td>
			        <table cellspacing="0" cellpadding="0" width="900px" border="0">
											
			            <tr>
							<td align="center">
							    
							    
							                
                                            
							   
							    
							    
					        </td>
			            </tr>
			        </table>
			    </td>
			</tr>
	    </table>
	    <input type="hidden" id="HDNdefaultSearch" runat="server" />

	   <script type="text/javascript" language="javascript">
	       function insertLink(NomeLink,HrefLink) {
	           var returnValue = {
				href: HrefLink,
				target: "_self",
				name: NomeLink
	           };
	           CloseDlg(returnValue);
	           return false;
	       }

//	       function insertLinkBase() //fires when the Insert Link button is clicked
//	       {
//	           //create an object and set some custom properties to it      
//	           workLink.href = linkUrl.value;
//	           workLink.target = linkTarget.value;
//	           workLink.className = linkClass.value;
//	           workLink.name = linkName.value;

//	           getRadWindow().close(workLink); //use the close function of the getRadWindow to close the dialog and pass the arguments from the dialog to the callback function on the main page.
//	       }
	   </script>
	 <script type="text/javascript" language="javascript" >
	     args = GetDialogArguments()
	     var TextContainer = document.getElementById("TXB_search");
	     var HiddenContainer = document.getElementById("HDNdefaultSearch");
	     if (TextContainer) {
	         if (TextContainer.value == "" && args.name !="") {
	             TextContainer.value = args.name
	             HiddenContainer.value = args.name
	             document.getElementById("BTN_search").click();
	             var theForm = document.forms['aspnetForm'];
	             if (!theForm) {
	                 theForm = document.aspnetForm;
	             }
	             function __doPostBack(eventTarget, eventArgument) {
	                 if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
	                     theForm.__EVENTTARGET.value = eventTarget;
	                     theForm.__EVENTARGUMENT.value = eventArgument;
	                     theForm.submit();
	                 }
	             }
	         }
	     }
	   </script>	
            <asp:MultiView ID="MultiView" runat="server">
                <asp:View ID="View_Nessuna" runat="server">
                <div class="TestoInfo">
                    <asp:Label ID="LBL_Con_SezioneNO" runat="server" CssClass="Testo_campoSmall">
                                                        Nessun topic trovato				
                                                </div>
                    		                                                </asp:Label>
                </asp:View>
                <asp:View ID="View_noPermessi" runat="server">
                    <div class="TestoInfo">
                    <asp:Label ID="LBL_NoPermessi" runat="server">
					    		        Non si dispone dei permessi necessari per visualizzare la pagina.
					    		    </asp:Label>
					    		    </div>
                </asp:View>
                <asp:View ID="View_risultati" runat="server">
                
                <asp:DataList 
						                            ID="DLS_result"
						                            runat="server"
						                            CellPadding="4"
						                            ForeColor="#333333"
						                            DataKeyField="id"
						                            Width="100%"  >
                                                    <FooterStyle
                                                        BackColor="#507CD1"
                                                        Font-Bold="True"
                                                        ForeColor="White" />
                                                    <SelectedItemStyle
                                                        BackColor="#D1DDF1"
                                                        Font-Bold="True"
                                                        ForeColor="#333333" />
                                                    <AlternatingItemStyle BackColor="White" />
                                                    <ItemStyle BackColor="#EFF3FB" />
                                                    <HeaderStyle 
                                                         CssClass="RigaTitolo" />
                                                        <HeaderTemplate>
                                                        <table >
                                                            <tr>
                                                            <td width="28px"></td>
                                                            <td width="250px" ><asp:Label ID="LBL_intest1" runat="server" ForeColor="White" Text="Voce"></asp:Label>
                                                            </td>
                                                            <td width="180px"><asp:Label ID="Label1" runat="server" ForeColor="White" Text="Sezione"></asp:Label>
                                                            </td>
                                                            <td><asp:Label ID="Label2" runat="server" ForeColor="White" Text="Comunità"></asp:Label>
                                                            </td>
                                                            </tr>
                                                        </table>
                                                            
                                                        </HeaderTemplate>
                                                        
                                                    <ItemTemplate>
                                                        
                                                        <table>
                                                        <tr>
                                                        <td width="28px">
                                                        <a href="#" onclick="return insertLink('<%#DataBinder.Eval(Container.DataItem, "Nome")%>','<%=Me.BaseUrl %>wiki/wiki_comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>');return false;"><img src="../images/filemanager/upload.gif"  alt="Inserisci voce:<%#DataBinder.Eval(Container.DataItem, "Nome")%>"/></a>
                                                        </td>
                                                        <td width="250px">
                                                            
                                                                <span style="left"
                                                                    class="Titolo_Campo" 
                                                                    style="text-align: left;">
                                                                    <a href="#" onclick="return insertLink('<%#DataBinder.Eval(Container.DataItem, "Nome")%>','<%=Me.BaseUrl %>wiki/wiki_comunita.aspx?id=<%#DataBinder.Eval(Container.DataItem, "ID")%>');return false;"><%#DataBinder.Eval(Container.DataItem, "Nome")%></a>
                                                                </span><td/>
                                                                
                                                            
                                                            <td width="180px">
                                                            <span width="right"
                                                                class="Testo_Campo" 
                                                                    style="text-align:left;">
                                                                    <a name="<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                                                                    <%#DataBinder.Eval(Container.DataItem, "Sezione.NomeSezione")%></a>
                                                                </span>
                                                                
                                                            </td>
                                                            <td>
                                                            <span width="right"
                                                                class="Testo_Campo" 
                                                                    style="text-align:left;">
                                                                    <a name="<%#DataBinder.Eval(Container.DataItem, "ID")%>">
                                                                    <%#DataBinder.Eval(Container.DataItem, "Comunita")%></a>
                                                                </span>
                                                            </td>
                                                            
                                                            </tr>
                                                         </table>
                                                </ItemTemplate>
                                                </asp:DataList>
                                                <div >
                                                <table align="right" width="35px" >
                                                    
                                                    <tr align="right">
                                                        
                                                        <td align="right">
                                                        
                                                            <asp:ImageButton ID="IMG_indietro" runat="server" CssClass="PulsanteFiltro" ImageUrl="../questionari/img/indietro.gif" AlternateText="Pagina precedente"/>
                                                           
                                                        
                                                        </td>    
                                                        <td align="right">
                                                             <asp:ImageButton ID="IMG_avanti" runat="server" CssClass="PulsanteFiltro" ImageUrl="../questionari/img/avanti.gif" AlternateText="Pagina successiva"/>
                                                        </td>
                                                    </tr>
                                                
                                                </table></div>
                                                
                
                                                    <br />
                                                    <br />
                
                
                </asp:View>
            </asp:MultiView>
    </form>
</body>
</html>
