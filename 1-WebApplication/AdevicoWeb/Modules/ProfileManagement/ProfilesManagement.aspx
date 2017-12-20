<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ProfilesManagement.aspx.vb" Inherits="Comunita_OnLine.AuthenticationProfilesManagement" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagName="CTRLmessages" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="AuthenticationProviders" Src="./UC/UC_ProfileAuthenticationProviders.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AlphabetSelector" Src="~/Modules/Common/UC/UC_AlphabetSelector.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProfileManagement/css/ProfileManagement.css?v=201602221000lm" rel="Stylesheet" type="text/css" />
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

      function showDialog(id) {
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        $(document).ready(function () {
            $('#AuthenticationProviders').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "<%#Me.TranslateModalView("AuthenticationProviders") %>",
                width: 850,
                height: 500,
                minHeight: 400,
                minWidth: 700,
                zIndex: 99999,
                resizable:false,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }
            });
        });

        
        function onUpdating() {
            $.blockUI({ message: '<h1><%#Me.OnLoadingTranslation %></h1>' });
            return true;
        }     
                    
    </script>
    <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
        <script language="javascript">
            $(function () {
                showDialog("AuthenticationProviders");
            });
        </script>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" class="DVmenu" runat="server">
        <asp:HyperLink ID="HYPimportProfile" runat="server" CssClass="Link_Menu" Text="Add profile"
            Height="18px" CausesValidation="false"></asp:HyperLink>
        <asp:HyperLink ID="HYPaddProfile" runat="server" CssClass="Link_Menu" Text="Add profile"
            Height="18px" CausesValidation="false"></asp:HyperLink>
    </div>
    <!--<div class="DVmenu">-->
    <div >
         <CTRL:CTRLmessages ID="CTRLmessages" runat="server" Visible="false" />
         <div style="display:none" id="updateProgressDiv">
         <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> 
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
                                    <div style="float: left; width: 100px;">
                                        <span style="vertical-align: middle">
                                            <asp:Label ID="LBauthenticationType_t" runat="server" CssClass="FiltroVoceSmall">Auth. Type:</asp:Label>
                                        </span>
                                    </div>
                                    <div style="float: left; width: 370px;">
                                        <span style="vertical-align: middle">
                                            <asp:DropDownList ID="DDLauthenticationType" runat="server" CssClass="FiltroCampoSmall"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div style="float: left; width: 270px" id="DVagencyFilter" runat="server" visible="false">
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
                                            <asp:Label ID="LBdisplayLoginInfo" runat="server" CssClass="FiltroVoceSmall" AssociatedControlID="CBXdisplayLoginInfo">Display login:</asp:Label>
                                        </span>
                                    </div>
                                    <div style="float: left; width: 140px;">
                                        <span style="vertical-align: middle">
                                            <asp:CheckBox ID="CBXdisplayLoginInfo" runat="server" AutoPostBack="true"/>
                                        </span>
                                    </div>
                                    <div style="float: left; width: 75px;">
                                        <span style="vertical-align: middle">
                                            <asp:Label ID="LBtipoRicerca_t" runat="server" CssClass="FiltroVoceSmall">Find by:</asp:Label>&nbsp;
                                        </span>
                                    </div>
                                    <div style="float: left; vertical-align: middle; width: 145px;">
                                        <asp:DropDownList ID="DDLsearchBy" runat="server" CssClass="FiltroCampoSmall" Width="130px">
                                            <asp:ListItem Value="-1">tutti</asp:ListItem>
                                            <asp:ListItem Value="1">Nome</asp:ListItem>
                                            <asp:ListItem Value="2" Selected="True">Cognome</asp:ListItem>
                                            <asp:ListItem Value="3">Data di Nascita</asp:ListItem>
                                            <asp:ListItem Value="4">Matricola</asp:ListItem>
                                            <asp:ListItem Value="5">Mail</asp:ListItem>
                                            <asp:ListItem Value="6">Codice Fiscale</asp:ListItem>
                                            <asp:ListItem Value="7">Login</asp:ListItem>
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
                                            <tr>
                                                <th style="width: 50%;">
                                                    <asp:Label ID="LBnameSurname_t" runat="server">Surname and Name</asp:Label>
                                                </th>
                                                <th id="THcompanyName" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                                    <asp:Label ID="LBcompanyName_t" runat="server">Company name</asp:Label>
                                                </th>
                                                <th id="THagencyName" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                                    <asp:Label ID="LBagencyName_t" runat="server">Agency</asp:Label>
                                                </th>
                                                <th style="width: 12%;" runat="server" id="THlogin" visible='<%#me.isColumnVisible(2) %>'>
                                                    <asp:Label ID="LBlogin_t" runat="server">Login</asp:Label>
                                                </th>
                                                <th id="THprofileType" style="width: 12%;" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                                    <asp:Label ID="LBprofileType_t" runat="server">Profile</asp:Label>
                                                </th>
                                                <th style="width: 2%;">
                                                    <asp:Label ID="LBstatusInfo_t" runat="server">S</asp:Label>
                                                </th>
                                                <th style="width: 11%;">
                                                    <asp:Label ID="LBstatus_t" runat="server">Status</asp:Label>
                                                </th>
                                                <th style="width: 18%;">
                                                    <asp:Label ID="LBauthentication_t" runat="server">Aut.</asp:Label>
                                                </th>
                                                <th class="actions" style="width: 6%; text-align: right;">
                                                    <asp:Label ID="LBactions" runat="server"></asp:Label>
                                                </th>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class='<%#me.BackGroundItem(Container.DataItem.Status) %>'>
                                            <td class="user">
                                                <div class="fieldinfo">
                                                    <%#Container.DataItem.Profile.SurnameName%>
                                                    -
                                                    <asp:HyperLink NavigateUrl='<%# "mailto:" & Container.Dataitem.Profile.Mail%>' Text='<%# Container.Dataitem.Profile.Mail%>'
                                                        runat="server" ID="HYPMail" CssClass="ROW_ItemLink_Small" />
                                                    <asp:Label ID="LBlogin" runat="server" Visible="false"></asp:Label>
                                                </div>
                                                <div class="fieldcommands">
                                                    <div id="DVlogin" runat="server" style="float: left;" visible="false">
                                                        [<asp:LinkButton ID="LNBlogin" runat="server" Text="" CommandName="login" CausesValidation="false"
                                                            CommandArgument='<%#Container.DataItem.Id %>' CssClass="ROW_ItemLink_Small"></asp:LinkButton>]&nbsp;
                                                    </div>
                                                    <div id="DVeditType" runat="server" style="float: left;" visible="false">
                                                        &nbsp;[<asp:HyperLink ID="HYPeditType" runat="server" CssClass="ROW_ItemLink_Small">edit</asp:HyperLink>]
                                                    </div>
                                                    <div id="DVcommunities" runat="server" style="float: left;" visible="false">
                                                        &nbsp;[<asp:HyperLink ID="HYPcommunities" runat="server" CssClass="ROW_ItemLink_Small">edit</asp:HyperLink>]
                                                    </div>
                                                </div>
                                            </td>
                                            <td  class="company" id="TDcompanyName" runat="server" visible='<%#me.isColumnVisible(1) %>'>
                                                <asp:Label ID="LBcompanyName" runat="server"></asp:Label>
                                            </td>
                                             <td class="agency" id="TDagencyName" runat="server" visible='<%#me.isColumnVisible(7) %>'>
                                                <asp:Label ID="LBagencyName" runat="server"></asp:Label>
                                            </td>
                                            <td class="login" id="TDlogin" runat="server" visible='<%#me.isColumnVisible(2) %>'>
                                                <%#Container.DataItem.Profile.Login%>
                                            </td>
                                            <td class="type" id="TDtype" runat="server" visible='<%#me.isColumnVisible(3) %>'>
                                                <%#Container.DataItem.TypeName%>
                                            </td>
                                            <td  class="statusimg">
                                                <div style="text-align: center;">
                                                    <asp:Image ID="IMGstatus" runat="server" />
                                                </div>
                                            </td>
                                            <td  class="status">
                                                <asp:Label ID="LBstatus" runat="server" Visible="false"></asp:Label>
                                                <asp:LinkButton ID="LNBstatus" runat="server" Text="" Visible="false" CommandName="changestatus"
                                                    CausesValidation="false" CssClass="ROW_ItemLink_Small" CommandArgument='<%#Container.DataItem.Id %>'></asp:LinkButton>
                                            </td>
                                            <td class="authtype">
                                                <div class="fieldinfo">
                                                    <asp:Label ID="LBpwd" runat="server" Text='<%#Container.Dataitem.AuthenticationTypeName%>'></asp:Label>
                                                </div>
                                                <div class="fieldcommands">
                                                    <asp:LinkButton ID="LNBloginInfoManage" runat="server" Visible="false" CommandName="manageProvider"
                                                        CommandArgument='<%#Container.DataItem.Id %>' > (gestione)</asp:LinkButton>
                                                    <asp:LinkButton ID="LNBpassword" runat="server" Text="Send new" Visible="false" CausesValidation="false"
                                                        CommandName="renewPassword" CssClass="ROW_ItemLink_Small" CommandArgument='<%#Container.DataItem.Id %>'></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBloginInfoAdd" runat="server" Visible="false" CommandName="addProvider"
                                                        CommandArgument='<%#Container.DataItem.Id %>'>+</asp:LinkButton>
                                                </div>
                                            </td>
                                            <td class="actions">
                                                <span class="icons">
                                                    <asp:HyperLink ID="HYPdelete" runat="server" CssClass="icon delete ">D</asp:HyperLink>
                                                    <asp:HyperLink ID="HYPedit" runat="server" CssClass="icon edit">E</asp:HyperLink>
                                                    <asp:HyperLink ID="HYPinfo" runat="server" CssClass="icon info newWindow" Target="_blank">I</asp:HyperLink>
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
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
    <div id="AuthenticationProviders" style="display: none;">
        <CTRL:AuthenticationProviders ID="CTRLprofileProviders" runat="server" isAjaxPanel="false"  />
    </div>
</asp:Content>