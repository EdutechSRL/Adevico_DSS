<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="FullPlay.aspx.vb" Inherits="Comunita_OnLine.FullPlay" %>
<%@ Register TagPrefix="CTRL" TagName="ProgressBar" Src="UC/UC_ProgressBar.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register Src="UC/UC_HelpStatus.ascx" TagName="helpStatus" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_TextAction.ascx" TagName="TextAction" TagPrefix="CTRL" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="QuestionnaireItem" Src="~/Modules/Questionnaire/UC/UC_ModuleQuizAction.ascx" %>
<%@ Register Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" TagName="ModalPlayerHeader" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<!-- START HEADER FULLPLAY-->
    <CTRL:ModalPlayerHeader ID="CTRLmodalHeader" runat="server" />
    <script src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.anchor.js" type="text/javascript"></script>
    <script type="text/javascript">
        var DisplayTitle = ""
        var DisplayMessage = "<h1><%=GetBlockUiMessage%></h1>"
    </script>
    
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            $(".units").collapsableTreeAdv({
                preserve: true,
                selUl: "ul.activities, ul.subactivities",
                cookiePrefix : "ep.FullPlay",
                onToggle: function (el, handle) {
                    if (el.hasClass("unit")) {
                        el.find(".collapsable").not(".activity .collapsable").toggle();
                    } else {
                        el.find(".collapsable").toggle();
                    }
                },
                onExpand: function (el, handle) {
                    el.find(".handle").removeClass("collapsed");
                    if (el.hasClass("unit")) {
                        el.find(".collapsable").not(".activity .collapsable").show();
                    } else {
                        el.find(".collapsable").show();
                    }
                },
                onCollapse: function (el, handle) {
                    el.find(".handle").addClass("collapsed");
                    if (el.hasClass("unit")) {
                        el.find(".collapsable").not(".activity .collapsable").hide();
                    } else {
                        el.find(".collapsable").hide();
                    }
                }
            });
            /*$(".activities").collapsableTree({
            selExtraCollapse:".collapsable",
            preserve:true,
            selUl:"ul.subactivities, ul.rules"
            });*/

            $(".subactivities").each(function () {
                $(this).find(".subactivity").last().addClass("last");
            });

            $("body").anchor({
            sel:"div.activitycontent",
            highlightedColor: "#fff8dc"
            });
        });
    </script>
