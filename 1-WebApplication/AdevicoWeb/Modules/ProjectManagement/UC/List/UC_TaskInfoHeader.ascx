<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TaskInfoHeader.ascx.vb" Inherits="Comunita_OnLine.UC_TaskInfoHeader" %>
   <script language="javascript" type="text/javascript">
       $(function () {
           $(".dlgtask.hiddendialog").each(function () {
               var $dlg = $(this);
               var $td = $dlg.parents("td.name").first();
               var $tr = $dlg.parents("tr.task").first();
               //$dlg.data("task",$tr);
               var id = $td.find(".opendlgtask").first().data("dlg");
               $dlg.attr("id", "task-dlg-" + id);
               $dlg.dialog({
                   appendTo: "form",
                   autoOpen: false,
                   modal: true,
                   /*width:"auto",
                   height:"auto",*/
                   width: 800,
                   height: 550
               })
           });

           $("table.tasks tr.task td.name a.opendlgtask").click(function () {
               $(".dlgtask.hiddendialog#task-dlg-" + $(this).data("dlg")).dialog("open");
               $(".dlgtask.hiddendialog#" + $(this).data("dlg")).find(".tabs").tabs({ active: 0 });
           });

           /* NEL CASO IN CUI LA DIALOG SIA ALL'INTERNO DELLA TABLE
           $("table.tasks tr.task td.name a.opendlgtask").click(function () {
           $(this).parents("tr.task").first().find(".dlgtask.hiddendialog").dialog("open");
           $(this).parents("tr.task").first().find(".dlgtask.hiddendialog").find(".tabs").tabs({ active: 0 });
           });

           $("table.tasks tr.task td.name span.icon.attacchment").click(function () {
           $(this).parents("tr.task").first().find(".dlgtask.hiddendialog").dialog("open");
           $(this).parents("tr.task").first().find(".dlgtask.hiddendialog").find(".tabs").tabs({ active: 1 });
           return false;
           });
           */
       });
    </script>