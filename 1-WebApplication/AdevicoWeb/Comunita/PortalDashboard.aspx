<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PortalDashboard.aspx.vb" Inherits="Comunita_OnLine.PortalDashboard" Theme="Materiale" EnableTheming="true"%>
<%@ Register TagPrefix="CTRL" TagName="CTRLnoticeboardBlock" Src="~/Modules/DashBoard/UC/UC_NoticeboardBlock.ascx" %>
<%@ Register Src="~/Questionari/UserControls/ucDaySurvey.ascx" TagName="UC_DaySurvey" TagPrefix="UC" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div>
        <div style="width:900px; padding-top:7px;">
            <UC:UC_DaySurvey runat="server" ID="UCdaySurvey" IsActive="true" SurveyType="2" ShowPreview="false" ShowResults="false" Width="895" Height="50" NumberOfDaySurveys="1"  />
        </div>
        <div style="width:900px;">
            <div style="float:left; width:450px;">
                 <ctrl:CTRLnoticeboardBlock id="CTRLnoticeboard" runat="server"></ctrl:CTRLnoticeboardBlock>
            </div>
            <div style="float:left; width:20px;">&nbsp;</div>
            <div style="float:left; width:430px;">
                <div style="margin-top:5px;">
                    <div class="fakeFieldset">
	                    <span class="left"><asp:Literal ID="LTlastTenCommunities" runat="server">Le ultime 10 comunità visitate</asp:Literal></span>
	                    <span class="right"> <asp:HyperLink ID="HYPmoreCommunities" runat="server" Text=" More " CssClass="Link_Menu" NavigateUrl="~/Comunita/EntrataComunita.aspx"></asp:HyperLink></span>
                        <hr/>
                    </div>
                    <div>                 
                        <asp:Repeater ID="RPTfirstTenCommunities"  runat="server">
                            <HeaderTemplate>
                                <table width="430px">
                                    <tr class="ROW_header_Small_Center">
                                        <td class="ROW_header_Small_Center"><asp:Literal ID="LTname" runat="server" Text="Nome"></asp:Literal></td>
                                    </tr>
                            </HeaderTemplate>															
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
                                    <tr class="ROW_Normal_Small">
                                        <td class="ROW_Normal_Small" align="right">
                                            <div style="text-align:right; width:400px;">
                                                <asp:HyperLink ID="HYPallCommunities" runat="server" Text=" All communities " NavigateUrl="~/Comunita/EntrataComunita.aspx" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <br /><br /><br /><br />
                    <div class="fakeFieldset">
                         <span class="left"><asp:Literal ID="LTlastNews" runat="server">Ultime NOVITA NELLE TUE COMUNITA'</asp:Literal></span>
                         <span class="right"><asp:HyperLink ID="HYPmoreNews" runat="server" Text=" More " CssClass="Link_Menu" NavigateUrl="~/Notification/CommunitiesLastUpdates.aspx?PageSize=100&Page=0"></asp:HyperLink></span>
                         <hr/> 
                    </div>
                    <div>
                        <asp:Literal ID="LTlastNewsDescription" runat="server">Per conoscere che cosa è successo oggi o nei giorni scorsi all'interno delle comunità alle quali partecipi. Basta premere il pulsante Accedi.
                        </asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>