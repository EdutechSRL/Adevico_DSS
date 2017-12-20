<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_OpenDialogHeaderScripts.ascx.vb" Inherits="Comunita_OnLine.UC_OpenDialogHeaderScripts" %>
<asp:Literal ID="LTopendialogcssclassprefix" runat="server" Visible="false">opendlg</asp:Literal>
<asp:Literal ID="LTdivdialogcssclassprefix" runat="server" Visible="false">dlg</asp:Literal>
<asp:Literal ID="LTplaceholderTitle" runat="server" Visible="false">#title#</asp:Literal>
<asp:Literal ID="LTplaceholderOpendialog" runat="server" Visible="false">#opendialogclass#</asp:Literal>
<asp:Literal ID="LTplaceholderAutoOpen" runat="server" Visible="false">#autoopen#</asp:Literal>
<asp:Literal ID="LTplaceholderdDialogIdentifyer" runat="server" Visible="false">#dialogclass#</asp:Literal>
<asp:Literal ID="LTplaceholderdWidth" runat="server" Visible="false">#width#</asp:Literal>
<asp:Literal ID="LTplaceholderdHeight" runat="server" Visible="false">#height#</asp:Literal>
<asp:Literal ID="LTplaceholderdMinWidth" runat="server" Visible="false">#minwidth#</asp:Literal>
<asp:Literal ID="LTplaceholderdMinHeight" runat="server" Visible="false">#minheight#</asp:Literal>
<asp:Literal ID="LTplaceholderdCloseScripts" runat="server" Visible="false">#closescripts#</asp:Literal>
<asp:Literal ID="LTwindowSizes" runat="server" Visible="false">700,500,400,200</asp:Literal>

<asp:Literal ID="LTbaseScript" runat="server" Visible="false">
    var dlg = $(".dialog.#dialogclass#").dialog({
        appendTo: "form",
        autoOpen: false,
        closeOnEscape: false,
        modal: true,
        width: #width#,
        height: #height#,
        minHeight: #minwidth#,
        minWidth: #minheight#,
        title: '#title#',
        open: function (type, data) {
            //$(this).parent().appendTo("form");
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();

            $(this).parent().css("top","50px");			
			$(this).parent().css("position","fixed");			
			/*$('.ui-dialog').css("top","50px");			
			$('.ui-dialog').css("position","fixed");			*/
        },
		resizeStart: function(event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                             (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option','position',position);
        },
        resizeStop: function(event, ui) {
            var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                             (Math.floor(ui.position.top) - $(window).scrollTop())];
            $(event.target).parent().css('position', 'fixed');
            $(dlg).dialog('option','position',position);
        },
        close: function(type,data){
            #closescripts#
            $("input[type='hidden'].autoopendialog").first().val('');
        }
    });

    $(".#opendialogclass#").click(function(){
        $(".#dialogclass#").dialog("open");
        #autoopen#
        return false;
    });
</asp:Literal>
<asp:Literal ID="LTautoOpenScript" runat="server" Visible="false">$("input[type='hidden'].autoopendialog").first().val(".#dialogclass#");</asp:Literal>