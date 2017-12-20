<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddField.ascx.vb" Inherits="Comunita_OnLine.UC_AddField" %>

<%@ Register Src="~/Modules/CallForPapers/UC/UC_InputField.ascx" TagName="CTRLInputField" TagPrefix="CTRL" %>

<asp:MultiView ID="MLVaddField" runat="server">
	<asp:View ID="VIWfield" runat="server">
		<div class="leftfield accordion clearfix">
			<div class="accordion-group columnblock " runat="server" id="DVgenericFields">
				<asp:Label ID="LBgenericFields" runat="server" cssclass="title accordion-handle">Generici</asp:Label>
				<ul class="fieldtypes">
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypeSingleLine" runat="server" value="1.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged"  AutoPostBack="true"/>
						<asp:Label ID="LBtypesSingleLine" runat="server" AssociatedControlID="RBtypeSingleLine"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesMultiLine" runat="server" value="2.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesMultiLine" runat="server" AssociatedControlID="RBtypesMultiLine"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDropDownList" runat="server" value="12.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDropDownList" runat="server" AssociatedControlID="RBtypesDropDownList"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesRadioButtonList" runat="server" value="11.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesRadioButtonList" runat="server" AssociatedControlID="RBtypesRadioButtonList"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesCheckboxList" runat="server" value="13.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesCheckboxList" runat="server" AssociatedControlID="RBtypesCheckboxList"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesNote" runat="server" value="19.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesNote" runat="server" AssociatedControlID="RBtypesNote"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesTableSimple" runat="server" value="30.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTableSimple" runat="server" AssociatedControlID="RBtypesTableSimple">Tabella semplice</asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesTableReport" runat="server" value="32.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTableReport" runat="server" AssociatedControlID="RBtypesTableReport">Tabella report</asp:Label>
					</li>
                    <li class="fieldtype">
						<asp:RadioButton ID="RBtypesTableSummary" runat="server" value="34.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTableSummary" runat="server" AssociatedControlID="RBtypesTableSummary">Tabella report</asp:Label>
					</li>
				</ul>
			</div>
			<div class="accordion-group columnblock " runat="server" id="DVspecificFields">
				<asp:Label ID="LBspecificFields" runat="server" cssclass="title accordion-handle">*Generici</asp:Label>
				<ul class="fieldtypes">
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesName" runat="server" value="17.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesName" runat="server" AssociatedControlID="RBtypesName"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesSurname" runat="server" value="18.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesSurname" runat="server" AssociatedControlID="RBtypesSurname"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesMail" runat="server" value="4.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesMail" runat="server" AssociatedControlID="RBtypesMail"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesZipCode" runat="server" value="10.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesZipCode" runat="server" AssociatedControlID="RBtypesZipCode"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesVatCode" runat="server" value="16.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesVatCode" runat="server" AssociatedControlID="RBtypesVatCode"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesTaxCode" runat="server" value="9.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTaxCode" runat="server" AssociatedControlID="RBtypesTaxCode"></asp:Label>
					</li>
					
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesTelephoneNumber" runat="server" value="5.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTelephoneNumber" runat="server" AssociatedControlID="RBtypesTelephoneNumber"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesFileInput" runat="server" value="20.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesFileInput" runat="server" AssociatedControlID="RBtypesFileInput"></asp:Label>
					</li>
					
				</ul>
			</div>
			<div class="accordion-group columnblock " runat="server" id="DVdisclaimerFields">
				<asp:Label ID="LBdisclaimerFields" runat="server" cssclass="title accordion-handle">*Disclaimer</asp:Label>
				<ul class="fieldtypes">
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDisclaimerStandard" runat="server" value="3.1" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDisclaimerStandard" runat="server" AssociatedControlID="RBtypesDisclaimerStandard"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDisclaimerCustomDisplayOnly" runat="server" value="3.4" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDisclaimerCustomDisplayOnly" runat="server" AssociatedControlID="LBtypesDisclaimerCustomDisplayOnly"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDisclaimerCustomSingleOption" runat="server" value="3.2" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDisclaimerCustomSingleOption" runat="server" AssociatedControlID="RBtypesDisclaimerCustomSingleOption"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDisclaimerCustomMultiOptions" runat="server" value="3.3" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDisclaimerCustomMultiOptions" runat="server" AssociatedControlID="RBtypesDisclaimerCustomMultiOptions"></asp:Label>
					</li>
				</ul>
			</div>
			<div class="accordion-group columnblock " runat="server" id="DVcompanyFields">
				<asp:Label ID="LBcompanyFields" runat="server" cssclass="title accordion-handle">*Company fields</asp:Label>
				<ul class="fieldtypes">
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesCompanyCode" runat="server" value="14.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesCompanyCode" runat="server" AssociatedControlID="RBtypesCompanyCode"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesCompanyTaxCode" runat="server" value="15.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesCompanyTaxCode" runat="server" AssociatedControlID="RBtypesCompanyTaxCode"></asp:Label>
					</li>
				</ul>
			</div>
			<div class="accordion-group columnblock " runat="server" id="DVdateFields">
				<asp:Label ID="LBdateFields" runat="server" cssclass="title accordion-handle">*Date fields</asp:Label>
				<ul class="fieldtypes">
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDate" runat="server" value="6.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDate" runat="server" AssociatedControlID="RBtypesDate"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesDateTime" runat="server" value="7.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesDateTime" runat="server" AssociatedControlID="RBtypesDateTime"></asp:Label>
					</li>
					<li class="fieldtype">
						<asp:RadioButton ID="RBtypesTime" runat="server" value="8.0" GroupName="fieldType" OnCheckedChanged="RBfieldType_CheckedChanged" AutoPostBack="true"/>
						<asp:Label ID="LBtypesTime" runat="server" AssociatedControlID="RBtypesTime"></asp:Label>
					</li>
				</ul>
			</div>
		</div>
		<div class="rightfield clearfix preview">
			<div class="divpreview" id="DVpreview" runat="server">
				<asp:Label ID="LBfieldName" runat="server" cssclass="title accordion-handle"></asp:Label>
				<CTRL:CTRLInputField ID="CTRLinputField" runat="server" />
				<asp:MultiView ID="MLVoptions" runat="server">
					<asp:View ID="VIWnoOptions" runat="server">
					</asp:View>
					<asp:View ID="VIWstandardFields" runat="server">
						<!-- gruppo opzioni nidificato dentro divpreview -->
						<div class="options">
							<asp:Label ID="LBfieldOptions" runat="server" cssclass="title"></asp:Label>
							<div class="fieldobject singleline vatcode">
								<div class="fieldrow fielddescription">
									<asp:Label ID="LBstandardOptionsDescription" runat="server" cssclass="description"></asp:Label>
								</div>
								<div class="fieldrow fieldinput">
									<asp:Label ID="LBstandardFieldsNumber_t" runat="server" cssclass="description" AssociatedControlID="TXBstandardFieldsNumber"></asp:Label>
									<asp:TextBox ID="TXBstandardFieldsNumber" runat="server" cssclass="inputtext"></asp:TextBox>
									<asp:RangeValidator ID="RNVstandardFieldsNumber" runat="server" ControlToValidate="TXBstandardFieldsNumber" MinimumValue="1" MaximumValue="30" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
									<asp:Label ID="LBstandardFieldsNumberHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBstandardFieldsNumber"></asp:Label>
								</div>
							</div>
						</div>
						<!-- fine gruppo opzioni --> 
					</asp:View>
					<asp:View ID="VIWmultipleChoiceFields" runat="server">
						<!-- gruppo opzioni nidificato dentro divpreview -->
						<div class="options">
							<asp:Label ID="LBmultipleFieldOptions" runat="server" cssclass="title"></asp:Label>
							<div class="fieldobject singleline vatcode">
								<div class="fieldrow fielddescription">
									<asp:Label ID="LBadvancedFieldOptionsDescription" runat="server" cssclass="description"></asp:Label>
								</div>
								<div class="fieldrow fieldinput" id="DVspecialFieldsNumber" runat="server" visible="false">
									<asp:Label ID="LBspecialFieldsNumber_t" runat="server" cssclass="description" AssociatedControlID="TXBspecialFieldsNumber"></asp:Label>
									<asp:TextBox ID="TXBspecialFieldsNumber" runat="server" cssclass="inputtext"></asp:TextBox>
									<asp:RangeValidator ID="RNVspecialFieldsNumber" runat="server" ControlToValidate="TXBspecialFieldsNumber" MinimumValue="1" MaximumValue="30" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
									<asp:Label ID="LBspecialFieldsNumberHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBspecialFieldsNumber"></asp:Label>
								</div>
								<div class="fieldrow fieldinput">
									<asp:Label ID="LBadvancedFieldOptionsList" runat="server" cssclass="description" AssociatedControlID="TXBfieldOptions"></asp:Label>
									<asp:TextBox ID="TXBfieldOptions" runat="server" cssclass="inputtext" TextMode="MultiLine" Rows="5"></asp:TextBox>
									<asp:Label ID="LBadvancedFieldOptionsListHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBfieldOptions"></asp:Label>
								</div>
							</div>
						</div>
						<!-- fine gruppo opzioni -->    
					</asp:View>
				</asp:MultiView>
			</div>
			&nbsp;
		</div>
	</asp:View>
	<asp:View ID="VIWempty" runat="server">
	
	</asp:View>
