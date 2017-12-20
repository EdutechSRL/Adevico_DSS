<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditorStyles.ascx.vb"
    Inherits="Comunita_OnLine.UC_EditorStyles" %>

<asp:MultiView ID="MLVeditorStyle" runat="server" ActiveViewIndex="0">
    <asp:View ID="VIWnone" runat="server"></asp:View>
    <asp:View ID="VIWtextareaEditor" runat="server"></asp:View>
    <asp:View ID="VIWliteEditor" runat="server"></asp:View>
    <asp:View ID="VIWtinyEditor" runat="server"></asp:View>
    <asp:View ID="VIWtelerikEditor" runat="server">
        <style type="text/css">
            .contenitoreEditor
            {
                width: 100%;
                text-align: left;
            }
            .reTool .InsertEmoticons
            {
                background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/Emoticons/1.gif) !important;
            }
            .reTool .InsertLatex
            {
                background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/LatexIcon.gif) !important;
            }
            .reTool .InsertYoutube
            {
                background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/YoutubeIcon.gif) !important;
            }
            .reTool .InsertWiki
            {
                background-image: url(<%= me.AppUrl %>Graphics/Modules/Editor/img/WikiIcon.gif) !important;
            }
            span.noteditable
            {
                display: inline;
                background-color: #eee;
                border: 1px solid #ddd;
                padding-left: 3px;
                padding-right: 3px;
                color: #777;
                font-style: italic;
            }
            
            span.noteditable span.editable
            {
                background-color: #fff;
                border: 0;
                color: #000;
                font-style: normal;
                padding-left: 3px;
                padding-right: 3px;
            }
        </style>
    </asp:View>
</asp:MultiView>