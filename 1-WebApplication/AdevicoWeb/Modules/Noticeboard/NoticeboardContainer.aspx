<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="NoticeboardContainer.aspx.vb" Inherits="Comunita_OnLine.NoticeboardContainer" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLmessage" Src="./UC/UC_NoticeboardDisplay.ascx" %>
<%@ Import Namespace="lm.Comol.Modules.Base.BusinessLogic" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <%--<link href="me.baseurlGenerici/Bacheca.css" type="text/css" rel="stylesheet" />--%>
    <asp:MultiView ID="MLVnoticeboard" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWmessage" runat="server">
            <div style="text-align:right; padding-top:5px;" >
                 <asp:HyperLink ID="HYPtoDashboard" runat="server" Text="Go to Dashboard" CssClass="linkMenu" Visible="false"></asp:HyperLink>
                 &nbsp;
                 <asp:HyperLink ID="HYPnewMessageADV" runat="server" Text="Open Advanced Editor" CssClass="linkMenu" ></asp:HyperLink>
                 <asp:HyperLink ID="HYPnewMessageHTML" runat="server" Text="Open editor HTML" CssClass="linkMenu" ></asp:HyperLink>
                 &nbsp;
                 
            </div>
            <div>
                <div style="float:left; width:700px;">
                     <CTRL:CTRLmessage id="CTRLmessage" runat="server" ContenitoreWidth="700px"  ContenitoreBachecaWidth="700px" ContenitoreBachecaHeight="450px" isPreview="false" isViewedInDashBoard="false"></CTRL:CTRLmessage>
                </div>
                <div style="float:left;">
                    <div style="width:190px; text-align:left; padding-top:85px; padding-left:10px; clear:both; float:left;">
                        <table border="0" style="text-align:center;" align="center">
                            <asp:Repeater ID="RPTmessages" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td><b><asp:Literal ID="LTheaderMessages" runat="server">Messaggi precedenti</asp:Literal></b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>

                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                     <tr>

                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td align="right">
                                    <CTRL:GridPager id="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
                                </td>
                            </tr>
                        </table>
                    </div> 
                    <div style="width:190px; text-align:right; padding-top:5px; clear:both; height:22px;">
                        <asp:HyperLink ID="HYPfirstFive" runat="server" Text="First Five" SkinID="Menu_Link"></asp:HyperLink>
                        <asp:HyperLink ID="HYPmore" runat="server" Text="More messages" SkinID="Menu_Link"></asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HYPmoreNotDeleted" runat="server" Text="More messages & deleted" SkinID="Menu_Link"></asp:HyperLink>
                    </div>
                </div>
            </div>       
        </asp:View>
        <asp:View runat="server" ID="VIWnone">
      
        </asp:View>
    </asp:MultiView>
    <br />
</asp:Content>