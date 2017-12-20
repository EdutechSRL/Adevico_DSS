<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_GanttHeader.ascx.vb"
    Inherits="Comunita_OnLine.UC_GanttHeader" %>
<%--<link href="../../Graphics/Modules/ProjectManagement/Css/jsgantt.css" rel="Stylesheet" />
<link href="../../Graphics/Modules/ProjectManagement/Css/jsgantt-custom.css" rel="Stylesheet" />
<script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/jsgantt2.js"></script>--%>

<link href="../../Graphics/Modules/ProjectManagement/Css/jsgantt-1.5.1.css" rel="Stylesheet" />
<link href="../../Graphics/Modules/ProjectManagement/Css/jsgantt-1.5.1-custom.css" rel="Stylesheet" />
<script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/jsgantt-1.5.1.js"></script>

   <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>
<script type="text/javascript">
    $(function () {
        $(".page-width").addClass("fullwidth");

        /*$(".commands .command.ganttres").click(function(){
        var g=$(this).parents(".commands").first().data("gantt");
        var x = g.getShowRes();
        if(x==0){g.setShowRes(1);
        }else {g.setShowRes(0)}
        g.Draw();
        });

        $(".commands .command.ganttdur").click(function(){
        var g=$(this).parents(".commands").first().data("gantt");
        var x = g.getShowDur();
        if(x==0){g.setShowDur(1);
        }else {g.setShowDur(0)}
        g.Draw();
        });

        $(".commands .command.ganttcomp").click(function(){
        var g=$(this).parents(".commands").first().data("gantt");
        var x = g.getShowComp();
        if(x==0){g.setShowComp(1);
        }else {g.setShowComp(0)}
        g.Draw();
        });

        $(".commands .command.ganttstart").click(function(){
        var g=$(this).parents(".commands").first().data("gantt");
        var x = g.getShowStartDate();
        if(x==0){g.setShowStartDate(1);
        }else {g.setShowStartDate(0)}
        g.Draw();
        });

        $(".commands .command.ganttend").click(function(){
        var g=$(this).parents(".commands").first().data("gantt");
        var x = g.getShowEndDate();
        if(x==0){g.setShowEndDate(1);
        }else {g.setShowEndDate(0)}
        g.Draw();
        });
        */


        $(".commands .command.gantttoday").click(function () {
            //var g=$(this).parents(".commands").first().data("gantt");
            g.setScrollTo("today");
            g.Draw();
        });

        $(".commands .command.gantttostart").click(function () {

            //var g=$(this).parents(".commands").first().data("gantt");

            g.setScrollTo("0");
            g.Draw();
        });

        $("#GanttChartDIV").on("click", "tr.gname .gstartdate, tr.gname .genddate", function () {
            //var g1 = $(this).parents("#GanttChartDIV").data("gantt");

            var x = $(this).html().split("/");


            g.setScrollTo(x[2] + "-" + x[1] + "-" + x[0]);
            g.Draw();

        });


    });
</script>