</asp:MultiView>
<asp:Literal ID="LTcssClass" runat="server" Visible="false">accordion-group columnblock </asp:Literal>
<%--<asp:MultiView ID="MLVaddField" runat="server">
	<asp:View ID="VIWfield" runat="server">
		<div class="leftfield clearfix">
			<div>
				<asp:Label ID="LBgenericFields" runat="server" cssclass="title">Generici</asp:Label>
				<asp:Repeater ID="RPTgenericItems" runat="server">
					<HeaderTemplate>
						<ul class="fieldtypes">
					</HeaderTemplate>
					<ItemTemplate>
						 <li class="fieldtype" id="field-<%#Container.DataItem.Id %>">                     
							<input name="type" type="radio" value="<%#Container.DataItem.Id %>" id="radio-<%#Container.DataItem.Id %>"  />
							<label for="radio-<%#Container.DataItem.Id %>"><%#Container.DataItem.Name%></label>
						 </li>
					</ItemTemplate>
					<FooterTemplate>
						 </ul>
					</FooterTemplate>
				</asp:Repeater>
			</div>
			<div>
				<asp:Label ID="LBspecificFields" runat="server" cssclass="title">Generici</asp:Label>
				<asp:Repeater ID="RPTspecificItems" runat="server">
					<HeaderTemplate>
						<ul class="fieldtypes">
					</HeaderTemplate>
					<ItemTemplate>
						 <li class="fieldtype" id="field-<%#Container.DataItem.Id %>">
							<input name="type" type="radio" value="<%#Container.DataItem.Id %>" id="radio-<%#Container.DataItem.Id %>" />
							<label for="radio-<%#Container.DataItem.Id %>"><%#Container.DataItem.Name%></label>
						 </li>
					</ItemTemplate>
					<FooterTemplate>
						 </ul>
					</FooterTemplate>
				</asp:Repeater>
			</div>
			
		</div>
		<div class="rightfield clearfix preview">
			<asp:Repeater id="RPTrenderFields" runat="server">
				<HeaderTemplate>
		
				</HeaderTemplate>
				<ItemTemplate>
					<div class="divpreview" id="preview-field-<%#cint(Container.DataItem.Field.Type) %>">
						<asp:literal ID="LTfieldType" runat="server" Text="<%#cint(Container.DataItem.Field.Type) %>" Visible="false"></asp:literal>
						<asp:Label ID="LBfieldName" runat="server" cssclass="title"></asp:Label>
						<CTRL:CTRLInputField ID="CTRLinputField" runat="server" />
						<asp:MultiView ID="MLVoptions" runat="server">
							<asp:View ID="VIWnoOptions" runat="server">
							</asp:View>
							<asp:View ID="VIWstandardFields" runat="server">
								<!-- gruppo opzioni nidificato dentro divpreview -->
								<div class="options">
									<asp:Label ID="LBfieldOptions" runat="server" cssclass="title"></asp:Label>
									<div class="fieldobject singleline vatcode">
										<div class="fieldrow fielddescription">
											<asp:Label ID="LBstandardOptionsDescription" runat="server" cssclass="description"></asp:Label>
										</div>
										<div class="fieldrow fieldinput">
											<asp:Label ID="LBstandardFieldsNumber_t" runat="server" cssclass="description" AssociatedControlID="TXBstandardFieldsNumber"></asp:Label>
											<asp:TextBox ID="TXBstandardFieldsNumber" runat="server" cssclass="inputtext"></asp:TextBox>
											<asp:RangeValidator ID="RNVstandardFieldsNumber" runat="server" ControlToValidate="TXBstandardFieldsNumber" MinimumValue="1" MaximumValue="30" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
											<asp:Label ID="LBstandardFieldsNumberHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBstandardFieldsNumber"></asp:Label>
										</div>
									</div>
								</div>
								<!-- fine gruppo opzioni --> 
							</asp:View>
							<asp:View ID="VIWmultipleChoiceFields" runat="server">
								<!-- gruppo opzioni nidificato dentro divpreview -->
								<div class="options">
									<asp:Label ID="LBmultipleFieldOptions" runat="server" cssclass="title"></asp:Label>
									<div class="fieldobject singleline vatcode">
										<div class="fieldrow fielddescription">
											<asp:Label ID="LBadvancedFieldOptionsDescription" runat="server" cssclass="description"></asp:Label>
										</div>
										<div class="fieldrow fieldinput">
											<asp:Label ID="LBadvancedFieldOptionsList" runat="server" cssclass="description" AssociatedControlID="TXBfieldOptions"></asp:Label>
											<asp:TextBox ID="TXBfieldOptions" runat="server" cssclass="inputtext" TextMode="MultiLine" Rows="5"></asp:TextBox>
											<asp:Label ID="LBadvancedFieldOptionsListHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBfieldOptions"></asp:Label>
										</div>
									</div>
								</div>
							   <!-- fine gruppo opzioni -->    
							</asp:View>
						</asp:MultiView>
					</div>
				</ItemTemplate>
				<FooterTemplate>
		
				</FooterTemplate>
			</asp:Repeater>
			&nbsp;
		</div>
	</asp:View>
	<asp:View ID="VIWempty" runat="server">
	
	</asp:View>
</asp:MultiView>
--%>