<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Gantt.aspx.vb" Inherits="Comunita_OnLine.Gantt"
    MasterPageFile="~/AjaxPortal.Master" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <link rel="stylesheet" type="text/css" href="jsgantt.css" />

    <script language="javascript" src="jsgantt.js" type="text/javascript"></script>

    <asp:MultiView ID="MLVgantt" runat="server">
        <asp:View ID="VIWgantt" runat="server">
            <div id="DVtitle" style="text-align: left; width: 100%" class="RigaTitolo" align="center">
                <asp:Label ID="LBtitolo" runat="server"></asp:Label>
            </div>
            <div runat="server" id="DIVbuttonEditable" align="right" style="text-align: right;
                width: 100%; clear: right; height: 24px; padding: 5px">
                <asp:HyperLink ID="HYPprevius" runat="server" Text="*Return " CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <div style="clear: both; width:100%; display:block;">
                <div style="float: left; padding: 2px">
                    <table cellspacing="0" border="1">
                        <tr>
                            <td style="width: 10px; background-color: #ffff80">
                                &nbsp;
                            </td>
                            <td class="ROW_Page_Small">
                                <asp:Label ID="LBstarted" runat="server" Text="*iniziato"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; padding: 2px">
                    <table cellspacing="0" border="1">
                        <tr>
                            <td style="width: 10px; background-color: #ff4070">
                                &nbsp;
                            </td>
                            <td class="ROW_Page_Small">
                                <asp:Label ID="LBnotStarted" runat="server" Text="*notstarde"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; padding: 2px">
                    <table cellspacing="0" border="1">
                        <tr>
                            <td style="width: 10px; background-color: #00e100">
                                &nbsp;
                            </td>
                            <td class="ROW_Page_Small">
                                <asp:Label ID="LBcompleted" runat="server" Text="*completato"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                
            </div>
           <br />  <br />
            <div class="gantt" id="GanttChartDIV">  </div>
         
                <asp:Literal runat="server" ID="ltprojectId"></asp:Literal>

                <script type="text/javascript" language="javascript">



                    var g = new JSGantt.GanttChart('g', document.getElementById('GanttChartDIV'), 'day');

                   
                    g.setShowRes(0); // Show/Hide Responsible (0/1)
                    g.setShowDur(0); // Show/Hide Duration (0/1)
                    g.setShowComp(1); // Show/Hide % Complete(0/1)
                    g.setDateInputFormat('dd/mm/yyyy');
                    g.setDateDisplayFormat('dd/mm/yyyy');
                    g.setCaptionType('Complete');  // Set to Show Caption
                    
                    if (g) {

                        JSGantt.parseXML("GenerateGanttXML.aspx?ProjectID=" + projectId, g);
                        g.Draw();
                        //g.DrawDependencies();
                    }
                    else {
                        alert("not defined");
                    }
                </script>

          
        </asp:View>
        <asp:View ID="VIWerror" runat="server">
            <div id="DVerror" align="center">
                <div align="right" style="text-align: right; clear: right;">
                    <asp:HyperLink ID="HYPreturnError" runat="server" Text="Torna all'elenco" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio"></asp:Label>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
