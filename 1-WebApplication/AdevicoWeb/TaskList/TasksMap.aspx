<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TasksMap.aspx.vb" Inherits="Comunita_OnLine.TasksMap" 
    MasterPageFile="~/AjaxPortal.Master" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="Map" Src="UC/UC_TasksMap.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SwichTask" Src="UC/UC_SwichTaskMap.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="DVtitle" style="width: 900px; height:auto; text-align: left;" class="RigaTitolo" align="center">
        <asp:Label ID="LBtitolo" runat="server"></asp:Label>
    </div>
    <div align="right" style="padding:5px">
        <asp:HyperLink ID="HYPreturnToTaskList" runat="server" Text="*Return To TaskList"
            CssClass="Link_Menu"></asp:HyperLink>
        <asp:HyperLink ID="HYPaddSubTask" runat="server" Text="*Add Sub Task" CssClass="Link_Menu"></asp:HyperLink>
        <asp:HyperLink ID="HYPmap" runat="server" Text="*Swich Task WBS Position" CssClass="Link_Menu" />
        <asp:HyperLink ID="HYPgantt" runat="server" Text="gantt**" CssClass="Link_Menu" />
    </div>
    <asp:MultiView ID="MLVtaskDetail" runat="server">
        <asp:View ID="VIWmap" runat="server">
            <div style="padding: 5px;">
                <CTRL:Map ID="CTRLmap" runat="server" />
            </div>
        </asp:View>
        <asp:View ID="VIWswichMap" runat="server">
            <div style="padding: 5px;">
                <CTRL:SwichTask ID="CTRLswichTask" runat="server" />
            </div>
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div align="center">
                <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
