<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EditPassword.aspx.vb" Inherits="Comunita_OnLine.EditPassword" ValidateRequest="true" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Authentication.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="change_pwd">
        <asp:MultiView ID="MLVaccountInfo" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWinfo" runat="server">
                <br />
                <br />
                <asp:Repeater ID="RPTaccountInfo" runat="server">
                    <ItemTemplate>
                        <fieldset>
                            <div>
                                <div class="Box80">
                                    <asp:Label ID="LBproviderName" runat="server" CssClass="Titolo_Campo"></asp:Label>
                                    <br />
                                    <asp:Label ID="LBproviderDescription" runat="server"></asp:Label>
                                </div>
                                <div class="Box20">
                                    &nbsp;
                                    <asp:Label ID="LBproviderInfo" runat="server"></asp:Label>
                                    <asp:Label ID="LBpasswordExpiresOn" runat="server"></asp:Label>
                                    <asp:HyperLink ID="HYPchangePassword" runat="server" Target="_blank" CssClass="Link_menu" Visible="false"></asp:HyperLink>
                                    <asp:Button ID="BTNchangePassword" CssClass="Link_menu" runat="server" Visible="false" CommandName="edit"/>
                                    &nbsp;
                                </div>
                            </div>
                            <div style="clear:both;"></div>
                        </fieldset>
                    </ItemTemplate>
                </asp:Repeater>
                <br />
                <br />
            </asp:View>
            <asp:View ID="VIWinternal" runat="server">
                <div class="EditPassword_DataBox">
                    <asp:Label ID="LBpasswordOld_t" runat="server" AssociatedControlID="TXBoldPpassword" CssClass="Titolo_Campo">Current password:</asp:Label>
                    <asp:TextBox ID="TXBoldPpassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVoldPassword" Display="Dynamic" runat="server" 
                        ControlToValidate="TXBoldPpassword" EnableClientScript="true" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="LBpasswordNew_t" runat="server" AssociatedControlID="TXBnewPassword" CssClass="Titolo_Campo">New password:</asp:Label>
                    <asp:TextBox ID="TXBnewPassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVnewPassword" Display="Dynamic" runat="server" 
                        ControlToValidate="TXBnewPassword" ErrorMessage="*" EnableClientScript="true"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="LBpasswordConfirm_t" runat="server" AssociatedControlID="TXBconfirmPassword" CssClass="Titolo_Campo">Confirm password:</asp:Label>
                    <asp:TextBox ID="TXBconfirmPassword" CssClass="textbox" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVconfirmPassword" runat="server" 
                        ControlToValidate="TXBconfirmPassword" ErrorMessage="*" EnableClientScript="true"  Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CMVpassword" runat="server" ControlToCompare="TXBconfirmPassword"
                        ControlToValidate="TXBnewPassword"  Display="Dynamic"  Operator="Equal" EnableClientScript="true"></asp:CompareValidator>
                    <br />
                    <asp:Button ID="BTNundoSavePassword" runat="server" CssClass="Link_Menu" Text="Change password" />
                    <asp:Button ID="BTNsavePassword" runat="server" CssClass="Link_Menu" Text="Change password" CausesValidation="true" />
                </div>
            </asp:View>
            <asp:View ID="VIWmessage" runat="server">
                <span class="Spacing2">
                    <asp:Label ID="LBmessage" runat="server" CssClass="Spacing2"></asp:Label>
                    <asp:Button ID="BTNback" runat="server" CssClass="Link_Menu Spacing2" Text="Back" />
                </span>
            </asp:View>
            <asp:View ID="VIWunknownUser" runat="server">
                <asp:Label ID="LBunknownUser" runat="server" CssClass="Spacing4"></asp:Label>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
