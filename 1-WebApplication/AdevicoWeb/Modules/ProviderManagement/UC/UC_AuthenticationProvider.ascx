<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AuthenticationProvider.ascx.vb" Inherits="Comunita_OnLine.UC_AuthenticationProvider" %>

<asp:MultiView id="MLVcontrolData" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <span class="Field_Row">
            <asp:Label ID="LBproviderName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBname">Name:</asp:Label>
            <asp:TextBox ID="TXBname" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVname" runat="server" CssClass="Validatori" ControlToValidate="TXBname"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBuniqueCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBuniqueCode">Unique code:</asp:Label>
            <asp:TextBox ID="TXBuniqueCode" runat="server" Columns="20" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFVuniqueCode" runat="server" CssClass="Validatori" ControlToValidate="TXBuniqueCode"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
            <asp:Label ID="LBduplicateUniqueCode" runat="server" CssClass="Testo_Campo" AssociatedControlID="TXBuniqueCode"></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBproviderType_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBproviderType">Type:</asp:Label>
            <asp:Label ID="LBproviderType" runat="server" CssClass="Testo_Campo"></asp:Label>
            <asp:HyperLink ID="HYPadvancedSettings" runat="server" Visible="false" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
            <asp:Literal ID="LTidProviderType" runat="server" Visible="false"></asp:Literal>
        </span>
        <span class="Field_Row" id="SPNlogoutMode" runat="server" visible="false">
            <asp:Label ID="LBlogoutMode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLlogoutMode">Type:</asp:Label>
            <asp:DropDownList ID="DDLlogoutMode" runat="server" CssClass="Testo_Campo">

            </asp:DropDownList>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBdisplayToUser_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXdisplayToUser">Display to users:</asp:Label>
            <asp:CheckBox ID="CBXdisplayToUser" runat="server" CssClass="Testo_Campo" />
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBallowAdminProfileInsert_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXallowAdminProfileInsert">Allow:</asp:Label>
            <asp:CheckBox ID="CBXallowAdminProfileInsert" runat="server" CssClass="Testo_Campo" />
        </span>      
        <span class="Field_Row">
            <asp:Label ID="LBallowMultipleInsert_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXallowMultipleInsert">Multiple insert:</asp:Label>
            <asp:CheckBox ID="CBXallowMultipleInsert" runat="server" CssClass="Testo_Campo" />
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBenabledProvider_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXenabled">Enabled:</asp:Label>
            <asp:CheckBox ID="CBXenabled" runat="server" CssClass="Testo_Campo" />
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBidentifierFields_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBLidentifierFields">Identifier Fields:</asp:Label>
            <asp:CheckBoxList ID="CBLidentifierFields" runat="server" CssClass="Testo_Campo" RepeatLayout="Flow" RepeatDirection="Horizontal">
                <asp:ListItem Value="longField">Long</asp:ListItem>
                <asp:ListItem Value="stringField">String</asp:ListItem>
            </asp:CheckBoxList>
        </span>
        <span class="Field_Row" id="SPNmultipleValues" runat="server">
            <asp:Label ID="LBmultipleValue_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXseparator">Multiple value:</asp:Label>
            <asp:CheckBox ID="CBXseparator" runat="server" CssClass="Testo_Campo" />
            <asp:TextBox ID="TXBseparator" runat="server" Columns="5" CssClass="Testo_Campo" MaxLength="5"></asp:TextBox>
        </span>
        <asp:MultiView ID="MLVtypes" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWbase" runat="server">
            </asp:View>
            <asp:View ID="VIWinternalProvider" runat="server">
                <span class="Field_Row">
                    <asp:Label ID="LBdateToChangePassword_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLpasswordDays">Change password after:</asp:Label>
                    <asp:DropDownList ID="DDLpasswordDays" runat="server" CssClass="Testo_Campo">
                        <asp:ListItem Value="3"></asp:ListItem>
                        <asp:ListItem Value="6"></asp:ListItem>
                        <asp:ListItem Value="9"></asp:ListItem>
                        <asp:ListItem Value="12"></asp:ListItem>
                        <asp:ListItem Value="24"></asp:ListItem>
                        <asp:ListItem Value="36"></asp:ListItem>
                        <asp:ListItem Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </span>
            </asp:View>
            <asp:View ID="VIWgenericUrlProvider" runat="server">
                <span class="Field_Row">
                    <asp:Label ID="LBnotifySubscriptionTo_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBnotifySubscriptionTo">Notify subscription to:</asp:Label>
                    <asp:TextBox ID="TXBnotifySubscriptionTo" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="900"></asp:TextBox>
