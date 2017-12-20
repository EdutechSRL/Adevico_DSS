<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ModalCommunitySelectorHeader.ascx.vb" Inherits="Comunita_OnLine.UC_ModalCommunitySelectorHeader" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/Common/UC/UC_FindCommunitiesByServiceHeader.ascx" %>

<link href="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.css?v=201512151100lm" rel="Stylesheet" />
<script type="text/javascript" src="<%=GetBaseUrl()%>Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
<CTRL:Header id="CTRLheader" runat="server" ></CTRL:Header>


<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(".dlgaddcommunities.hiddendialog").each(function () {
            var $dlg = $(this);
            $dlg.dialog({
                appendTo: "form",
                autoOpen: false,
                modal: true,
                width: <%=Width%>,
                height: <%=Height%>,
                minHeight: <%=MinWidth%>,
                minWidth: <%=MinHeight%>,
                title: '<%=ModalTitle %>',
                close:function( event, ui )
                {
                    $("input[type='hidden'].autoopendialog").val("");
                }
            })
        });

        $(".hiddendialog .close").click(function () {
            $(this).parents(".hiddendialog").first().dialog("close");
            $("input[type='hidden'].autoopendialog").val("");
            return false;
        });

        $(".dlgaddcommunitiesopen").click(function () {
            $(".dlgaddcommunities").dialog("open");
            return false;
        });
    });
</script>
<asp:Literal ID="LTdefaultWidth" runat="server" Visible="false">890</asp:Literal>
<asp:Literal ID="LTdefaultHeight" runat="server" Visible="false">650</asp:Literal>
<asp:Literal ID="LTdefaultMinWidth" runat="server" Visible="false">500</asp:Literal>
<asp:Literal ID="LTdefaultMinHeight" runat="server" Visible="false">700</asp:Literal>
<asp:Literal ID="LTidentifier" runat="server" Visible="false">.dlgaddcommunities</asp:Literal>
<asp:Literal ID="LTclientOpenDialogCssClass" runat="server" Visible="false">dlgaddcommunitiesopen</asp:Literal>