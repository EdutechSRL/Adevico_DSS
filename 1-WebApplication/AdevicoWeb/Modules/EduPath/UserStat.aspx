<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="UserStat.aspx.vb" Inherits="Comunita_OnLine.EpUserStat" %>

<%@ Register Src="UC/UC_TimeStat.ascx" TagName="SelectTime" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />
    <script>
        $(document).ready(function () {
            $(".DialogDetail").dialog({
                autoOpen: false,
                width: '400px'
            });
            $(".openDetail_Path").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();

                var values = value.split('#');

                $(".DialogDetailPath").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });

                $(".DialogDetailPath").dialog("open");
                $(".DialogDetailPath").dialog("option", "title", "Stato");
                $(".DialogDetailPath").find(".ItemName").html(name);
                //row.removeClass("ui-state-highlight");

                return false;
            });
            $(".openDetail_Unit").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();
                var values = value.split('#');

                $(".DialogDetailUnit").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });

                //            for (i = 0; i < values.length; i++) {



                //                //                alert(values[i]);
                //            }
                $(".DialogDetailUnit").dialog("open");
                $(".DialogDetailUnit").dialog("option", "title", "Stato");
                $(".DialogDetailUnit").find(".ItemName").html(name);


                return false;
            });

            $(".openDetail_Activity").click(function () {

                var row = $(this).parents("tr").first();
                //row.addClass("ui-state-highlight");

                var name = row.find(".ItemName").html();



                var value = row.find("span.hideDetail").html();
                var values = value.split('#');

                $(".DialogDetailAct").find("span.infoDetail").each(function (i, e) {
                    $(this).html(values[i]);
                });
                $(".DialogDetailAct").dialog("open");
                $(".DialogDetailAct").dialog("option", "title", "Stato");
                $(".DialogDetailAct").find(".ItemName").html(name);
                return false;
            });
        });
    </script>
    <link href="PfStyle.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVstat" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWstat" runat="server">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:HyperLink>
                <asp:HyperLink ID="HYPback" runat="server" Text="**back" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <ul class="StatButtons">
                <li>
                    <asp:HyperLink ID="HYPuserStat" runat="server" CssClass="img_link ico_usr_perm_l"
                        Visible="false"></asp:HyperLink></li>
                <li>
                    <asp:HyperLink ID="HYPpathStat" runat="server" CssClass="img_link ico_stat_l" Visible="false"></asp:HyperLink></li>
            </ul>
            <br />
            <ctrl:SelectTime id="CTRLselTime" runat="server"></ctrl:SelectTime>
            <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
            <br />
            <asp:Label ID="LBuserName" runat="server" CssClass="Titolo_campo"></asp:Label>
            <br />
            <table cellpadding="0" cellspacing="0" border="1" class="DataGrid_Generica userStat">
                <tr class="ROW_header_Small_Center">
                    <th class="name ">
                        <asp:Label ID="LBnameTit" runat="server" Text="*Nome"> </asp:Label>
                    </th>
                    <th class="completion ">
                        <asp:Label ID="LBcompletionTit" runat="server" Text="*completion"></asp:Label>
                    </th>
                    <th id="TDmarkTit" runat="server" class="mark ">
                        <asp:Label ID="LBmarkTit" runat="server" Text="*mark"></asp:Label>
                    </th>
                    <th class="status ">
                        <asp:Label ID="LBstatusTit" runat="server" Text="*status"></asp:Label>
                    </th>
                    <th class="detail">
                        &nbsp
                    </th>
                </tr>
                <tr class="ROW_Normal_Small path">
                    <td class="name ">
                        <asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label>
                        <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="openDetail_Path" />
                        <asp:Label ID="LBpathMandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                        <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" Visible="false"></asp:LinkButton>
                        <asp:Image ID="IMGblocked" runat="server" Visible="false" />
                    </td>
                    <td class="completion ">
                        <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                        <asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                    </td>
                    <td id="TDmark" runat="server" class="mark ">
                        <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                        <asp:Image ID="IMGmark" runat="server" Visible="false" />
                    </td>
                    <td class="status ">
                        <asp:Label ID="LBstatus" runat="server" Text="*status" Width="65%"></asp:Label>
                    </td>
                    <td class="detail">
                        &nbsp
                    </td>
                </tr>
                <asp:Repeater ID="RPunit" runat="server" OnItemCommand="RP_ItemCommand">
                    <ItemTemplate>
                        <tr class="ROW_Normal_Small unit">
                            <td class="name ">
                                <asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label>
                                <asp:Image ID="IMGisMandatory" runat="server" Visible="false" />
                                <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="openDetail_Unit" />
                                <asp:Label ID="LBmandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" CommandName="blockedU"
                                    Visible="false"></asp:LinkButton>
                                <asp:Image ID="IMGblocked" runat="server" Visible="false" />
                            </td>
                            <td class="completion ">
                                <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                <asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                            </td>
                            <td id="TDmark" runat="server" visible="false" class="mark ">
                                <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                                <asp:Image ID="IMGmark" runat="server" Visible="false" />
                            </td>
                            <td class="status ">
                                <asp:Label ID="LBstatus" runat="server" Text="-" Width="65%"></asp:Label>
                            </td>
                            <td class="detail">
                                &nbsp
                            </td>
                        </tr>
                        <asp:Repeater ID="RPactivity" runat="server" OnItemCommand="RP_ItemCommand">
                            <ItemTemplate>
                                <tr class="ROW_Normal_Small activity">
                                    <td class="name ">
                                        &nbsp &nbsp
                                        <asp:Label ID="LBname" runat="server" CssClass="ItemName"> </asp:Label>
                                        <asp:Image ID="IMGisMandatory" runat="server" Visible="false" />
                                        <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="openDetail_Activity" />
                                        <asp:Label ID="LBmandatoryItem" runat="server" CssClass="hideDetail"></asp:Label>
                                        <asp:LinkButton ID="LKBblocked" runat="server" Text="Visible**" CommandName="blockedA"
                                            Visible="false"></asp:LinkButton>
                                        <asp:Image ID="IMGblocked" runat="server" Visible="false" />
                                    </td>
                                    <td class="completion ">
                                        <asp:Label ID="LBcompletion" runat="server" Text="-"></asp:Label>
                                        <asp:Image ID="IMGcompletion" runat="server" Visible="false" />
                                    </td>
                                    <td id="TDmark" runat="server" class="mark ">
                                        <asp:Label ID="LBmark" runat="server" Text="-"></asp:Label>
                                        <asp:Image ID="IMGmark" runat="server" Visible="false" />
                                    </td>
                                    <td class="status ">
                                        <asp:Label ID="LBstatus" runat="server" Text="*status" Width="65%"></asp:Label>
                                    </td>
                                    <td class="detail">
                                        <asp:HyperLink ID="HYPdetail" runat="server" Text="*detail" CssClass="Link_Menu" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="DialogDetail DialogDetailPath">
                <%--<span>avanzamento delle unità del percorso </span><span class="ItemName"></span><br /><br />--%>
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogPath" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyCompletedPath" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyCompPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyPassedPath" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyPassPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplPath" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="DialogDetail DialogDetailUnit">
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogUnit" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbOnlyCompUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbOnlyPassUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplUnit" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            <div class="DialogDetail DialogDetailAct">
                <table>
                    <thead>
                        <tr>
                            <td>
                                <asp:Label ID="LbDialogActivity" runat="server"></asp:Label><span class="ItemName"></span>
                            </td>
                        </tr>
                        <%--<tr><td>&nbsp;</td><td>&nbsp;</td></tr>--%>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Label ID="LbTotMandAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td id="TRonlyCompletedAct" runat="server">
                                <asp:Label ID="LbOnlyCompAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr id="TRonlyPassedAct" runat="server">
                            <td>
                                <asp:Label ID="LbOnlyPassAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LbPassComplAct" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span class="infoDetail"></span>
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                ---
                            </td>
                        </tr>
                    </tfoot>
                </table>
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
    </asp:MultiView>
</asp:Content>
