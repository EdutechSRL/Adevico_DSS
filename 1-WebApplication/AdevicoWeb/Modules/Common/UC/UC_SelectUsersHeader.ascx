<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectUsersHeader.ascx.vb" Inherits="Comunita_OnLine.UC_SelectUsersHeader" %>
<!-- SELECT USERS SCRIPT START-->
<script language="javascript" type="text/javascript">
     $(function () {

         $("table .headercheckbox input[type='checkbox']").change(function () {
             var $checkbox = $(this);
             var $table = $checkbox.parents("div.tablewrapper table");
             var ischecked = $checkbox.is(":checked");
             var $rows = $table.children("tbody").children("tr").find(".submittercheckbox input[type='checkbox']").attr("checked", ischecked);
         });

         $("table .submittercheckbox input[type='checkbox']").change(function () {
             var $checkbox = $(this);
             var $table = $checkbox.parents("div.tablewrapper table");
             var checked = $table.find(".submittercheckbox input[type='checkbox']:checked").size();
             var total = $table.find(".submittercheckbox input[type='checkbox']").size();

             if (total != checked) {
                 $table.find(".headercheckbox input[type='checkbox']").attr("checked", false);
             } else {
                 $table.find(".headercheckbox input[type='checkbox']").attr("checked", true);
             }

         });

         $(".table th input[type='checkbox']").change(function () {
             var $this = $(this);
             $(this).parents("table").first().find("td input[type='checkbox']").prop("checked", $this.is(":checked"));

             var $el;
             var $elout;

             var ultrafast = 1;
             var fast = 200;
             var slow = 3000;

             if ($this.is(":checked")) {
                 $el = $this.siblings(".selectorpopup.checkall");
                 $elout = $this.siblings(".selectorpopup.checknone");
             } else {
                 $el = $this.siblings(".selectorpopup.checknone");
                 $elout = $this.siblings(".selectorpopup.checkall");
             }

             if ($el.size() > 0) {
                 $el.fadeIn(fast).addClass("open");
                 var ovt = setTimeout(function () { $el.fadeOut(fast, function () { $el.removeClass("open"); }); clearTimeout(ovt); }, slow);
             }
             if ($elout.size() > 0) {
                 $elout.fadeOut(ultrafast, function () { $elout.removeClass("open"); });
             }

         });





         $("table.checkboxtable span.icon.delete").click(function () {
             var $tr = $(this).parents("tr").first();
             var checked = $tr.find("td.checkbox input[type=checkbox]").prop("checked");
             $tr.find("td.checkbox input[type=checkbox]").prop("checked", !checked);
             $tr.toggleClass("removed");

             var $table = $(this).parents("table.checkboxtable").first();
             if ($table.find("tbody tr:not(.removed, .last)").size() == 0) {
                 $table.addClass("empty");
             } else {
                 $table.removeClass("empty");
             }
         });

      
     });
</script>
<!-- SELECT USERS SCRIPT END-->