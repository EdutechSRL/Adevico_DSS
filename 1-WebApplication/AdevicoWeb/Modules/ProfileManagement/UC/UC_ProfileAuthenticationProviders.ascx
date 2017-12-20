<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProfileAuthenticationProviders.ascx.vb"
    Inherits="Comunita_OnLine.UC_ProfileAuthenticationProviders" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagName="CTRLmessages" TagPrefix="CTRL" %>
<asp:Label ID="LBprofileTitle" runat="server" CssClass="Titolo_campo"></asp:Label>
<br />
<asp:MultiView ID="MLVaccountInfo" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWinfo" runat="server">
        <div class="TopButton BottomSpacing" id="DVmenu" runat="server">
            <asp:Button ID="BTNcloseModal" CssClass="Link_menu" runat="server" CausesValidation="false" />
            <asp:Button ID="BTNaddNewProvider" CssClass="Link_menu" runat="server" Visible="false"
                CausesValidation="false" />
        </div>
        <asp:Repeater ID="RPTaccountInfo" runat="server">
            <ItemTemplate>
                <fieldset>
                    <div class="BoxContainer">
                        <div>
                            <asp:Label ID="LBproviderName" runat="server" CssClass="Titolo_campo"></asp:Label>
                            <br />
                            <asp:Label ID="LBproviderDescription" runat="server"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="LBproviderInfo" runat="server" Visible="false"></asp:Label>
                            <br />
                            <asp:Label ID="LBpasswordExpiresOn" runat="server" Visible="false"></asp:Label>
                        </div>

                        <asp:Literal ID="LTtype" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="LTidLoginInfo" runat="server" Visible="false"></asp:Literal>
                        <div>
                            <asp:Label ID="LBidentifyer_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBidentifyer"></asp:Label>
                            <asp:Label ID="LBidentifyer" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="BoxContainer">
                        <asp:Button ID="BTNsetPassword" CssClass="Link_menu" runat="server" Visible="false"
                            CommandName="setPassword" CommandArgument='<%#Container.DataItem.Id %>' CausesValidation="false" />
                        <asp:Button ID="BTNnewPassword" CssClass="Link_menu" runat="server" Visible="false"
                            CommandName="newPassword" CommandArgument='<%#Container.DataItem.Id %>' CausesValidation="false" />
                        <asp:Button ID="BTNenable" CssClass="Link_menu" runat="server" Visible="false" CommandName="enable"
                            CommandArgument='<%#Container.DataItem.Id %>' CausesValidation="false" />
                        <asp:Button ID="BTNsetDefault" CssClass="Link_menu" runat="server" Visible="false"
                            CommandName="default" CommandArgument='<%#Container.DataItem.IdProvider %>' CausesValidation="false" />
                        <asp:Button ID="BTNdeleteProvider" CssClass="Link_menu" runat="server" Visible="false"
                            CommandName="delete" CommandArgument='<%#Container.DataItem.Id %>' CausesValidation="false" />
                        <asp:Button ID="BTNeditProvider" CssClass="Link_menu" runat="server" Visible="false"
                            CommandName="edit" CommandArgument='<%#Container.DataItem.Id %>' CausesValidation="false" />
                    </div>
                    <%--<div class="clear">
                    </div>--%>
                </fieldset>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <br />
    </asp:View>
    <asp:View ID="VIWinternal" runat="server">
        <span class="Fieldrow BottomSpacing">
            <asp:Label ID="LBlogin_t" runat="server" AssociatedControlID="TXBlogin" CssClass="Titolo_campo">Current password:</asp:Label>
            <asp:TextBox ID="TXBlogin" CssClass="textbox" runat="server"></asp:TextBox>
            <asp:Label ID="LBloginDuplicate" runat="server" CssClass="Testo_Campo" Visible="false"
                AssociatedControlID="TXBlogin"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVlogin" runat="server" SetFocusOnError="true" ControlToValidate="TXBlogin"></asp:RequiredFieldValidator>
        </span><span class="Fieldrow BottomSpacing">
            <asp:Button ID="BTNundoSaveLogin" runat="server" CssClass="Link_Menu" Text="Back"
                CausesValidation="false" />&nbsp;
            <asp:Button ID="BTNnewPassword" runat="server" CssClass="Link_Menu" Text="New password"
                CausesValidation="false" />
            <asp:Button ID="BTNsaveLogin" runat="server" CssClass="Link_Menu" Text="Edit login" />
        </span>
    </asp:View>
    <asp:View ID="VIWinternalPassword" runat="server">
        <span class="Fieldrow BottomSpacing">
            <br />
            <asp:Label ID="LBpasswordEditDescription" runat="server" ></asp:Label>
        </span>
        <span class="Fieldrow BottomSpacing">
            <asp:Label ID="LBloginPasswordEdit_t" runat="server" AssociatedControlID="LBloginPasswordEdit" CssClass="Titolo_campo">*User:</asp:Label>
            <asp:Label ID="LBloginPasswordEdit" runat="server">*User:</asp:Label>
        </span>
        <span class="Fieldrow BottomSpacing">
            <asp:Label ID="LBpasswordEdit_t" runat="server" AssociatedControlID="LBloginPasswordEdit" CssClass="Titolo_campo">*User:</asp:Label>
            <asp:TextBox ID="TXBpasswordEdit" CssClass="textbox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVpassword" runat="server" SetFocusOnError="true" ControlToValidate="TXBpasswordEdit" ErrorMessage="*"></asp:RequiredFieldValidator>
        </span>
        <span class="Fieldrow BottomSpacing">
            <asp:Label ID="LBpasswordEditConfirm_t" runat="server" AssociatedControlID="TXBpasswordEditConfirm" CssClass="Titolo_campo">*User:</asp:Label>
            <asp:TextBox ID="TXBpasswordEditConfirm" CssClass="textbox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFVpasswordConfirm" runat="server" SetFocusOnError="true" ControlToValidate="TXBpasswordEditConfirm" ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CMVpassword" runat="server" ControlToCompare="TXBpasswordEdit" ControlToValidate="TXBpasswordEditConfirm" SetFocusOnError="true" ></asp:CompareValidator>
        </span>
        <span class="Fieldrow BottomSpacing">
            <asp:Button ID="BTNundoSetPassword" runat="server" CssClass="Link_Menu" Text="Back"
                CausesValidation="false" />&nbsp;
            <asp:Button ID="BTNsaveNewPassword" runat="server" CssClass="Link_Menu" Text="New password"/>
        </span>
    </asp:View>
    <asp:View ID="VIWexternal" runat="server">
        <span class="Fieldrow" id="SPNexternalLong" runat="server">
            <asp:Label ID="LBexternalLong_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalLong">Identificativo numerico (long):</asp:Label>
            <asp:TextBox ID="TXBexternalLong" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBexternalDuplicateLong" runat="server" CssClass="Testo_Campo" Visible="false"
                AssociatedControlID="TXBexternalLong"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVexternalLong" runat="server" ControlToValidate="TXBexternalLong"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="RNVexternalLong" runat="server" SetFocusOnError="true" ControlToValidate="TXBexternalLong"
                MinimumValue="0" Type="Integer"></asp:RangeValidator>
        </span><span class="Fieldrow BottomSpacing" id="SPNexternalString" runat="server">
            <asp:Label ID="LBexternalString_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBexternalString">Identificativo stringa:</asp:Label>
            <asp:TextBox ID="TXBexternalString" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBexternalDuplicateString" runat="server" CssClass="Testo_Campo" Visible="false"
                AssociatedControlID="TXBexternalString"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVexternalString" runat="server" ControlToValidate="TXBexternalString"></asp:RequiredFieldValidator>
        </span><span class="Fieldrow BottomSpacing">
            <asp:Button ID="BTNundoExternal" runat="server" CssClass="Link_Menu" Text="Back"
                CausesValidation="false" />&nbsp;
            <asp:Button ID="BTNsaveExternal" runat="server" CssClass="Link_Menu" Text="Save" />
        </span>
    </asp:View>
    <asp:View ID="VIWadd" runat="server">
        <span class="Fieldrow">
            <asp:Label ID="LBauthenticationTypes_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLauthentication">Tipi:</asp:Label>
            <asp:DropDownList ID="DDLauthentication" runat="server" AutoPostBack="true" CssClass="Testo_Campo">
            </asp:DropDownList>
        </span>
        <span class="Fieldrow" id="SPNaddExternalLong" runat="server">
            <asp:Label ID="LBaddExternalLong_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBaddExternalLong">Identificativo numerico (long):</asp:Label>
            <asp:TextBox ID="TXBaddExternalLong" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBaddExternalDuplicateLong" runat="server" CssClass="Testo_Campo"
                Visible="false" AssociatedControlID="TXBaddExternalLong"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVaddExternalLong" runat="server" ControlToValidate="TXBaddExternalLong" ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="RNVaddExternalLong" runat="server" SetFocusOnError="true"
                ControlToValidate="TXBaddExternalLong" MinimumValue="0" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
        </span>
        <span class="Fieldrow" id="SPNaddExternalString" runat="server">
            <asp:Label ID="LBaddExternalString_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBaddExternalString">Identificativo stringa:</asp:Label>
            <asp:TextBox ID="TXBaddExternalString" runat="server" Columns="40" CssClass="Testo_Campo"
                MaxLength="50"></asp:TextBox>
            <asp:Label ID="LBaddExternalDuplicateString" runat="server" CssClass="Testo_Campo"
                Visible="false" AssociatedControlID="TXBaddExternalString"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVaddExternalString" runat="server" ControlToValidate="TXBaddExternalString" ErrorMessage="*"></asp:RequiredFieldValidator>
        </span>
        <span class="Fieldrow BottomSpacing" id="SPNaddInternal" runat="server">
            <asp:Label ID="LBaddLogin_t" runat="server" AssociatedControlID="TXBaddLogin" CssClass="Titolo_campo">Current login:</asp:Label>
            <asp:TextBox ID="TXBaddLogin" CssClass="textbox" runat="server"></asp:TextBox>
            <asp:Label ID="LBaddLoginDuplicate" runat="server" CssClass="Testo_Campo" Visible="false"
                AssociatedControlID="TXBaddLogin"></asp:Label>
            <asp:RequiredFieldValidator ID="RFVaddLogin" runat="server" SetFocusOnError="true"
                ControlToValidate="TXBaddLogin" ErrorMessage="*"></asp:RequiredFieldValidator>
        </span>
        <span class="Fieldrow BottomSpacing">
            <asp:Button ID="BTNundoAdd" runat="server" Text="Back" CausesValidation="false" />&nbsp;
            <asp:Button ID="BTNadd" runat="server"  Text="Add" />
        </span>
    </asp:View>
    <asp:View ID="VIWmessage" runat="server">
        <asp:Label ID="LBmessage" runat="server" CssClass="TopSpacing BottomSpacing"></asp:Label>
        <asp:Button ID="BTNback" runat="server" CssClass="Link_Menu BottomSpacing" Text="Back" />
    </asp:View>
    <asp:View ID="VIWunknownUser" runat="server">
        <asp:Label ID="LBunknownUser" runat="server" CssClass="TopSpacing BottomSpacing"></asp:Label>
    </asp:View>
</asp:MultiView>