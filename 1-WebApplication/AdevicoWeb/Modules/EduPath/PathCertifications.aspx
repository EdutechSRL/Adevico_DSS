<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PathCertifications.aspx.vb" Inherits="Comunita_OnLine.PathCertifications" %>
<%@ Register Src="UC/UC_TimeStat.ascx" TagName="SelectTime" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
*Path Certifications
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView runat="server" ID="MLVpathCertifications">
        <asp:View runat="server" ID="VIWcertificates">
            <div class="DivEpButton">
                <div class="viewType">                    
                    <asp:HyperLink ID="HYPuserStat" runat="server" CssClass="img_link ico_usr_perm_l"  Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPpathStat" runat="server" CssClass="img_link ico_stat_l" Visible="false"></asp:HyperLink>
                </div>
                <asp:HyperLink ID="HYPeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:HyperLink>
                <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="Link_Menu"></asp:HyperLink>
            </div>            
            <div class="pathCertifications certificates">
                <div class="fieldobject filters">
                    <div class="fieldrow">                    
                        <asp:Label runat="server" ID="LBcertificatefilterTitle" CssClass="fieldlabel">**Certificate</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLcertificateFilter" />                    
                    </div>
                    <div class="fieldrow">                    
                        <asp:Label runat="server" ID="LBuserfilterTitle" CssClass="fieldlabel">**User</asp:Label>                                        
                        <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox><span class="icons">&nbsp;<span class="icon delete clearIt">&nbsp;</span></span>
                    </div>
                    <div class="fieldrow">                    
                        <asp:Label runat="server" ID="LBcommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
                        <asp:DropDownList runat="server" ID="DDLroleFilter" />                    
                    </div>
                    <div class="fieldrow">                    
                        <asp:Button runat="server" ID="BTNupdateFilter" Text="**Update Filter" CssClass="linkMenu" />
                    </div>
                </div>
                <asp:Repeater runat="server" ID="RPTCertificates">
                <HeaderTemplate>
                    <table class="table light">
                    <thead>
                        <tr>
                            <th>*Certificate Name</th>
                            <th>*Stats</th>
                        </tr>
                    </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label runat="server" ID="LBcertificateName">**CertificateName</asp:Label></td>
                        <td>*0 / 100 participants</td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
                </asp:Repeater>
            </div>
        </asp:View>
        <asp:View ID="VIWusers" runat="server">

        </asp:View>
        <asp:View ID="VIWuser" runat="server">
        </asp:View>
        <asp:View runat="server" ID="VIWerror">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
        </asp:View>
   </asp:MultiView>
</asp:Content>