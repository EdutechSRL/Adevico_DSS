<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TelerikEditor.ascx.vb" Inherits="Comunita_OnLine.UC_TelerikEditor" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .contenitoreEditor
    {
        width: 100%;
        text-align: left;
    }
    .reTool .InsertEmoticons {
        background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/Emoticons/1.gif) !important;
    }
    .reTool .InsertLatex {
        background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/LatexIcon.gif) !important;
    }
    .reTool .InsertYoutube {
       background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/YoutubeIcon.gif) !important;
    }
    .reTool .InsertWiki {
       background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/WikiIcon.gif) !important;
    }
      span.noteditable
    {
        display:inline;
        background-color:#eee;
        border:1px solid #ddd;
        padding-left:3px;
        padding-right:3px;
        color:#777;
        font-style:italic;         
    }
    
    span.noteditable span.editable
    {        
        background-color:#fff;
        border:0;
        color:#000;        
        font-style:normal;
        padding-left:3px;
        padding-right:3px;
    }
</style>

<!--START SCRIPT EDITOR-->
<script type="text/javascript">
 function OnClientPasteHtml(editor, args) {
        var html = args.get_value();
        var div = editor.get_document().createElement('div');
        div.innerHTML = html;
        var paragraphs = div.getElementsByTagName('p');
        for (var i = paragraphs.length - 1; i >= 0; i--) {
            var innerText = Telerik.Web.UI.Editor.Utils.getTextContent(paragraphs[i]);
            if (innerText.trim() == "") {
                $telerik.$(paragraphs[i]).remove();
            }
        }
        args.set_value(div.innerHTML);
 }
</script>
<asp:Literal id="LTscript" runat="server"></asp:Literal>
<!--END SCRIPT EDITOR-->

<telerik:radeditor ID="RDEtelerik" runat="server" Visible="true" 
    EditModes="Design" Skin="Default" 
    AutoResizeHeight="false" EnableResize="false" NewLineMode="P" OnClientPasteHtml="OnClientPasteHtml" MaxImageSize="360000">
</telerik:radeditor>

<asp:Literal id="LTpostEditorScripts" runat="server"></asp:Literal>
    <!--
    OnClientLoad="RadEditorLoad"
    llowCustomColors="false" 
     ContentAreaMode="Div"  
      ContentAreaMode="Div"
                StripFormattingOnPaste="All" OnClientLoad="OnClientLoad" ContentAreaMode="Div"
                Editable="True" EnableHtmlIndentation="false" EnableTab="True" MaxImageSize="180000" MaxDocumentSize="600000"
                ShowPreviewMode="false" ShowSubmitCancelButtons="false"  ShowHtmlMode="false"  ConvertToXhtml="false"
                Language="it-IT"  EnableClientSerialize="false" EnableDocking="false" EnableViewState="true"-->