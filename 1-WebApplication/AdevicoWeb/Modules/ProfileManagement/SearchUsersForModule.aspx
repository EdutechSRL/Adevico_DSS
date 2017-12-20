<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SearchUsersForModule.aspx.vb" Inherits="Comunita_OnLine.SearchUsersForModule" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagName="CTRLmessages" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201604071200lm" rel="Stylesheet" type="text/css" />
    <script language="Javascript" type="text/javascript">
        var windowSizeArray = [ "width=400,height=400",
                                "width=1024,height=600,scrollbars=no" ];
 
        $(document).ready(function(){
            $('.newWindow').live("click",function (event){
 
                var url = $(this).attr("href");
                var windowName = "popUp";//$(this).attr("name");
                var windowSize = windowSizeArray[$(this).attr("rel")];
 
                window.open(url, windowName, windowSize);
 
                event.preventDefault();
 
            });
        });

       
        function onUpdating() {
//            var updateProgressDiv =  $get('updateProgressDiv'); 
//            var gridView = $get('DVprofilesTable');
//            var gridViewBounds = Sys.UI.DomElement.getBounds(gridView);
//            var updateProgressDivBounds = Sys.UI.DomElement.getBounds(updateProgressDiv);
//            var x = gridViewBounds.x + Math.round(gridViewBounds.width / 2) - Math.round(updateProgressDivBounds.width / 2);
//            var y = gridViewBounds.y + Math.round(gridViewBounds.height / 2) - Math.round(updateProgressDivBounds.height / 2);
//            Sys.UI.DomElement.setLocation(updateProgressDiv, x, y);  
//            document.getElementById("updateProgressDiv").style.display = '';//displays the div
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
  
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPback" runat="server" CssClass="Link_Menu" Text="Back" visible="false"></asp:HyperLink>
    </div>
    <!--<div class="DVmenu">-->
    <div >
         <CTRL:CTRLmessages ID="CTRLmessages" runat="server" Visible="false" />
         <div style="display:none" id="updateProgressDiv">
         <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> 
            <%-- <div id="imgdivLoading" align="center" valign="middle" runat="server" style="border-style: dotted;
                padding: inherit; margin: auto; position: absolute; visibility: visible; vertical-align: middle;
                border-color: #000066 black black black; border-width: medium; background-color: Gray; width: 900px;">--%>
                Loading...<br />
                <asp:Image ID="imgLoading" runat="server" ImageUrl="./../../Images/Ajax/loading4.gif" />
            </div>
        </div>
        <asp:MultiView ID="MLVprofiles" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWlist" runat="server">
                <div id="DVprofilesTable">
                    <div class="ProfileManagementFilters blocking">
                        <div style="text-align: left; padding-top: 4px; clear: both;">
                            <div style="float: left; width: 100px;">
                                <span class="middle">
                                    <asp:Label ID="LBorganizzazione_t" runat="server" CssClass="FiltroVoceSmall">Organization:</asp:Label>&nbsp;
                                </span>
                            </div>
                            <div style="float: left; width: 370px;">
                                <span class="middle">
                                    <asp:DropDownList ID="DDLorganizations" runat="server" CssClass="FiltroCampoSmall autoblock" 
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div style="float: left; width: 270px;">
                                <span class="middle">
                                    <asp:Label ID="LBprofileType_t" runat="server" CssClass="FiltroVoceSmall">Profile type:</asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDLprofileType" runat="server" CssClass="FiltroCampoSmall autoblock"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                            <div style="float: left; width: 150px;">
                                <span class="middle">
                                    <asp:Label ID="LBstatus_t" runat="server" CssClass="FiltroVoceSmall">Status:</asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDLstatus" runat="server" CssClass="FiltroCampoSmall" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <div style="text-align: left; padding-top: 4px; clear: both;">
                            <div style="float: left; " id="DVagencyFilter" runat="server" visible="false">
                                <span style="vertical-align: middle">
                                    <asp:Label ID="LBagencyFilter_t" runat="server" CssClass="FiltroVoceSmall" AssociatedControlID="DDLagencies">Agency:</asp:Label>&nbsp;
                                    <asp:DropDownList ID="DDLagencies" runat="server" CssClass="FiltroCampoSmall" Width="200px" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <div style="text-align: left; padding-top: 4px; clear: both;">
                            <div style="float: left; width: 100px;">
                                <span style="vertical-align: middle">
                                    <asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Find by:</asp:Label>&nbsp;
                                </span>
                            </div>
                            <div style="float: left; vertical-align: middle; width: 140px;">
                                <asp:DropDownList ID="DDLsearchBy" runat="server" CssClass="FiltroCampoSmall" Width="130px">
                                    <asp:ListItem Value="-1">tutti</asp:ListItem>
                                    <asp:ListItem Value="1">Nome</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="True">Cognome</asp:ListItem>
                                    <asp:ListItem Value="3">Data di Nascita</asp:ListItem>
                                    <asp:ListItem Value="4">Matricola</asp:ListItem>
                                    <asp:ListItem Value="5">Mail</asp:ListItem>
                                    <asp:ListItem Value="6">Codice Fiscale</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div style="float: left; vertical-align: middle; ">
                                <span style="vertical-align: middle">
                                    <asp:Label ID="LBvalore_t" runat="server" CssClass="FiltroVoceSmall">Valore:</asp:Label>
                                    <asp:TextBox ID="TXBvalue" runat="server" MaxLength="300" CssClass="FiltroCampoSmall"
                                        Columns="30"></asp:TextBox>
                                </span>
                            </div>
                            <div style="float: left; text-align: right; width: 150px;">
                                <span style="vertical-align: middle">
                                    <asp:Button ID="BTNcerca" runat="server" Text="Cerca" OnClientClick="return onUpdating();"/> <%--OnClientClick="$(document).ready(function (){onUpdating();});return true;" --%>
                                </span>
                            </div>
                        </div>
                        <div class="abcupdate clearfix">
                            <div class="fieldrow left abcfilters" runat="server" id="Div1">
                                <CTRL:AlphabetSelector ID="CTRLalphabetSelector" runat="server" RaiseSelectionEvent="true">
                                </CTRL:AlphabetSelector>
                            </div>
                            <div class="fieldrow right updatefilter clearfix">
                                <asp:CheckBox ID="CBXautoUpdate" runat="server" AutoPostBack="True" Text="Update results"
                                    CssClass="lettera"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                    <div style="width: 900px; text-align: center; padding-top: 5px; margin: 0px auto;
                        clear: both;">
                        <asp:Repeater ID="RPTprofiles" runat="server">
                            <HeaderTemplate>
                                <table class="table light">
                                    <thead>
                                        <tr><%-- class="ROW_header_Small_Center">--%>
                                            <th style="width: 50%;">
                                                <asp:Label ID="LBnameSurname_t" runat="server">Surname and Name</asp:Label>
                                            </th>
                                            <th id="THcompanyName" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                                <asp:Label ID="LBcompanyName_t" runat="server">Company name</asp:Label>
                                            </th>
                                            <th id="THagencyName" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                                <asp:Label ID="LBagencyName_t" runat="server">Agency</asp:Label>
                                            </th>
                                            <th id="THprofileType" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                                <asp:Label ID="LBprofileType_t" runat="server">Profile</asp:Label>
                                            </th>
                                            <th style="width: 2%;">
                                                <asp:Label ID="LBstatusInfo_t" runat="server">S</asp:Label>
                                            </th>
                                            <th class="actions" style="width: 6%; text-align: right;">
                                                <asp:Label ID="LBactions" runat="server"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class='<%#me.BackGroundItem(Container.DataItem.Status) %>'>
                                    <td style="width: 50%; padding-left: 5px;">
                                        <div>
                                            <%#Container.DataItem.Profile.SurnameName%>
                                        </div>
                                    </td>
                                    <td style="width: 12%; padding-left: 5px;" id="TDcompanyName" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                        <asp:Label ID="LBcompanyName" runat="server"></asp:Label>
                                    </td>
                                        <td style="width: 12%; padding-left: 5px;" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                        <asp:Label ID="LBagencyName" runat="server"></asp:Label>
                                    </td>
                                    <td style="width: 12%; padding-left: 5px;" id="TDtype" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                        <%#Container.DataItem.TypeName%>
                                    </td>
                                    <td style="width: 2%; text-align: center;">
                                        <div style="text-align: center;">
                                            <asp:Image ID="IMGstatus" runat="server" />
                                        </div>
                                    </td>
                                    <td class="actions" style="width: 8%; text-align: right;">
                                        <span class="icons">
                                            <asp:HyperLink ID="HYPinfo" runat="server" CssClass="icon info newWindow" Target="_blank" Visible="false">I</asp:HyperLink>
                                            <asp:HyperLink ID="HYPmoduleAction" runat="server" CssClass="icon view"></asp:HyperLink>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                    </tbody>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
                        <div style="text-align: left; width: 50%; float: left;">
                            <div style="text-align: left;" runat="server" id="DIVpageSize" visible="false">
                                <asp:Label ID="LBpagesize" runat="server" CssClass="Titolo_campoSmall"></asp:Label>&nbsp;
                                <asp:DropDownList ID="DDLpage" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                                    <asp:ListItem Value="50">50</asp:ListItem>
                                    <asp:ListItem Value="100">100</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div style="text-align: right; width: 50%; float: left;">
                            <CTRL:GridPager ID="PGgrid" runat="server" ShowNavigationButton="false" EnableQueryString="false"
                                Visible="false"></CTRL:GridPager>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWerrors" runat="server">
                <div style="padding-top: 180px; padding-bottom: 180px;">
                    <asp:Label ID="LBerrors" runat="server"></asp:Label>
                </div>
                <div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>