<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditSkinImport.ascx.vb" Inherits="Comunita_OnLine.UC_EditSkinImport" %>

<%@ Register TagPrefix="CTRL" TagName="OrgnSel" Src="~/Modules/DocTemplate/Uc/UcOrganizationSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ComSel" Src="~/uc/UC_SearchCommunityByService.ascx" %>



<div id="Wizard">
	<div class="wiz_header">
		<div class="wiz_top_nav">
			<div class="right stepButton"> 
				<asp:LinkButton ID="LKBundot" runat="server" CssClass="linkMenu " OnClientClick="closeDialog('detailFile');">#Undo</asp:LinkButton>
                <asp:LinkButton ID="LKBbackt" runat="server" CssClass="linkMenu">#Back</asp:LinkButton>
				<asp:LinkButton ID="LKBnextt" runat="server" CssClass="linkMenu ">#Next</asp:LinkButton>
				<asp:LinkButton ID="LKBconfirmt" runat="server" CssClass="linkMenu">#Confirm</asp:LinkButton>
			</div>

		</div>
		<div class="wiz_top_info clearfix">
			<div class="wiz_top_desc clearfix">
				<h3>
                    <span class="title">
                        <asp:Literal ID="LITstepTitle_t" runat="server"></asp:Literal>
                    </span>
                </h3>
				<span class="Testo_campo">
                    <asp:Literal ID="LITstepDescription_t" runat="server"></asp:Literal>
                </span>
                <br/>
			</div>
		</div>
	</div>
	<div class="wiz_content">
		<asp:Panel ID="PNLmain" runat="server">
			<div class="StepData IW_Step1">
				<div class="fieldobject">
					<div class="fieldrow skintype">
					<asp:RadioButtonList ID="RBLsource" runat="server" 
						AutoPostBack="false"
						RepeatLayout="Flow" CssClass="options">
						<asp:ListItem Value="0" Text="#Current"></asp:ListItem>
						<asp:ListItem Value="1" Text="#Portal"></asp:ListItem>
						<asp:ListItem Value="2" Text="#Organization"></asp:ListItem>
						<asp:ListItem Value="3" Text="#Community"></asp:ListItem>
					</asp:RadioButtonList>

					<%--<asp:TextBox ID="TXBsourceName" runat="server" Enabled="false" Visible="false"></asp:TextBox>--%>
					</div>
				</div>
			</div>
		</asp:Panel>
		<asp:Panel ID="PNLselOrgn" runat="server">
			<div class="StepData IW_Step2">
				<div class="fieldobject">
					<div class="fieldrow skinOrgn">
					
						<asp:LinkButton ID="LKBundoOrgn" runat="server" CssClass="linkMenu RightButton">Deselect Organization</asp:LinkButton>
						<!-- Begin UC ORGN_Select -->
						<CTRL:OrgnSel ID="UCorgSelector" runat="server" /> 
						<!-- END UC ORGN_Select -->

					</div>
				</div>
			</div>
		</asp:Panel>

		<asp:Panel ID="PNLselCom" runat="server">
			<div class="StepData IW_Step2">
				<div class="fieldobject">
					<div class="fieldrow skinOrgn">
						<asp:LinkButton ID="LKBundoCom" runat="server" CssClass="linkMenu RightButton">Deselect community</asp:LinkButton>

						<!-- Begin UC COM_Select -->
						<CTRL:ComSel ID="UCcomSelector" runat="server" />
						<!-- End UC COM_Select -->
					</div>
				</div>
			</div>
		</asp:Panel>

		<asp:Panel ID="PNLelSelector" runat="server">
			<asp:Repeater ID="RPTfotElm" runat="server">
				<ItemTemplate>
					<div>
						<span class="col0">
							<asp:CheckBox id="CbxSel" runat="server" />
						</span>
						<span>
							<asp:DropDownList ID="DDLnewPosition" runat="server"></asp:DropDownList>
						</span>
						<span class="ColContent">
							<asp:literal ID="LITpreview" runat="server"></asp:literal>
						</span>
						<asp:HiddenField ID="HIDid" runat="server" />
					</div>
				</ItemTemplate>
			
			</asp:Repeater>
		</asp:Panel>
	</div>
    <div class="wiz_bot_nav clearfix">
		<div class="right stepButton"> 
        		<asp:LinkButton ID="LKBundob" runat="server" CssClass="linkMenu " OnClientClick="closeDialog('detailFile');">#Undo</asp:LinkButton>
                <asp:LinkButton ID="LKBbackb" runat="server" CssClass="linkMenu">#Back</asp:LinkButton>
				<asp:LinkButton ID="LKBnextb" runat="server" CssClass="linkMenu ">#Next</asp:LinkButton>
				<asp:LinkButton ID="LKBconfirmb" runat="server" CssClass="linkMenu">#Confirm</asp:LinkButton>
        </div>
    </div>
</div>

<%--<asp:LinkButton ID="LKBcomSelector" runat="server" CssClass="linkMenu ">#Com Selector</asp:LinkButton>
<asp:LinkButton ID="LKBorgnSelector" runat="server" CssClass="linkMenu ">#Com Selector</asp:LinkButton>        --%>