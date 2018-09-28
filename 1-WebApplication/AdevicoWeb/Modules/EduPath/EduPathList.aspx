<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EduPathList.aspx.vb" Inherits="Comunita_OnLine.EPList" %>
<%@ Register Src="~/Modules/Common/UC/UC_ActionMessages.ascx" TagPrefix="CTRL" TagName="Messages" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- HEADER EDUPATH START-->
    <link href="../../Graphics/Modules/Edupath/css/<%=GetCssFileByType()%>edupath.css?v=201605041410lm" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            function OnUpdate(e, ui) {
                var Data = $(this).sortable("serialize");

                $.ajax({
                    type: "POST",
                    url: "Reordering.asmx/PathsReorder",
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

                //CheckOrderButtons($(this));
            }

            $("ul.paths.manage").sortable({
                handle: "span.movepath",
                tolerance: 'pointer',
                placeholder: 'ui-state-highlightHelper',
                forcePlaceholderSize: true,
                forceHelperSize: true,
                axis: "y",                
                cancel: "li.path.default",
                items: "li.path:not(li.path.default)",
                start: function (event, ui) {

                    //un-comment this
                    /*height = $(ui.item).outerHeight();
                    width = $(ui.item).outerWidth();
                    $(".units").css("padding-bottom", height);
                    $(".ui-state-highlightHelper").css("height", height);
                    $(".ui-state-highlightHelper").css("width", width);*/
                    $(ui.item).addClass("dragging");
                    $(ui.item).parents(".paths").addClass("dragging");
                    $(this).sortable("refresh");
                },
                stop: function (event, ui) {
                    //$(".units").css("padding-bottom", "0px");
                    $(ui.item).removeClass("dragging");
                    $(ui.item).parents(".paths").removeClass("dragging");
                },
                update: OnUpdate

            });

            

            /*$(".paths").dragsort({
            dragSelector: "span.movepath"
            });*/
            $('#selectPerson').dialog({
                appendTo:"form",
                closeOnEscape: false,
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 800,
                height: 600,
                minHeight: 400,
                //                minWidth: 700,
                zIndex: 1000,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }

            });

            $('.dialog.dlgeditoptions').dialog({
                appendTo:"form",
                closeOnEscape: false,
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 800,
                height: 400,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 1000,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }

            });

            $(".opendlgeditoptions").click(function () {
                var id = $(this).parents("li.path").first().attr("id");
                $(".hiddenpathid").val(id);
                $(".dialog.dlgeditoptions").dialog("open");
                return false;
            });

            $('.dialog.dlgresetstatistics').dialog({
                appendTo:"form",
                closeOnEscape: false,
                autoOpen: false,
                draggable: true,
                modal: true,
                title: "",
                width: 800,
                height: 400,
                minHeight: 200,
                //                minWidth: 700,
                zIndex: 1000,
                open: function (type, data) {
                    //                $(this).dialog('option', 'width', 700);
                    //                $(this).dialog('option', 'height', 600);
                    //$(this).parent().appendTo("form");
                    $(".ui-dialog-titlebar-close", this.parentNode).hide();
                }

            });
            $(".opendlgresetstatistics").click(function () {
                var id = $(this).parents("li.path").first().attr("id");
                $(".hiddenpathid").val(id);
                $(".dialog.dlgresetstatistics").dialog("open");
                return false;
            });

            $(".closedlgresetstatistics").click(function () {
                $(".hiddenpathid").val("");
                $(".dialog.dlgresetstatistics").dialog("close");
                return false;
            });
        });

        function showDialog(id) {
            $('#' + id).dialog("open");
            return false;
        }

        function closeDialog(id) {
            $('#' + id).dialog("close");
        }
    </script>         
    <asp:Literal ID="LTscriptOpen" runat="server" Visible="false">
        <script type="text/javascript">
            $(function () {
                showDialog("selectPerson");
            });
        </script>
    </asp:Literal>
    <!-- HEADER EDUPATH END-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:Literal runat="server" ID="LTalert"></asp:Literal>
    <asp:MultiView ID="MLVeduPathList" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWeduPathList" runat="server">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPnewEp" runat="server" Text="**add ep" CssClass="Link_Menu"
                    Visible="false"></asp:HyperLink>
                <asp:LinkButton ID="LNBSwitchView" runat="server"  CssClass="Link_Menu">Switch View</asp:LinkButton>
                <asp:HyperLink ID="HYPsummary" runat="server" Text="**summary" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <CTRL:Messages runat="server" ID="CTRLmoduleStatusMessage" Visible="false" />
            <CTRL:Messages runat="server" ID="CTRLgenericMessage" Visible="false" />
            <CTRL:Messages runat="server" ID="CTRLactionMessage" Visible="false" />
            <div>
                <asp:RadioButtonList ID="RBLvisibility" runat="server" CssClass="Titolo_dettagliSmall"
                    OnSelectedIndexChanged="RBLvisibility_SelectedIndexChanged" AutoPostBack="true"
                    Visible="false">
                </asp:RadioButtonList>
            </div>
            <div class="dialog dlgeditoptions" title="<%=GetEditingOptionsTitleTranslation() %>">                
                <div class="fieldobject">
                    <div class="fieldrow title">
                        <div class="description">
                            <asp:Label ID="LBeditingOptions" runat="server">*Si sta cercando di accedere ad un mooc con statistiche già registrate, selezionare una delle opzioni sottostanti prima di procedere</asp:Label>
                        </div>                        
                    </div>
                    <div class="fieldrow commandoptions clearfix">
                        <div class="commandoption left">
                            <asp:Button Text="* Visualizza in sola lettura" runat="server" CssClass="commandbutton editoption1" ID="BTNreadonlyOption" />
                            <asp:Label ID="LBreadonlyOption" runat="server" CssClass="commanddescription" >* Visualizza in sola lettura: visualizza il moocs in sola lettura. Non saranno consentite modifiche alla struttura del mooc.</asp:Label>
                        </div>
                        <div class="commandoption right">
                            <asp:Button Text="* Consenti modifiche" runat="server" CssClass="commandbutton editoption2" ID="BTNeditOption" />
                            <asp:Label ID="LBeditOption" runat="server" CssClass="commanddescription" >* Modifica: verranno rimosse tutte le statistiche utente raccolte ad oggi, il mooc sarà bloccato e le modifiche saranno abilitate. Per consentire agli utenti di riprendere il mooc dovrai successivamente togliere il blocco impostato automaticamente.</asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="dialog dlgresetstatistics" title="<%=GetClearStatisticsOptionsTitleTranslation() %>" >                
                <div class="fieldobject">
                    <div class="fieldrow title">
                        <div class="description">
                            <asp:Label ID="LBclearStatisticsOptions" runat="server">*Si sta cercando di cancellare le eventuali statistiche utente del mooc, selezionare una delle opzioni sottostanti prima di procedere</asp:Label>
                        </div>                        
                    </div>
                    <div class="fieldrow commandoptions clearfix">
                        <div class="commandoption left">
                            <asp:Button Text="* Annulla" runat="server" CssClass="commandbutton editoption1 closedlgresetstatistics" ID="BTNundoResetOption" />
                            <asp:Label ID="LBundoResetOption" runat="server" CssClass="commanddescription" >* Annulla: annulla l'operazione in corso NON cancellare le statistiche utente del mooc.</asp:Label>
                        </div>
                        <div class="commandoption right">
                            <asp:Button Text="* Consenti modifiche" runat="server" CssClass="commandbutton editoption2" ID="BTNresetOption" />
                            <asp:Label ID="LBdoResetOption" runat="server" CssClass="commanddescription" >* Procedi: verranno rimosse tutte le statistiche utente raccolte fino ad ora.</asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list">
                <input type="hidden" runat="server" id="HDNidPath" class="hiddenpathid" value="" />
                <ul class="paths" runat="server" id="ULPathList">
                    <asp:Repeater ID="RPeduPathList" runat="server">
                    <ItemTemplate>
                    
                    <li class="path clearfix<%# Me.IsDefaultPath(Container.DataItem)%><%# Me.CssIsBlocked(Container.DataItem)%>" id="PATH-<%# DataBinder.Eval(Container.DataItem,"Id")%>">
                        <a name="<%# DataBinder.Eval(Container.DataItem,"Id")%>"></a>
                        <div class="externalleft icons">
                            <span class="icon movepath" runat="server" id="SPANupdown" visible="false">X</span>
                            <asp:LinkButton ID="LKBup" runat="server" Visible="false" CommandName="moveUp" CssClass="bntOrderUp img_link ico_moveup_s"></asp:LinkButton>
                            <asp:LinkButton ID="LKBdown" runat="server" Visible="false" CommandName="moveDown" CssClass="bntOrderDown img_link ico_movedown_s"></asp:LinkButton>
                        </div>
                        <div class="pathcontainer">
                            <div class="pathheader clearfix">
                                <span class="left">
                                    <span class="titlecont">
                                        <span class="icons">
                                            <asp:LinkButton runat="server" ID="LKBvisibility" Text="Visible**" Visible="false" CommandName="visibility" CssClass="icon locked"></asp:LinkButton>
                                            <span runat="server" id="IMGblocked" CssClass="icon locked" visible="false">&nbsp;</span>
                                        </span>
                                        <span class="title">
                                            <asp:HyperLink ID="HYPeduPath" runat="server" Text="pathname***" Visible="false" CssClass="Link_evidenziato"></asp:HyperLink>
                                            <asp:Label ID="LBeduPath" runat="server" Text="pathname**" Visible="false" CssClass="Details_titolo"></asp:Label>
                                        </span>
                                        <span class="icons">
                                            <span id="IMGdefault" class="icon default" runat="server" visible="false">&nbsp;</span>
                                            <asp:LinkButton ID="LKBdefault" runat="server" Text="Default**" Visible="false" CommandName="default" CssClass="icon makedefault"></asp:LinkButton>
                                        </span>
                                    </span>
                                </span>
                                <span class="right">
                                    <span class="icons">
                                        <asp:LinkButton ID="LKBdelete" runat="server" CssClass="icon delete" Visible="false" Text="del**" CommandName="virtualdelete"></asp:LinkButton>
                                        <asp:HyperLink ID="HYPupdate" runat="server" CssClass="icon edit" Text="**up" Visible="false"></asp:HyperLink>
                                        <asp:HyperLink ID="HYPstatistic" runat="server" Text="**Stat" CssClass="icon stats" Visible="false"></asp:HyperLink>
                                        <asp:HyperLink ID="HYPevaluate" runat="server" Text="**evaluate" CssClass="icon evaluate" Visible="false"></asp:HyperLink>
                                    </span>
                                </span>
                                <div class="clearfix">&nbsp;</div>
                            </div>
                            <div class="pathdesc clearfix <%# Me.DescriptionIsEmpty(Container.DataItem.Description)%>">
                                <div class="description left">
                                    <asp:Label ID="LBdetail" runat="server" CssClass="detailsTitle" Text="detail***" Visible="false"></asp:Label>
                                    <asp:Label ID="LBdescription" runat="server" CssClass="descrizioneFile renderedtext" Text="description***">&nbsp;</asp:Label>
                                </div>
                                <div class="right">
                                    <asp:HyperLink ID="HYPstart" CssClass="icon floatright img_link ico_playmmd_m" runat="server" Text="**start" Visible="false"></asp:HyperLink>
                                </div>
                            </div>
                            <div class="pathfooter clearfix">
                                <div class="left">
                                    <div class="manageunits" runat="server" id="DIVmanageUnits">
                                        <asp:Label ID="LBunit" runat="server" Text="**unit" CssClass="label"></asp:Label>
                                        <asp:HyperLink ID="HYPunitToMan" runat="server" Text="**HYPunitToMan" Visible="false" CssClass="management"></asp:HyperLink>
                                        <asp:Label ID="LBunitToMan" runat="server" CssClass="management" Text="LBunitToMan***" Visible="false"></asp:Label>
                                        <span class="link_separator" runat="server" id="SPANUnitSep">&nbsp;</span>
                                        <asp:HyperLink ID="HYPunitToEval" runat="server" Text="**HYPunitToEval" Visible="false" CssClass="evaluation"></asp:HyperLink>
                                        <asp:Label ID="LBunitToEval" runat="server" CssClass="evaluation" Text="LBunitToEval***" Visible="false"></asp:Label>
                                    </div>
                                    <div class="manageactivities" runat="server" id="DIVmanageActivities">
                                        <asp:Label ID="LBactivity" runat="server" Text="**activity" CssClass="label"></asp:Label>
                                        <asp:HyperLink ID="HYPactivityToMan" runat="server" Text="**HYPactivityToMan" Visible="false" CssClass="management"></asp:HyperLink>
                                        <asp:Label ID="LBactivityToMan" runat="server" CssClass="management" Text="LBactivityToMan***" Visible="false"></asp:Label>
                                        <span class="link_separator" runat="server" id="SPANActivitySep">&nbsp;</span>
                                        <asp:HyperLink ID="HYPactivityTOEval" runat="server" Text="**HYPactivityTOEval" Visible="false" CssClass="evaluation"></asp:HyperLink>
                                        <asp:Label ID="LBactivityTOEva" runat="server" CssClass="evaluation" Text="LBactivityTOEva***" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <div class="right">
                                    <asp:Button ID="BTNdeleteStat" CssClass="resetstats" runat="server" Text="Cancella tutte le statistiche(temporaneo-solo manager)" CommandName="delStat"  />
                                </div>
                            </div>
                            <div class="clearer"></div>
                        </div>
                    </li>
                    </ItemTemplate>
                    </asp:Repeater>
                </ul>        
            </div>            
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <CTRL:Messages runat="server" ID="CTRLerrorMessage" />               
            </div>
        </asp:View>
        <asp:View ID="VIWmessages" runat="server">
            <CTRL:Messages runat="server" ID="CTRLmessages" />
        </asp:View>
    </asp:MultiView><asp:Literal ID="LTitemLinkManagementCssClass" runat="server" Visible="false">management</asp:Literal><asp:Literal ID="LTitemLinkStandardCssClass" runat="server" Visible="false">Link_evidenziato</asp:Literal><asp:Literal ID="LTitemLinkMoocCssClass" runat="server" Visible="false">opendlgeditoptions</asp:Literal><asp:Literal ID="LTresetstatsCssClass" runat="server" Visible="false">resetstats</asp:Literal><asp:Literal ID="LTresetstatsMoocCssClass" runat="server" Visible="false">opendlgresetstatistics</asp:Literal>
    <div id="selectPerson" style="display: none;">
        <div style="clear: both;">
        <asp:UpdatePanel ID="UDPperson" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Literal ID="LTidPath" runat="server" Visible="false"></asp:Literal>
                <div style="clear: both;">
                    <asp:label ID="LBclearStatistics" runat="server">La cancellazione delle statistiche è al momento disponibile solo per utenti che all'interno 
                    della comunita' svolgono il ruolo di amministratori: sono quindi ESCLUSI tutti gli utenti che nella comunità NON svolgano tale ruolo.<br />
                    <br />Sostanzialmente NON è possibile cancellare le statistiche ad un utente che svolge il ruolo di partecipante
                    per ovvi motivi di correttezza verso gli altri partecipanti"</asp:label>

                    <br />
                    <asp:CheckBoxList id="CBLusers" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="inputgroup"></asp:CheckBoxList>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
            <div style="clear: both; text-align: right;">
                <asp:Button ID="BTNundoClearStatistics" runat="server" Text="Annulla" OnClientClick="closeDialog('selectPerson')" />
                <asp:Button ID="BTNclearStatistics" runat="server" Text="Cancella" ToolTip="Cancella per i selezionati" OnClientClick="closeDialog('selectPerson')" />
            </div>
        </div>
    </div>
</asp:Content>
