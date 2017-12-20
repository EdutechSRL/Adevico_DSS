<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintUserAccessReport.aspx.vb"
    Inherits="Comunita_OnLine.PrintUserAccessReport" Theme="Materiale" EnableTheming="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Comunità On Line - Print User/s Access Report</title>
    <link href="./../../Styles.css" type="text/css" rel="stylesheet" />
</head>

<script language="javascript" type="text/javascript">
    function ChiudiMi() {
        this.window.close();
    }

    function stampa() {
        this.window.print();
    }
</script>

<body style="margin-top: 0px; margin-right: 0px; margin-left: 0px;" id="popup">
     <form id="aspnetForm" runat="server">
     <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
    <div style="margin: 0 auto; bottom: 0px 0px; width: 95%">
        <div style="width: 100%; text-align: right; padding: 5px 5px;">
            <asp:Button runat="server" ID="BTNclose" Text="Chiudi" CssClass="LINK_MENU" OnClientClick="return ChiudiMi();">
            </asp:Button>&nbsp;
            <asp:Button runat="server" ID="BTNprintItems" Text="Stampa" CssClass="LINK_MENU"
                OnClientClick="stampa();return false;"></asp:Button>
        </div>
        <div style="width: 800px; text-align: center; border: 1px; border-color: Black; border-style: solid;
            clear: both; padding-left: 10px;">
            <div style="width: 800px; text-align: center; height: 24px;" class="RigaTitolo">
                <asp:Label ID="LBtitle" runat="server">Report</asp:Label>
            </div>
            <div runat="server" id="DIVintestazione" style="width: 900px; text-align: center">
                <div style="width: 800px; text-align: left">
                    <asp:Label ID="LBdata_t" runat="server" class="Titolo_campo"></asp:Label>&nbsp;
                    <asp:Label ID="LBdata" runat="server" class="Testo_campo"></asp:Label>
                </div>
                <div style="width: 800px; text-align: left">
                    <asp:Label ID="LBuser_t" runat="server" class="Titolo_campo"></asp:Label>&nbsp;
                    <asp:Label ID="LBuser" runat="server" class="Testo_campo"></asp:Label>
                </div>
                <br />
            </div>
            <div style="text-align: left; float: left;" id="DVtotalUsage">
                <asp:Label ID="LBtotalTime" runat="server" />
            </div>
            <br />
            <div runat="server" id="DIVdata" style="text-align: center;">
                <asp:GridView CssClass="table light" width="800px"   runat="server" ID="GDVstatistic" AutoGenerateColumns="false"
                    AllowSorting="false" UseAccessibleHeader="true" HorizontalAlign="Center">
                    <Columns>
                        <asp:TemplateField HeaderText="Community" SortExpression="Community">
                            <ItemTemplate>
                                <%#Container.DataItem.CommunityName%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User" SortExpression="Owner">
                            <ItemTemplate>
                                &nbsp;<%#Container.DataItem.PersonName%>&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Day" SortExpression="Day">
                            <ItemTemplate>
                                &nbsp;<%#FormatDateTime(Container.DataItem.Day, DateFormat.ShortDate)%>&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hours" SortExpression="Hour">
                            <ItemTemplate>
                                <%#Container.DataItem.HourToInterval%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time" SortExpression="UsageTime">
                            <ItemTemplate>
                                &nbsp;<%# GetTimeTranslatedString(Container.DataItem.ToTimeSpan)%>&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div style="padding: 150px 0px; margin: 0px auto; width: 100%; text-align: center;
            height: 400px; vertical-align: middle;" runat="server" id="DIVpermessi">
            <asp:Label ID="LBNopermessi" runat="server" CssClass="messaggio">Spiacente, non dispone dei permessi necessari per visualizzare il diaro personale.</asp:Label>
        </div>
        <br />
    </div>
    </form>
</body>
</html>
