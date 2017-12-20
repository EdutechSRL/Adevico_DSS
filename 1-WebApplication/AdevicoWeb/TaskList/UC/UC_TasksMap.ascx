<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TasksMap.ascx.vb"
    Inherits="Comunita_OnLine.UC_TasksMap" %>


<script type="text/javascript" language="javascript">
    $(document).ready(function() {
        $(".hideColumn").click(function() {
            var el = $(this).attr("name");
            $("td." + el).toggle();
            return false;
        });
        $(".hideDeleted").click(function() {
            $(".deleted").toggle();
            return false;
        });
        $(".LevelHider").change(function() {
            var name = $(this).attr("name");
            var value = $(this).val();

            $("#" + name + " tr").each(function() {
                var cur = $(this).attr("title").replace("WBS level: ","");
                if (cur > value) {
                    $(this).hide();
                } else {
                    $(this).show();
                }

            });
        });
    });
</script>

<div style="height: 24px;" id="DIVwbsLevel" runat="server">
    <div style="text-align: left; float: left; width: 100px">
        <asp:Label ID="LBwbsLevel" runat="server" CssClass="Titolo_campoSmall" Text="*WBS Level:"></asp:Label>
    </div>
    <div style="text-align: left;">
        <select class="LevelHider" name="tableMap">
            <asp:Literal ID="LTddl" runat="server"></asp:Literal>
        </select>
    </div>
</div>

<br />
<div>
 <%-- <div style="width: 100%">--%>
        <div style="float: left; display:block; width:15%; text-align: left ;">
          <table cellspacing="0" border="1">
            <tr>
                <td class="ROW_Disabilitate_Small" style="width: 10px; ">
                    &nbsp;
                </td>
                <td class="ROW_Page_Small " style="border : 1">
                    <asp:Label ID="LBlegendaVD" runat="server" Text="*Task non disponibile"></asp:Label>
                </td>          
            </tr>
        </table>
        </div >  
            <div style="margin-left: 15px; border :0 ; width:90%; display:block;">
                    <asp:Literal ID="LThideStartDate" runat="server"></asp:Literal>&nbsp;
                    <asp:Literal ID="LThideEndDate" runat="server"></asp:Literal>&nbsp;
                    <asp:Literal ID="LThideDeletedTask" runat="server"></asp:Literal>
            </div>
  <%--  </div>--%>
    
</div>
<br />
<div>
    <asp:Repeater ID="RPlistProjectTask" runat="server" EnableViewState="false">
        <HeaderTemplate>
            <table id="tableMap" border="1" width="880px" cellspacing="0" >
            
                <tr class="ROW_header_Small_Center">
                    <td runat="server" id="TDd">
                        <asp:Label ID="LBdeleteTitle" runat="server" Text="*D"></asp:Label>
                    </td>
                    <td runat="server" id="TDm">
                        <asp:Label ID="LBmodifyTitle" runat="server" Text="*M"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBtaskNameTitle" runat="server" Text="*"></asp:Label>
                    </td>
                    <td class="hide_startDate">
                        <asp:Label ID="LBstartDateTitle" runat="server"></asp:Label>
                    </td>
                    <td class="hide_endDate">
                        <asp:Label ID="LBendDateTitle" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBdeadlineTitle" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBcompletenessTitle" runat="server"></asp:Label>
                    </td>
                    <td id="TDstatusTitle" style="min-width:20">
                        <asp:Label ID="LBstatusTitle" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="LBpriorityTitle" runat="server"></asp:Label>
                    </td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr  title="WBS level: <%#(Container.DataItem).Level %>" class="<%# me.DeletedClass((Container.DataItem).isDeleted,false) %>">
                <td runat="server" id="TDd" align="center"  >
                    <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                        CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="delete"></asp:LinkButton>
                </td>
                <td runat="server" id="TDm" align="center">                
                    <asp:HyperLink ID="HYPmodify" Text="*mod" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                </td>
                <td align="left"  >
                    <asp:Literal ID="LTspace" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server"></asp:Label>
                    <asp:HyperLink ID="HYPtaskName" runat="server" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                    <asp:Label ID="LBtaskName" runat="server" Text="taskNAme"></asp:Label>
                </td>
                <td class="hide_startDate">
                    <asp:Label ID="LBstartDate" runat="server"></asp:Label>
                </td>
                <td class="hide_endDate">
                    <asp:Label ID="LBendDate" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LBdeadline" runat="server"></asp:Label>
                </td>
                <td>
                    <div style="width: 165px;">
                        <div style="float: left; width: 50px; text-align: left; padding: 5px 0px 5px 5px;">
                            <asp:Label ID="LBcompleteness" runat="server"></asp:Label>
                        </div>
                        <div style="padding: 5px 0px 10px 10px;">
                            <div style="float: left; width: 100px; text-align: left; width: 100px; height:15px ; border: 1px solid black">
                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                            </div>
                        </div>
                    </div>
                </td>
                <td align="center">
                    <asp:Image runat="server" ID="IMstatus"></asp:Image>
                </td>
                <td align="center">
                    <asp:Image runat="server" ID="IMpriority"></asp:Image>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
              <tr title="WBS level: <%#(Container.DataItem).Level %>" class="<%# me.DeletedClass((Container.DataItem).isDeleted,false) %>">
                <td runat="server" id="TDd" align="center"  >
                    <asp:LinkButton ID="LNBvirtualDelete" runat="server" CommandName="virtualdelete"
                        CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"></asp:LinkButton>
                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="delete"></asp:LinkButton>
                </td>
                <td runat="server" id="TDm" align="center">                
                    <asp:HyperLink ID="HYPmodify" Text="*mod" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                </td>
                <td align="left"  >
                    <asp:Literal ID="LTspace" runat="server"></asp:Literal>
                    <asp:Label ID="LBwbs" runat="server"></asp:Label>
                    <asp:HyperLink ID="HYPtaskName" runat="server" CssClass="ROW_ItemLink_Small"></asp:HyperLink>
                    <asp:Label ID="LBtaskName" runat="server" Text="taskNAme"></asp:Label>
                </td>
                <td class="hide_startDate">
                    <asp:Label ID="LBstartDate" runat="server"></asp:Label>
                </td>
                <td class="hide_endDate">
                    <asp:Label ID="LBendDate" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LBdeadline" runat="server"></asp:Label>
                </td>
                <td>
                    <div style="width: 165px;">
                        <div style="float: left; width: 50px; text-align: left; padding: 5px 0px 5px 5px;">
                            <asp:Label ID="LBcompleteness" runat="server"></asp:Label>
                        </div>
                        <div style="padding: 5px 0px 10px 10px;">
                            <div style="float: left; width: 100px; text-align: left; width: 100px; height:15px ; border: 1px solid black">
                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                            </div>
                        </div>
                    </div>
                </td>
                <td align="center">
                    <asp:Image runat="server" ID="IMstatus"></asp:Image>
                </td>
                <td align="center">
                    <asp:Image runat="server" ID="IMpriority"></asp:Image>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<asp:Literal ID="LTscriptPopUp" runat="server" EnableViewState="false"></asp:Literal>