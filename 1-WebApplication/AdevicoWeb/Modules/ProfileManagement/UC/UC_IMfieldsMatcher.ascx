<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMfieldsMatcher.ascx.vb" Inherits="Comunita_OnLine.UC_IMfieldsMatcher" %>
<%@ Register Assembly="lm.Comol.Core.BaseModules" Namespace="lm.Comol.Core.BaseModules.Web.Controls" TagPrefix="asp" %>
<%@ Register TagPrefix="CTRL" TagName="AutoCompleteField" Src="~/Modules/Common/UC/UC_AutoCompleteField.ascx" %>
<div class="StepData IMfieldsMatcher">
     <asp:MultiView ID="MLVcontrolData" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <div class="fieldobject">
                <div class="fieldrow">
                    <br /><br /><br /><br />
                    <asp:Label ID="LBemptyMessage" runat="server" CssClass="Testo_campo"></asp:Label>
                    <br /><br /><br /><br />
                </div>
            </div>
         </asp:View>
        <asp:View ID="VIWmatchColumns" runat="server">
            <div class="fieldobject">
                <div class="fieldrow">
                    <asp:Label ID="LBauthenticationProviders_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLauthenticationProviders">Scegli il tipo di autenticazione</asp:Label>
                    <asp:DropDownList ID="DDLauthenticationProviders" runat="server" CssClass="Testo_Campo"  AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="fieldrow" runat="server" visible="false" id="DVpassword">
                    <asp:Label ID="LBallowPWDinsert_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXallowPWDinsert">Consenti inserimento password:</asp:Label>
                    <asp:CheckBox ID="CBXallowPWDinsert" runat="server" CssClass="inputgroup"  AutoPostBack="true"/>
                </div>
                <div class="fieldrow" runat="server" visible="false" id="DVlogin">
                    <asp:Label ID="LBdefaultLogin_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CBXdefaultLogin">Login assegnata  dal sistema:</asp:Label>
                    <asp:CheckBox ID="CBXdefaultLogin" runat="server" CssClass="inputgroup"  AutoPostBack="true"/>
                </div> 
                <div class="fieldrow">
                    <asp:Label ID="LBprofileToSelect_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RBLuserTypes">Scegli il profilo</asp:Label>
                    <span class="rbl_userType">
                        <asp:RadioButtonList ID="RBLuserTypes" runat="server" CssClass="Testo_Campo rbl_MultiElement"
                            RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="4"  AutoPostBack="true">
                        </asp:RadioButtonList>
                    </span>
                </div>
                <div class="fieldrow" runat="server" visible="false" id="DVagency">
                    <asp:Label ID="LBdefaultAgency_t" runat="server" CssClass="fieldlabel" AssociatedControlID="CTRLagency">Agency:</asp:Label>
                    <CTRL:AutoCompleteField runat="server" id="CTRLagency"
                    TextClass="AgencyText" ValueClass="AgencyValue"
                    CssClass="Testo_Campo"></CTRL:AutoCompleteField>
                </div> 
                <div class="fieldrow">
                    <asp:Label ID="LBinfo" runat="server" CssClass="Titolo_Campo full"></asp:Label>
                </div>
                <div class="fieldrow">
                    <asp:Repeater ID="RPTmatcher" runat="server">
                        <HeaderTemplate>
                            <table class="table light fullwidth">
                                <thead>
                                    <tr>
                                        <th><asp:Label ID="LBsourceColumn" runat="server"></asp:Label></th>
                                        <th><asp:Label ID="LBdestinationColumn" runat="server"></asp:Label></th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Literal ID="LTcolumnNumber" runat="server" Visible="false" Text="<%#Container.Dataitem.Number %>"></asp:Literal>
                                        <asp:Label ID="LBsourceName" runat="server" CssClass="Titolo_Campo" AssociatedControlID="DDLdestination" ><%#Container.Dataitem.SourceColumn %></asp:Label>
                                    </td>
                                    <td class="Field">
                                        <asp:ExtendedDropDown ID="DDLdestination" runat="server" CssClass="Testo_Campo">
                                        </asp:ExtendedDropDown>
                                        <asp:Label ID="LBerror" runat="server" Text="*" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                 </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
         </asp:View>
    </asp:MultiView>
</div>