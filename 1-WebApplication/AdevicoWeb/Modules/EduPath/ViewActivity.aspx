<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="ViewActivity.aspx.vb" Inherits="Comunita_OnLine.ViewActivity" ValidateRequest="false" %>

<%@ Register TagPrefix="CTRL" TagName="AddAction" Src="~/Modules/EduPath/UC/UC_AddAction.ascx" %>
<%@ Register Src="UC/UC_CertificationAction.ascx" TagName="CertificationAction" TagPrefix="CTRL" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
    <!--<link href="../../Graphics/Modules/Edupath/css/PfStyle.css" rel="Stylesheet" />-->
    <style type="text/css"> 

  
        .LinkMenu_JQuery
        {
            font-size:1em;
        }
            .Ep_ViewActivity_Detail
        {
            width: 100%;
            height: auto;
            padding: 5px;
            display: block;
        }
        .Ep_ViewActivity_TitleLeft
        {
            width: 20%;
            height: auto;
            float: left;
        }
        .Ep_ViewActivity_TitleRight
        {
            width: 79%;
            height: auto;
            float: left;
        }
  
        ul.EpActivities
        {
            text-indent: 0;
            padding: 0;
            margin: 0;
            width: 100%;
        }
        
        ul.EpActivities li
        {
            list-style: none;
            text-indent: 0;
            padding: 0;
            margin: 0;
            margin-bottom: 0.5em;
            width: 100%;
        }
        
        ul.Columns
        {
            display: block;
        }
        
        ul.Columns li
        {
            width: 100%;
            display: inline-block;
            *display:inline;
            vertical-align: top;
        }
        
        ul.Columns li.left
        {
            width: 7%;
            text-align: center;
            padding-top: 3.5em;
        }
        
        ul.Columns li.center
        {
            width: 77%;
        }
        
        ul.Columns li.right
        {
            width: 10%;
            text-align: center;
            padding-top: 3.5em;
        }
        
        span.ui-icon-arrowthick-2-n-s
        {
            float: right;
            cursor: move;
        }
        
        .pathTitle
        {
            width: 100%;
            display: block;
            text-align: center;
            padding: 0.5em;
            margin-bottom: 0.5em;
            font-weight: bold;
        }
        
        .description
        {
            width: 100%;
            display: block;
            
            padding: 0.5em;
            padding-bottom: 2em;
            border-top: 1px solid black;
            border-left: 1px solid black;
            border-right: 1px solid black;
            font-style: italic;
            color: Gray;           
        }
        
        .detailsTitle
        {
            width: 100%;
            display: block;
            text-align: center;
            padding-left: 0.5em;
            padding-right: 0.5em;
            border-top: 1px solid black;
            border-left: 1px solid black;
            border-right: 1px solid black;
            background-color: #ccc;
        }
        
        .subctivityStatus
        {
            display: block;
            padding: 0.5em;
            width: 100%;
            border-bottom: 1px solid black;
            border-left: 1px solid black;
            border-right: 1px solid black;
            background-color:#ccc;
        }
        
        .icon
        {
            display: inline-block;
            *display:inline;
            width: 24px;
            height: 24px;
            text-indent: -9999999px;
            *text-indent:0; line-height:0; font-size:0; overflow:hidden;
            background-position: center center;
            background-repeat: no-repeat;
            margin: 1px;
            padding: 0;
        }
        
        .newIcon
        {
            display: inline-block;
            *display:inline;
            width: 24px;
            height: 24px;
            margin: 1px;
            padding: 0;
        }
        
        .inline
        {
            height: 24px;
            width: 24px; 
            display: inline-block;
            *display:inline;
            vertical-align: text-bottom;
            background-position: center bottom;
        }
        
        .floatright
        {
            margin-top:1em;
            float:right;
        }
        
        .textRight
        {
            text-align:right;
        }
                
        .switch
        {
            height: 30px;
            width: 50px;
            display: inline-block;
            *display:inline;
            vertical-align: text-bottom;
            background-position: center bottom;
        }
        
      
          .ContainerLeft
        {
            float: left;
            width: 30%;


        }
        .ContainerRight
        {
            float: left;
            border: 1px solid;
            height: 80px;
            padding: 1 5 1 5;
            width: 65%;
        }
        .Row

        {
            text-align: left;
            padding-top: 10px;
            padding-left: 10px;
            clear: both;
        }
        
        div.DivEpButton
        {
            width: 900px;            
        }
        
        div.DivEpButton div
        {
            
            display:inline-block;
            *display:inline;
            
        }
        
        div.divBlock
        {
            display:inline-block;
            *display:inline;
        }
        
       .confirmMSG
        {
            font-weight: bold;
            color: green;
        }
        .errorMSG
        {
            font-weight: bold;
            color: red;
        }
        
        .DivTime
        {
            
            
        }

        div.renderedtext li{
			display:list-item;
		}
        
    </style>
    <script type="text/javascript">
        var enableSort = <%=me.sortVisibility() %>;
        
    </script>
    <script type="text/javascript">
          var DisplayTitle = ""
          var DisplayMessage = "<h1><%=GetBlockUiMessage%></h1>"
    </script>
    <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
    <script type="text/javascript">


        function showDialog(id) {
            $('#' + id).dialog("open");
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }

        function OnUpdate(e, ui) {
            var Data = $(this).sortable("serialize");

            $.ajax({
                type: "POST",
                url: "Reordering.asmx/SubActivitiesReorder",
                data: "{'position':'" + Data + "'}",
                processData: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (msg) {
                    //alert(msg.d);
                },
                error: function (result) {
                    alert("Error: (" + result.status + ') [' + result.statusText + ']');

                }
            });

            CheckOrderButtons($(this));
        }

        function CheckOrderButtons(el) {

            el.each(function () {
                $(this).children("li").find(".bntOrderUp").show();
                $(this).children("li").find(".bntOrderDown").show();
                $(this).children("li").first().find(".bntOrderUp").first().hide();
                $(this).children("li").last().find(".bntOrderDown").first().hide();
            });

        }



        $(document).ready(function () {
            CheckOrderButtons($("ul.EpActivities"));

//            $(".view-modal").dialog({
//                closeOnEscape: false,
//               // modal: true,
//                width: 900,
//                height: 650,
//                minHeight: 400,
//                minWidth: 840,
//                open: function (type, data) {
////                    $(this).parent().appendTo("form");
//                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
//                }
//            });

            $('#addSubActivity').dialog({
                appendTo: "form",
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 900,
                height: 650,
                minHeight: 400,
                minWidth: 600,
                zIndex: 99999,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }
            });

            if (enableSort == true) {

                $("ul.EpActivities").sortable({
                    tolerance: 'pointer',
                    axis: 'y',
                    opacity: 0.5,
                    scroll: true,
                    forceHelperSize: true,
                    placeholder: 'ui-state-highlight',
                    forcePlaceholderSize: true,
                    helper: "clone",
                    handle: "span.ui-icon",
                    update: OnUpdate
                });

                $("span.ui-icon").disableSelection();
            }
        });
    </script>
     <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
        <script language="javascript" type="text/javascript">
            $(function () {
                showDialog("addSubActivity");
            });
        </script>
    </asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVviewActivity" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWactivity" runat="server">
            <div class="divBlock">
                <div class="DivEpButton">
                    <asp:LinkButton runat="server" ID="LKBupdateWeight" Visible="false" CssClass="Link_Menu"></asp:LinkButton>
