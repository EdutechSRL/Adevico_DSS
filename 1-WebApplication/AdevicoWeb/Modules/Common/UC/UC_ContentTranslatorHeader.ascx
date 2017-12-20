<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ContentTranslatorHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ContentTranslatorHeader" %>
<script language="javascript" type="text/javascript">
    $.fn.insertAtCaret = function (tagName) {
        return this.each(function () {
            if (document.selection) {
                //IE support
                this.focus();
                sel = document.selection.createRange();
                sel.text = tagName;
                this.focus();
            } else if (this.selectionStart || this.selectionStart == '0') {
                //MOZILLA/NETSCAPE support
                startPos = this.selectionStart;
                endPos = this.selectionEnd;
                scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + tagName + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + tagName.length;
                this.selectionEnd = startPos + tagName.length;
                this.scrollTop = scrollTop;
            } else {
                this.value += tagName;
                this.focus();
            }
        });
    };

    $(document).ready(function () {
        $(".addTextTelerik").click(function () {
            var editor = $find($(this).data("editor"));
            var add = $(this).data("tag");
            try {
                editor.pasteHtml(add);
            }
            catch (ex) {
                alert(add);
                alert(ex);
            }
            return false;
        });
        $(".addTextTextarea").click(function () {
            var textarea = $(".addTextToMe");
            var add = $(this).data("tag");
            textarea.insertAtCaret(add);
            return false;
        });
    });
    $(function () {
        $(".dialog.dlgkeyword").dialog({
            autoOpen: false,
            width: 450,
            height: 300,
            minHeight: 100,
            minWidth: 200
        });

        $(".varbuttons .more").click(function () {
            $(".dialog.dlgkeyword").dialog("open");
            return false;
        });

        $(".expandlistwrapper:visible:not(.initialized)").each(function () {
            InitializeExpandList($(this));
        });

        function InitializeExpandList(el) {
            if (!el.is(".initialized")) {
                el.addClass("initialized");
                var $children = el.find("ul.expandlist");
                var $content = $children.children().wrapAll('<div class="overflow">');

                //$children.wrapInner('<div class="clearfix" />');
                var delta = 3;

                var $el = el.find("div.overflow");
                var HasOverflow = $children.height() + delta < $el.height();

                if (!HasOverflow) {
                    el.addClass("disabled");
                    el.removeClass("compressed");
                } else {
                    el.removeClass("disabled");
                }
            }
        }

        $(".expandlistwrapper .command.expand").click(function () {
            $(this).parents(".expandlistwrapper").first().removeClass("compressed");
            return false;
        });

        $(".expandlistwrapper .command.compress").click(function () {
            $(this).parents(".expandlistwrapper").first().addClass("compressed");
            return false;
        });
    });

</script>