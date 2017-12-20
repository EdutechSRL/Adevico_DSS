<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="EditRole.aspx.vb" Inherits="Comunita_OnLine.AddRoleToSystem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenuTop" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;"
        runat="server">
        <asp:HyperLink ID="HYPbackToManagementTop" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBaddRoleTop" Visible="false" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
    <div style="width: 900px; text-align: left; padding-top: 5px; margin: 0px auto;">
        <span class="Fieldrow">
            <asp:Label ID="LBroleName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">Name:</asp:Label>
            <asp:textbox id="TXBname" Runat="server" CssClass="Testo_campo_obbligatori" MaxLength="255" Columns="60"></asp:textbox>
            <asp:requiredfieldvalidator id="RFVname" runat="server" CssClass="Validatori" Display="Static" ControlToValidate="TXBname">*</asp:requiredfieldvalidator>
        </span>
        <fieldset>
            <label><asp:Literal ID="LTnameTranslations" runat="server">Translations:</asp:Literal></label>
            <asp:Repeater id="RPTnameTranslations" Runat="server">
				<HeaderTemplate>
                    <table>
                        <thead>
                        </thead>
					    <tbody>
				</HeaderTemplate>
				<ItemTemplate>
					    <tr>
						    <td>
                                <asp:Literal ID="LTidLanguage" runat="server" Visible="false" Text='<%# Databinder.eval(Container.DataItem, "IdLanguage")%>'></asp:Literal>
							    <asp:Label id="LBlanguage" Text='<%# Databinder.eval(Container.DataItem, "Name")%>' runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtranslation"/>
						    </td>
						    <td>
							    <asp:TextBox ID="TXBtranslation" Runat="server" CssClass="Testo_campoSmall" MaxLength="255" Columns="60"  > </asp:TextBox>&nbsp;&nbsp;
						    </td>
					    </tr>
				</ItemTemplate>
				<FooterTemplate>
					     </tbody>
                    </table>
				</FooterTemplate>
			</asp:Repeater>
        </fieldset>
        <span class="Fieldrow">
            <asp:Label ID="LBroleDescription_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBroleDescription">Description:</asp:Label>
            <asp:textbox id="TXBroleDescription" Runat="server" CssClass="Testo_campo" MaxLength="500" Columns="60"></asp:textbox>
        </span>
        <fieldset>
            <label><asp:Literal ID="LTdescriptionTranslations" runat="server">Translations:</asp:Literal></label>
            <asp:Repeater id="RPTdescriptionTranslations" Runat="server">
				<HeaderTemplate>
                    <table>
                        <thead>
                        </thead>
					    <tbody>
				</HeaderTemplate>
				<ItemTemplate>
					    <tr>
						    <td>
                                <asp:Literal ID="LTidLanguage" runat="server" Visible="false" Text='<%# Databinder.eval(Container.DataItem, "IdLanguage")%>'></asp:Literal>
							    <asp:Label id="LBlanguage" Text='<%# Databinder.eval(Container.DataItem, "Name")%>' runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBtranslation"/>
						    </td>
						    <td>
							    <asp:TextBox ID="TXBtranslation" Runat="server" MaxLength="500" Columns="60"  CssClass="Testo_campo" > </asp:TextBox>&nbsp;&nbsp;
						    </td>
					    </tr>
				</ItemTemplate>
				<FooterTemplate>
					     </tbody>
                    </table>
				</FooterTemplate>
			</asp:Repeater>
        </fieldset>
        <span class="Fieldrow">
            <asp:Label ID="LBavailability_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBLavailability">Available for:</asp:Label>
            <asp:CheckBoxList ID="CBLavailability" runat="server" RepeatLayout="Flow" RepeatColumns="4" RepeatDirection="Vertical"></asp:CheckBoxList>
        </span>
        <span class="Fieldrow">
            <asp:Label ID="LBroleLevel_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="RBLroleLevel">Role level:</asp:Label>
            <asp:RadioButtonlist ID="RBLroleLevel" runat="server" RepeatLayout="Flow" RepeatColumns="10" RepeatDirection="Horizontal"></asp:RadioButtonlist>
        </span>

        <fieldset>
            <label><asp:Literal ID="LTlevelInfos" runat="server">Summary</asp:Literal></label>
            <asp:Repeater id="RPTlevelInfos" Runat="server">
				<HeaderTemplate>
                    <ul>
				</HeaderTemplate>
				<ItemTemplate>
					    <li>
						    <asp:Label id="LBlevelValue" Text='<%# Databinder.eval(Container.DataItem, "Name")%>' runat="server"/>
                            <asp:Label id="LBlevelRoles" Text='<%# Databinder.eval(Container.DataItem, "Roles")%>' runat="server"/>
					    </li>
				</ItemTemplate>
				<FooterTemplate>
                    </ul>
				</FooterTemplate>
			</asp:Repeater>
        </fieldset>
    </div>
    <div id="DVmenuBottom" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;"
        runat="server">
        <asp:HyperLink ID="HYPbackToManagementBottom" runat="server" CssClass="Link_Menu" Text="Back"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:LinkButton ID="LNBaddRoleBottom" Visible="false" runat="server" CssClass="Link_Menu"> </asp:LinkButton>
    </div>
</asp:Content>
