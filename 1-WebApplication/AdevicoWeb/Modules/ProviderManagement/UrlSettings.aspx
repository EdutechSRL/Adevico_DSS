<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Admin_Globale/AdminPortal.Master" CodeBehind="UrlSettings.aspx.vb" Inherits="Comunita_OnLine.UrlSettings" %>

<%@ Register TagPrefix="CTRL" TagName="LoginFormat" Src="UC/UC_AuthenticationProviderLoginFormat.ascx" %>
<%@ MasterType VirtualPath="~/Admin_Globale/AdminPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProviderManagement/Css/ProviderManagement.css" type="text/css" rel="stylesheet" />
    <script language="Javascript" type="text/javascript">
        function showDialog(id) {
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        $(document).ready(function () {
            $('#LoginFormat').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "<%#Me.TranslateModalView("LoginFormat") %>",
                width: 700,
                height: 400,
                minHeight: 400,
                minWidth: 650,
                zIndex: 99999,
                resizable:false,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPbackToManagementTop" runat="server" CssClass="Link_Menu" Text="Back to management"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
         <asp:HyperLink ID="HYPbackToProviderEdit" runat="server" CssClass="Link_Menu" Text="Back to provider"
            Height="18px" CausesValidation="false" Visible="false"></asp:HyperLink>
    </div>
    <asp:MultiView ID ="MLVdata" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
              <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label ID="LBmessage" runat="server"></asp:Label>
                <br />
                <br />
                <br />
                <br />
                <br />
        </asp:View>
        <asp:View ID="VIWproviderData" runat="server">
              <asp:UpdateProgress AssociatedUpdatePanelID="UDPloginFormatList" runat="server" ID="UPPformatsLoading"
                    DisplayAfter="5">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter">
                        </div>
                        <div id="processMessage">
                            <asp:Literal ID="LTprogress" runat="server">Loading...</asp:Literal><br />
                            <asp:Image ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <fieldset class="light">
                    <div class="innerwrapper">
                        <span class="Field_Row topbuttons">
                            <asp:Button ID="BTNupdateConfigurationParameters" runat="server" />
                            <asp:Button ID="BTNvalidate" runat="server" />
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBencryptionAlgorithm_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLencryptionAlgorithm">Encryption Algorithm:</asp:Label>
                                                                    <asp:DropDownList ID="DDLencryptionAlgorithm" runat="server" CssClass="Testo_Campo">
                        <asp:ListItem Value="Des">Des</asp:ListItem>
                        <asp:ListItem Value="Rc2">Rc2</asp:ListItem>
                        <asp:ListItem Value="Rijndael">Rijndael</asp:ListItem>
                        <asp:ListItem Value="TripleDes">TripleDes</asp:ListItem>
                    </asp:DropDownList>
                        </span>  
                        <span class="Field_Row">
                            <asp:Label ID="LBkey_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBkey">Identifier:</asp:Label>
                            <asp:TextBox ID="TXBkey" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVkey" runat="server" CssClass="Validatori" ControlToValidate="TXBkey"
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                    
                            <asp:Label ID="LBkeyValidated" runat="server"></asp:Label>
                        </span> 
                        <span class="Field_Row">
                            <asp:Label ID="LBinitializationVector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBinitializationVector">Initialization Vector:</asp:Label>
                            <asp:TextBox ID="TXBinitializationVector" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFVinitializationVector" runat="server" CssClass="Validatori" ControlToValidate="TXBinitializationVector"
                            Display="Dynamic">*</asp:RequiredFieldValidator>
                            <asp:Label ID="LBvectorValidated" runat="server"></asp:Label>
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBencryptedValue_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBencryptedValue">Encrypted value:</asp:Label>
                            <asp:TextBox ID="TXBencryptedValue" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                            <asp:Button ID="BTNdecrypt" runat="server" CausesValidation="false"/>
                        </span> 
                        <span class="Field_Row" id="SPNdecrypted" runat="server" visible="false">
                            <asp:Label ID="LBdecrypted_t" runat="server" CssClass="Titolo_campo"></asp:Label>
                            <asp:Label ID="LBdecryptedValue" runat="server" CssClass="Testo_Campo"></asp:Label>
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBdecryptedValue_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBdencryptedValue">Decrypted value:</asp:Label>
                            <asp:TextBox ID="TXBdencryptedValue" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                            <asp:Button ID="BTNcrypt" runat="server" CausesValidation="false"/>
                        </span>
                         <span class="Field_Row" id="SPNcrypted" runat="server" visible="false">
                            <asp:Label ID="LBcrypted_t" runat="server" CssClass="Titolo_campo"></asp:Label>
                            <asp:Label ID="LBcryptedValue" runat="server" CssClass="Testo_Campo"></asp:Label>
                        </span>
                    </div>
                </fieldset>
             <asp:UpdatePanel ID="UDPloginFormatList" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
                        clear: both;">
                        <asp:Button ID="BTNaddLoginFormat" runat="server"  Visible="false" CausesValidation="false"></asp:Button>
                    </div>
                    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;
                        clear: both;">
                        <asp:Repeater ID="RPTproviders" runat="server">
                            <HeaderTemplate>
                                <table width="900px" cellpadding="0" cellspacing="0" border="0" class="light">
                                    <tr class="ROW_header_Small_Center">
                                        <th style="width: 20px;">
                                            <asp:Label ID="LBdefault_t" runat="server">Default</asp:Label>
                                        </th>
                                        <th style="width: 430px;">
                                            <asp:Label ID="LBformatName_t" runat="server">Name</asp:Label>
                                        </th>
                                        <th style="width: 200px;">
                                            <asp:Label ID="LBformatBefore_t" runat="server">Before</asp:Label>
                                        </th>
                                        <th style="width: 200px;">
                                            <asp:Label ID="LBformatAfter_t" runat="server">After</asp:Label>
                                        </th>
                                        <th style="width: 50px;">
                                            <asp:Label ID="LBactions_t" runat="server">D</asp:Label>
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class='<%#me.BackGroundItem(Container.DataItem.Deleted,Container.itemType) %>'>
                                    <td style="width: 20px; padding-left: 5px;">
                                        <span class="icons">
                                            <asp:Label ID="LBisDefault" runat="server" class="icon "></asp:Label>
                                        </span>
                                    </td>
                                    <td style="width: 430px; padding-left: 5px;">
                                         <asp:Label ID="LBformatName" runat="server"><%#Container.DataItem.Name %></asp:Label>
                                    </td>
                                    <td style="width: 200px; text-align: center;">
                                        <asp:Label ID="LBformatBefore" runat="server"><%#Container.DataItem.Before%></asp:Label>
                                    </td>
                                    <td style="width: 200px; text-align: center;">
                                        <asp:Label ID="LBformatAfter" runat="server"><%#Container.DataItem.After %></asp:Label>
                                    </td>
                                    <td style="width: 50px; vertical-align: middle; text-align:right;" class="actions">
                                        <span class="icons">
                                            <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualDelete" CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon virtualdelete needconfirm" Visible="false" CausesValidation="false">&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBvirtualUnDelete" runat="server" CommandName="undelete" CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon recover needconfirm" Visible="false" CausesValidation="false">&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon delete needconfirm" Visible="false" CausesValidation="false">&nbsp;</asp:LinkButton>
                                            <asp:LinkButton ID="LNBedit" runat="server" CommandName="edit" CommandArgument='<%#Container.DataItem.Id %>' CssClass="icon edit" Visible="false" CausesValidation="false">&nbsp;</asp:LinkButton>
                                            &nbsp;
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
    <div id="LoginFormat" style="display: none;">
        <asp:UpdatePanel ID="UDPloginFormatView" runat="server" UpdateMode="Conditional"
            ChildrenAsTriggers="true">
            <ContentTemplate>
                <div>
                     <asp:Button ID="BTNundoSaveTop" runat="server" CausesValidation="false"></asp:Button>
                     <asp:Button ID="BTNsaveTop" runat="server" ></asp:Button>
                </div>
                <div>
                    <CTRL:LoginFormat ID="CTRLloginFormat" runat="server" />
                </div>
                <div>
                     <asp:Button ID="BTNundoSaveBottom" runat="server"  CausesValidation="false"></asp:Button>
                     <asp:Button ID="BTNsaveBottom" runat="server" ></asp:Button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>