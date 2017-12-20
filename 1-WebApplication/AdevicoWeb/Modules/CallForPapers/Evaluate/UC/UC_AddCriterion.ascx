<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AddCriterion.ascx.vb" Inherits="Comunita_OnLine.UC_AddCriterion" %>
<%@ Register Src="~/Modules/Dss/UC/UC_RatingSetScales.ascx" TagName="CTRLscales" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/CallForPapers/Evaluate/UC/UC_InputCriterion.ascx" TagName="CTRLInputCriterion" TagPrefix="CTRL" %>

<asp:MultiView ID="MLVaddCriterion" runat="server">
    <asp:View ID="VIWfield" runat="server">
        <div class="leftfield clearfix child">
            <div>
                <asp:Label ID="LBgenericCriteria" runat="server" cssclass="title">Generici</asp:Label>
                <ul class="fieldtypes">
                    <li class="fieldtype" runat="server" visible="false" id="LIcriterionTypeTextual">
                        <asp:RadioButton ID="RBtypesTextual" runat="server" value="1" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged"  AutoPostBack="true"/>
                        <asp:Label ID="LBtypesTextual" runat="server" AssociatedControlID="RBtypesTextual"></asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeBoolean">
                        <asp:RadioButton ID="RBtypesBoolean" runat="server" value="10" Text="Sì/No" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged"  AutoPostBack="true"/>
                        <asp:Label ID="LBtypesBoolean" runat="server" AssociatedControlID="RBtypesBoolean">Sì/No</asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeIntegerRange">
                        <asp:RadioButton ID="RBtypesIntegerRange" runat="server" value="2" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged" AutoPostBack="true"/>
                        <asp:Label ID="LBtypesIntegerRange" runat="server" AssociatedControlID="RBtypesIntegerRange"></asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeDecimalRange">
                        <asp:RadioButton ID="RBtypesDecimalRange" runat="server" value="3" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged" AutoPostBack="true"/>
                        <asp:Label ID="LBtypesDecimalRange" runat="server" AssociatedControlID="RBtypesDecimalRange"></asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeStringRange">
                        <asp:RadioButton ID="RBtypesStringRange" runat="server" value="4" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged" AutoPostBack="true"/>
                        <asp:Label ID="LBtypesStringRange" runat="server" AssociatedControlID="RBtypesStringRange"></asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeRatingScale">
                        <asp:RadioButton ID="RBtypesRatingScale" runat="server" value="5" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged" AutoPostBack="true"/>
                        <asp:Label ID="LBtypesRatingScale" runat="server" AssociatedControlID="RBtypesRatingScale"></asp:Label>
                    </li>
                    <li class="fieldtype" runat="server" id="LIcriterionTypeRatingScaleFuzzy">
                        <asp:RadioButton ID="RBtypesRatingScaleFuzzy" runat="server" value="6" GroupName="fieldType" OnCheckedChanged="RBcriterionType_CheckedChanged" AutoPostBack="true"/>
                        <asp:Label ID="LBtypesRatingScaleFuzzy" runat="server" AssociatedControlID="RBtypesRatingScaleFuzzy"></asp:Label>
                    </li>
                </ul>
            </div>
            <div></div>
        </div>
        <div class="rightfield clearfix preview child">
            <div class="divpreview" id="DVpreview" runat="server">
                <asp:Label ID="LBcriterionName" runat="server" cssclass="title"></asp:Label>
                <CTRL:CTRLInputCriterion ID="CTRLinputCriterion" runat="server" />
                <br /><br /><br />
                <!-- gruppo opzioni nidificato dentro divpreview -->
                <div class="options">
                    <asp:Label ID="LBmultipleCriterionOptions" runat="server" cssclass="title" Visible="false"></asp:Label>
	                <asp:Label ID="LBcriterionOptions" runat="server" cssclass="title"></asp:Label>
	                <div class="fieldobject">
	                    <div class="fieldrow fielddescription">
                            <asp:Label ID="LBstandardOptionsDescription" runat="server" cssclass="description"></asp:Label>
	                    </div>
	                    <div class="fieldrow fieldinput" id="DVstandardCriteriaDescription" runat="server">
                            <asp:Label ID="LBstandardCriteriaNumber_t" runat="server" cssclass="description" AssociatedControlID="TXBstandardCriteriaNumber"></asp:Label>
                            <asp:TextBox ID="TXBstandardCriteriaNumber" runat="server" cssclass="inputtext"></asp:TextBox>
                            <asp:RangeValidator ID="RNVstandardCriteriaNumber" runat="server" ControlToValidate="TXBstandardCriteriaNumber" MinimumValue="1" MaximumValue="30" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
                            <asp:Label ID="LBstandardCriteriaNumberHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBstandardCriteriaNumber"></asp:Label>
	                    </div>
                        <div class="fieldrow fieldinput" id="DVmultipleChoiceCriteriaDescription" runat="server" visible="false">
	                        <asp:Label ID="LBadvancedCriterionOptionsDescription" runat="server" cssclass="description"></asp:Label>
	                    </div>
	                    <div class="fieldrow fieldinput" id="DVmultipleChoiceCriteria" runat="server" visible="false">
                            <asp:Label ID="LBadvancedCriterionOptionsList" runat="server" cssclass="description" AssociatedControlID="TXBcriterionOptions"></asp:Label>
                            <asp:TextBox ID="TXBcriterionOptions" runat="server" cssclass="inputtext" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <asp:Label ID="LBadvancedCriterionOptionsListHelp" runat="server" cssclass="inlinetooltip" AssociatedControlID="TXBcriterionOptions"></asp:Label>
	                    </div>
                        <CTRL:CTRLscales runat="server" id="CTRLscales"  visible="false"/>
                        <CTRL:CTRLscales runat="server" id="CTRLfuzzyScales" visible="false" />
	                </div>
                </div>
                <!-- fine gruppo opzioni --> 
            </div>
            &nbsp;
        </div>
    </asp:View>
    <asp:View ID="VIWempty" runat="server"></asp:View>
</asp:MultiView>