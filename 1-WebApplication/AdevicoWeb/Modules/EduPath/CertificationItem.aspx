<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CertificationItem.aspx.vb" Inherits="Comunita_OnLine.CertificationItem" %>
<%@ Register TagPrefix="CTRL" TagName="EditCertificationAction" Src="~/Modules/EduPath/UC/UC_EditCertificationAction.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessage.ascx" TagPrefix="CTRL" TagName="Message" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">

<asp:MultiView ID="MLVcertificationItem" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWcertificationItem" runat="server">
            <div class="DivEpButton">
                <asp:Button ID="BTNsave" runat="server"  CssClass="Link_Menu" text="*save" />
                <asp:HyperLink ID="HYPviewActivity" runat="server" Text="*view Act" CssClass="Link_Menu"></asp:HyperLink>
              
            </div>      
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />      
            <CTRL:Message runat="server" ID="CTRLmsg" Visible="false" />
            <CTRL:Messages runat="server" ID="CTRLmsgs" Visible="false"  />

            <%--<div>--%>
                <CTRL:EditCertificationAction runat="server" ID="CTRLcertificationAction" IsInAjaxPanel="false" HideSaveButton="true"  />
            <%--</div>--%>

            <div class="DetailItem DetailOptions">
            <div class="DetailLeftItem">
                
            </div>
            <div class="DetailEditor">
                
            </div>
        </div> 
            
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmessages" />
        </asp:View>
    </asp:MultiView>
</asp:Content>