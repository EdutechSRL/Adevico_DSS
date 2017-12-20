<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SelectRepositoryItemsTableModeHeader.ascx.vb" Inherits="Comunita_OnLine.UC_SelectRepositoryItemsTableModeHeader" %>
<!-- SELECT REPOSITORY SCRIPT START-->
<script type="text/javascript">
     $(function () {
         $("table .headercheckbox input[type='checkbox']").change(function () {
             var $checkbox = $(this);
             var $table = $checkbox.parents("div.tablewrapper table");
             var ischecked = $checkbox.is(":checked");
             var $rows = $table.children("tbody").children("tr").find(".submittercheckbox input[type='checkbox']").attr("checked", ischecked);
         });

         $("table .fileitemcheckbox input[type='checkbox']").change(function () {
             var $checkbox = $(this);
             var $table = $checkbox.parents("div.tablewrapper table");
             var checked = $table.find(".fileitemcheckbox input[type='checkbox']:checked").size();
             var total = $table.find(".fileitemcheckbox input[type='checkbox']").size();

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
     });
</script>
<asp:Literal ID="LTscriptSingle" runat="server" EnableViewState="true" Visible="false">
    <script type="text/javascript">
        $(function () {
            $('[name$="selectsingleradio"]').attr("name", $('[name$="selectsingleradio"]').attr("name"));

            $('[name$="selectsingleradio"]').click(function () {
                //set name for all to name of clicked 
                $('[name$="selectsingleradio"]').attr("name", $(this).attr("name"));
            });
        });
    </script>
</asp:Literal>
<!-- SELECT REPOSITORY ITEMS SCRIPT END-->