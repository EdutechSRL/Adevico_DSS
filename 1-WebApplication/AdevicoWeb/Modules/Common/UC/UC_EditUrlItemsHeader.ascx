<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditUrlItemsHeader.ascx.vb" Inherits="Comunita_OnLine.UC_EditUrlItemsHeader" %>
<%@ Register TagPrefix="CTRL" TagName="Scripts" Src="~/Modules/Common/UC/UC_OpenDialogHeaderScripts.ascx" %>

<asp:Literal ID="LTdefaultWindow" runat="server" Visible="false">700,500,400,200</asp:Literal>
<asp:Literal ID="LTcloseScripts" runat="server" Visible="false"> $(this).find("input[type='text']").val('');</asp:Literal>
<asp:Literal ID="LTplaceholderdScript" runat="server" Visible="false">#script#</asp:Literal>
<CTRL:scripts id="CTRLscripts" runat="server"></CTRL:scripts>
<asp:Literal ID="LTscript" runat="server" Visible="false">
    <!-- START SCRIPT EDITING URL ITEMS-->
    <script language="javascript" type="text/javascript">
        $(function() { 
            #script#
        });
    </script>
    <!-- END SCRIPT EDITING URL ITEMS-->
</asp:Literal>