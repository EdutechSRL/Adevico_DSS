<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModalPlayerHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ModalPlayerHeader" %>
<script type="text/javascript">
    
    function CloseDialog() {
        
        $('body').unblock();
        // Questo reload fa un postback !
        //location.reload();
        window.location = window.location.href;
        //$("<meta http-equiv='refresh' content='0'>").appendTo("head");
    }
    function CloseDialogNoReload() {
        
        $('body').unblock();
    }
    function CloseDialogByModal(reload) {
        if (reload)
            CloseDialog();
        else
            CloseDialogNoReload();
    }
    $(function(){

        if ($(".redirectmodalbutton").size() > 0) {
            var src = $(".redirectmodalhidden").val();
            if (src != "") {
                open(src);
                $(".redirectmodalhidden").val('');
            }
        }

        $("a.openmodal.scorm").click(function () {
            alert("scorm");
            var src = $(this).attr("href");

            if ($(".redirectmodalbutton").size() > 0) {

                $(".redirectmodalhidden").val(src);

                $(".redirectmodalbutton").click();

            } else {

                open(src);

            }

            return false;
        })
        $("a.openmodal.multimedia").click(function () {
            open($(this).attr("href"));
            return false;
        })
        $("a.openmodal.scormpackage").click(function () {
            var src = $(this).attr("href");

            if ($(".redirectmodalbutton").size() > 0) {

                $(".redirectmodalhidden").val(src);

                $(".redirectmodalbutton").click();

            } else {

                open(src);

            }
            return false;
        })
        function open(src) {

            var delta = 50;

            var message_height = 768;
            var message_width = 1024;
            
            $("<div id='dialog' style='width:" + message_width + "px; height: " + message_height + "px; display:none;'><iframe id='myIframe' src='' width='100%' height='100%'></iframe></div>").appendTo("body");

            
            var url = src;
            $("#dialog iframe").attr("src", url);
            $.blockUI({
                message: $('#dialog'),

                theme: false,
                css: {
                    padding: 0,
                    margin: 0,
                    width: message_width,
                    height: message_height,
                    textAlign: 'center',
                    color: '#000',
                    border: '1px solid #bbb',
                    backgroundColor: '#fff',
                    cursor: 'wait',
                    top: '50%',
                    left: '50%',
                    margin: (-message_height / 2) + 'px 0 0 ' + (-message_width / 2) + 'px'
                },
                overlayCSS: {
                    backgroundColor: '#ccc',
                    opacity: 0.6,
                    cursor: 'wait'
                },
            });
                        

            //$('.blockUI.blockMsg').center();

            return false;
        }
        
});
</script>
<style type="text/css">			
    #dialog iframe{
        border:0;
    }
</style>