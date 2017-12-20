<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UCDialog.ascx.vb" Inherits="Comunita_OnLine.UCDialog" %>

<style type="text/css">
    .dialog
    {        
        display:none;        
    }
    .RadioButtonList
    {
        margin-top:1em;
        width:100%;
        padding-left:0.5em;
        display:block;        
    }
    
    .RadioButtonList input
    {
       border:0;
       padding-right:1em;
    }
    
    .RadioButtonList label
    {
       border:0;
       padding-right:1em;       
    }
    
    .dialogButtons
    {
        margin:0.5em;        
        margin-top:1em;
        width:100%;
        text-align:center;
        display:block;        
        bottom:0;
        margin-bottom:0;
    }
    
    .dialogButton input
    {
        width:30%;        
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        var id = "<%= Me.DialogID %>";
        var cssClass = "<%= Me.DialogClass%>";

        var dlg;

        var Modal = ("<%= Me.DialogModal %>" == "True") ? true : false;
        var PreventClose = ("<%= Me.DialogPreventClose %>" == "True") ? true : false;
        var CloseOnEscape = !PreventClose;

        if (id != "") {
            dlg = $(".dialog").filter("#" + id).dialog({
                appendTo:"form",
                autoOpen: false,
                modal: Modal,
                closeOnEscape: CloseOnEscape,
                title: "<%= Me.DialogTitle %>",
                open: onOpen,
                width: 400

            });

        }
        if (cssClass != "") {
            dlg = $(".dialog").filter("." + cssClass).dialog({
                appendTo: "form",
                autoOpen: false,
                modal: Modal,
                closeOnEscape: CloseOnEscape,
                title: "<%= Me.DialogTitle %>",
                open: onOpen,
                width: 400
            });
        }
        function onOpen() {
            //dlg.parent().appendTo($('form:first'));
            if (PreventClose == true) {
                $(".ui-dialog-titlebar-close").hide();
            }
        }

        $(".ClientSide input").live("click", function () {

            $(this).parents(".dialog").dialog("close");
            return false;
        });

    });
</script>
<div class='dialog <%= Me.DialogClass %>' id='<%= Me.DialogID %>'>
    <span class="dialog_text"><%= Me.DialogText %><br /></span>
    <asp:RadioButtonList runat="server" ID="RBLoptions" CssClass="RadioButtonList">
    </asp:RadioButtonList>
    <asp:CheckBoxList runat="server" ID="CHBoptions" CssClass="CheckboxList">
    </asp:CheckBoxList>
    <hr runat="server" id="HRline" visible="false" />
    <span class="dialogButtons">
        <span class="dialogButton ok"><asp:Button ID="BTNok" Text="Ok" runat="server" /></span>
        <span class="dialogButton cancel <%=Me.JSServerSideClass %>"><asp:Button ID="BTNcancel" Text="Cancel" runat="server" /></span>
    </span>    
    <input class="CommandArgument" id="HIDcommandArgument" runat="server" type="hidden" />
    <input class="CommandName" id="HIDcommandName" runat="server" type="hidden" />
</div>