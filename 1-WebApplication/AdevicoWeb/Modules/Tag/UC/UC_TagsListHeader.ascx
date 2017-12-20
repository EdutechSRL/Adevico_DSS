<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TagsListHeader.ascx.vb" Inherits="Comunita_OnLine.UC_TagsListHeader" %>
<%@ Register TagPrefix="CTRL" TagName="FiltersHeader" Src="~/Modules/Common/UC/UC_FiltersHeader.ascx" %>

<link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201604071200lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
<link href="<%=GetBaseUrl()%>Graphics/Modules/TileTag/css/TileTag.css?v=201604071200lm" rel="Stylesheet" />
<script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/TileTag.js" type="text/javascript"></script>
<script src="<%=GetBaseUrl()%>Scripts/angular.min.js" type="text/javascript"></script>
<CTRL:FiltersHeader id="CTRLfiltersHeader" FilterModuleCode="SRVTAGS" FilterModuleScope="Community" runat="server"></CTRL:FiltersHeader>
<script language="javascript" type="text/javascript">
    var tagasmx = "<%=GetBaseUrl()%>Modules/Tag/Tags.asmx";       
</script>
<script src="<%=GetBaseUrl()%>Jscript/Modules/TileTag/ngTag.js" type="text/javascript"></script>
   <script type="text/javascript" language="javascript">
       $(function () {
           $(".groupedselector .selectoricon, .groupedselector .selectorlabel").click(function () {
               var $group = $(this).parents(".groupedselector").first();

               $(".groupedselector").not($group).removeClass("clicked");
               $group.toggleClass("clicked");

           });

           $(".groupedselector .selectoritem").click(function () {
               var $group = $(this).parents(".groupedselector").first();
               $group.removeClass("clicked");

               $group.find(".selectoritem").removeClass("active");
               $(this).addClass("active");

               $group.find(".selectorgroup .selectorlabel").html($(this).find(".selectorlabel").html());

           });

           $(".groupedselector").mouseout(function () {
               //$(this).removeClass("clicked");
           });

           //$(".ddbuttonlist.enabled").dropdownButtonList();

           $(".tablewrapper.hasbulk table.table .select input[type='checkbox']").change(function () {



               var $tablew = $(this).parents(".tablewrapper.hasbulk");
               var $table = $(this).parents("table.table").first();
               var $checks = $table.find(".select input[type='checkbox']").filter(":not([disabled])");

               var n_checks = $checks.size();

               var n_checks_checked = $checks.filter(":checked").size();

               if ($(this).parents(".select").first().is("th.select")) {
                   $checks.prop("checked", $(this).is(":checked"));
               } else {
                   if (n_checks_checked >= n_checks - 1) {

                       $table.find("th.select input[type='checkbox']").prop("checked", true);
                   } else {

                       $table.find("th.select input[type='checkbox']").prop("checked", false);
                   }
               }

               n_checks_checked = $checks.filter(":checked").size();

               if (n_checks_checked > 0) {
                   $tablew.addClass("bulkon").removeClass("bulkoff");
               } else {
                   $tablew.removeClass("bulkon").addClass("bulkoff");
               }


           });
       })
    </script>