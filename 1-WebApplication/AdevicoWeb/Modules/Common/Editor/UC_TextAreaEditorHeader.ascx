<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TextAreaEditorHeader.ascx.vb" Inherits="Comunita_OnLine.UC_TextAreaEditorHeader" %>

<script type="text/javascript">
 //function OnClientPasteHtml(editor, args) {
 //       var html = args.get_value();
 //       var div = editor.get_document().createElement('div');
 //       div.innerHTML = html;
 //       var paragraphs = div.getElementsByTagName('p');
 //       for (var i = paragraphs.length - 1; i >= 0; i--) {
 //           var innerText = Telerik.Web.UI.Editor.Utils.getTextContent(paragraphs[i]);
 //           if (innerText.trim() == "") {
 //               $telerik.$(paragraphs[i]).remove();
 //           }
 //       }
 //       args.set_value(div.innerHTML);
 //   }
    function OnClientLoad(editor, args) {
        var toolAdapter = editor.get_toolAdapter();
        toolAdapter.enableContextMenus(false);
    }
</script>

<style>
    .TextAreaEditor div.reToolBarWrapper
    {
        display: none;
    }
</style>