<!-- END HEADER FULLPLAY-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVeduPathView" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWplay" runat="server">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPlistEduPath" runat="server" Text="**list edu path" CssClass="Link_Menu"></asp:HyperLink>
                <asp:HyperLink ID="HYPstat" runat="server" CssClass="Link_Menu" />
            </div>
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <div class="EPprogressBar Temp" runat="server" id="divProgressBar">
                <CTRL:ProgressBar ID="CTRLprogressBar" runat="server"></CTRL:ProgressBar>
            </div>
            <div class="lastActivity" id="DIVLastActivity" runat="server" visible="false">
               <asp:Label ID="LBplayerTitle" runat="server" CssClass="" Text="***attività in corso"></asp:Label> 
               <span class="statuslight" runat="server" id="ImgStatus">&nbsp;</span><asp:Literal runat="server" ID="LTRLastLink"></asp:Literal>
            </div>
            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
            <div class="tree">                
                 
                <asp:Repeater runat="server" ID="RPTunits">
                    <HeaderTemplate>
                        <ul class="units playmode">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="unit clearfix<%# Me.AutoOpen(Container.Dataitem) %><%# Me.LockedUnit(Container.Dataitem) %><%# Me.UnitNote(Container.Dataitem) %><%# Me.UnitEmpty(Container.Dataitem) %>" id="unit-<%# Container.Dataitem.Id %>">                            
                            <div class="externalleft">
                                <span class="<%# Me.MoveUnit(Container.Dataitem) %>">&nbsp;</span><% 'TODO 1R: Unable/disable with permissions%>
                            </div>
                            <div class="unitcontent" runat="server" id="DIVGenericUnit">
                                <a name="unit-<%# Container.Dataitem.Id %>"></a>
                                <span class="switchunit handle">+</span>
                                <div class="innerwrapper">
                                    <div class="internal clearfix">
                                        <span class="left">                                            
                                            <span class="statuslight" runat="server" id="ImgStatus">&nbsp;</span>
                                            <span class="lockUnlock" runat="server" id="ImgLocked" visible="false">&nbsp;</span>
                                            <asp:Label CssClass="title  switchunity handle" runat="server" ID="LBLUnitTitle"></asp:Label>
                                            <asp:Label CssClass="duration" runat="server" ID="LBLUnitDuration"></asp:Label>
                                        </span>
                                        <span class="right">
                                            <asp:Label CssClass="expiration" runat="server" ID="LBLUnitExpiration"></asp:Label>
                                            <span class="icons">                                                
                                                <span class="icon mandatory" runat="server" id="ImgMandatory" visible="false">&nbsp;</span>
                                            </span>
                                        </span>
                                    </div>
                                    <div class="unitdetails">
                                        <div class="description<%# Me.UnitDescriptionEmpty(Container.Dataitem) %>" >
                                            <div class="renderedtext">
                                                <asp:Label ID="LBLUnitDescription" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <asp:Repeater ID="RPTUnitRules" runat="server">
                                        <HeaderTemplate><div class="unitrules collapsable"></HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="divRule">                                                        
                                                <asp:Label CssClass="ruleName" ID="LBLUnitRuleName" runat="server"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate></div></FooterTemplate>
                                        </asp:Repeater>                                                                                        
                                    </div>
                                </div>
                                <div class="clearer"></div>                            
                                <asp:Repeater runat="server" ID="RPTactivities">
                                    <HeaderTemplate>
                                        <ul class="activities">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li title="<%#GetItemStatusTitle(Container.DataItem)%>" class="activity clearfix<%#GetItemStatus(Container.DataItem)%><%# Me.LockedActivity(Container.Dataitem) %><%# Me.SingleMulti(Container.Dataitem) %><%# Me.ActivityNote(Container.Dataitem) %><%# Me.ActivityEmpty(Container.Dataitem) %><%# Me.IsDataConstrained(Container.Dataitem) %><%# Me.HasDataConstrain(Container.Dataitem) %>" id="act-<%# Container.Dataitem.Id %>">
                                            <asp:MultiView ID="MUVSingleMultiActivity" runat="server" ActiveViewIndex="0">
                                                <asp:View ID="VIWMultiActivity" runat="server">                                                                                            
                                                    <!-- multi -->
                                                    <div class="externalleft">
                                                        <span class="<%# Me.MoveActivity(Container.Dataitem) %>">&nbsp;</span>
                                                    </div>
                                                    <div class="activitycontent" id="DIVGenericActivity" runat="server">
                                                        <a name="act-<%# Container.Dataitem.Id %>"></a>
                                                        <span class="switchactivity handle">+</span>
                                                        <div class="internal clearfix">
                                                            <span class="left">
                                                                <span class="statuslight" runat="server" id="ImgStatus">&nbsp;</span>
                                                                <span class="lockUnlock" runat="server" id="ImgLocked" visible="false">&nbsp;</span>
                                                                <asp:Label CssClass="title switchactivity handle" ID="LBLActivityTitle" runat="server"></asp:Label>                                                                
                                                                <asp:Label CssClass="duration" ID="LBLActivityDuration" runat="server"></asp:Label>
                                                            </span>
                                                            <span class="right">
                                                                <asp:Label CssClass="expiration" ID="LBLActivityExpiration" runat="server"></asp:Label>
                                                                <span class="icons">                                                                    
                                                                    <span class="icon mandatory" runat="server" id="ImgMandatory" visible="false">&nbsp;</span>
                                                                </span>
                                                            </span>
                                                        </div>
                                                        <div class="activitydetails">
                                                            <div class="description<%# Me.ActivityDescriptionEmpty(Container.Dataitem) %>">
                                                                <div class="renderedtext">
                                                                    <asp:Label ID="LBLActivityDescription" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="activityrules collapsable"></div>
                                                        </div>                                                
                                                        <asp:Repeater ID="RPTsubactivities" runat="server">
                                                            <HeaderTemplate>
                                                                <ul class="subactivities">
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <li class="subactivity<%#GetItemStatus(Container.DataItem)%>" title="<%#GetItemStatusTitle(Container.DataItem)%>">
                                                                    <div class="subactivitycontent">
                                                                        <a name="subact-<%# Container.Dataitem.Id %>"></a>
                                                                        <span class="statuslight" runat="server" id="ImgStatus">&nbsp;</span>
                                                                        <CTRL:QuestionnaireItem ID="CTRLquestionnaire" runat="server" EnableAnchor="false"  Visible="false"/>
                                                                        <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false"   Visible="false"/>
                                                                        <CTRL:TextAction runat="server" id="CTRLtextAction" OnRefreshContainerEvent="RefreshContainerEvent"  Visible="false"/>
                                                                        <CTRL:CertificationAction runat="server" id="CTRLcertificationAction" OnRefreshContainerEvent="RefreshContainerEvent" onGetHiddenIdentifierValueEvent="GetHiddenIdentifierValueEvent" onItemActionResult="ItemActionResult" visible="false" />
                                                                    </div>
                                                                </li>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </ul>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                    <div class="activitycontent" id="DIVActivityNote" runat="server" visible="false">
                                                        <a name="act-<%# Container.Dataitem.Id %>"></a>
                                                        <div class="internal clearfix">
                                                            <span class="left">
                                                                <div class="notecontent renderedtext">
                                                                    <asp:Label runat="server" ID="LBLActivityNote"></asp:Label>
                                                                </div>
                                                            </span>
                                                            <span class="right"><span class="icons"></span></span>
                                                        </div>
                                                    </div>
                                                    <div class="activity clearfix" id="DIVActivityNoteFix" runat="server" visible="false"></div>
                                                </asp:View>
                                                <asp:View ID="VIWSingleActivity" runat="server">
                                                <!-- single -->
                                                    <div class="externalleft">
                                                        <span class="<%# Me.MoveActivity(Container.Dataitem) %>">&nbsp;</span>
                                                    </div>
                                                    <div class="activitycontent">
                                                        <a name="act-<%# Container.Dataitem.Id %>"></a>
                                                        <span class="switchactivity spacer">&nbsp;</span>
                                                        <div class="internal clearfix">
                                                            <span class="left">
                                                                <span class="statuslight" runat="server" id="IMGStatusActivity">&nbsp;</span>
                                                                <span class="lockUnlock" runat="server" id="IMGLockedActivity" visible="false">&nbsp;</span>
                                                                <CTRL:QuestionnaireItem ID="CTRLquestionnaire" runat="server" Visible="false" />
                                                                <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false"  Visible="false"/>
                                                                <CTRL:TextAction runat="server" id="CTRLtextAction" OnRefreshContainerEvent="RefreshContainerEvent"  Visible="false"/>
                                                                <CTRL:CertificationAction runat="server" id="CTRLcertificationAction"  OnItemActionResult="ItemActionResult" OnRefreshContainerEvent="RefreshContainerEvent" onGetHiddenIdentifierValueEvent="GetHiddenIdentifierValueEvent"  />
                                                            </span>
                                                            <span class="right">
                                                                <asp:Label CssClass="expiration" ID="LBLSingleActivityExpiration" runat="server"></asp:Label>
                                                                <span class="icons">                                                            
                                                                    <span class="icon mandatory" runat="server" id="IMGMandatorySingle" visible="false">&nbsp;</span>
                                                                </span>
                                                            </span>
                                                        </div>
                                                        <div class="activitydetails">
                                                            <div class="description<%# Me.ActivityDescriptionEmpty(Container.Dataitem) %>">
                                                                <div class="renderedtext">
                                                                    <asp:Label ID="LBLActivitySingleDescription" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="activityrules collapsable"></div>
                                                        </div>
                                                    </div>
                                            </asp:View>
                                            </asp:MultiView>
                                        </li><%--End li.activity--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul><%--End ul.activities--%>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="unitcontent" runat="server" id="DIVUnitNote" visible="false">
                                <a name="unit-<%# Container.Dataitem.Id %>"></a>
                                <div class="internal clearfix">
                                    <span class="left">
                                        <div class="notecontent clearfix renderedtext"><asp:Label runat="server" ID="LBLUnitNote"></asp:Label></div>
                                    </span>                                    
                                    <span class="right"><span class="icons"></span> </span>
                                </div>
                            </div>
                            <div class="clearfix" runat="server" id="DIVUnitNoteFix" visible="false"></div>                            
                        </li>
                    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
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
         <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLgenericMessages" />
        </asp:View>
    </asp:MultiView>
    <CTRL:helpStatus ID="CTRLhelpStatus" runat="server" /><asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
    <!--"LTmodalCssClass">openmodal scorm -->
</asp:Content>