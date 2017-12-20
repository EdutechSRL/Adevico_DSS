<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UC_EsportaLink.ascx.vb" Inherits="Comunita_OnLine.UC_EsportaLink" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="radTree" Namespace="Telerik.WebControls" Assembly="RadTreeView.Net2" %>

<input type=hidden id="HDN_hasExport" runat=server NAME="HDN_hasExport"/>
<table  class="TableUcFile" border=1 cellspacing=0 cellpadding=0 width=700px>
	<tr>
		<td align=left>
			<br/>
			<asp:Panel ID="PNLesporta" Runat=server HorizontalAlign=Center width=700px>
				<asp:Table Runat=server>
					<asp:TableRow>
						<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan=4>
							<asp:Label ID="LBesporta" Runat=server CssClass="info_blackMedium">
								Scegliere i link da inserire nei propri preferiti:
							</asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>&nbsp;</asp:TableCell>
						<asp:TableCell ColumnSpan=2>
							<radTree:RadTreeView id="RDTesporta" runat="server" align="left"
								CausesValidation="False"
								CssFile="~/RadControls/TreeView/Skins/BookMark/StyleSelectFolder.css" PathToJavaScript="~/Jscript/RadTreeView_Client_3_1.js" skin="BookMark"
								ImagesBaseDir="~/RadControls/TreeView/Skins/BookMark/" 
								XPStyle=true MultipleSelect=false DragAndDrop=false
								AllowNodeEditing="false"
								ShowLineImages=true width=600px AfterClientCheck="CheckChildNodes" CheckBoxes=True>
							</radTree:RadTreeView>
						</asp:TableCell>
						<asp:TableCell>&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan=4>&nbsp;</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:Panel>
		</td>
	</tr>
</table>