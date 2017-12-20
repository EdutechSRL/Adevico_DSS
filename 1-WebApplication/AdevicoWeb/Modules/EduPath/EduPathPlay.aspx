<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="EduPathPlay.aspx.vb" Inherits="Comunita_OnLine.EduPathPlay" %>

<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register Src="UC/UC_HelpStatus.ascx" TagName="helpStatus" TagPrefix="CTRL" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Comol_Elle3/Jscript/FileDownloadCookie.js"></script>
    <%= "<!--"%>
    <script type="text/javascript" src="../../Jscript/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="../../Jscript/TSMmenu/jquery-ui-1.7.3.custom.min.js"></script>
    <%= "-->"%>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
    <link href="../../Graphics/Modules/Edupath/css/PfStyle2.css" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            $("li span.switch.autoOpen").addClass("open").removeClass("close");
            CheckOpen();

            $("li span.switch").not(".autoOpen").each(function () {
                var li_el = $(this).parent("li");
                var child_ul = $(li_el).children("ul");
                var sib_li = $(li_el).siblings("li");
                child_ul.hide();
                if ($(child_ul).is(":visible")) {
                    //                    var childSib = $(sib_li).children("ul");
                    //                    childSib.hide();
                    //                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");

                    $(this).addClass("open").removeClass("close");
                } else {
                    $(this).removeClass("open").addClass("close");
                }

                CheckOpen();
            });

            $("li span.switch").live("click", function () {

                var li_el = $(this).parent("li");
                var child_ul = $(li_el).children("ul");
                var sib_li = $(li_el).siblings("li");
                child_ul.slideToggle();
                if ($(child_ul).is(":visible")) {
                    //                    var childSib = $(sib_li).children("ul");
                    //                    childSib.slideUp();
                    //                    childSib.parent("li").children("span.switch").removeClass("open").addClass("close");
                    //$(this).addClass("open").removeClass("close");
                } else {
                    //$(this).removeClass("open").addClass("close");
                }

                $(this).toggleClass("open").toggleClass("close");

                CheckOpen();
            });

            function CheckOpen() {
                $("span.switch.close").each(function () {
                    var li_el = $(this).parent("li");
                    $(li_el).addClass("collapsed").removeClass("expanded");
                });

                $("span.switch.open").each(function () {
                    var li_el = $(this).parent("li");
                    $(li_el).removeClass("collapsed").addClass("expanded");
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <%--<asp:UpdatePanel ID="UDPpath" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>--%>
            <asp:MultiView ID="MLVeduPathView" runat="server" ActiveViewIndex="0">
                <asp:View ID="VIWeduPathView" runat="server">
                    <div class="DivEpButton">
                        <asp:HyperLink ID="HYPlistEduPath" runat="server" Text="**list edu path" CssClass="Link_Menu"></asp:HyperLink>
                        <asp:HyperLink ID="HYPstat" runat="server" CssClass="Link_Menu" />
                    </div>
                    <div align="center">
                        <asp:Label ID="LBErrorMSG" runat="server" CssClass="errore"></asp:Label>
                    </div>
                    <div id="edupath-container">
                        <%--       <asp:Panel class="" runat="server" ID="SPANactivitiesInProgress">
                        <span class="titleInProgress ">
                            
                            
                            
                        </span><span class="activitiesInProgress"><span class="activityInProgressTitle">
                            
                        </span>
                            <asp:Label ID="LBdate" runat="server" Text="**da fare tra .. e .." CssClass="dettagli_CampoSmall"></asp:Label><br />
                            <asp:Label ID="LBdescriptionTitle" runat="server" Text="**Description:" CssClass="Titolo_dettagliSmall"></asp:Label>
                            <br />
                        </span>
                    </asp:Panel>
                    <br />
                    <span class="titleFuture">
                        <asp:Label ID="LBunitList" runat="server" Text="**UnitList" Visible="false"></asp:Label></span>--%>
                        <asp:MultiView ID="MLVprogress" runat="server" ActiveViewIndex="0">
                            <asp:View ID="VIWprogress" runat="server">
                                <div id="progress-bar">
                                    <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                        <asp:MultiView ID="MLVlastActivity" runat="server" ActiveViewIndex="0">
                            <asp:View ID="VIWlastActivity" runat="server">
                                <div id="last-activity">
                                    <h4><asp:Label ID="LBplayerTitle" runat="server" CssClass="" Text="***attività in corso"></asp:Label></h4>
                                    <span class="left">
                                        <span class="img-set">
                                            <span title="status" class="status">
                                                <asp:Image ID="IMGstatus" runat="server" />
                                            </span>
                                            <!-- 	End status	-->
                                            <span title="help" class="help">
                                                <span class=" openStatus inline img_span ico_help_s"></span>
                                            </span>
                                            <!-- 	End status	-->
                                        </span>
                                        <asp:HyperLink ID="HYPcurrentActivityName" runat="server" CssClass="Titolo_dettagliSmall"></asp:HyperLink>&nbsp;
                                      
                                        <p class="description">
                                            <asp:Label ID="LBdescription" runat="server" Text="" CssClass="dettagli_CampoSmall renderedtext"></asp:Label>
                                        </p>
                                    </span>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Repeater ID="RPunit" runat="server"> <%--Repeater UNIT--%>
                            <HeaderTemplate>
                                <ul class="units ui-sortable"> <!-- "units > ui-sortable" -->
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li class="unit" id="LIunit" runat="server">
                                    <span id="SPopenClose" runat="server" class="switch">
                                        <span>[+]</span>
                                    </span>
                                    <div class="heading">
                                        <span class="title left">
                                            <span class="img-set small">
                                                 <!-- 	immagini sinistra	-->
                                                <span title="locked" class="locked">
                                                    <asp:Image ID="IMGvisibility" runat="server" />
                                                </span>
                                                <!-- 	End lucchetto	-->
                                                <span title="status" class="status">
                                                    <asp:Image ID="IMGstatus" runat="server" />
                                                </span>
                                                <!-- 	End status	-->
                                            </span>
                                            <h3>
                                                <asp:Label ID="LBunit" runat="server" CssClass="Titolo_campoSmall" Text="description***"></asp:Label>

                                                &nbsp;
                                            </h3>
                                            <span class="duration">
                                                <asp:Label ID="LBunitWeight" runat="server" />
                                            </span>
                                        </span>
                                        <div class="clearer">
                                        </div>
                                    </div>
                                    <div class="description" id="DIVunitDescription" runat="server">
                                        <asp:Label ID="LBunitDesc" runat="server" CssClass="renderedtext"></asp:Label>
                                    </div>
                                    <asp:Repeater ID="RPactivity" runat="server" OnItemCommand="RpSubAct_ItemCommand"> <%--Repeater ACTIVITY--%>
                                        <HeaderTemplate>
                                            <ul class="activities">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:MultiView ID="MLVsingleSubAct" runat="server">
                                                    <asp:View ID="VIWSingle" runat="server">
                                                        <%-- Single SubActivity --%>
                                                        <li id="LIact_single" runat="server" class="single activity">
                                                            <a name="<%# DataBinder.Eval(Container.DataItem,"Id")%>" ></a>
                                                            <span id="SPsubActName" runat="server" class="row left">
                                                                            <span class="img-set">
                                                                                <!-- 	immagini sinistra	-->
                                                                                <span title="locked" class="locked ">
                                                                                    <asp:Image ID="IMGvisibility_single" runat="server" CssClass="imgSize " />
                                                                                </span>
                                                                                <!-- 	End lucchetto	-->
                                                                                <span title="status" class="status">
                                                                                    <asp:Image ID="IMGstatus_single" CssClass="  " runat="server" />
                                                                                </span>
                                                                                <!-- 	End status	-->
                                                                            </span>
                                                                        <%--<a title="" class="" id="" href="#">--%>
                                                                            <%--<span class="img">[>]</span>--%>
                                                                            <asp:Label ID="LBpoints_single" runat="server" CssClass="" Text="**points"></asp:Label>
                                                                            <asp:PlaceHolder ID="PLHaction_single" runat="server"></asp:PlaceHolder>
                                                                            <asp:LinkButton ID="LKBexecute_single" runat="server" CommandName="executed" CssClass="img_link ico_playmmd_m" />
                                                                            <asp:Label ID="LBdescription_single" runat="server" CssClass="dettagli_CampoSmall" Text="**descrizione testo"></asp:Label>
                                                                            <asp:HyperLink ID="HYPname_single" runat="server" Text="**name"></asp:HyperLink>&nbsp;
                                                                        <%--</a>--%>
                                                                        </span>
                                                                        <span class="right">
                                                                            <span class="img-set small">
                                                                                <%--<span class="mandatory_s">
                                                                                    <asp:Image ID="IMGmandatory_single" runat="server" Visible="false"  />
                                                                                </span>--%>
                                                                                <!-- 	End mandatory	-->
                                                                            </span>
                                                                        </span>
                                                             <span class="title left">
                                                                    <span class="img-set small">
                                                                        <!-- 	immagini sinistra	-->
                                                                        <%--<span title="locked" class="locked imgSize ">
                                                                            <asp:Image ID="IMGvisibility_single_activity" runat="server" />
                                                                        </span>--%>
                                                                        <!-- 	End lucchetto	-->
                                                                        <%--<span title="status" class="status">
                                                                            <asp:Image ID="IMGstatus_single_activity" CssClass="  " runat="server" />
                                                                        </span>--%>
                                                                    </span>
                                                                          <!-- 	End status	-->
                                                                    
                                                                    <span class="duration left">
                                                                        <asp:Label ID="LBactWeight_single_activity" runat="server" />
                                                                    </span>
                                                                </span>
                                                                <span class="title right">
                                                                    <span class="expireDate">
                                                                        <asp:Label ID="LBdate_single_activity" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                                                                    </span>
                                                                    <span class="img-set small">
                                                                        <%--<span title="mandatory" class="mandatory_s">
                                                                            <asp:Image ID="IMGmandatory_single_activity" runat="server" />
                                                                        </span>--%>

                                                                        <span class="mandatory_s">
                                                                                    <asp:Image ID="IMGmandatory_single" runat="server" Visible="false"  />
                                                                                </span>
                                                                        <!-- 	End mandatory	-->
                                                                    </span>
                                                                </span>
                                                        </li>
                                                    </asp:View>
                                                    <asp:View ID="VIWMulti" runat="server">
                                                    <%-- Multi SubActivities --%>
                                                    <li class="note" id="LInote" runat="server">
                                                <asp:Label ID="LBtextNote" runat="server" />
                                            </li>
                                            <!-- 	End li.note  -->
                                            <li id="LIact" runat="server" class="activity">
                                                <a name="<%# DataBinder.Eval(Container.DataItem,"Id")%>" ></a>
                                                <div class="heading">
                                                    <span class="title left">
                                                        <span class="img-set small">
                                                            <!-- 	immagini sinistra	-->
                                                            <span title="locked" class="locked imgSize ">
                                                            <asp:Image ID="IMGvisibility" runat="server" />
                                                            </span>
                                                            <!-- 	End lucchetto	-->
                                                            <span title="status" class="status">
                                                                <asp:Image ID="IMGstatus" runat="server" />
                                                            </span>
                                                        </span>
                                                              <!-- 	End status	-->
                                                        <h4><asp:Label ID="LBactName" runat="server" CssClass="Titolo_campoSmall" Text="name" />&nbsp;</h4>
                                                        <span class="duration left">
                                                            <asp:Label ID="LBactWeight" runat="server" />
                                                        </span>
                                                    </span>
                                                    <span class="title right">
                                                        <span class="expireDate">
                                                            <asp:Label ID="LBdate" runat="server" CssClass="dettagli_CampoSmall"></asp:Label>
                                                        </span>
                                                        <span class="img-set small">
                                                            <span title="mandatory" class="mandatory_s">
                                                                <asp:Image ID="IMGmandatory" runat="server" />
                                                            </span>
                                                            <!-- 	End mandatory	-->
                                                        </span>
                                                    </span>
                                                    <div class="clearer">
                                                    </div>
                                                </div>
                                                <div class="description">
                                                    <asp:Label ID="LBdescription" runat="server" />
                                                </div>
                                                <ul id="ULactRules" runat="server" class=" rules ">
                                                    <asp:Repeater ID="RPrule" runat="server">
                                                        <ItemTemplate>
                                                            <span class="right">
                                                                <span class="img-set small"></span>
                                                                <li>
                                                                    <asp:Label ID="LBrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                                                </li>
                                                            </span>
                                                            <div class="clearer">
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ul>
                                                <ul id="UlsubAct" runat="server">
                                                            <asp:Repeater ID="RPsubAct" runat="server" OnItemCommand="RpSubAct_ItemCommand"><%--Repeater SUBACTIVITY--%>
                                                                <ItemTemplate>
                                                                    <li class="subactivity" id="">
                                                                        <span id="SPsubActName" runat="server" class="row left">
                                                                            <span class="img-set">
                                                                                <!-- 	immagini sinistra	-->
                                                                                <span title="locked" class="locked ">
                                                                                    <asp:Image ID="IMGvisibility" runat="server" CssClass="imgSize " />
                                                                                </span>
                                                                                <!-- 	End lucchetto	-->
                                                                                <span title="status" class="status">
                                                                                    <asp:Image ID="IMGstatus" CssClass="  " runat="server" />
                                                                                </span>
                                                                                <!-- 	End status	-->
                                                                            </span>
                                                                        <%--<a title="" class="" id="" href="#">--%>
                                                                            <%--<span class="img">[>]</span>--%>
                                                                            <asp:Label ID="LBpoints" runat="server" CssClass="" Text="**points"></asp:Label>
                                                                            <asp:PlaceHolder ID="PLHaction" runat="server"></asp:PlaceHolder>
                                                                            <asp:LinkButton ID="LKBexecute" runat="server" CommandName="executed" CssClass="img_link ico_playmmd_m" />
                                                                            <asp:Label ID="LBdescription" runat="server" CssClass="dettagli_CampoSmall renderedtext" Text="**descrizione testo"></asp:Label>
                                                                            <asp:HyperLink ID="HYPname" runat="server" Text="**name"></asp:HyperLink>&nbsp;
                                                                        <%--</a>--%>
                                                                        </span>
                                                                        <span class="right">
                                                                            <span class="img-set small">
                                                                                <span title="mandatory" class="mandatory_s">
                                                                                    <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="" />
                                                                                </span>
                                                                                <!-- 	End mandatory	-->
                                                                            </span>
                                                                        </span>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>                                                
                                                
                                            </li>
                                                        
                                                    </asp:View>
                                                </asp:MultiView>


                                            
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <ul id="ULunitRules" runat="server" class=" rules ">
                                        <asp:Repeater ID="RPunitrule" runat="server">
                                            <ItemTemplate>
                                                <li class="rule">
                                                    <asp:Label ID="LBunitrule" runat="server" CssClass="Titolo_campoSmall" Text="rule***"></asp:Label>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    </li>
                                    <li class="note" id="LInote" runat="server">
                                        <%--<p>--%>
                                            <asp:Label ID="LBtextNote" runat="server" />
                                       <%-- </p>--%>
                                    </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
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
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <div id="dialog_target">
    </div>
    <CTRL:helpStatus ID="CTRLhelpStatus" runat="server" />
</asp:Content>
