<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Startup_Chat.aspx.vb" Inherits="Comunita_OnLine.Startup_Chat" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<%--<%@ Register TagPrefix="HEADER" TagName="CtrLHeader" Src="../UC/UC_header.ascx" %>
<%@ Register TagPrefix="FOOTER" TagName="CTRLFOOTER" Src="../UC/UC_Footer.ascx" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language=javascript type="text/javascript">
        function openChat() {
            window.open('<%=me.BaseUrl %>Chat_Messenger/Chat_Ext.aspx', 'win', 'menubar=no,location=no,toolbar=no,scrollbars=yes,resizable=yes,status=no,width=430,height=440');
            return false;

        }

    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVmenu" style="width: 900px; text-align:right;" align="center">
        &nbsp;
    </div>

    <div align="left" style="width: 900px;  padding-top:5px; ">
        <asp:MultiView ID="MLVchat" runat="server" ActiveViewIndex=0>
			<asp:View id="VIWchat" runat="server">
				<div style="width: 700px; text-align:left; padding-top: 50px; padding-bottom: 100px;">
					<asp:Label ID="LBlink" Runat=server>Se la finestra relativa alla chat non si apre entro 5 secondi premere su questo</asp:Label>
					<asp:LinkButton ID="LNBapri" Runat="server" OnClientClick="return openChat();" EnableViewState="false">LINK</asp:LinkButton>
				</div>
			</asp:View>
			<asp:View id="VIWnoRegistri" runat="server">
				<div align="center" style="width: 700px; text-align:left; padding-top: 100px; padding-bottom: 200px;">
					<asp:Label id="LBNopermessi" runat="server" EnableViewState="false"></asp:Label>
				</div>
			</asp:View>
		</asp:MultiView>
    </div>

</asp:Content>




