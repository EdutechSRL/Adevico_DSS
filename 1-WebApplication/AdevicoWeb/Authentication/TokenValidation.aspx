<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master" CodeBehind="TokenValidation.aspx.vb" Inherits="Comunita_OnLine.TokenValidation" %>

<%@ MasterType VirtualPath="~/Authentication.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
     <asp:Literal ID="LTredirect" runat="server"></asp:Literal>
    <script type="text/javascript" src="<%=PageUtility.ApplicationUrlBase() %>Jscript/Modules/Common/jquery.preloader.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#loaderImage").preloader({
                frames: 8,
                speed: 20,
                width: 32
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
     <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitleTokenValidation" runat="server">
                Accesso automatico
            </asp:Literal>
        </h2>
        <asp:MultiView ID="MLVtokenValidation" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWvalidation" runat="server">
                <div class="loaderinfo">
                    <asp:Label ID="LBvalidationInfo_t" runat="server" CssClass="text">ATTENZIONE !</asp:Label><br />
                    <asp:Label ID="LBvalidationInfo" runat="server" CssClass="text">
                        IL sistema sta per farla accedere alla piattaforma, attendere la conclusione delle operazioni senza aggiornare la pagina !
                    </asp:Label>
                    <div id="loaderImage"></div>
                </div>
            </asp:View>
            <asp:View ID="VIWinvalidToken" runat="server">
                <div id="submit-feedback">
                    <span class="" runat="server" id="SPNmessages">
                        <asp:Literal ID="LTtaxCodeDuplicate" runat="server">
                            
                        </asp:Literal>
                        <asp:Literal ID="LTinvalidToken" runat="server">
                            
                        </asp:Literal>
                        <br />
                        <asp:Literal ID="LTtokenSupportInfo" runat="server">
                        </asp:Literal>
                        <br />
                        <asp:Literal ID="LTtokenUrl" runat="server">
                        </asp:Literal>
                    </span>
                </div>
            </asp:View>
            <asp:View ID="VIWdebug" runat="server">
                <span >
                    <asp:Label ID="LBstartValidationOn_t" runat="server" AssociatedControlID="LBstartValidationOn" CssClass="Titolo_Campo"></asp:Label>
                    <asp:Label ID="LBstartValidationOn" runat="server"></asp:Label>
                    <br />
                    <asp:Repeater ID="RPTattributes" runat="server">
                        <ItemTemplate>
                            <asp:Label ID="LBattribute_t" runat="server" AssociatedControlID="LBattribute" Text="<%#Container.Dataitem.QueryName %>" CssClass="Titolo_Campo"></asp:Label>
                            <asp:Label ID="LBattribute" runat="server" Text="<%#Container.Dataitem.QueryValue %>"></asp:Label>
                            <br />
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="LBcalculatedMac_t" runat="server" AssociatedControlID="LBcalculatedMac" CssClass="Titolo_Campo"></asp:Label>
                    <asp:Label ID="LBcalculatedMac" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LBtokenResult_t" runat="server" AssociatedControlID="LBtokenResult" CssClass="Titolo_Campo"></asp:Label>
                    <asp:Label ID="LBtokenResult" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LBendValidationOn_t" runat="server" AssociatedControlID="LBendValidationOn" CssClass="Titolo_Campo" ></asp:Label>
                    <asp:Label ID="LBendValidationOn" runat="server"></asp:Label>
                    <br />
                </span>
            </asp:View>
            <asp:View ID="VIWerror" runat="server">
                <div  >
                    <span>
                        <asp:Literal ID="LTmessage" runat="server">
                        </asp:Literal>
                        <br />
                    </span>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Timer ID="TMvalidationTimer" runat="server" >
    </asp:Timer>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>