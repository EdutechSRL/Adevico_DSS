<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Gantt.ascx.vb" Inherits="Comunita_OnLine.UC_Gantt" %>
<asp:MultiView ID="MLVgantt" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWgantt" runat="server">
        <div class="fieldobject top clearfix" id="DVcommandsTop" runat="server" visible="false">
            <div class="fieldrow left">&nbsp;</div>
            <div class="fieldrow right">
                <span class="commands top">
                    <a class="command gantttostart"><asp:literal ID="LTganttGoToStartDateTop" runat="server">*Go To Start</asp:literal></a>
                    <a class="command gantttoday"><asp:literal ID="LTganttGoToTodayTop" runat="server">*Go To Today</asp:literal></a>
                </span>
            </div>
        </div>
        <div class="ganttwrapper">
            <div class="gantt" id="GanttChartDIV">
            </div>
            <asp:Literal ID="LTganttScript" runat="server">
            </asp:Literal>
            <asp:Literal ID="LTganttLanguage_it" runat="server" Visible="false">
                <script type="text/javascript" language="javascript" src="{0}Jscript/Modules/ProjectManagement/jsgantt-i18n.it.js"></script>
            </asp:Literal>
            <asp:Literal ID="LTganttLanguage_de" runat="server" Visible="false">
                <script type="text/javascript" language="javascript" src="{0}Jscript/Modules/ProjectManagement/jsgantt-i18n.de.js"></script>
            </asp:Literal>
        </div>
        <div class="fieldobject bottom clearfix">
            <div class="fieldrow right">
                <span class="commands bottom">
                    <a class="command gantttostart"><asp:literal ID="LTganttGoToStartDateBottom" runat="server">*Go To Start</asp:literal></a>
                    <a class="command gantttoday"><asp:literal ID="LTganttGoToTodayBottom" runat="server">*Go To Today</asp:literal></a>
                </span>
            </div>
            <div class="fieldrow left">
                <span class="legend hor">
                    <asp:Label ID="LBlegendLabel" runat="server" CssClass="fieldlabel">*Legend</asp:Label>
                    <span class="inlinewrapper">
                        <span class="group first">
                            <span class="legenditem" title="*Critical" runat="server" id="SPNcritical">
                                <span class="legendicon critical">&nbsp;</span>
                                <asp:Label ID="LBganttCriticalLegend" runat="server" CssClass="legendtext">*critical</asp:Label>
                            </span>
                            <span class="legenditem" title="*Task" runat="server" id="SPNactivity">
                                <span class="legendicon task">&nbsp;</span>
                                <asp:Label ID="LBganttActivityLegend" runat="server" CssClass="legendtext">*task</asp:Label>
                            </span>
                            <span class="legenditem" title="*Summary Task" runat="server" id="SPNsummary">
                                <span class="legendicon group">&nbsp;</span>
                                <asp:Label ID="LBganttSummaryLegend" runat="server" CssClass="legendtext">*summary task</asp:Label>
                            </span>
                        </span>
                        <span class="group last">
                            <asp:Label ID="LBganttActivityLinksLegend" runat="server" CssClass="grouptitle">*task links:</asp:Label>
                            <span class="legenditem" title="*Finish-Start" runat="server" id="SPNfinishStart">
                                <span class="legendicon line fs">&nbsp;</span>
                                <asp:Label ID="LBganttFinishStartLegend" runat="server" CssClass="legendtext">*finish-start</asp:Label>
                            </span>
                            <span class="legenditem" title="*Finish-Finish" runat="server" id="SPNfinishFinish">
                                <span class="legendicon line ff">&nbsp;</span>
                                <asp:Label ID="LBganttFinishFinishLegend" runat="server" CssClass="legendtext">*finish-finish</asp:Label>
                            </span>
                            <span class="legenditem" title="*Start-Finish" runat="server" id="SPNstartFinish">
                                <span class="legendicon line sf">&nbsp;</span>
                                <asp:Label ID="LBganttStartFinishLegend" runat="server" CssClass="legendtext">*start-finish</asp:Label>
                            </span>
                            <span class="legenditem" title="*Start-Start" runat="server" id="SPNstartStart">
                                <span class="legendicon line ss">&nbsp;</span>
                                <asp:Label ID="LBganttStartStartLegend" runat="server" CssClass="legendtext">*start-start</asp:Label>
                            </span>
                            <span class="legenditem" title="*Today" runat="server" id="SPNtoday">
                                <span class="legendicon line ver today">&nbsp;</span>
                                <asp:Label ID="LBganttTodayLegend" runat="server" CssClass="legendtext">*today</asp:Label>
                            </span>
                            <span class="legenditem" title="*Deadline" runat="server" id="SPNdeadline">
                                <span class="legendicon line ver deadline">&nbsp;</span>
                                <asp:Label ID="LBganttDeadLineLegend" runat="server" CssClass="legendtext">*deadline</asp:Label>
                            </span>
                        </span>
                    </span>
                </span>
            </div>
            
        </div>
    </asp:View>
    <asp:View ID="VIWempty" runat="server">
    </asp:View>
</asp:MultiView>
<asp:Literal ID="LTganttScriptTemplate" runat="server" Visible="false">
    <script type="text/javascript">
        var g = new JSGantt.GanttChart(document.getElementById('GanttChartDIV'), 'day');
        if (g) {
            g.setCaptionType('Complete');  // Set to Show Caption (None,Caption,Resource,Duration,Complete)
            g.setQuarterColWidth(36);

            g.setDateTaskDisplayFormat('day dd month yyyy'); // Shown in tool tip box
            g.setDayMajorDateDisplayFormat('mon yyyy - Week ww') // Set format to display dates in the "Major" header of the "Day" view
            g.setWeekMinorDateDisplayFormat('dd mon') // Set format to display dates in the "Minor" header of the "Week" view
            g.setDateInputFormat("dd/mm/yyyy");
            g.setUseSort(0);
            g.setShowTaskInfoDur(0);
            g.setShowTaskInfoLink(1); //Show link in tool tip (0/1)
            g.setShowEndWeekDate(0); // Show/Hide the date for the last day of the week in header for daily view (1/0)
            g.setUseSingleCell(1);
            g.setUseMove(0);
            g.setUseFade(1);
            
            g.setShowRes(0);
            g.setShowDur(0);
            g.setShowComp(0);

            g.setFormatArr("Day", "Week", "Month", "Quarter");
            g.setUseSort(0);

            JSGantt.parseXML('#url#', g)
            g.Draw();


            $(function () {
                $(".commands").data("gantt", g);
                $("#GanttChartDIV").data("gantt", g);

                var width1 = parseInt($(".gcontainercol").css("width"));
                var width2 = parseInt($(".gcharttable").css("width"));
                //                alert(width1);
                //                alert(width2);
                //                alert(width1/width2);
                if (width1 / width2 > 1.2) {
                    var value = Math.floor(18 * width1 / width2);
                    
                    g.setDayColWidth(value);
                }

                //var x = g.getFormatArr();
                //alert(x);

                g.Draw();
            });

        } else {
            alert("#error#");
        }
        </script>
</asp:Literal>