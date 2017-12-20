<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditCriterion.ascx.vb" Inherits="Comunita_OnLine.UC_EditCriterion" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyNumber.ascx" TagName="CTRLfuzzyNumber" TagPrefix="CTRL" %>
<asp:MultiView ID="MLVcriterion" runat="server">
    <asp:View ID="VIWunknown" runat="server">
    
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWcriterion" runat="server">
        <div class="fieldrow fielddescription">			
            <asp:Label AssociatedControlID="TXBdescription" runat="server" id="LBcriterionDescription_t" CssClass="fieldlabel" >Description</asp:Label>
            <asp:TextBox runat="server" ID="TXBdescription" Columns="40" class="textarea" TextMode="MultiLine"></asp:TextBox>
		</div>
        <div class="fieldrow fieldhelp" runat="server" id="DVhelp"  visible="false">
			<asp:Label AssociatedControlID="TXBhelp" runat="server" ID="LBcriterionHelp_t" CssClass="fieldlabel" >Help</asp:Label>
            <asp:TextBox runat="server" ID="TXBhelp" CssClass="inputtext"></asp:TextBox>
		</div>
        <div class="fieldrow fieldmaxchar" runat="server" id="DVmaxChar" visible="false">			
            <asp:Label AssociatedControlID="TXBmaxChar" runat="server" ID="LBcriterionMaxChar_t" CssClass="fieldlabel" >Max Char</asp:Label>            
            <asp:TextBox runat="server" ID="TXBmaxChar" CssClass="inputtext"></asp:TextBox>
            <asp:RangeValidator ID="RNVBmaxChar" ControlToValidate="TXBmaxChar" MinimumValue="0" MaximumValue="300000" SetFocusOnError="true" Type="Double" runat="server" Display="None" Text="*"></asp:RangeValidator>
		</div>
        <div class="fieldrow fieldinput">
            <asp:Label AssociatedControlID="RBLcommentType" runat="server" ID="LBcommentType_t" CssClass="fieldlabel" >Comments</asp:Label>
            <asp:RadioButtonList ID="RBLcommentType" runat="server" CssClass="inputradiobuttonlist" RepeatLayout="Flow" RepeatDirection="Vertical">
                <asp:ListItem Value="None"></asp:ListItem>
                <asp:ListItem Value="Allowed"></asp:ListItem>
                <asp:ListItem Value="Mandatory"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="fieldrow" id="DVrangeType" runat="server" visible="false">
            <asp:Label AssociatedControlID="CBXintegerRange" runat="server" ID="LBrangeType_t" CssClass="fieldlabel">Integer</asp:Label>
            <asp:CheckBox ID="CBXintegerRange" runat="server" AutoPostBack="true" />
        </div>
        <div class="fieldrow" id="DVrangeMinValue" runat="server" visible="false">
            <asp:Label AssociatedControlID="TXBminValue" runat="server" ID="LBrangeMinValue_t" CssClass="fieldlabel">Min value</asp:Label>
            <asp:TextBox runat="server" ID="TXBminValue" CssClass="inputchar"></asp:TextBox>
        </div>
        <div class="fieldrow" id="DVrangeMaxValue" runat="server" visible="false">
            <asp:Label AssociatedControlID="TXBmaxValue" runat="server" ID="LBrangeMaxValue_t" CssClass="fieldlabel">Max value</asp:Label>
            <asp:TextBox runat="server" ID="TXBmaxValue" CssClass="inputchar"></asp:TextBox>
        </div>

        <%--<a href="#" class="linkMenu advanced">Avanzate</a>--%>
       <asp:MultiView ID="MLVadvanced" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWnone" runat="server">
    
            </asp:View>
            <asp:View ID="VIWdropdownlist" runat="server">
                <div class="fieldrow">
                     <asp:Repeater ID="RPTcomboOptions" runat="server">
                        <HeaderTemplate>
                            <label for="" class="fieldlabel"><asp:Literal ID="LToptionsTitle" runat="server"></asp:Literal></label>
                            <span class="fielditemswrapper">
                            <ul class="fielditems criterionoptions">
                                <li class="fielditem fixed header clearfix collapsed">
                                    <div class="fielditemcontent clearfix">
                                        <span class="movecfielditem" style="visibility: hidden"></span>
                                        <asp:label ID="LBevaluationName_t" runat="server" CssClass="spaninputtext">Evaluation</asp:label>
                                        <asp:label ID="LBevaluationValue_t" runat="server" CssClass="spaninputchar">Value</asp:label>
                                    </div>
                                </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li class="fielditem clearfix" id="criterionoption_<%#Container.DataItem.Id %>">
                                    <div class="fielditemcontent clearfix">
                                        <asp:Label ID="LBmoveOption" runat="server" cssclass="movecfielditem">M</asp:Label>
                                        <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorderoption"/>
                                        <asp:Literal ID="LTidOption" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                        <a name="#option_<%#Container.DataItem.Id %>"></a>
                                        <asp:TextBox ID="TXBoptionName" runat="server" CssClass="inputtext"></asp:TextBox>
                                        <asp:TextBox ID="TXBoptionValue" runat="server" CssClass="inputchar"></asp:TextBox>
                                        <span class="icons">
                                            <asp:Button ID="BTNdeleteOption" runat="server" Text="D" CssClass="icon delete" CommandName="virtualDelete"/>
                                        </span>
                                    </div>
                                </li>
                        </ItemTemplate>
                        <FooterTemplate>
                                <li class="fielditem clearfix fixed" id="DVfooter" runat="server">
                                    <div class="fielditemcontent clearfix">
                                        <span class="movecfielditem" style="visibility:hidden;">&nbsp;</span>
                                        <input type="hidden" id="HDNdisplayPlaceholder">
                                        <a name="#criterionaddoption_<%#IdCriterion %>"></a>
                                        <asp:TextBox ID="TXBoptionAddName" CssClass="inputtext" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="TXBoptionAddValue" runat="server" CssClass="inputchar"></asp:TextBox>
                                        <span class="icons">
                                            <asp:Button ID="BTNaddOption" runat="server" Text="ADD" CommandName="addoption"  />
                                        </span>
                                    </div>
                                </li>
                            </ul>
                            </span>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:View>
            <asp:View ID="VIWdssValues" runat="server">
                <div class="fieldrow">
                    <asp:Repeater ID="RPTcomboDssOptions" runat="server">
                        <HeaderTemplate>
                            <label for="" class="fieldlabel"><asp:Literal ID="LToptionsTitle" runat="server"></asp:Literal></label>
                            <span class="fielditemswrapper">
                            <ul class="fielditems criterionoptions">
                                <li class="fielditem fixed header clearfix collapsed">
                                    <div class="fielditemcontent clearfix">
                                        <asp:label ID="LBevaluationName_t" runat="server" CssClass="spaninputtext">Evaluation</asp:label>
                                        <asp:label ID="LBevaluationValue_t" runat="server" CssClass="spaninputchar">Value</asp:label>
                                    </div>
                                </li>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li class="fielditem clearfix" id="criterionoption_<%#Container.DataItem.Id %>">
                                    <div class="fielditemcontent clearfix">
                                        <a name="#option_<%#Container.DataItem.Id %>"></a>
                                        <span class="inputtext">
                                            <%#Container.DataItem.Name %>
                                        </span>
                                        <span class="inputtext">
                                            <asp:Literal ID="LTnumber" runat="server" Visible="false"></asp:Literal>
                                            <CTRL:CTRLfuzzyNumber id="CTRLfuzzyNumber" ShowFazzyValueWithCog="true" runat="server"></CTRL:CTRLfuzzyNumber>
                                        </span>
                                    </div>
                                </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                            </span>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:View>
       </asp:MultiView>
    </asp:View>
</asp:MultiView>