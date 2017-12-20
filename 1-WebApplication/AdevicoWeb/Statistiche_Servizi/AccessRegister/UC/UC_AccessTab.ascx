<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AccessTab.ascx.vb" Inherits="Comunita_OnLine.UC_AccessTab" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadTabStrip ID="TBSusageTime" runat="server" Align="Justify" Width="100%" Height="20px" CausesValidation="False" AutoPostBack="True"  Skin="Outlook" SelectedIndex="0">
    <Tabs>
        <telerik:RadTab Selected="true" Text="My Portal Presence" Value="0" NavigateUrl="MyPortalAccessRegister.aspx?View=MyPortalPresence&Page=0&PageSize=25&Dir=asc&Order=Day" ></telerik:RadTab>
        <telerik:RadTab Text="My communities Presence" Value="1" NavigateUrl="MyCommunityAccessRegister.aspx?View=MyCommunitiesPresence&Dir=asc&Order=Community&Page=0&PageSize=25"></telerik:RadTab>
        <telerik:RadTab Text="Users Portal Presence" Value="2" NavigateUrl="UsersPortalList.aspx?View=UsersPortalPresence&SubView=UsersList&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
        <telerik:RadTab Text="Current community Presence" Value="3" NavigateUrl="MyCurrentCommunityRegister.aspx?View=CurrentCommunityPresence&Page=0&Dir=asc&Order=Hour"></telerik:RadTab>
        <telerik:RadTab Text="Users community Presence" Value="4" NavigateUrl="UsersCurrentCommunityList.aspx?View=UsersCurrentCommunityPresence&SubView=UsersList&Page=0&Dir=asc&Order=Owner"></telerik:RadTab>
        <telerik:RadTab Text="Users reports between date" Value="5" NavigateUrl="FindPortalUserRegisterByDate.aspx?View=BetweenDateUsersPortal&Page=0&Dir=asc&Order=Owner" ></telerik:RadTab>
        <telerik:RadTab Text="Users reports between date" Value="6" NavigateUrl="FindCommunityUserRegisterByDate.aspx?View=BetweenDateUsersCommunity&Dir=asc&Order=Owner" ></telerik:RadTab>
        <telerik:RadTab Text="User Communities List" Value="7" NavigateUrl="OtherCommunityAccessRegister.aspx?View=OtherUserCommunityList&Dir=asc&Order=Community&Page=0&PageSize=25" ></telerik:RadTab>
        <telerik:RadTab Text="User Presence" Value="8" NavigateUrl="OtherUserRegister.aspx?View=OtherUserPresence&Dir=asc&Order=Community&Page=0&PageSize=25" ></telerik:RadTab>
    </Tabs>
</telerik:RadTabStrip>