<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditMacUrlAttribute.ascx.vb" Inherits="Comunita_OnLine.UC_EditMacUrlAttribute" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>

<asp:MultiView ID="MLVattribute" runat="server">
    <asp:View ID="VIWunknown" runat="server">
    
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
    <asp:View ID="VIWattribute" runat="server">
        <div class="dynamicfieldwrapper">
            <div class="dynamicfieldContent">
                <span class="Field_Row">
                    <label for="" class="Titolo_campo dynamicfieldname"><asp:Literal id="LTmacUrlAttributeName" runat="server"></asp:Literal></label>
                    <asp:Label ID="LBmacUrlAttributeType" runat="server" CssClass="Testo_Campo dynamicfieldtype"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="LBmacUrlAttributeQueryName" runat="server" CssClass="Testo_Campo dynamicfieldtype"></asp:Label>
                    <span class="icons">
                        <asp:Button ID="BTNdeleteAttribute" runat="server" Text="D" CssClass="icon delete needconfirm" CommandName="virtualDelete" />
                        <asp:Button ID="BTNeditAttribute" runat="server" Text="E" CssClass="icon edit" CommandName="edit" CausesValidation="false"/>
                    </span>
                </span>
                <div class="editarea" runat="server" id="DVeditArea" visible="false">
                    <div class="details common">
                        <span class="Field_Row">
                            <asp:Label AssociatedControlID="TXBname" runat="server" id="LBmacUrlAttributeName_t" CssClass="Titolo_campo">Name:</asp:Label>
                            <asp:TextBox runat="server" ID="TXBname" Columns="40" class="Testo_Campo" MaxLength="150"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVname" runat="server" SetFocusOnError="true" 
                             ControlToValidate="TXBname" CssClass="Validatori" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </span>
                        <span class="Field_Row" id="DVmultipleValue" runat="server" Visible = "false">
                            <asp:Label AssociatedControlID="CBXmultipleValue" runat="server" id="LBmacUrlAttributeMultipleValue_t" CssClass="Titolo_campo">MultipleValue/Separator:</asp:Label>
                            <label for="" class="Titolo_campo"></label>
                            <span class="Testo_Campo">
                                <input type="checkbox" id="CBXmultipleValue" runat="server" class="inputActivator" />
                                <label for="" class="inline"><asp:Literal ID="LTmacUrlAttributeMultipleValue" runat="server"></asp:Literal></label>
                                <asp:TextBox runat="server" ID="TXBinputchar" Columns="1" MaxLength="1" class="inputchar"></asp:TextBox>
                            </span>
                        </span>
                        <span class="Field_Row">
                            <asp:Label AssociatedControlID="TXBdescription" runat="server" id="LBmacUrlAttributeDescription_t" CssClass="Titolo_campo">Description</asp:Label>
                            <asp:TextBox runat="server" ID="TXBdescription" Columns="40" class="textarea Testo_Campo" TextMode="MultiLine"></asp:TextBox>
                        </span>
                        <span class="Field_Row" id="DVqueryStringName" runat="server" >
                            <asp:Label AssociatedControlID="TXBqueryStringName" runat="server" id="LBmacUrlAttributeQueryStringName_t" CssClass="Titolo_campo">Query string name:</asp:Label>
                            <asp:TextBox runat="server" ID="TXBqueryStringName" Columns="40" class="Testo_Campo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVqueryStringName" runat="server" SetFocusOnError="true" 
                             ControlToValidate="TXBqueryStringName" CssClass="Validatori" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </span>
                        <span class="Field_Row" id="DVrequiredValue" runat="server" visible="false">
                            <asp:Label AssociatedControlID="TXBrequiredValue" runat="server" id="LBmacUrlAttributeRequiredValue_t" CssClass="Titolo_campo">Value:</asp:Label>
                            <asp:TextBox runat="server" ID="TXBrequiredValue" Columns="40" class="Testo_Campo"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVrequiredValue" runat="server" SetFocusOnError="true" 
                             ControlToValidate="TXBrequiredValue" CssClass="Validatori" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    <asp:MultiView ID="MLVadvanced" runat="server" ActiveViewIndex="0">
                        <asp:View ID="VIWnoneAttribute" runat="server">
    
                        </asp:View>
                        <asp:View ID="VIWtimestamp" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <asp:Label ID="LBtimeStampAttributeFormatType_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="SLformat"></asp:Label>
                                    <select id="SLformat" runat="server" class="expandoption">
                                        <option value="utc">Utc</option>
                                        <option value="aaaammgghhmmss">AAAAMMGGHHMMSS</option>
                                    </select>
                                </span>
                                <%--<span class="Field_Row other hidden">
                                   <asp:Label ID="LBtimeStampAttributeFormat_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="Titolo_campo"></asp:Label>
                                   <asp:TextBox runat="server" ID="TXBtimestampFormat" Columns="40" class="Testo_Campo" MaxLength="150"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RFV" runat="server" SetFocusOnError="true" 
                                        ControlToValidate="TXBname" CssClass="Validatori" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </span>--%>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWprofile" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <asp:Label ID="LBprofileAttribute_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLprofileFields"></asp:Label>
                                    <asp:ExtendedDropDown ID="DDLprofileFields" runat="server"></asp:ExtendedDropDown>
                                </span>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWmac" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <label for="" class="Titolo_campo"><asp:Literal ID="LTmacAttributeItemsTitle" runat="server">Composizione:</asp:Literal></label>
                                    <span class="fielditemswrapper">
                                    <asp:Repeater ID="RPTmacAttributeItems" runat="server">
                                        <HeaderTemplate>
                                            <ul class="fielditems ui-sortable">
                                                <li class="fielditem fixed clearfix header collapsed">
                                				    <div class="fielditemcontent clearfix">
                                   					    <span class="movecfielditem" style="visibility: hidden"></span>
                                                        <asp:Label ID="LBmacAttributeItemsTitle_t" runat="server" cssclass="spaninputtext attribute"></asp:Label>
                                				    </div>
                            				    </li>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li class="fielditem clearfix" id="option_<%#Container.DataItem.Id %>">
                                                    <a name="attribute_<%=IdAttribute %>_<%#Container.DataItem.Id %>"></a>
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:Label ID="LBmoveMacAttributeItem" runat="server" cssclass="movecfielditem">M</asp:Label>
                                                        <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorderoption"/>
                                                        <asp:Literal ID="LTidMacAttributeItem" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                        <asp:Label ID="LBmacAttributeItem" runat="server" CssClass="inputtext attribute"></asp:Label>
                                                        <span class="icons">
                                                            <asp:Button ID="BTNdeleteMacAttributeItem" runat="server" Text="D" CssClass="icon delete" CommandArgument="<%#Container.DataItem.Id %>" CommandName="virtualDelete"/>
                                                        </span>
                                                    </div>
                                                </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <li class="fielditem clearfix fixed footer collapsed" id="DVfooter" runat="server">
                                                    <div class="fielditemcontent clearfix">
                                                        <span class="movecfielditem" style="visibility:hidden;">&nbsp;</span>
                                                        <asp:DropDownList CssClass="inputtext attribute" runat="server" ID="DDLattributes"></asp:DropDownList>
                                                        <asp:Button ID="BTNaddMacAttributeitem" runat="server" Text="ADD" CommandName="addoption"/>
                                                    </div>
                                                </li>
                                            </ul>
                                            </span>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    </span>
                                </span>
                            </div>
                         </asp:View>
                        <asp:View ID="VIWorganization" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <label for="" class="Titolo_campo"><asp:Literal ID="LTorganizationAttributeItemsTitle" runat="server">Opzioni:</asp:Literal></label>
                                    <span class="fielditemswrapper">
                                        <asp:Repeater ID="RPTorganizationItems" runat="server">
                                        <HeaderTemplate>
                                            <ul class="fielditems ui-sortable">
                                                <li class="fielditem fixed clearfix header collapsed">
                                				    <div class="fielditemcontent clearfix">
                                                        <asp:Label ID="LBorgNameTitle_t" runat="server" cssclass="spaninputtext organization"></asp:Label>
                                                        <asp:Label ID="LBorgProfileTitle_t" runat="server" cssclass="spaninputtext profile"></asp:Label>
                                                        <asp:Label ID="LBorgPageTitle_t" runat="server" cssclass="spaninputtext page"></asp:Label>
                                                        <asp:Label ID="LBorgCodeTitle_t" runat="server" cssclass="spaninputtext code"></asp:Label>
                                				    </div>
                            				    </li>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li class="fielditem clearfix" id="option_<%#Container.DataItem.Id %>">
                                                    <a name="attribute_<%=IdAttribute %>_<%#Container.DataItem.Id %>"></a>
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:Literal ID="LTidOrganizationAttributeItem" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                        <asp:Label ID="LBorgName" runat="server" CssClass="inputtext organization"></asp:Label>
                                                        <asp:DropDownList id="DDLorgProfile" runat="server" CssClass="inputtext profile"></asp:DropDownList>
                                                        <asp:DropDownList id="DDLorgPage" runat="server" CssClass="inputtext page"></asp:DropDownList>
                                                        <asp:TextBox ID="TXBorgCode" runat="server" MaxLength="100" CssClass="inputchar code"></asp:TextBox>
                                                        <span class="icons">
                                                            <asp:Button ID="BTNdeleteOrganizationAttributeItem" runat="server" Text="D" CssClass="icon delete" CommandArgument="<%#Container.DataItem.Id %>" CommandName="virtualDelete"/>
                                                        </span>
                                                    </div>
                                                </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <li class="fielditem clearfix fixed footer collapsed" id="DVfooter" runat="server">
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:DropDownList id="DDLorganization" runat="server" CssClass="inputtext organization"></asp:DropDownList>
                                                        <asp:DropDownList id="DDLorgProfile" runat="server" CssClass="inputtext profile"></asp:DropDownList>
                                                        <asp:DropDownList id="DDLorgPage" runat="server" CssClass="inputtext page"></asp:DropDownList>
                                                        <asp:TextBox ID="TXBorgCode" runat="server" MaxLength="100" CssClass="inputchar code"></asp:TextBox>
                                                        <asp:Button ID="BTNaddOrganizationAttributeitem" runat="server" Text="ADD" CommandName="addoption"/>
                                                    </div>
                                                </li>
                                            </ul>
                                            </span>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    </span>
                                </span>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWcatalogue" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <label for="" class="Titolo_campo"><asp:Literal ID="LTcatalogueAttributeItemsTitle" runat="server">Opzioni:</asp:Literal></label>
                                    <span class="fielditemswrapper">
                                        <asp:Repeater ID="RPTcatalogueItems" runat="server">
                                        <HeaderTemplate>
                                            <ul class="fielditems ui-sortable">
                                                <li class="fielditem fixed clearfix header collapsed">
                                				    <div class="fielditemcontent clearfix">
                                                        <asp:Label ID="LBctgNameTitle_t" runat="server" cssclass="spaninputtext organization"></asp:Label>
                                                        <asp:Label ID="LBctgCodeTitle_t" runat="server" cssclass="spaninputtext code"></asp:Label>
                                				    </div>
                            				    </li>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li class="fielditem clearfix" id="option_<%#Container.DataItem.Id %>">
                                                    <a name="attribute_<%=IdAttribute %>_<%#Container.DataItem.Id %>"></a>
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:Literal ID="LTidCatalogueAttributeItem" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                        <asp:Literal ID="LTidCatalogue" runat="server" Visible="false" Text="<%#Container.DataItem.Catalogue.Id %>"></asp:Literal>
                                                        <asp:Label ID="LBctgName" runat="server" CssClass="inputtext catalogue"></asp:Label>
                                                        <asp:TextBox ID="TXBctgCode" runat="server" MaxLength="100" CssClass="inputchar code"></asp:TextBox>
                                                        <span class="icons">
                                                            <asp:Button ID="BTNdeleteCatalogueAttributeItem" runat="server" Text="D" CssClass="icon delete" CommandArgument="<%#Container.DataItem.Id %>" CommandName="virtualDelete"/>
                                                        </span>
                                                    </div>
                                                </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <li class="fielditem clearfix fixed footer collapsed" id="DVfooter" runat="server">
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:DropDownList id="DDLcatalogues" runat="server" CssClass="inputtext catalogue"></asp:DropDownList>
                                                        <asp:TextBox ID="TXBctgCode" runat="server" MaxLength="100" CssClass="inputchar code"></asp:TextBox>
                                                        <asp:Button ID="BTNaddCatalogueAttributeitem" runat="server" Text="ADD" CommandName="addoption"/>
                                                    </div>
                                                </li>
                                            </ul>
                                            </span>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    </span>
                                </span>
                            </div>
                        </asp:View>
                        <asp:View ID="VIWcomposite" runat="server">
                            <div class="details extra">
                                <span class="Field_Row">
                                    <asp:Label ID="LBcompositeProfileAttribute_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLcompositeProfileFields"></asp:Label>
                                    <asp:ExtendedDropDown ID="DDLcompositeProfileFields" runat="server"></asp:ExtendedDropDown>
                                </span>
                                 <span class="Field_Row">
                                    <label for="" class="Titolo_campo"><asp:Literal ID="LTcompositeAttributeItemsTitle" runat="server">Composizione:</asp:Literal></label>
                                    <span class="fielditemswrapper">
                                    <asp:Repeater ID="RPTcompositeAttributeItems" runat="server">
                                        <HeaderTemplate>
                                            <ul class="fielditems ui-sortable">
                                                <li class="fielditem fixed clearfix header collapsed">
                                				    <div class="fielditemcontent clearfix">
                                   					    <span class="movecfielditem" style="visibility: hidden"></span>
                                                        <asp:Label ID="LBurlAttributeItemsTitle_t" runat="server" cssclass="spaninputtext attribute"></asp:Label>
                                				    </div>
                            				    </li>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li class="fielditem clearfix" id="option_<%#Container.DataItem.Id %>">
                                                    <a name="attribute_<%=IdAttribute %>_<%#Container.DataItem.Id %>"></a>
                                                    <div class="fielditemcontent clearfix">
                                                        <asp:Label ID="LBmoveUrlAttributeItem" runat="server" cssclass="movecfielditem">M</asp:Label>
                                                        <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorderoption"/>
                                                        <asp:Literal ID="LTidUrlAttributeItem" runat="server" Visible="false" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                        <asp:Label ID="LBurlAttributeItem" runat="server" CssClass="inputtext attribute"></asp:Label>
                                                        <span class="icons">
                                                            <asp:Button ID="BTNdeleteUrlAttributeItem" runat="server" Text="D" CssClass="icon delete" CommandArgument="<%#Container.DataItem.Id %>" CommandName="virtualDelete"/>
                                                        </span>
                                                    </div>
                                                </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                <li class="fielditem clearfix fixed footer collapsed" id="DVfooter" runat="server">
                                                    <div class="fielditemcontent clearfix">
                                                        <span class="movecfielditem" style="visibility:hidden;">&nbsp;</span>
                                                        <asp:DropDownList CssClass="inputtext attribute" runat="server" ID="DDLattributes"></asp:DropDownList>
                                                        <asp:Button ID="BTNaddUrlAttributeitem" runat="server" Text="ADD" CommandName="addoption"/>
                                                    </div>
                                                </li>
                                            </ul>
                                            </span>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    </span>
                                </span>
                            </div>
                        </asp:View>
                   </asp:MultiView>
                   <div class="rightbutton">
                        <asp:Button id="BTNcancelAttributeEditing" runat="server" CausesValidation="false" Text="Cancel"/>
                        <asp:Button id="BTNsaveAttributeSettings" runat="server" Text="Save"/>
                   </div>
                </div>
            </div>
        </div>
    </asp:View>
</asp:MultiView>