<%--                    <asp:RegularExpressionValidator ID="REVnotifySubscriptionTo" runat="server" 
                    ControlToValidate="TXBnotifySubscriptionTo" SetFocusOnError="true" 
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBsenderUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBsenderUrl">Sender url:</asp:Label>
                    <asp:TextBox ID="TXBsenderUrl" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVsenderUrl" runat="server" CssClass="Validatori" ControlToValidate="TXBsenderUrl" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="REXsenderUrl" SetFocusOnError="true" ControlToValidate="TXBsenderUrl" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?"></asp:RegularExpressionValidator>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBverifyRemoteUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXverifyRemoteUrl">Verify remote url:</asp:Label>
                    <asp:CheckBox ID="CBXverifyRemoteUrl" runat="server" CssClass="Testo_Campo" />
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBremoteLoginUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBremoteLoginUrl">Remote url:</asp:Label>
                    <asp:TextBox ID="TXBremoteLoginUrl" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                    <asp:RegularExpressionValidator runat="server" ID="REXremoteLogin" SetFocusOnError="true" ControlToValidate="TXBremoteLoginUrl" ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?"></asp:RegularExpressionValidator>
                </span>
                <span class="Field_Row" runat="server" id="SPNurlIdentifier" visible="false">
                    <asp:Label ID="LBurlIdentifier_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBurlIdentifier">Identifier:</asp:Label>
                    <asp:TextBox ID="TXBurlIdentifier" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVidentifier" runat="server" CssClass="Validatori" ControlToValidate="TXBurlIdentifier"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                </span> 
                <span class="Field_Row">
                    <asp:Label ID="LBdeltaTime_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBdeltaTime">Time validity:</asp:Label>
                    <asp:TextBox ID="TXBdeltaTime" runat="server" Columns="5" CssClass="Testo_Campo" MaxLength="10"></asp:TextBox>
                    <asp:Label ID="LBdeltaTime" runat="server" CssClass="Titolo_campo info_text" AssociatedControlID="TXBdeltaTime"> seconds.</asp:Label>
                    <asp:RequiredFieldValidator ID="RFVdeltaTime" runat="server" CssClass="Validatori" ControlToValidate="TXBdeltaTime"
                    Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:rangevalidator ID="RNVdeltaTime" runat="server" CssClass="Validatori" ControlToValidate="TXBdeltaTime"
                    Display="Dynamic" Type="Integer" MinimumValue="5" MaximumValue="7200"></asp:rangevalidator>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBencryptionAlgorithm_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLencryptionAlgoritm">Encryption Algorithm:</asp:Label>
                    <asp:DropDownList ID="DDLencryptionAlgoritm" runat="server" AutoPostBack="true" CssClass="Testo_campo">
                        <asp:ListItem value="None">None</asp:ListItem>
                        <asp:ListItem value="Des">Des</asp:ListItem>
                        <asp:ListItem value="Rc2">Rc2</asp:ListItem>
                        <asp:ListItem value="Rijndael">Rijndael</asp:ListItem>
                        <asp:ListItem value="TripleDes">TripleDes</asp:ListItem>
                        <asp:ListItem value="Md5">Md5</asp:ListItem>
                    </asp:DropDownList>
                    <asp:MultiView ID="MLVencryptionKeys" runat="server" ActiveViewIndex="0">
                        <asp:View runat="server" ID="VIWnoKeys"> </asp:View>
                        <asp:View runat="server" ID="VIWkeys"> 
                            <span class="Field_Row">
                                <asp:Label ID="LBkey_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBkey">Identifier:</asp:Label>
                                <asp:TextBox ID="TXBkey" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVkey" runat="server" CssClass="Validatori" ControlToValidate="TXBkey" Enabled="false"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Button ID="BTNvalidateKey" runat="server" />
                                <asp:Label ID="LBkeyValidated" runat="server"></asp:Label>
                            </span> 
                            <span class="Field_Row">
                                <asp:Label ID="LBinitializationVector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBinitializationVector">Initialization Vector:</asp:Label>
                                <asp:TextBox ID="TXBinitializationVector" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVinitializationVector" runat="server" CssClass="Validatori" ControlToValidate="TXBinitializationVector" Enabled="false"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Button ID="BTNvalidateVector" runat="server" />
                                <asp:Label ID="LBvectorValidated" runat="server"></asp:Label>
                            </span> 
                        </asp:View>
                        <asp:View runat="server" ID="VIWprivatekey"> 
                            <span class="Field_Row">
                                <asp:Label ID="LBprivateKey_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBprivatekey">Private key:</asp:Label>
                                <asp:TextBox ID="TXBprivatekey" runat="server" Columns="80" CssClass="Testo_Campo" MaxLength="250"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="RFVprivateKey" runat="server" CssClass="Validatori" ControlToValidate="TXBprivatekey" Enabled="false"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                            </span>
                        </asp:View>
                    </asp:MultiView>
                    <br /> 
                </span>
                
                <asp:MultiView ID="MLVurlProvider" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWurlEmpty" runat="server"></asp:View>
                    <asp:View ID="VIWurlProvider" runat="server">
                        <span class="Field_Row">
                            <asp:Label ID="LBtokenFormat_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="DDLtokenFormat">Token format:</asp:Label>
                            <asp:DropDownList ID="DDLtokenFormat" runat="server" CssClass="Testo_Campo">
                                <asp:ListItem Value="PrefixDateTimeLogin">PrefixDateTimeLogin</asp:ListItem>
                                <asp:ListItem Value="DateTimeLogin">DateTimeLogin</asp:ListItem>
                                <asp:ListItem Value="LoginDateTime">LoginDateTime</asp:ListItem>
                                <asp:ListItem Value="PrefixLoginDateTime">PrefixLoginDateTime</asp:ListItem>
                            </asp:DropDownList>
                        </span>   
                    </asp:View>
                    <asp:View ID="VIWmacUrlProvider" runat="server">
                        <span class="Field_Row">
                            <asp:Label ID="LBallowRequestFromIpAddresses_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBallowRequestFromIpAddresses">*Allow from IP:</asp:Label>
                            <asp:TextBox ID="TXBallowRequestFromIpAddresses" runat="server" CssClass="Testo_Campo" Rows="3" Columns="100" TextMode="MultiLine"></asp:TextBox>
                        </span>
                        <span class="Field_Row">
                            <label class="Titolo_campo">&nbsp;</label>
                            <asp:Label ID="LBallowRequestFromIpAddressesInfo" runat="server" CssClass="Testo_Campo" AssociatedControlID="TXBallowRequestFromIpAddresses"></asp:Label>
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBdenyRequestFromIpAddresses_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="TXBdenyRequestFromIpAddresses"></asp:Label>
                            <asp:TextBox ID="TXBdenyRequestFromIpAddresses" runat="server" CssClass="Testo_Campo" Rows="3" Columns="100" TextMode="MultiLine"></asp:TextBox>
                        </span>
                        <span class="Field_Row">
                            <label class="Titolo_campo">&nbsp;</label>
                            <asp:Label ID="LBdenyRequestFromIpAddressesInfo" runat="server" CssClass="Testo_Campo" AssociatedControlID="TXBdenyRequestFromIpAddresses"></asp:Label>
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBautoEnroll_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXautoEnroll">Verify remote url:</asp:Label>
                            <asp:CheckBox ID="CBXautoEnroll" runat="server" CssClass="Testo_Campo" />
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBautoAddAgency_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXautoAddAgency">Verify remote url:</asp:Label>
                            <asp:CheckBox ID="CBXautoAddAgency" runat="server" CssClass="Testo_Campo" />
                        </span>
                        <span class="Field_Row">
                            <asp:Label ID="LBallowTaxCodeDuplication" runat="server" CssClass="Titolo_campo" AssociatedControlID="CBXallowTaxCodeDuplication">Verify remote url:</asp:Label>
                            <asp:CheckBox ID="CBXallowTaxCodeDuplication" runat="server" CssClass="Testo_Campo" />
                        </span>
                    </asp:View>
                </asp:MultiView>
            </asp:View>
        </asp:MultiView>
    </asp:View>
</asp:MultiView>