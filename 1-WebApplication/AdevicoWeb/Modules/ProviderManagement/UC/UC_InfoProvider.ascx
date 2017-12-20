<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InfoProvider.ascx.vb" Inherits="Comunita_OnLine.UC_InfoProvider" %>

<asp:MultiView id="MLVcontrolData" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWempty" runat="server">
    
    </asp:View>
    <asp:View ID="VIWdata" runat="server">
        <span class="Field_Row">
            <asp:Label ID="LBproviderName_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBproviderName">Name:</asp:Label>
            <asp:Label ID="LBproviderName" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBuniqueCode_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBuniqueCode">Unique code:</asp:Label>
            <asp:Label ID="LBuniqueCode" runat="server" CssClass="Testo_Campo"></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBproviderType_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBproviderType">Type:</asp:Label>
            <asp:Label ID="LBproviderType" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBdisplayToUser_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBdisplayToUser">Display to users:</asp:Label>
            <asp:Label ID="LBdisplayToUser" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBallowAdminProfileInsert_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBallowAdminProfileInsert">Allow:</asp:Label>
            <asp:Label ID="LBallowAdminProfileInsert" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>      
        <span class="Field_Row">
            <asp:Label ID="LBallowMultipleInsert_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBallowMultipleInsert">Multiple insert:</asp:Label>
            <asp:Label ID="LBallowMultipleInsert" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>
        <span class="Field_Row">
            <asp:Label ID="LBidentifierFields_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBidentifierFields">Identifier Fields:</asp:Label>
            <asp:Label ID="LBidentifierFields" runat="server" CssClass="Testo_Campo" ></asp:Label>
        </span>

        <asp:MultiView ID="MLVtypes" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWbase" runat="server">
            </asp:View>
            <asp:View ID="VIWinternalProvider" runat="server">
                <span class="Field_Row">
                    <asp:Label ID="LBdateToChangePassword_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBdateToChangePassword">Change password after:</asp:Label>
                    <asp:Label ID="LBdateToChangePassword" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span>
            </asp:View>
            <asp:View ID="VIWurlProvider" runat="server">
                <span class="Field_Row">
                    <asp:Label ID="LBsenderUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBsenderUrl">Sender url:</asp:Label>
                    <asp:Label ID="LBsenderUrl" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBverifyRemoteUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBverifyRemoteUrl">Verify remote url:</asp:Label>
                    <asp:Label ID="LBverifyRemoteUrl" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBremoteLoginUrl_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBremoteLoginUrl">Remote url:</asp:Label>
                    <asp:Label ID="LBremoteLoginUrl" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span>
       
                <span class="Field_Row">
                    <asp:Label ID="LBurlIdentifier_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBurlIdentifier">Identifier:</asp:Label>
                    <asp:Label ID="LBurlIdentifier" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span> 
                <span class="Field_Row">
                    <asp:Label ID="LBdeltaTime_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBdeltaTimeInfo">Time validity:</asp:Label>
                    <asp:Label ID="LBdeltaTimeInfo" runat="server" CssClass="Testo_Campo" ></asp:Label>
                    <asp:Label ID="LBdeltaTime" runat="server" CssClass="Testo_Campo" ></asp:Label>
                </span>
                <span class="Field_Row">
                    <asp:Label ID="LBtokenFormat_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBtokenFormat">Token format:</asp:Label>
                     <asp:Label ID="LBtokenFormat" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>   
                <span class="Field_Row">
                    <asp:Label ID="LBkey_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBkey">Identifier:</asp:Label>
                    <asp:Label ID="LBkey" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span> 
                <span class="Field_Row">
                    <asp:Label ID="LBinitializationVector_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBinitializationVector">Initialization Vector:</asp:Label>
                    <asp:Label ID="LBinitializationVector" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span> 
                <span class="Field_Row">
                    <asp:Label ID="LBencryptionAlgorithm_t" runat="server" CssClass="Titolo_campo" AssociatedControlID="LBencryptionAlgorithm">Encryption Algorithm:</asp:Label>
                    <asp:Label ID="LBencryptionAlgorithm" runat="server" CssClass="Testo_Campo"></asp:Label>
                </span>  
            </asp:View>
        </asp:MultiView>


    </asp:View>
</asp:MultiView>