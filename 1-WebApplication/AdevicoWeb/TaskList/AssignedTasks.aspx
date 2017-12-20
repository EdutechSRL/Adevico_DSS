<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="AssignedTasks.aspx.vb" Inherits="Comunita_OnLine.AssignedTasks" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="TSKContent1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
        align="center">
        <div style="width:900px">
            <telerik:RadTabStrip ID="TBStasklist"  runat="server" Align="Justify" Width="100%"
                Height="20px" CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true"
                SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="**Today's Tasks List" Value="0" >
                    </telerik:RadTab>
                    <telerik:RadTab Text="**Involving Projects" Value="1" >
                    </telerik:RadTab>
                    <telerik:RadTab Text="**Management Tasks" Value="2" >
                    </telerik:RadTab>
                    <telerik:RadTab Text="*Tasks Administration" Value="3">
                        </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
                <br />
        </div>
        <div>
            <br />
        </div>
        <div id="DivSopra" style="float:left; width: 40%;" > 
                <div id="DDLdiv" style="float: left; display:block; width:100%; text-align: left ;">
                    <asp:Literal ID="LTfilterby" runat="server">**Filter by : </asp:Literal>
                    <asp:DropDownList ID="DDLfilterby" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                </div>
                <div style="float: left; width: 100%; display:block; text-align: left ; clear:both; margin-top :7px" >
                    <asp:Literal ID="LTsortby" runat="server">Sort by** :&nbsp  </asp:Literal>
                    <asp:DropDownList ID="DDLsortBy" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Deadline**" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Alphabetical**" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div> 
        </div>

        <div id="DivSotto" style=" float:left; width:40% ">
            <div id="RBLactiveCompletedDiv" style="float: left; width: 100%; text-align: left; margin-top: 0px" >
                <asp:Literal ID="LTorder" runat="server">**Group by: </asp:Literal>
                <asp:RadioButtonList ID="RBLview" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Text="Community**" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Project**" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
            </div>
        </div>
            
            <div style="float: right; width:14%;  text-align: right ; margin-top : 2px" >
                <asp:HyperLink ID="HYPaddProject" runat="server" Text="**New Project" CssClass="Link_Menu"></asp:HyperLink>
            </div>
        </div>
        <div style="text-align: left; float: left; width: 100%;">
            <br />
            <table cellspacing="0" border="1">
                <tr>
                    <td class="ROW_Disabilitate_Small" style="width: 10px;">
                        &nbsp;
                    </td>
                    <td class="ROW_Page_Small">
                        <asp:Label ID="LBlegendaVD" runat="server" Text="*Task non disponibile"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
            align="center">
            <asp:Repeater ID="RPTassignedTasksByCommunity" runat="server">
                <ItemTemplate>
                    <div>
                        <h3>
                            <b>
                                <asp:Literal ID="LTheader" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.CommunityName %>' /></b></h3>
                    </div>
                    <asp:Repeater ID="RPTassignedTasksComponentCommunity" SkinID="griglia900" runat="server"
                        OnItemCommand="RPTassignedTasksComponent_ItemCommand">
                        <HeaderTemplate>
                            <table border="1" align="center" width="900px" cellspacing="0">
                                <tr class="ROW_header_Small_Center">
                                    <td align="center" width="40px">
                                        <b>
                                            <asp:Literal ID="LTheaderStatus" Text="S*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="60px">
                                        <b>
                                            <asp:Literal ID="LTheaderDelete" Text="E*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="40px">
                                        <b>
                                            <asp:Literal ID="LTheaderModify" Text="M*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="300px">
                                        <b>
                                            <asp:Literal ID="LTheaderTask" Text="TASK*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="300px">
                                        <b>
                                            <asp:Literal ID="LTheaderProject" Text="PROJECT*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="150px">
                                        <b>
                                            <asp:Literal ID="LTheaderDeadline" Text="DEADLINE*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="110px">
                                        <b>
                                            <asp:Literal ID="LTheaderCompleteness" Text="COMPLETENESS*" runat="server"></asp:Literal></b>
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="<%# me.DeletedClass((Container.DataItem).isDeleted,false) %>">
                                <td>
                                    <center>
                                        <asp:Image runat="server" ImageUrl="" ID="IMstartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMsuspendedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMnotStartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMcompletedTask"></asp:Image>                                        
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                            Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                    </center>
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPTaskByCommunity" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPprojectByCommunity" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td >
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 55px; text-align: left;padding-left:5px; vertical-align:middle ">                                           
                                             <asp:Literal ID="LTcompleteness" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                        </div>
                                        <div>
                                            <div style="float: left; width: 100px; text-align: left;  width: 100px; height: 15px;
                                                border: 1px solid black">
                                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="<%# me.DeletedClass((Container.DataItem).isDeleted,true) %>">
                                <td>
                                    <center>
                                        <asp:Image runat="server" ImageUrl="" ID="IMstartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMsuspendedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMnotStartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl="" ID="IMcompletedTask"></asp:Image>                                      
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                            Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPTaskByCommunity" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPprojectByCommunity" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td >
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 55px; text-align: left; padding-left:5px;vertical-align:middle">                                           
                                             <asp:Literal ID="LTcompleteness" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                        </div>
                                        <div >
                                            <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                                                border: 1px solid black">
                                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="RPTassignedTasksByProject" SkinID="griglia900" runat="server">
                <ItemTemplate>
                    <div>
                        <h3>
                            <b>
                                <asp:Literal ID="LTheader" runat="server" EnableViewState="false" Text="" /></b></h3>
                    </div>
                    <asp:Repeater ID="RPTassignedTasksComponentProject" SkinID="griglia900" runat="server"
                        OnItemCommand="RPTassignedTasksComponent_ItemCommand">
                        <HeaderTemplate>
                            <table border="1" align="center" width="900px" cellspacing="0">
                                <tr class="ROW_header_Small_Center">
                                    <td align="center" width="40px">
                                        <asp:Literal ID="LTheaderStatus" Text="S*" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center" width="50px">
                                        <asp:Literal ID="LTheaderDelete" Text="E*" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center" width="40px">
                                        <asp:Literal ID="LTheaderModify" Text="M*" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center" width="300px">
                                        <asp:Literal ID="LTheaderTask" Text="TASK*" runat="server"></asp:Literal>
                                    </td>
                                    <%--<td align ="center" width="310px">
                                        <asp:Literal ID="LTheaderProject" Text="ROLE" runat="server"></asp:Literal>
                                    </td> --%>
                                    <td align="center">
                                        <asp:Literal ID="LTheaderDeadline" Text="DEADLINE*" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="LTheaderCompleteness" Text="COMPLETENESS*" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="<%# me.DeletedClass((Container.DataItem).isDeleted,false) %>">
                                <td>
                                    <center>
                                        <asp:Image runat="server" ImageUrl="" ID="IMstartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMsuspendedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMnotStartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMcompletedTask"></asp:Image>                                        
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                            Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBdelete" runat="server" CommandName="delete" CausesValidation="false"></asp:LinkButton>
                                    </center>
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl=" " runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPtaskByProject" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <%--<td align ="center">
                                         <asp:Literal EnableViewState="false" ID="LTrole" runat="server" Text="" ></asp:Literal>
                                    </td>--%>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td align="center">
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 50px; text-align: left; padding-left:5px;">
                                            <asp:Literal ID="LTcompleteness" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                        </div>
                                        <div >
                                            <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                                                border: 1px solid black">
                                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="<%# me.DeletedClass((Container.DataItem).isDeleted,true) %>">
                                <td>
                                    <center>
                                        <asp:Image runat="server" ImageUrl="" ID="IMstartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMsuspendedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMnotStartedTask"></asp:Image>
                                        <asp:Image runat="server" ImageUrl=" " ID="IMcompletedTask"></asp:Image>                                       
                                    </center>
                                </td>
                                <td>
                                    <center>
                                        <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                            Visible="false"></asp:LinkButton>
                                        <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="delete"></asp:LinkButton>
                                    </center>
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl=" " runat="server"></asp:HyperLink>
                                </td>
                                <td align="center" class="ROW_ItemLink_Small">
                                    <asp:HyperLink ID="HYPtaskByProject" Text="" NavigateUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <%--<td align ="center">
                                         <asp:Literal EnableViewState="false" ID="LTrole" runat="server" Text="" ></asp:Literal>
                                    </td>--%>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td align="center">
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 50px; text-align: left; padding-left:5px">
                                            <asp:Literal ID="LTcompleteness" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                        </div>
                                            <div>
                                            <div style="float: left; width: 100px; text-align: left; width: 100px; height: 15px;
                                                border: 1px solid black">
                                                <asp:Image ID="IMcompleteness" runat="server" BackColor="Green" Height="15px" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </AlternatingItemTemplate> 
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div style="width: 900px; text-align: right; padding-top: 5px; clear: both; height: 22px;">
            <CTRL:GridPager ID="PGgrid" runat="server" EnableQueryString="true"></CTRL:GridPager>
        </div>
    <%--</div>--%>
    <asp:Literal ID="ltlscript" runat="server" EnableViewState="false"></asp:Literal>
</asp:Content>
