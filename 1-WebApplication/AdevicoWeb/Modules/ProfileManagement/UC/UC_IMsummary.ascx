<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_IMsummary.ascx.vb" Inherits="Comunita_OnLine.UC_IMsummary" %>
<style type="text/css">
    /*.GenericRow
    {
        clear:both; 
        
    }
    .RowCellLeft
    {
        float: left;
        text-align:left;
        vertical-align:top;
        width: 120px;
    }  
    .RowCellRight
    {
        float: left;
        text-align:left;
    } */ 
</style>

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
    <asp:View ID="VIWsummary" runat="server">
        <div class="fieldobject">
            <div class="fieldrow">
                <asp:Label ID="LBsummaryAuthenticationType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBauthenticationType"></asp:Label>
                <asp:Label ID="LBauthenticationType" runat="server" CssClass="Testo_Campo"></asp:Label>
            </div>
             <div class="fieldrow">
                <asp:Label ID="LBsummaryUserType_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBuserType"></asp:Label>
                <asp:Label ID="LBuserType" runat="server" CssClass="Testo_Campo"></asp:Label>
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBsummaryPrimaryOrganization_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsummaryPrimaryOrganization">Organization:</asp:Label>
                <asp:Label ID="LBsummaryPrimaryOrganization" runat="server" CssClass="Testo_Campo"></asp:Label>
            </div>
            <div class="fieldrow" runat="server" id="DVotherOrganizations">
                <asp:Label ID="LBsummaryOtherOrganizations_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsummaryOtherOrganizations">Other organization:</asp:Label>
                <div class="inlinewrapper">
                    <asp:Label ID="LBsummaryOtherOrganizations" runat="server" CssClass="Testo_Campo"></asp:Label>
                </div>
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBsummarySubscriptions_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsummarySubscriptions"></asp:Label>
                <asp:Label ID="LBsummarySubscriptions" runat="server" CssClass="Testo_Campo"></asp:Label>   
            </div>
            <div class="fieldrow">
                <asp:Label ID="LBsummaryMailSelector_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsummaryMailSelector">Notify to user:</asp:Label>
                <asp:Label ID="LBsummaryMailSelector" runat="server" CssClass="Testo_Campo" ></asp:Label>
            </div>
            <div class="fieldrow" runat="server" id="DVsubscribeTo">
                <asp:Label ID="LBsummarySubscribeTo_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBsummarySubscribeTo"></asp:Label>
                <div class="inlinewrapper">
                    <asp:Label ID="LBsummarySubscribeTo" runat="server" CssClass="Testo_Campo"></asp:Label>
                </div>
            </div> 
        </div>
    </asp:View>
</asp:MultiView>