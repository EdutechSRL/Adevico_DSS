<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InputCriterion.ascx.vb" Inherits="Comunita_OnLine.UC_InputCriterion" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyInputGeneric.ascx" TagName="CTRLfuzzyInput" TagPrefix="CTRL" %>
<asp:MultiView ID="MLVcriterion" runat="server">
    <asp:View ID="VIWunknown" runat="server">
    
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWcriterion" runat="server">
        <div class="fieldobject multiline" runat="server" id="DVsingleline">
            <div class="fieldrow fielddescription">
                <asp:Label runat="server" ID="LBcriterionDescription_t" CssClass="description">Description</asp:Label>
            </div>
            <div class="fieldrow fieldinput" id="DVcriterion" runat="server">
                <asp:MultiView ID="MLVcriterionType" runat="server">
                    <asp:View ID="VIWboolean" runat="server">
                        <asp:Label runat="server" ID="LBbooleanText" AssociatedControlID="TXBtextual">Items</asp:Label>
                        <div class="boolCriteria">
                            <asp:RadioButtonList ID="RBLboolean" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="Non superato" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Superato" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <%--<asp:CheckBox ID="CBXboolean" runat="server" text="Superato"/>--%>
                        <asp:Label runat="server" ID="LBbooleanHelp" CssClass="inlinetooltip"></asp:Label>
                    </asp:View>
                    <asp:View ID="VIWtextual" runat="server">
                        <asp:Label runat="server" ID="LBtextualText" AssociatedControlID="TXBtextual">Items</asp:Label>
                        <asp:TextBox runat="server" ID="TXBtextual" TextMode="multiline" CssClass="textarea"></asp:TextBox>
                        <asp:Label runat="server" ID="LBtextualHelp" CssClass="inlinetooltip"></asp:Label>
    
                    </asp:View>
                    <asp:View ID="VIWintegerrange" runat="server">
                        <asp:Label runat="server" ID="LBintegerrangeText" AssociatedControlID="DDLintegerRange">Items</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLintegerRange"></asp:DropDownList>
                        <asp:Label runat="server" ID="LBintegerrangeHelp" CssClass="inlinetooltip"></asp:Label>
                    </asp:View>
                    <asp:View ID="VIWdecimalrange" runat="server">
                        <asp:Label runat="server" ID="LBdecimalrangeText" AssociatedControlID="TXBdecimalrange">Items</asp:Label>
                        <asp:TextBox ID="TXBdecimalrange" runat="server"></asp:TextBox>
                        <asp:RangeValidator ID="REVdecimal" runat="server" ControlToValidate="TXBdecimalrange" EnableClientScript="false" Type="Double"></asp:RangeValidator>
                        <asp:Label runat="server" ID="LBdecimalHelp" CssClass="inlinetooltip"></asp:Label>
                    </asp:View>
                    <asp:View ID="VIWstringrange" runat="server">
                        <asp:Label runat="server" ID="LBstringrangeText" AssociatedControlID="DDLstringRange">Items</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLstringRange"></asp:DropDownList>
                        <asp:Label runat="server" ID="LBstringrangeHelp" CssClass="inlinetooltip"></asp:Label>
                    </asp:View>
                    <asp:View ID="VIWdss" runat="server">
                        <CTRL:CTRLfuzzyInput ID="CTRLfuzzyInput" runat="server"  CssClass="criterialevel" />
                    </asp:View>
                </asp:MultiView>
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxChartextual"  Visible="false">
                        <asp:Literal ID="LTmaxCharstextual" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBmessageError" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>  
            </div>
            <div class="fieldrow fieldinput" id="DVcomment" runat="server" Visible="false">
                <asp:Label runat="server" id="LBcommentText" AssociatedControlID="TXBcomment" CssClass="fieldlabel">Text</asp:Label>
                <asp:TextBox runat="server" ID="TXBcomment" TextMode="multiline" CssClass="textarea"></asp:TextBox>
                <asp:Label runat="server" ID="LBcommentHelp" CssClass="inlinetooltip"></asp:Label>     
                <br/>
                <span class="fieldinfo ">
                    <span class="maxchar" runat="server" id="SPNmaxCharsComment" visible="false" >
                        <asp:Literal ID="LTmaxCharsComment" runat="server"></asp:Literal>
                        <span class="availableitems">{available}</span>/<span class="totalitems">{total}</span>
                    </span>
                    <asp:Label ID="LBerrorMessageComment" runat="server" Visible="false" cssClass="generic"></asp:Label>
                </span>        
            </div>
        </div>
    </asp:View>
</asp:MultiView>
<asp:Literal ID="LTidCriterion" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTidOption" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTintValueString" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTfloatValueString" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="LTvalueString" runat="server" Visible="false"></asp:Literal>