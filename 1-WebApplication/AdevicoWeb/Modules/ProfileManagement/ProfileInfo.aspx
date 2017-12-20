<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master"
    CodeBehind="ProfileInfo.aspx.vb" Inherits="Comunita_OnLine.ProfileInfo" %>

<%@ Register TagPrefix="CTRL" TagName="ProfileBaseInfo" Src="./UC/UC_ProfileBaseInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileAdvancedInfo" Src="./UC/UC_ProfileAdvancedInfo.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ProfileCommunities" Src="./UC/UC_ProfileCommunities.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="UserInfo.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="Info_Main">
    <asp:MultiView ID="MLVcontent" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWmessages" runat="server">
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBmessages" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWprofile" runat="server">
            <div class="TopInfo">
                <span class="ImageInfo">
                    <asp:Image ID="IMGavatar" runat="server" Visible="False" ToolTip="Immagine Personale"
                        Height="125px" Width="100px"></asp:Image>
                </span>
                <span class="TextInfo">
                    <span class="Field_Row">
                        <asp:Label ID="LBdisplayName_t" runat="server" CssClass="Titolo_Campo">User</asp:Label>
                        <asp:Label ID="LBdisplayName" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                    <span class="Field_Row">
                        <asp:Label ID="LBprofileType_t" runat="server" CssClass="Titolo_Campo"></asp:Label>
                        <asp:Label ID="LBprofileType" runat="server" CssClass="Testo_Campo"></asp:Label>
                    </span>
                </span>

            </div>
            <div>
                <telerik:RadTabStrip ID="TBSprofile" runat="server"  Align="Justify"
                    CausesValidation="false" AutoPostBack="false" Skin="Outlook" EnableEmbeddedSkins="true" CssClass="InfoTab">
                    <Tabs>
                        <telerik:RadTab Text="Base info" Value="baseInfo" Visible="false">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Advanced Info" Value="advancedInfo" Visible="false">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Communities" Value="communityInfo" Visible="false">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
            </div>
            <div class="BottomInfo">
                
                <asp:MultiView ID="MLVuserInfo" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWempty" runat="server">
                    </asp:View>
                    <asp:View ID="VIWbaseInfo" runat="server">
                        <CTRL:ProfileBaseInfo ID="CTRLbaseInfo" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWadvancedInfo" runat="server">
                        <CTRL:ProfileAdvancedInfo ID="CTRLadvancedInfo" runat="server" />
                    </asp:View>
                    <asp:View ID="VIWcommunitiesInfo" runat="server">
                        <CTRL:ProfileCommunities ID="CTRLprofileCommunities" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
    </asp:MultiView>
    </div>
</asp:Content>
