 <%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditField.ascx.vb" Inherits="Comunita_OnLine.UC_EditField"  %>


<asp:MultiView ID="MLVfield" runat="server">
	<asp:View ID="VIWunknown" runat="server">
	
	</asp:View>
	<asp:View ID="VIWempty" runat="server">
	
	</asp:View>
	<asp:View ID="VIWfield" runat="server">
		<div class="fieldrow fielddescription">
			<asp:Label AssociatedControlID="TXBdescription" runat="server" id="LBdisclaimer_t"  Visible="false">Description</asp:Label>
			<asp:Label AssociatedControlID="TXBdescription" runat="server" id="LBfieldDescription_t" CssClass="fieldlabel" >Description</asp:Label>
			<asp:TextBox runat="server" ID="TXBdescription" Columns="40" class="textarea" TextMode="MultiLine"></asp:TextBox>
		</div>
		<div class="fieldrow fielddescription">
			<asp:Label ID="LBfieldtags_t" runat="server" CssClass="fieldlabel">Tags:</asp:Label>
			<asp:textbox id="TXBtags" runat="Server" CssClass="tagsinput hide"></asp:textbox>
		</div>
		<div class="fieldrow fieldhelp" runat="server" id="DVhelp">
			<asp:Label AssociatedControlID="TXBhelp" runat="server" ID="LBfieldHelp_t" CssClass="fieldlabel" >Help</asp:Label>
			<asp:TextBox runat="server" ID="TXBhelp" CssClass="inputtext"></asp:TextBox>
		</div>
		<div class="fieldrow fieldmaxchar" runat="server" id="DVmaxChar" visible="false">			
			<asp:Label AssociatedControlID="TXBmaxChar" runat="server" ID="LBfieldMaxChar_t" CssClass="fieldlabel" >Max Char</asp:Label>            
			<asp:TextBox runat="server" ID="TXBmaxChar" CssClass="inputtext"></asp:TextBox>
			<asp:RangeValidator ID="RNVBmaxChar" ControlToValidate="TXBmaxChar" MinimumValue="0" MaximumValue="300000" SetFocusOnError="true" Type="Double" runat="server" Display="None" Text="*"></asp:RangeValidator>
		</div>
		<div class="fieldrow fielddisclaimertype" runat="server" id="DVdisclaimerType" visible="false">
			<asp:Label AssociatedControlID="RBLdisclaimerType" runat="server" ID="LBfieldDisclaimerType_t" CssClass="fieldlabel" >Type:</asp:Label>     
			<div class="inlinewrapper">
				<asp:RadioButtonList ID="RBLdisclaimerType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="2" AutoPostBack="true">
					<asp:ListItem Value="Standard"></asp:ListItem>
					<asp:ListItem Value="CustomSingleOption"></asp:ListItem>
					<asp:ListItem Value="CustomMultiOptions"></asp:ListItem>
					<asp:ListItem Value="CustomDisplayOnly"></asp:ListItem>
				</asp:RadioButtonList>
			</div>
		</div>
		<%--<a href="#" class="linkMenu advanced">Avanzate</a>--%>
	   <asp:MultiView ID="MLVadvanced" runat="server" ActiveViewIndex="0">
			<asp:View ID="VIWnone" runat="server">
	
			</asp:View>
			<asp:View ID="VIWmultipleChoice" runat="server">
				<div class="fieldrow">
					<fieldset class="expandable disabled hideall" runat="server" id="FSmultipleChoice">
						<legend class="inlinelegend"><asp:Label ID="LBmultipleChoice_t" runat="server">Consenti risposte multiple</asp:Label><input type="checkbox" id="CBXmultipleChoice" runat="server"/></legend>
						<div class="fieldobject">
							<div class="fieldrow fieldmaxchar">
								<asp:Label ID="LBminOptions_t" CssClass="fieldlabel" runat="server"></asp:Label><asp:TextBox runat="server" ID="TXTminOptions" CssClass="inputtext"></asp:TextBox><span class="hint"><em>&nbsp;<asp:Literal ID="LTminOptionsInfo" runat="server"></asp:Literal></em></span><br />
								<asp:Label ID="LBmaxOptions_t" CssClass="fieldlabel" runat="server"></asp:Label><asp:TextBox runat="server" ID="TXTmaxOptions" CssClass="inputtext"></asp:TextBox><span class="hint"><em>&nbsp;<asp:Literal ID="LTmaxOptionsInfo" runat="server"></asp:Literal></em></span>
							</div>
						</div>
					</fieldset>
					<asp:Repeater ID="RPToptions" runat="server">
						<HeaderTemplate>
							<label for="" class="fieldlabel"><asp:Literal ID="LToptionsTitle" runat="server"></asp:Literal></label>
							<span class="fielditemswrapper">
							<ul class="fielditems">
						</HeaderTemplate>
						<ItemTemplate>
								<li class="fielditem clearfix" id="#option_<%#Container.DataItem.Id %>">
									<a name="option_<%#Container.DataItem.Id %>"></a>
									<div class="fielditemcontent clearfix">
										<asp:Label ID="LBmoveOption" runat="server" cssclass="movecfielditem">M</asp:Label>
										<input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorderoption"/>
										<asp:Literal ID="LTidOption" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
										<asp:Literal ID="LTisFreeValue" runat="server" Visible="false" Text="<%#Container.DataItem.IsFreeValue %>"></asp:Literal>
										<asp:Literal ID="LTisDefault" runat="server" Visible="false" Text="<%#Container.DataItem.IsDefault %>"></asp:Literal>
										<asp:TextBox ID="TXBoptionName" runat="server" CssClass="inputtext"></asp:TextBox>
										<span class="icons">
											<asp:Button ID="BTNsetDefaultOption" runat="server" CssClass="icon checked" CommandName="setDefault" Visible="false"/>
											<asp:Button ID="BTNremoveAsDefaultOption" runat="server" CssClass="icon notchecked" CommandName="removeDefault" Visible="false"/>
											<asp:Button ID="BTNdeleteOption" runat="server" Text="D" CssClass="icon delete" CommandName="virtualDelete"/>
										</span>
										<label>
											<asp:Literal id="LToptionInfo" runat="server" ></asp:Literal>
										</label>
									</div>
								</li>
						</ItemTemplate>
						<FooterTemplate>
								<li class="fielditem clearfix fixed" id="DVfooter" runat="server">
									<div class="fielditemcontent clearfix">
										<span class="movecfielditem" style="visibility:hidden;">&nbsp;</span>
										<input type="hidden" id="HDNdisplayPlaceholder">
										<a name="fieldaddoption_<%#IdField %>"></a>
										<asp:TextBox ID="TXBoptionAddName" runat="server"></asp:TextBox>
										<span class="icons">
											<asp:Button ID="BTNaddOption" runat="server" Text="ADD" CommandName="addOption"/>
											<asp:Button ID="BTNaddFreeOption" runat="server" CommandName="addFreeOption" />
										</span>
									</div>
								</li>
							</ul>
							</span>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</asp:View>
			<asp:View ID="VIWdropdownlist" runat="server">
				<div class="fieldrow">
					 <asp:Repeater ID="RPTcomboOptions" runat="server">
						<HeaderTemplate>
							<label for="" class="fieldlabel"><asp:Literal ID="LToptionsTitle" runat="server"></asp:Literal></label>
							<span class="fielditemswrapper">
							<ul class="fielditems">
						</HeaderTemplate>
						<ItemTemplate>
								<li class="fielditem clearfix" id="option_<%#Container.DataItem.Id %>">
									<div class="fielditemcontent clearfix">
										<asp:Label ID="LBmoveOption" runat="server" cssclass="movecfielditem">M</asp:Label>
										<input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorderoption"/>
										<asp:Literal ID="LTidOption" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
										  <asp:Literal ID="LTisFreeValue" runat="server" Visible="false" Text="<%#Container.DataItem.IsFreeValue %>"></asp:Literal>
										<asp:Literal ID="LTisDefault" runat="server" Visible="false" Text="<%#Container.DataItem.IsDefault %>"></asp:Literal>
										<a name="option_<%#Container.DataItem.Id %>"></a>
										<asp:TextBox ID="TXBoptionName" runat="server" CssClass="inputtext"></asp:TextBox>
										<span class="icons">
											<asp:Button ID="BTNsetDefaultOption" runat="server" CssClass="icon checked" CommandName="setDefault" Visible="false"/>
											<asp:Button ID="BTNremoveAsDefaultOption" runat="server" CssClass="icon notchecked" CommandName="removeDefault" Visible="false"/>
											<asp:Button ID="BTNdeleteOption" runat="server" Text="D" CssClass="icon delete" CommandName="virtualDelete"/>
										</span>
										<label>
											<asp:Literal id="LToptionInfo" runat="server" ></asp:Literal>
										</label>
									</div>
								</li>
						</ItemTemplate>
						<FooterTemplate>
								<li class="fielditem clearfix fixed" id="DVfooter" runat="server">
									<div class="fielditemcontent clearfix">
										<span class="movecfielditem" style="visibility:hidden;">&nbsp;</span><asp:TextBox ID="TXBoptionAddName" runat="server"></asp:TextBox><asp:Button ID="BTNaddOption" runat="server" Text="ADD" CommandName="addoption"  />
									</div>
								</li>
							</ul>
							</span>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</asp:View>
		   <asp:View ID="VIWtable" runat="server">
				<div class="fieldrow fieldtable">
					<span class="tablecols">
						<asp:Label AssociatedControlID="TXBtableCols" runat="server" ID="LBtableCols_t" CssClass="fieldlabel" >Columns</asp:Label>            
						<asp:TextBox runat="server" ID="TXBtableCols" CssClass="inputtext dinamycCols">1,2</asp:TextBox>
						<asp:Label runat="server" ID="LBtableCols_desc" CssClass="description" >Intestazioni colonne, separati da |</asp:Label>    
					</span>
					<span class="tablecols">
						<asp:Label AssociatedControlID="TXBmaxRows" runat="server" ID="LBtableRows" CssClass="fieldlabel" >Max Rows</asp:Label>            
						<asp:TextBox runat="server" ID="TXBmaxRows" CssClass="inputtext">1</asp:TextBox>
						<asp:RangeValidator ID="RNVmaxRows" ControlToValidate="TXBmaxRows" MinimumValue="1" MaximumValue="500" SetFocusOnError="true" Type="Double" runat="server" Display="None" Text="*"></asp:RangeValidator>
					</span>
					<span id="SPmaxTotal" runat="Server">
						<asp:Label AssociatedControlID="TXBtableMaxTotal" runat="server" ID="LBtableMaxTotal" CssClass="fieldlabel" >Totale massimo:</asp:Label>
						<asp:TextBox runat="server" ID="TXBtableMaxTotal" CssClass="inputtext">0</asp:TextBox>
					</span>
				</div>
			</asp:View>
		   <asp:View ID="VIWtableSummary" runat="server">
				
			</asp:View>
	   </asp:MultiView>
	</asp:View>
</asp:MultiView>

