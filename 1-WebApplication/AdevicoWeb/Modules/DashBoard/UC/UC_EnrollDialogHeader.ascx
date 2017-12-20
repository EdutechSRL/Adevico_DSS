<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EnrollDialogHeader.ascx.vb" Inherits="Comunita_OnLine.UC_EnrollDialogHeader" %>
<script language="javascript" type="text/javascript">
       $(function () {
           $(".dlgconfirmsubscription.hiddendialog").each(function () {
               var $dlg = $(this);
               //$dlg.attr("id", "dlgconfirmunsubscription");
               $dlg.dialog({
                   appendTo: "form",
                   autoOpen: false,
                   modal: true,
                   /*width:"auto",
                   height:"auto",*/
                   width: 800,
                  // height: 550,
                   close:function( event, ui )
                   {
                       $("input[type='hidden'].autoopendialog").val("");
                   }
               })
           });
       });
     

       function onUpdating() {
           $.blockUI({ message: '<h1><%=GetOnUpdatingEnrollAction() %></h1>' });
           return true;
       }
</script>