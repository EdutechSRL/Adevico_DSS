<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MoveItemsDialogHeader.ascx.vb" Inherits="Comunita_OnLine.UC_MoveItemsDialogHeader" %>
<script type="text/javascript">
       $(function () {
           $(".dlgmoveitems.hiddendialog").each(function () {
               var $dlg = $(this);
               //$dlg.attr("id", "dlgconfirmunsubscription");
               $dlg.dialog({
                   appendTo: "form",
                   autoOpen: false,
                   modal: true,
                   /*width:"auto",
                   height:"auto",*/
                   width: 800,
                   height: 400,
                   close:function( event, ui )
                   {
                       $("input[type='hidden'].autoopendialog").val("");
                   }
               })
           });

           $(".dlgmoveitems.hiddendialog .close").click(function () {
               $(this).parents(".hiddendialog").first().dialog("close");
               $("input[type='hidden'].autoopendialog").val("");
               return false;
           });
       });
      
       function onMoveUpdating() {
           $.blockUI({ message: '<h1><%=GetOnUpdatingMoveAction() %></h1>' });
           return true;
       }
</script>