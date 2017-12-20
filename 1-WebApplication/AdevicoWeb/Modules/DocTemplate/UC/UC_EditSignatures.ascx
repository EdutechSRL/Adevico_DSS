<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditSignatures.ascx.vb" Inherits="Comunita_OnLine.UC_EditSignatures" %>
<%@ Register TagPrefix="CTRL" TagName="Signature" Src="~/Modules/DocTemplate/Uc/UC_EditSignature.ascx"%>
<%@ Register Src="~/Modules/DocTemplate/Uc/UC_EditVersions.ascx" TagName="CTRLprevVersion" TagPrefix="CTRL" %>

<div class="header">
    <div class="buttonwrapper">
        <asp:LinkButton ID="LKBaddSignature" runat="server" CssClass="linkMenu">Add signature</asp:LinkButton>
    </div>
</div>

<div class="tree">
	<ul class="sections playmode">
		<li class="section clearfix autoOpen" id="section_1">

			<div class="sectioncontent">
                <script type="text/javascript">
                    var setSelector = "#sortable";
                    function getOrder() {
                        $('#<%=Me.HF_Order.ClientID%>').val($(setSelector).sortable("toArray"));
                    }
                </script>
                <asp:HiddenField ID="HF_Order" runat="server"/>
				
				<asp:Repeater ID="RPTsignatures" runat="server" EnableViewState="true">
                    <HeaderTemplate>
                        <ul class="fields signature" id="sortable">        
                    </HeaderTemplate>
					<ItemTemplate>
						<li class="cfield clearfix autoOpen" id="sgn_<%#Container.DataItem.Data.Id %>">
							<div class="externalleft">
								<span class="movecfield">M</span>
							</div>
							<CTRL:Signature id="UCsignature" runat="server" OnDeleteSignature="OnDeleteSignature"></CTRL:Signature>
						</li>    
					</ItemTemplate>
                    <FooterTemplate>
                        </ul>        
                    </FooterTemplate>
				</asp:Repeater>
                <asp:Label ID="LBLnosign" runat="server">#Nessuna firma</asp:Label>
			</div>
		</li>
    </ul>

    <asp:Panel ID="PNLsubVersion" runat="server">
        <fieldset class="light">
	        <legend>
                <asp:literal ID="LITrevision_t" runat="server">*Revision</asp:literal>
            </legend>
            <div class="fieldrow">
                <CTRL:CTRLprevVersion ID="UCprevVersion" runat="server" />
            </div>
        </fieldset>
    </asp:Panel>
</div>