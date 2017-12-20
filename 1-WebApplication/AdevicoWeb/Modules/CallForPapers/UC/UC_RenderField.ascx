<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_RenderField.ascx.vb" Inherits="Comunita_OnLine.UC_RenderField" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>

<div id="DVtagContainer" class="tagContainer" runat="server">

<asp:MultiView ID="MLVfield" runat="server">
	<asp:View ID="VIWunknown" runat="server"></asp:View>
	<asp:View ID="VIWempty" runat="server"></asp:View>
	<asp:View ID="VIWsingleline" runat="server">
		<div class="fieldobject renderobject singleline">
			<div class="fieldrow fieldinput" runat="server" id="DVsingleline">
				<span class="revisionfield" id="SPNsinglelineRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXsinglelineRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBsinglelineRevisionField" runat="server"  visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" id="LBsinglelineText" AssociatedControlID="LBsinglelineValue" CssClass="fieldlabel">Text</asp:Label>
				<asp:Label runat="server" ID="LBsinglelineTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBsinglelineDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:Label runat="server" id="LBsinglelineValue" CssClass="readonlyinput"></asp:Label>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<span class="maxchar" runat="server" id="SPNmaxCharsingleline"  Visible="false">
						<asp:Literal ID="LTmaxCharssingleline" runat="server"></asp:Literal>
						<span class="availableitems"><asp:Literal ID="LTsinglelineUsed" runat="server"></asp:Literal></span>/<span class="totalitems"><asp:Literal ID="LTsinglelineTotal" runat="server"></asp:Literal></span>
					</span>
					<asp:Label ID="LBerrorMessagesingleline" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>        
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWmultiline" runat="server">
	   <div class="fieldobject renderobject multiline">
			<div class="fieldrow fieldinput" runat="server" id="DVmultiline">
				<span class="revisionfield" id="SPNmultilineRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXmultilineRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBmultilineRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" id="LBmultilineText" AssociatedControlID="LBmultilineValue" CssClass="fieldlabel">Text</asp:Label>
				<asp:Label runat="server" ID="LBmultilineTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBmultilineDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:Label runat="server" id="LBmultilineValue" CssClass="readonlytextarea"></asp:Label>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<span class="maxchar" runat="server" id="SPNmaxCharmultiline"  Visible="false">
						<asp:Literal ID="LTmaxCharsmultiline" runat="server"></asp:Literal>
						<span class="availableitems"><asp:Literal ID="LTmultilineUsed" runat="server"/></span>/<span class="totalitems"><asp:Literal ID="LTmultilineTotal" runat="server"></asp:Literal></span>
					</span>
					<asp:Label ID="LBerrorMessagemultiline" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span> 
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWdisclaimer" runat="server">
		<div class="fieldobject renderobject disclaimer">
			<div class="fieldrow fieldinput" runat="server" id="DVdisclaimer">
				 <span class="revisionfield" id="SPNdisclaimerRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXdisclaimerRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBdisclaimerRevisionField"  runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" id="LBdisclaimerText" CssClass="fieldlabel">Disclaimer</asp:Label>
				<asp:Label runat="server" ID="LBdisclaimerTags" CssClass="fieldTags"></asp:Label>
				<div class="fieldmaincontent clearfix">
					<!--<div class="left">-->
						<div class="disclaimerwrapper">
							<div class="disclaimertext">
								<asp:Literal ID="LTdisclaimerDescription" runat="server"></asp:Literal>
							</div>
							<div class="disclaimerinput">
								<asp:MultiView ID="MLVdisclaimer" runat="server">
									<asp:View ID="VIWNone" runat="server"></asp:View>
									<asp:View ID="VIWCustomDisplayOnly" runat="server"></asp:View>
									<asp:View ID="VIWStandard" runat="server">
										<asp:Label runat="server" ID="LBDisclaimerAccept" AssociatedControlID="RBDisclaimerAccept" CssClass="disclaimerlabel">Accept</asp:Label>
										<input type="radio" runat="server" id="RBDisclaimerAccept" readonly="true" disabled="disabled" />                        
										<asp:Label runat="server" ID="LBDisclaimerRefuse" AssociatedControlID="RBDisclaimerRefuse" CssClass="disclaimerlabel">Refuse</asp:Label>
										<input type="radio" runat="server" id="RBDisclaimerRefuse" readonly="true" disabled="disabled" />
									</asp:View>
									<asp:View ID="VIWCustomSingleOption" runat="server">
										<span id="" class="inputradiobuttonlist">
										<asp:Repeater ID="RPTsingleOption" runat="server">
											<ItemTemplate>
												<span>
													<asp:Label runat="server" ID="LBoptionName" AssociatedControlID="RBoption" CssClass="disclaimerlabel"><%#Container.DataItem.Name %></asp:Label>
													<input type="radio" runat="server" id="RBoption" readonly="true" disabled="disabled" />
												</span>
											</ItemTemplate>
										</asp:Repeater>
										</span>
									</asp:View>
									<asp:View ID="VIWCustomMultiOptions" runat="server">
										<span id="SPNdisclaimerCheckboxlist" class="inputcheckboxlist" runat="server">
										<asp:Repeater ID="RPTmultiOption" runat="server">
											<ItemTemplate>
												<span class="">
													<asp:Label runat="server" ID="LBoptionName" AssociatedControlID="CBoption" CssClass="disclaimerlabel"><%#Container.DataItem.Name %></asp:Label>
													<input type="checkbox" runat="server" id="CBoption" readonly="true" disabled="disabled"  />
												</span>
											</ItemTemplate>
										</asp:Repeater>
										</span>
										 <br/>
										<span class="fieldinfo ">
											<span class="minmax" runat="server" id="SPNminMaxCustomMultiOptions"  Visible="false">
												<asp:Literal ID="LTminOptionsCustomMultiOptions" runat="server"></asp:Literal>
												<asp:label ID="LBminOptionCustomMultiOptions" CssClass="min" runat="server"></asp:label>
												<asp:Literal ID="LTmaxOptionsCustomMultiOptions" runat="server"></asp:Literal>
												<asp:label ID="LBmaxOptionCustomMultiOptions" CssClass="max" runat="server"></asp:label>
											</span>
											<asp:Label ID="LBerrorMessagedisclaimerCustomMultiOptions" runat="server" Visible="false" cssClass="generic"></asp:Label>
										</span>     
									</asp:View>
								</asp:MultiView>
							</div>                    
						</div>    
					<!--</div>
					<div class="right">
						&nbsp;- sostituire &nbsp; con il contenuto che si vuole mettere a destra--
					</div>-->
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagedisclaimer" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>                
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWradioButtonlist" runat="server">
		<div class="fieldobject renderobject radiobuttonlist">
			<div class="fieldrow fieldinput" runat="server" id="DVradiobuttonlist">  
				<span class="revisionfield" id="SPNradiobuttonlistRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXradiobuttonlistRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBradiobuttonlistRevisionField"  runat="server" visible="false"></asp:Label>
				</span>              
				<asp:Label runat="server" ID="LBradioButtonlistText" AssociatedControlID="RBLitems" CssClass="fieldlabel">Items</asp:Label>
				<asp:Label runat="server" ID="LBradiobuttonlistTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBradioButtonlistDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:RadioButtonList runat="server" CssClass="inputradiobuttonlist" ID="RBLitems" RepeatLayout="Flow" Enabled="false"></asp:RadioButtonList>                
						<span class="textoption" id="SPNtextOptionRadioButtonList" runat="server" visible="false">
							<asp:TextBox ID="TXBradiobuttonlist" runat="server" CssClass="inputtext" ReadOnly="true"></asp:TextBox>
							<%--<asp:Label ID="LBradiobuttonlist" runat="server" CssClass="readonlyinput"></asp:Label>--%>
						</span>    
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>

				<br/>
				<span class="fieldinfo ">
					<span class="minmax" runat="server" id="SPNminMaxradioButtonlist"  Visible="false">
						<asp:Literal ID="LTminOptionsradioButtonlist" runat="server"></asp:Literal>
						<asp:label ID="LBminOptionradioButtonlist" CssClass="min" runat="server"></asp:label>
						<asp:Literal ID="LTmaxOptionsradioButtonlist" runat="server"></asp:Literal>
						<asp:label ID="LBmaxOptionradioButtonlist" CssClass="max" runat="server"></asp:label>
					</span>
					<asp:Label ID="LBerrorMessageradioButtonlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>    
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWcheckboxlist" runat="server">
		<div class="fieldobject renderobject checkboxlist">
			<div class="fieldrow fieldinput" runat="server" id="DVcheckboxlist">
				<span class="revisionfield" id="SPNcheckboxlistRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXcheckboxlistRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBcheckboxlistRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBcheckboxlistText" CssClass="fieldlabel">Items</asp:Label>
				<asp:Label runat="server" ID="LBcheckboxlistTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBcheckboxlistDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<span id="SPNcheckboxlist" class="inputcheckboxlist ver" runat="server">
							<asp:Repeater ID="RPTcheckboxlist" runat="server">
								<ItemTemplate>
									<span class="group">
										<input type="checkbox" runat="server" id="CBoption" value="<%#Container.DataItem.Id %>" disabled="disabled" />
										<asp:Label runat="server" ID="LBoptionName" AssociatedControlID="CBoption"><%#Container.DataItem.Name %></asp:Label>
									</span>
								</ItemTemplate>
							</asp:Repeater>
						</span>
						<span class="textoption" id="SPNtextOptionCheckBoxList" runat="server" visible="false">
							<asp:TextBox ID="TXBcheckboxlist" runat="server" CssClass="inputtext" ReadOnly="true"></asp:TextBox>
							<asp:Label ID="LBchecboxlist" CssClass="readonlyinput" Visible="false" runat="server" ></asp:Label>
						</span>   
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<span class="minmax" runat="server" id="SPNminMaxcheckboxlist" Visible="false">
						<asp:Literal ID="LTminOptionscheckboxlist" runat="server"></asp:Literal>
						<asp:label ID="LBminOptioncheckboxlist" CssClass="min" runat="server"></asp:label>
						<asp:Literal ID="LTmaxOptionscheckboxlist" runat="server"></asp:Literal>
						<asp:label ID="LBmaxOptioncheckboxlist" CssClass="max" runat="server"></asp:label>
					</span>
					<asp:Label ID="LBerrorMessagecheckboxlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>   
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWdropdownlist" runat="server">
		<div class="fieldobject renderobject dropdownlist">
			<div class="fieldrow fieldinput" runat="server" id="DVdropdownlist">
				<span class="revisionfield" id="SPNdropdownlistRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXdropdownlistRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBdropdownlistRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBdropdownlistText" AssociatedControlID="DDLitems" CssClass="fieldlabel">Items</asp:Label>
				<asp:Label runat="server" ID="LBdropdownlistTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBdropdownlistDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:DropDownList runat="server" ID="DDLitems" Enabled="false"></asp:DropDownList>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagedropdownlist" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>   
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWdatetime" runat="server">
		<div class="fieldobject  renderobject datetime">
			<div class="fieldrow fieldinput" runat="server" id="DVdatetime">
				<span class="revisionfield" id="SPNdatetimeRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXdatetimeRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBdatetimeRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBdatetimeText" CssClass="fieldlabel"></asp:Label>
				<asp:Label runat="server" ID="LBdatetimeTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBdateTimeDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:Label runat="server" ID="LBdatetimeValueData" CssClass="readonlyinput"></asp:Label>
						<asp:Label runat="server" ID="LBdatetimeValueHour" CssClass="readonlyinput"></asp:Label>
						.
						<asp:Label runat="server" ID="LBdatetimeValueMinutes" CssClass="readonlyinput"></asp:Label>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagedatetime" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span> 
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWdate" runat="server">
		<div class="fieldobject renderobject date">
			<div class="fieldrow fieldinput" runat="server" id="DVdate">
				<span class="revisionfield" id="SPNdateRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXdateRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBdateRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBdateText" AssociatedControlID="LBdateValue" CssClass="fieldlabel">Items</asp:Label>
				<asp:Label runat="server" ID="LBdateTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBdateDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:Label runat="server" ID="LBdateValue" CssClass="readonlyinput"></asp:Label>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagedate" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWtime" runat="server">
		<div class="fieldobject renderobject time">
			<div class="fieldrow fieldinput" runat="server" id="DVtime">
				<span class="revisionfield" id="SPNtimeRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXtimeRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBtimeRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBTimeText"  CssClass="fieldlabel"></asp:Label>
                <asp:Label runat="server" ID="LBtimeTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBtimeDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<asp:Label runat="server" ID="LBtimeValueHour" CssClass="readonlyinput"></asp:Label>
						.
						<asp:Label runat="server" ID="LBtimeValueMinutes" CssClass="readonlyinput"></asp:Label>
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagetime" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWnote" runat="server">
		<div class="fieldobject renderobject note">
			 <div class="fieldrow fieldinput">
				<asp:Label runat="server" id="LBnoteText" CssClass="fieldlabel">Text</asp:Label>
                 <asp:Label runat="server" ID="LBnoteTags" CssClass="fieldTags"></asp:Label>
			</div>
			<div class="fieldrow fielddescription">                
				<asp:Label runat="server" ID="LBNoteDescription" CssClass="description">Description</asp:Label>
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWfileinput" runat="server">
		<div class="fieldobject renderobject fileupload">
			<div class="fieldrow fieldinput" runat="server" id="DVfileinput">
				<span class="revisionfield" id="SPNfileinputRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXfileinputRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBfileinputRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBfileinputText" CssClass="fieldlabel">File</asp:Label>
                <asp:Label runat="server" ID="LBfileinputTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">
					<asp:Label runat="server" ID="LBfileinputDescription" CssClass="description">Description</asp:Label>
				</div>
				<div class="fieldmaincontent clearfix">
					<div class="left">
						<CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false" Visible="false"  />
					</div>
					<div class="right">
						&nbsp;<%-- sostituire &nbsp; con il contenuto che si vuole mettere a destra--%>
					</div>
				</div>
				<br/>
				<span class="fieldinfo ">
					<asp:Label ID="LBerrorMessagefileinput" runat="server" Visible="false" cssClass="generic"></asp:Label>
				</span>     
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWTableSimple" runat="server">
		<div class="fieldobject renderobject tableSimple">
			<div class="fieldrow fieldinput" runat="server" id="DVtablesimple">
				<span class="revisionfield" id="SPNtablesimpleRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXtablesimpleRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBtablesimpleRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBtablesimpleText" AssociatedControlID="LBtablesimpleDescription" CssClass="fieldlabel">Items</asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBtablesimpleDescription" CssClass="description">Description</asp:Label>
				</div>
			</div>
			<div class="fieldmaincontent clearfix">
				<table class="tableReport table table-striped">
					<asp:literal ID="LTtableSimpleContent" runat="server"></asp:literal>
				</table>
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWTableReport" runat="server">
	   <div class="fieldobject renderobject tableSimple">
			<div class="fieldrow fieldinput" runat="server" id="DVtablereport">
				<span class="revisionfield" id="SPNtablereportRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXtablereportRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBtablereportRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBtablereportText" AssociatedControlID="LBtablereportDescription" CssClass="fieldlabel">Items</asp:Label>
                <asp:Label runat="server" ID="LBtablereportTags" CssClass="fieldTags"></asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBtablereportDescription" CssClass="description">Description</asp:Label>
				</div>
			</div>
			<div class="fieldmaincontent clearfix">
				<table class="tableReport table table-striped">
					<asp:literal ID="LTtablereportContent" runat="server"></asp:literal>
				</table>
			</div>
		</div>
	</asp:View>
	<asp:View ID="VIWTableSummary" runat="server">
	   <div class="fieldobject renderobject tableSimple">
			<div class="fieldrow fieldinput" runat="server" id="DVtablesummary">
				<span class="revisionfield" id="SPNtablesummaryRevisionField" runat="server" visible="false">
					<asp:CheckBox ID="CBXtablesummaryRevisionField" runat="server" visible="false"/>
					<asp:Label ID="LBtablesummaryRevisionField" runat="server" visible="false"></asp:Label>
				</span>
				<asp:Label runat="server" ID="LBtablesummaryText" AssociatedControlID="LBtablesummaryDescription" CssClass="fieldlabel">Items</asp:Label>
				<div class="fielddescription">                
					<asp:Label runat="server" ID="LBtablesummaryDescription" CssClass="description">Description</asp:Label>
				</div>
			</div>
			<div class="fieldmaincontent clearfix">
				<table class="tableReport table table-striped">
				<tr>
					<th>Tabella</th>
					<th>Totale</th>
				</tr>
				<asp:repeater ID="RPTtabelSummary" runat="server">
				
					<ItemTemplate>
				<tr> 
					<td><asp:literal ID="LTtableName" runat="server"></asp:literal></td>
					<td><asp:literal ID="LTtableTotal" runat="server"></asp:literal></td>
				</tr>
					</ItemTemplate>
				</asp:repeater>
				<tr> 
					<td>Totale:</td>
					<td><asp:literal ID="LTsummaryTotal" runat="server"></asp:literal></td>
				</tr>

			</table>
			</div>
		</div>
	</asp:View>
</asp:MultiView>
<asp:Literal ID="LTrevisionedCssClass" runat="server" Visible="false">revisionfield revisioned</asp:Literal>
</div>