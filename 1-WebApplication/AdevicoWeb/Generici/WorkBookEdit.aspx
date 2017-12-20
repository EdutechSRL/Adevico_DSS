<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WorkBookEdit.aspx.vb" 
Inherits="Comunita_OnLine.WorkBookEdit" Theme="Materiale" EnableTheming="true" 
MasterPageFile="~/AjaxPortal.Master"%>

<%@ Register TagPrefix="CTRL" TagName="WorkBook" Src="UC_DiarioPersonale/UC_WorkBookData.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="USERlist" Src="../UC/UC_SearchUserByCommunities.ascx" %>

<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ Import Namespace="lm.Comol.Modules.Base.DomainModel" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<style type="text/css">
	    td{
		    font-size: 11px;
		}
	</style>

	<link href="./../dhtmlcentral.css" rel="STYLESHEET" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
		  <div id="DVmenu" style="width: 900px; text-align:right;" align="center">
                <asp:Button ID="BTNreturnToList" runat="server" Text="Torna all'elenco" />
		  </div>
		  <div align="left" style="width: 900px;  padding-top:5px; ">
			 <asp:MultiView ID="MLVworkBook" runat="server" ActiveViewIndex="0">
				<asp:View id="VIWwizardWorkBook" runat="server">
				     <div style="background-color:#EFF3FB; border-color:#B5C7DE; border-width:1px; width:850px; border-style:solid; margin: 0, auto; padding: 0,auto;" >
					   <asp:MultiView ID="MLVwizard" runat="server">
						  <asp:View runat="server"  ID="VIWedit">
							 <div class="titolo">
								<asp:Label runat="server" ID="LBworkBookData" CssClass="titolo">Dati WorkBook</asp:Label>
							 </div>
							 <hr style="color: #00008B;" />
							 <div style="padding:20px,auto; min-height:400px; background-color:White;">
								<CTRL:workBook id="CTRLworkBook" runat="server" Mode="Editing"></CTRL:workBook><br/>
							 </div>
							 <div style="text-align:right;">
								<asp:Button ID="BTNgoToWorkBookList" runat="server" CommandName="tolist" Text="Torna all'elenco" />
								<asp:Button ID="BTNsaveGoToWorkBookList" runat="server" CommandName="savetolist" Text="Torna all'elenco" />
								<asp:Button ID="BTNmanagementAuthors" runat="server" CommandName="tomanagement" Text="Gestione autori"/>
							 </div>
						  </asp:View>
						  <asp:View runat="server"  ID="VIWmanagementAuthors">
							 <div class="titolo">
							   <asp:Label runat="server" ID="LBworkBooManagement" CssClass="titolo">Management autori</asp:Label>
							   </div>
							 <hr style="color: #00008B;" />
							 <div style="padding: 20px, auto; margin:0, auto; background-color:White; ">
								<asp:GridView ID="GDVauthors" runat="server" HorizontalAlign="Center" SkinID="griglia60pc" UseAccessibleHeader="true">
								    <Columns>
									   <asp:TemplateField HeaderText="E" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
										  <ItemTemplate>
											  <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete" CausesValidation="false" ></asp:LinkButton>
											  <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false" Visible="false"></asp:LinkButton>
											  <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false"  CommandName="confirmDelete" Enabled="false"></asp:LinkButton>
										  </ItemTemplate>
										  <ItemStyle HorizontalAlign="Center" Width="40px" />
									   </asp:TemplateField>
									   <asp:TemplateField HeaderText="P" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
										  <ItemTemplate>
											 <asp:Literal ID="LTisOwner" runat="server"></asp:Literal>
										  </ItemTemplate>
									   </asp:TemplateField>
									   <asp:TemplateField HeaderText="Autore">
										  <ItemTemplate>
											 <asp:Literal ID="LTsurnameName" runat="server"></asp:Literal>
										  </ItemTemplate>
									   </asp:TemplateField>
									   <asp:TemplateField HeaderText="Aggiunto il">
										  <ItemTemplate>
											 <asp:Literal ID="LTcreatedOn" runat="server"></asp:Literal>
										  </ItemTemplate>
									   </asp:TemplateField>
								 </Columns>
								  </asp:GridView>
								  <br/><br/>
							   </div>
							 <div style="text-align:right;">
								<asp:Button ID="BTNgoToWorkBookList_1" runat="server" CommandName="tolist" Text="Torna all'elenco" />
								<asp:Button ID="BTNgoToData" runat="server" CommandName="todata" Text="Indietro" />
								<asp:Button ID="BTNsearchAuthors" runat="server" CommandName="searchauthors" Text="Aggiungi autore/i" />
								<asp:Button ID="BTNselectOwner" runat="server" CommandName="selectOwner" Text="Responsabile" />
							 </div>
						  </asp:View>
						  <asp:View runat="server"  ID="VIWaddAuthor">
							 <div class="titolo">
								<asp:Label runat="server" ID="LBaddAuthor" CssClass="titolo">Aggiungi autore/i</asp:Label>
							 </div>
							 <hr style="color: #00008B;" />
							 <div style="padding: 20px, auto; margin:0, auto; background-color:White; ">
								<CTRL:USERlist id="CTRLuserList" runat="server"></CTRL:USERlist>
							 </div>
							 <div style="text-align:right;">
								<asp:Button ID="BTNgoToWorkBookList_2" runat="server" CommandName="tolist" Text="Torna all'elenco" />
								<asp:Button ID="BTNgoToManagement" runat="server" CommandName="tomanagement" Text="Indietro" />
								<asp:Button ID="BTNaddAuthors" runat="server" CommandName="addauthors" Text="Conferma aggiunta"/>
							 </div>
						  </asp:View>
						  <asp:View runat="server"  ID="VIWselectOwner">
							 <div class="titolo">
								  <asp:Label runat="server" ID="LBselectOwner" CssClass="titolo">Seleziona responsabile</asp:Label>
							   </div>
							   <hr style="color: #00008B;" />
							   <div style="padding: 20px, auto; margin:0, auto; background-color:White; ">
								  <asp:RadioButtonList ID="RBLowner" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow"></asp:RadioButtonList>
							   </div>
							   <div style="text-align:right;">
								<asp:Button ID="BTNgoToManagement_2" runat="server" CommandName="tomanagement" Text="Indietro" />
								<asp:Button ID="BTNconfirmOwner" runat="server" CommandName="confirmowner" Text="Conferma selezione"/>
								<asp:Button ID="BTNfinish" runat="server" CommandName="confirmownertolist" Text="Finish" />
							 </div>
						  </asp:View>
					   </asp:MultiView>
				    </div>
				</asp:View>
				<asp:View ID="VIWnoPermission" runat="server">
				     <div id="DVpermessi" align="center">
				        <div align="right" style="text-align:right; clear:right">
				            <asp:Button ID="BTNreturnToWorkBookList" runat="server" Text="Torna all'elenco" />
				        </div>
					   <div style="height: 50px;"></div>
					   <div align="center">
						  <asp:Label id="LBNopermessi" Runat="server" CssClass="messaggio"></asp:Label>
					   </div>
					   <div style="height: 50px;"></div>
				    </div>
				</asp:View>
			 </asp:MultiView>
		  </div>
</asp:Content>