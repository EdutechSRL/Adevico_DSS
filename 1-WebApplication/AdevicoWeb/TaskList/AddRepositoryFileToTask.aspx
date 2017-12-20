<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" 
    CodeBehind="AddRepositoryFileToTask.aspx.vb" Inherits="Comunita_OnLine.AddRepositoryFileToTask" %>
<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_SelectCommunityFiles.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="LinkRepositoryItems" Src="~/Modules/Common/UC/UC_OtherModuleLinkRepositoryItems.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
        .columnsEdit
        {
            display: block;
            margin-top: 0.2em;
            margin-bottom: 0.2em;
            float: left;
            clear: both;
            width: 100%;
        }
        
        .columnsEdit .column
        {
            display: block;
            margin-top: 0.2em;
            margin-bottom: 0.2em;
        }
        
        .columnsEdit .left
        {
            clear: left;
            float: left; /*width:20%;*/
        }
        
        .columnsEdit .right
        {
            float: left; /* width:75%;*/
            clear: right;
        }
      ul.partecipants
        {
            margin: 0;
            padding: 0;
        }
        ul.partecipants li.partecipant
        {
            display: block;
        }
        ul.projectFiles
        {
            margin: 0;
            padding: 0;
        }
        ul.projectFiles li.projectFile
        {
            display: block;
        }
        
        .DivEpButton
        {
            text-align: right;
            padding: 10px 0px 5px 10px;
        }
        
        div.DivEpButton
        {
            width: 900px;
            text-align: right;
        }
        
        
        
         ul.fields, ul.sections
        {
            width:900px;
        }
        
        ul.fields li.field, ul.sections li.section
        {
            /*float:left;*/
            /*border-top:1px solid black;*/
            margin-bottom:0.5em;
            padding:0.3em;
            
        }
                
        
        ul.fields li.field
        {
            /*background-color:#eee;*/
        }
        
        div.containerSectionInput
        {
            font-weight:bold;
            padding-top:1em;
            padding-bottom:0.2em;
            padding-right: 0.2em;
        }
        fieldset.containerSectionInput
        {
            padding-top:10px;
            padding-bottom:10px;
        }
        div.container
        {
            padding-bottom:0.2em;
            border-bottom:1px solid black;
        }
        
        ul.fields li.fixed
        {
            
            margin:0;
            padding:0;
            border:0;
        }
        
        
        
        ul.fields li.field, ul.sections li.section, ul.partecipants li.partecipant
        {
            display:block;
            
            width:80%;
            
        }
        
        .buttons
        {
            
        }
        
        .buttons .button
        {
            display:inline-block;
            *display:inline;
            width:24px;
            height:24px;
            background-position:center center;
            background-repeat:no-repeat;
           /* text-indent:-99999px;*/
        }
        
        .buttons .moveSection, .buttons .moveField
        {
            background-image:url(img/move.png);            
            cursor:move;
        }
        
        .buttons .delete
        {
            background-image:url(img/delete16.gif);            
        }
        
        .buttons .addSection
        {
            background-image:url(img/addSection.png);            
        }
        .buttons .addField
        {
            background-image:url(img/addField.png);            
        }
        
        .buttons .moveDown
        {
            background-image:url("img/bullet_arrow_down.png");    
            
        }
        .buttons .moveUp
        {
            background-image:url("img/bullet_arrow_up.png");    
            
        }
        
        .buttons .switch
        { 
            background-position:center center;
            vertical-align: inherit;
            background-image:url(img/bullet_toggle_minus.png);            
        }
    /*    .switch
{
    height: 30px;
    width: 50px;
    display: inline-block;
    vertical-align: text-bottom;
    background-position: center bottom;
}*/
        .buttons .collapsed
        {
            background-image:url(img/bullet_toggle_plus.png);
        }
        
        /*Last Button*/        
        .buttons .invisible
        {
            background-image:none!important;    
        }
        
        .labelField
        {
            display:inline-block;
            *display:inline;
            width:100px;            
        }
         .shortText
        {
            display:block;
            width:50%;
        }
        .longText
        {
            display:block;
            width:90%;
        }
        
        ul.columns
        {
            width:100%;            
            margin:0;
            padding:0;
        }
        
        ul.columns li.column
        {
            display:inline-block;
            *display:inline;
            width:49%;
            
            vertical-align:top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
         <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to items" Height="18px"></asp:HyperLink>
          <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to item" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToFileManagement" runat="server" CssClass="Link_Menu" Visible="false"
            Text="Back to file management" Height="18px"></asp:HyperLink>
        <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Link" ID="LNBlinkToItem"
            CausesValidation="false"></asp:LinkButton>
    </div>
    <div>
        <CTRL:LinkRepositoryItems id="CTRLlinkRepositoryItems" runat="server" width="900px" tristateselection="false"
            folderselectable="false" />
    </div>


</asp:Content>

