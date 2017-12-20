<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MessageEditHeader.ascx.vb" Inherits="Comunita_OnLine.UC_MessageEditHeader" %>
<%@ Register TagPrefix="CTRL" TagName="SelectorHeader" Src="~/Modules/Common/UC/UC_ContentTranslationSelectorHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TranslatorHeader" Src="~/Modules/Common/UC/UC_ContentTranslatorHeader.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>


<CTRL:SelectorHeader ID="CTRLselectorHeader" runat="server" />
<CTRL:TranslatorHeader ID="CTRLtranslatorHeader" runat="server" />
<CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(".view-modal.view-previewmail").dialog({
            appendTo: "form",
            closeOnEscape: true,
            modal: true,
            width: 700,
            height: 450,
            minHeight: 200,
            minWidth: 400,
            open: function (type, data) {
               // $(this).parent().appendTo("form");
                //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
            }
        });

        $(".view-modal.view-dlgmessages").dialog({
            appendTo: "form",
            closeOnEscape: true,
            modal: true,
            width: 600,
            height: 450,
            open: function (type, data) {
                if ($(this).parents("form").size() == 0) {
                    //$(this).parent().appendTo("form");
                }
                $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
            }
        });
        $(".dlgmessages").dialog({
            appendTo: "form",
            autoOpen: false,
            closeOnEscape: true,
            modal: true,
            width: 400,
            height: 200,
            open: function (type, data) {
                if ($(this).parents("form").size() == 0) {
                    //$(this).parent().appendTo("form");
                }
                //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
            }
        });

        $(".opendlgmessages").click(function () {
            $(".dlgmessages").dialog("open");
            return false;
        });

        $(".table th input[type='checkbox']").change(function () {
            var $this = $(this);
            $(this).parents("table").first().find("td.check input[type='checkbox']:not(:disabled)").prop("checked", $this.is(":checked"));

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

        //             $("table.table th .selectall").click(function () {
        //                 var $table = $(this).parents("table.table").first();
        //                 var $th = $(this).parents("th").first();

        //                 var datacol = $th.data("col");
        //                 $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", true);
        //             });

        //             $("table.table th .selectnone").click(function () {
        //                 var $table = $(this).parents("table.table").first();
        //                 var $th = $(this).parents("th").first();

        //                 var datacol = $th.data("col");
        //                 $table.find("td[data-col='" + datacol + "'] input[type='checkbox']:not(:disabled)").prop("checked", false);
        //             });

        //             $("table.table td .selectall").click(function () {
        //                 //var $table =$(this).parents("table.table").first();
        //                 var $tr = $(this).parents("tr").first();
        //                 $tr.find("td:not(.check) input[type='checkbox']").prop("checked", true);
        //             });

        //             $("table.table td .selectnone").click(function () {
        //                 //var $table =$(this).parents("table.table").first();
        //                 var $tr = $(this).parents("tr").first();
        //                 $tr.find("td:not(.check) input[type='checkbox']").prop("checked", false);
        //             });
    });
</script>