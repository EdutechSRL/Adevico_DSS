<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Portal.aspx.vb" Inherits="Comunita_OnLine.PortalDefaultDashboard"  Theme="Materiale" EnableTheming="true" %>

<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboardBlock" Src="~/Modules/DashBoard/UC/UC_NoticeboardBlock.ascx" %>
<%@ Register Src="~/Modules/Questionnaire/UC/UC_DashboardDaySurvey.ascx" TagName="DaySurvey" TagPrefix="CTRL" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="Dashboard.Common.css?v=201604071200lm" />
    <link rel="Stylesheet" href="../CommunityInfo/CommunityInfo.Common.css?v=201604071200lm" />

    <link href="../../Graphics/Modules/Dashboard/Css/homepage.css?v=201604071200lm" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.pack.js"></script>
    <link rel="stylesheet" href="../../Jscript/Modules/Common/fancybox/jquery.fancybox-1.3.4.css?v=201604071200lm"/>
    <script type="text/javascript" src="../../Jscript/Modules/Dashboard/dashboard.js"></script>

    <script language="javascript" type="text/javascript">
        function showDialog(id) {
            $('#' + id).dialog("open");

        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
        <asp:Literal ID="LTscripts" runat="server" visible="true"></asp:Literal>
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <CTRL:DaySurvey runat="server" ID="UCdaySurvey" IsActive="true"  /> 
    <div class="homepage">
        <div class="list container_12 noticeboardleft clearfix">
            <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboard" runat="server"></ctrl:CTRLnoticeboardBlock>
            <div class="maincontent grid_7">
                <div class="communitylist clearfix first">
                    <div class="sectionheader clearfix">
                        <div class="left">
                            <div class="icon community comtype_48"></div>
                            <h3 class="sectiontitle clearifx"><asp:Literal ID="LTlastTenCommunities" runat="server">Le ultime 10 comunità visitate</asp:Literal></h3>
                        </div>
                        <div class="right "></div>
                    </div>
                    <div class="tablewrapper ">
                        <table class="minimal comlist fullwidth">
                            <thead>
                                <tr>
                                    <th class="communityname"><asp:Literal ID="LTname" runat="server" Text="Nome"></asp:Literal></th>
                                </tr>
                            </thead>
                            <tbody>
                                   <asp:Repeater ID="RPTfirstTenCommunities"  runat="server">													
                                    <ItemTemplate>
                                        <tr class="ROW_Normal_Small">
                                            <td class="ROW_Normal_Small">                              
                                                <asp:Literal ID="LTlogo" runat="server" EnableViewState="false"></asp:Literal>
                                                <asp:Literal ID="LTcommunityName" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.CommunityName %>'></asp:Literal>
                                                <asp:LinkButton ID="LNBlogin" runat="server" Commandname="Login" CausesValidation="false" Visible="false" Text="ENTRA" CssClass="ROW_ItemLink_Small"></asp:LinkButton>
                                                <b><asp:Literal ID="LThasnews" runat="server" Visible="false"></asp:Literal></b>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ROW_Alternate_Small">
                                            <td class="ROW_Alternate_Small">                              
                                                <asp:Literal ID="LTlogo" runat="server" EnableViewState="false"></asp:Literal>
                                                <asp:LinkButton ID="LNBlogin" runat="server" Commandname="Login" CausesValidation="false" Visible="false" Text="ENTRA"  CssClass="ROW_ItemLink_Small"></asp:LinkButton>
                                                <asp:Literal ID="LTcommunityName" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.CommunityName %>'></asp:Literal>
                                                <asp:Literal ID="LThasnews" runat="server" Visible="false"></asp:Literal>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                            <tr class="ROW_Normal_Small hide allcommunities">
                                                <td class="ROW_Normal_Small" align="right">
                                                    <div style="text-align:right; width:400px;">
                                                        <asp:HyperLink ID="HYPallCommunities" runat="server" Text=" All communities " NavigateUrl="~/Comunita/EntrataComunita.aspx" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
                 <div class="fakeFieldset clearfix allcommunities">
	                    <span class="left"></span>
	                    <span class="right"> <asp:HyperLink ID="HYPmoreCommunities" runat="server" Text=" More " CssClass="Link_Menu" NavigateUrl="~/Comunita/EntrataComunita.aspx"></asp:HyperLink></span>
                        <%--<hr/>--%>
                    </div>
                    <div>                 
                     
                    </div>
            </div>
        </div>
    </div>
</asp:Content>