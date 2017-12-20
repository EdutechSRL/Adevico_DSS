<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_AsyncUploadHeader.ascx.vb" Inherits="Comunita_OnLine.UC_AsyncUploadHeader" %>
<script type="text/javascript" language="javascript">
    
    function OnClientFileSelected(sender, args) {
        <asp:Literal ID="LTonClientFileSelected" runat="server"></asp:Literal>
    }
    function OnClientFilesUploaded(sender, args) {
        <asp:Literal ID="LTonClientFilesUploaded" runat="server"></asp:Literal>
        $(".hiddensubmit").click();
    }


    function validationFailed(radAsyncUpload, args) {
        var $row = args.get_row();
        var erMessage = getErrorMessage(radAsyncUpload, args);
        alert(erMessage);
    }

    function getErrorMessage(sender, args) {
        var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
        if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
            if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                return ("<%=TypeError%>");
            }
            else {
                return ("<%=SizeError%>");
            }
        }
        else {
            return ("<%=ExtensionError%>");
        }
    }

    //function createError(erorMessage) {
    //    var input = '<span class="ruErrorMessage">' + erorMessage + ' </span>';
    //    return input;
    //}
</script>

<asp:Literal ID="LTtemplateDisable" runat="server" Visible="false">if (document.getElementById('#clientid#')!=null) {document.getElementById('#clientid#').disabled = true;}</asp:Literal>
<asp:Literal ID="LTtemplateEnable" runat="server" Visible="false">if (document.getElementById('#clientid#')!=null) {document.getElementById('#clientid#').disabled = false;}</asp:Literal>
