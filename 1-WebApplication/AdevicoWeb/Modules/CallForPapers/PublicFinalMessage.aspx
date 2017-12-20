<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/ExternalService.Master" CodeBehind="PublicFinalMessage.aspx.vb" Inherits="Comunita_OnLine.PublicFinalMessage" %>
<%@ MasterType VirtualPath="~/ExternalService.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server"/>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
   <asp:MultiView ID="MLVpreview" runat="server">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBmessage" runat="server"></asp:Label>
            <br /><br /><br /><br />
        </asp:View>
        <asp:View ID="VIWcall" runat="server">
            <div class="view">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPlistTop" runat="server" Text="Lista bandi" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPviewSubmissionTop" runat="server" Visible="false" CssClass="Link_Menu"></asp:HyperLink>
                </div>
                <div class="cfpdescription ">
                    <div class="textwrapper">
                        <div class="renderedtext"><asp:Literal ID="LTendMessage" runat="server"></asp:Literal></div>
                    </div>
                </div>
                <div class="DivEpButton big">
                    <asp:HyperLink ID="HYPlistBottom" runat="server" Text="Lista bandi" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPviewSubmissionBottom" runat="server" Visible="false" CssClass="Link_Menu"></asp:HyperLink>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>