<%--                    <asp:UpdatePanel ID="UDPbuttons" runat="server">
                        <ContentTemplate>--%>
                            <asp:LinkButton ID="LNBnewSubAct" runat="server" Text="**new act" CssClass="Link_Menu"
                                Visible="false"></asp:LinkButton>
<%--                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <asp:LinkButton ID="LKBdeleteAct" runat="server" Visible="false" Text="del**" CssClass="Link_Menu"
                        CommandName="activityDelete"></asp:LinkButton>
                    <asp:HyperLink ID="HYPupdateAct" runat="server" Text="**Stat" CssClass="Link_Menu"
                        Visible="false"></asp:HyperLink>
                    <%--       <asp:LinkButton ID="LKBvisibActivity" runat="server" Text="Visible**" CommandName="activityVisibility"
                        Visible="false"></asp:LinkButton>--%>
                    <asp:LinkButton ID="LKBeduPathView" runat="server" Text="**edu view" CssClass="Link_Menu"></asp:LinkButton>
                    <asp:HyperLink ID="HYPpreviousAct" runat="server" Text="**previusAct" CssClass="Link_Menu"
                        Visible="false"></asp:HyperLink>
                    <asp:HyperLink ID="HYPnextAct" runat="server" Text="**nextAct" CssClass="Link_Menu"
                        Visible="false"></asp:HyperLink>
                    <br />
                    <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
                    <div align="center">
                        <asp:Label ID="LBErrorMSG" runat="server" CssClass="errorMSG"></asp:Label>
                    </div>
                    <div align="center">
                        <asp:Label ID="LBconfirmMSG" runat="server" CssClass="confirmMSG"></asp:Label>
                    </div>
                </div>
                <div id="DIVsummaryBlock" runat="server" class="summaryBlock">
                    <div id="DIVdescription" runat="server" class="Ep_ViewActivity_Detail">
                        <div class="Ep_ViewActivity_TitleLeft">
                            <asp:Label ID="LBdescriptionTitle" runat="server" CssClass="Titolo_campo" Text="**descrizione"></asp:Label>
                        </div>
                        <div class="Ep_ViewActivity_TitleRight">
                            <div class="renderedtext">
                                <asp:Label ID="LBdescription" runat="server">&nbsp</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div id="DIVdate" runat="server" class="Ep_ViewActivity_Detail">
                        <div class="Ep_ViewActivity_TitleLeft">
                            <asp:Label ID="LBdateTitle" runat="server" CssClass="Titolo_campo" Text="**titdata"></asp:Label>
                        </div>
                        <div class="Ep_ViewActivity_TitleRight">
                            <asp:Label ID="LBdate" runat="server" CssClass="dettagli_Campo" Text="**data"></asp:Label>
                        </div>
                    </div>
                    <div id="DIVmandatory" runat="server" class="Ep_ViewActivity_Detail">
                        <div class="Ep_ViewActivity_TitleRight">
                            <asp:Image ID="IMGactMandatory" CssClass="  " runat="server" Visible="false" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:Label ID="LBtext" runat="server"></asp:Label>
            <div id="DIVsubAct" runat="server">
                <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                <asp:Label ID="LBnoSubAct" runat="server" Text="*no sub act" Visible="false" CssClass="Details_titolo" />
                <ul class="EpActivities">
                    <asp:Repeater ID="RPsubActivity" runat="server">
                        <ItemTemplate>
                            <li id="SUB-<%# DataBinder.Eval(Container.DataItem,"Id")%>">
                                <ul class="Columns">
                                    <li class="left">
                                        <asp:Image ID="IMGmandatory" runat="server" Visible="false" CssClass="" />
                                        <asp:LinkButton ID="LKBsetMandatory" runat="server" Text="Mandatory**" CommandName="mandatory"
                                            Visible="false"></asp:LinkButton>
                                        <br />
                                        <asp:LinkButton ID="LKBdeleteSubAct" runat="server" Text="del**" CommandName="delete"
                                            CssClass=" delete img_link ico_delete_m" Visible='<%# me.isManageable%>'></asp:LinkButton>
                                        <asp:LinkButton ID="LKBeditSubAct" runat="server" Text="**up sub" CssClass="img_link ico_edit_m" Visible="false"
                                            CommandName="SetPermission"></asp:LinkButton>
                                        <asp:HyperLink ID="HYPeditSubAct" runat="server" Text="**up sub" CssClass="img_link ico_edit_m" Visible="false" />
                                    </li>
                                    <li class="center">
                                        <asp:Literal runat="server" ID="LITmove"></asp:Literal>
                                        <span class="pathTitle">
                                            <asp:Image ID="IMGvisibility" runat="server" Visible="false" />
                                            <asp:LinkButton runat="server" ID="LKBvisibSubAct" Text="Visible**" Visible="false"
                                                CommandName="visibility"></asp:LinkButton>
                                            <asp:Image ID="IMGtype" CssClass="  inline" runat="server" />
                                            <asp:Label ID="LBtype" runat="server" CssClass="Titolo_campo" Text="**forum"></asp:Label>
                                            <div id="DIVweight" runat="server">
                                                <asp:TextBox ID="TXBweight" runat="server" Width="40px"></asp:TextBox>
                                                <asp:Label ID="LBpoints" runat="server" CssClass="Titolo_campo" Text="**points"></asp:Label>
                                            </div>
                                   <%-- DA Attivare se subAct time con tempo specifico 
                                          <div runat="server" id="DIVtime" class="DivTime">
                                                <div >
                                                    <asp:Label runat="server" ID="LBhours" Text="Ore riconosciute*" CssClass="Titolo_campoSmall"></asp:Label>
                                                </div>
                                                <div >
                                                    <telerik:RadAjaxPanel runat="server" ID="AjaxPanel1">
                                                        <telerik:RadNumericTextBox DecimalDigits="0" Type="number" ShowSpinButtons="true"
                                                            IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                            Label="" runat="server" ID="TXBhours" Width="100px" MaxValue="100" MinValue="0">
                                                        </telerik:RadNumericTextBox>:<telerik:RadNumericTextBox DecimalDigits="0" Type="number"
                                                            ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" IncrementSettings-InterceptMouseWheel="true"
                                                            Label="" runat="server" ID="TXBmins" Width="100px" MaxValue="59" MinValue="0">
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:RadAjaxPanel>
                                                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                                        <AjaxSettings>
                                                            <telerik:AjaxSetting AjaxControlID="DropDownList1">
                                                                <UpdatedControls>
                                                                    <telerik:AjaxUpdatedControl ControlID="TXBhours" />
                                                                    <telerik:AjaxUpdatedControl ControlID="TXBmins" />
                                                                </UpdatedControls>
                                                            </telerik:AjaxSetting>
                                                            <telerik:AjaxSetting AjaxControlID="DropDownList2">
                                                                <UpdatedControls>
                                                                    <telerik:AjaxUpdatedControl ControlID="TXBhours" />
                                                                    <telerik:AjaxUpdatedControl ControlID="TXBmins" />
                                                                </UpdatedControls>
                                                            </telerik:AjaxSetting>
                                                        </AjaxSettings>
                                                    </telerik:RadAjaxManager>
                                                </div>                                             
                                            </div>--%>
                                            <asp:LinkButton ID="LKBup" runat="server" Visible="false" CommandName="moveUp" CssClass="bntOrderUp"></asp:LinkButton>
                                            <asp:LinkButton ID="LKBdown" runat="server" Visible="false" CommandName="moveDown"
                                                CssClass="bntOrderDown"></asp:LinkButton>
                                        </span>
                                        <asp:Label ID="LBdetail" runat="server" CssClass="detailsTitle" Text="detail***"></asp:Label>
                                         <a name="subact-<%# Container.Dataitem.Id %>"></a>
                                        <span class="description">
                                            <asp:PlaceHolder ID="PLHaction" runat="server" Visible="false"></asp:PlaceHolder>
                                            <CTRL:CertificationAction runat="server" id="CTRLcertificationAction" OnRefreshContainerEvent="RefreshContainerEvent" onGetHiddenIdentifierValueEvent="GetHiddenIdentifierValueEvent"  OnItemActionResult="ItemActionResult" visible="false" />
                                            <div class="renderedtext" id="DVrenderDescription" runat="server">
                                                <asp:Label ID="LBdescription" runat="server" Text="**descrizione testo"></asp:Label>
                                            </div>
                                            <asp:HyperLink ID="HYPname" runat="server" Text="**name"></asp:HyperLink>
                                            <asp:LinkButton ID="LKBexecute" runat="server" Visible="false" CommandName="executed" CssClass="img_link ico_playmmd_m" />
                                        </span><span class="subctivityStatus textRight">
                                            <asp:Label ID="LBcompletionTitle" runat="server" Text="**completion" Visible="false"></asp:Label>&nbsp<asp:Label
                                                ID="LBcompletion" runat="server" CssClass="dettagli_CampoSmall" Text="**0" Visible="false"></asp:Label><br />
                                            <asp:Label ID="LBstatusTitle" runat="server" Text="**status" Visible="false"></asp:Label>&nbsp
                                            <asp:Image ID="IMGstatus" CssClass="  " runat="server" Visible="false" />
                                        </span></li>
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" Text="*error" CssClass="Link_Menu"></asp:HyperLink>
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLgenericMessages"/>
        </asp:View>
    </asp:MultiView>
    <div id="addSubActivity">
         <CTRL:AddAction id="CTRLaddAction" runat="server" IsInAjaxPanel="true" onUpdateAndCloseContainer="CTRLaddAction_UpdateAndCloseContainer" ></CTRL:AddAction>
    </div>
    <asp:HiddenField runat="server" ID="HDNdownloadTokenValue" />
</asp:Content>