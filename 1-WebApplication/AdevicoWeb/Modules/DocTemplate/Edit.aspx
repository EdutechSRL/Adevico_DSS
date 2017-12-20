<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Edit.aspx.vb" Inherits="Comunita_OnLine.Edit" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="UC/UC_EditSteps.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="Settings" Src="UC/UC_EditSettings.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="HeaderFooter" Src="UC/UC_EditHeaderFooter.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Body" Src="UC/UC_EditBody.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Signatures" Src="UC/UC_EditSignatures.ascx" %>

<%@ Register TagPrefix="CTRL" TagName="SkinImport" Src="UC/UC_EditSkinImport.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

	<!-- Stili docTemplate -->
	<link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
	<link href="../../Graphics/Modules/DocTemplate/css/certificates.css" rel="Stylesheet" type="text/css" />
	<!-- fine stili docTemplate -->
	<link href="../../Graphics/Template/Wizard/css/wizard.css" rel="Stylesheet" type="text/css" />
	<!-- Script usati da DocTemplate-->
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/DocTemplate/DocTemplate.js"></script>
	<script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
	<script type="text/javascript" language="javascript" >
		var TokenHiddenFieldId = "<% = HDNdownloadTokenValue.ClientID %>";
		var CookieName = "<% = Me.CookieName %>";
		var DisplayMessage = "<% = Me.DisplayMessageToken %>";
		var DisplayTitle = "<% = Me.DisplayTitleToken %>";
	</script>
	<!-- Fine script DocTemplate-->

	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
	<asp:Literal ID="LTtitle_t" runat="server">Certificate Management</asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
	<div class="buttonwrapper">
		<span class="buttongroup">
			<asp:HyperLink ID="HYPbackUrl" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Torna al servizio</asp:HyperLink>
			<asp:PlaceHolder ID="PLHexport" runat="server" Visible="false">
				<div id="DIVexport" class="ddbuttonlist enabled" visible="true"><!--   
						--><asp:LinkButton ID="LKBexportPDF" runat="server" Text="*PDF" CssClass="linkMenu " OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
						--><asp:LinkButton ID="LKBexportRTF" runat="server" Text="*RTF" CssClass="linkMenu " OnClientClick="blockUIForDownload();return true;"></asp:LinkButton><!--   
				--></div>
			</asp:PlaceHolder>
			
			<asp:HyperLink ID="HYPlist" runat="server" Text="*Return_P" CssClass="linkMenu " Visible="false">#Torna alla lista</asp:HyperLink>
		</span>
		<asp:PlaceHolder ID="PLHsecButton" runat="server">
			<span class="buttongroup">
				<asp:Button ID="BTNaddUp" runat="server" Text="Add" Visible="true" />
				<asp:HyperLink ID="HYPGoToAdvance" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Advance</asp:HyperLink>
				<asp:HyperLink ID="HYPGoToSimple" runat="server" Text="*Return" CssClass="linkMenu " Visible="false">#Simple</asp:HyperLink>
			</span>
			<span class="buttongroup">
				<asp:LinkButton ID="LKBundo" runat="server" CssClass="linkMenu" CausesValidation="false">#Annulla</asp:LinkButton>
				<asp:LinkButton ID="LKBsave" runat="server" CssClass="linkMenu">#Salva</asp:LinkButton>
			</span>
		</asp:PlaceHolder>
	</div>

	<div class="contentwrapper edit clearfix persist-area _hasFloating">
	<asp:Panel ID="PnlData" runat="server">
		<CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>

		<div class="column right resizeThis" style="height: auto;">
			<div class="rightcontent">
		  
				<div class="contentouter">
					<div class="content clearfix">
						<asp:MultiView ID="MLVtemplatePart" runat="server">

							<asp:View ID="VWproperty" runat="server">
								<CTRL:Settings ID="UCsettings" runat="server" />
							</asp:View>

							<asp:View ID="VWheader" runat="server">
								<CTRL:HeaderFooter ID="UCheader" runat="server" />
							</asp:View>

							<asp:View ID="VWBody" runat="server">
								<CTRL:Body ID="UCbody" runat="server" />
							</asp:View>

							<asp:View ID="VWsignatures" runat="server">
								<CTRL:Signatures ID="UCsignatures" runat="server" />
							</asp:View>

							<asp:View ID="VWfooter" runat="server">
								<CTRL:HeaderFooter ID="UCfooter" runat="server" />
							</asp:View>
						</asp:MultiView>
					</div>
				</div>
				<div class="footer"> </div>
			</div>
		</div>
	
	<asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />

	<asp:Panel ID="PnlModal" runat="server" Visible="false">
		<div class="view-modal" style="display:none;">
			<CTRL:SkinImport ID="UCSkinImport" runat="server" />
		</div>
	</asp:Panel>
	
	</asp:Panel>
	</div>
	<asp:Label ID="LBLerror" runat="server" CssClass="error" Visible="false"></asp:Label>
</asp:Content>