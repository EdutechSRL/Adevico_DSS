<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="InvolvingProjects.aspx.vb" Inherits="Comunita_OnLine.InvolvingProjects" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="../UC/UC_PagerControl.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="TSKContent1" ContentPlaceHolderID="CPHservice" runat="server">
    <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
        align="center">
        <div id="FilterZone">
             <div id="RadTabDiv">
                <telerik:RadTabStrip ID="TBStasklist" runat="server" SelectedIndex="1" Align="Justify"
                    Width="100%" Height="20px" CausesValidation="False" AutoPostBack="True" 
                    Skin="Outlook">
                    <Tabs>
                        <telerik:RadTab Text="**Today's Tasks List" Value="0">
                        </telerik:RadTab>
                        <telerik:RadTab Text="**Involving Projects" Value="1">
                        </telerik:RadTab>
                        <telerik:RadTab Text="**Management Tasks" Value="2">
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
                    <asp:Literal ID="LTshowActive" runat="server">**Order:</asp:Literal>
                        <asp:RadioButtonList ID="RBLselectPrj" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                            AutoPostBack="true">
                            <asp:ListItem Text="**Active" Value="3"></asp:ListItem>
                            <asp:ListItem Text="**Completed" Value="4"></asp:ListItem>
                            <asp:ListItem Text="**Next" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>                     
            </div>
                <div style="float: right; width:14%;  text-align: right ; margin-top : 2px" >
                    <asp:HyperLink ID="HYPaddProject" runat="server" Text="**New Project" CssClass="Link_Menu"></asp:HyperLink>
                </div>
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
                        <asp:Label ID="LBlegendaVD2" runat="server" Text="*Task non disponibile"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 900px; text-align: center; margin: 0,auto; padding-top: 5px; clear: both;"
            align="center">
            <asp:Repeater ID="RPTinvolvingProjects" runat="server">
                <ItemTemplate>
                    <h3>
                        <b>
                            <asp:Literal ID="LTheader" runat="server" EnableViewState="false" Text='<%#Container.Dataitem.CommunityName %>' /></b></h3>
                    <asp:Repeater ID="RPTsingleProjects" runat="server" SkinID="griglia900" OnItemCommand="RPTsingleProjects_ItemCommand">
                        <HeaderTemplate>
                            <table border="1" align="center" width="900px" cellspacing="0">
                                <tr class="ROW_header_Small_Center">
                                    <td align="center" width="40px">
                                        <b>
                                            <asp:Literal ID="LTheaderStatus" Text="S*" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center" width="60px">
                                        <b>
                                            <asp:Literal ID="LTheaderDelete" Text="E*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="40px">
                                        <asp:Literal ID="LTheaderModify" Text="M*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="500px">
                                        <b>
                                            <asp:Literal ID="LTheaderProject" Text="PROJECT*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="150px">
                                        <b>
                                            <asp:Literal ID="LTheaderDeadline" Text="DEADLINE*" runat="server"></asp:Literal></b>
                                    </td>
                                    <td align="center" width="130px">
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
                                <td align="center">
                                    <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                        Visible="false"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="delete"></asp:LinkButton>
                                    &nbsp
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="left">
                                
                                    <div id="DIVproject" runat="server" style=" padding-bottom: 5px; padding-top: 3px; padding-left: 2px;">
                                        
                                        <div id="Container1" style="float: left; width:100%;" >
                                            
                                            <div id="DivHypTitle" style=" float:right;display:inline; width: 87%;">
                                                <asp:HyperLink ID="HYPproject" Text="" NavigateUrl="" runat="server" class="ROW_ItemLink_Small_Bold"></asp:HyperLink><br>
                                            </div>  
                                            
                                            <div id="DivLblTitle" style="width:45px">
                                                <b>
                                                    <asp:Label ID="LBLtitle" runat="server" Text="" EnableViewState="false" 
                                                        Style="padding-bottom: 2px; padding-top: 2px;"></asp:Label>&nbsp;
                                                </b>
                                           </div>                                     
                                           
                                          
                                        <div id="Container2" style="float: left; display:inline; width:100%; text-align:left; padding-bottom: 3px; padding-top: 2px;" >
                                        
                                             <div id="DivLblRoles" style="  float:left; width: 45px;">
                                                    <b>
                                                        <asp:Label ID="LBLroles" runat="server" Text="" EnableViewState="false" CssClass="Titolo_campoSmall" style="padding-bottom: 2px; padding-top: 2px;"></asp:Label>
                                                    </b>
                                               </div> 
                                        
                                        <div id="DivLTRroles" style="width:87%; float:right; display:inline;  ">
                                                
                                                    <asp:Literal ID="LTroles" runat="server" Text="" EnableViewState="false"  ></asp:Literal>
                                        </div>                                                                         
                                                                                  
                                        </div>                                       
                                                                           
                                       </div>     
                                    </div>
                                </td>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td align="center">
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 55px; text-align: left; padding: 0px 0px 0px 5px">
                                            <asp:Literal ID="LTcompleteness" runat="server" EnableViewState="false" Text=""></asp:Literal>
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
                                <td align="center">
                                    <asp:LinkButton ID="LNBelimina" runat="server" CommandName="virtualdelete" CausesValidation="false"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBundelete" runat="server" CommandName="undelete" CausesValidation="false"
                                        Visible="false"></asp:LinkButton>
                                    <asp:LinkButton ID="LNBdelete" runat="server" CausesValidation="false" CommandName="delete"></asp:LinkButton>
                                     &nbsp
                                </td>
                                <td align="center">
                                    <asp:HyperLink ID="HYPmodifica" Text="" NavigateUrl="" ImageUrl="" runat="server"></asp:HyperLink>
                                </td>
                                <td align="left">
                                
                                        <div id="DIVproject" runat="server" style=" padding-bottom: 5px; padding-top: 3px; padding-left: 2px;">
                                        
                                        <div id="Container1alt" style="float: left; width:100%;" >
                                            
                                            <div id="DivHypTitle" style=" float:right;display:inline; width: 87%;">
                                                <asp:HyperLink ID="HYPproject" Text="" NavigateUrl="" runat="server" class="ROW_ItemLink_Small_Bold"></asp:HyperLink><br>
                                            </div>  
                                            
                                            <div id="DivLblTitle" style="width:45px">
                                                <b>
                                                    <asp:Label ID="LBLtitle" runat="server" Text="" EnableViewState="false" 
                                                        Style="padding-bottom: 2px; padding-top: 2px;"></asp:Label>&nbsp;
                                                </b>
                                            </div>
                                        </div>
                                           
                                          
                                        <div id="Container2alt" style="float: left; display:inline; width:100%; text-align:left; padding-bottom: 4px; padding-top: 2px; padding-left: 2px; " >
                                        
                                             <div id="DivLblRoles" style="  float:left; width: 45px;">
                                                    <b>
                                                        <asp:Label ID="LBLroles" runat="server" Text="" EnableViewState="false" CssClass="Titolo_campoSmall" style="padding-bottom: 2px; padding-top: 2px;"></asp:Label>
                                                    </b>
                                               </div> 
                                        
                                            <div id="DivLTRroles" style="width:87%; float:right; display:inline;  ">
                                                    
                                                    <asp:Literal ID="LTroles" runat="server" Text="" EnableViewState="false"></asp:Literal>                                                        
                                            </div>                                                                                    
                                                                                  
                                        </div>                                                                     
                                            
                                    </div>
                                
                                 <%--   <div id="DIVproject" runat="server" style="text-align: left; padding-bottom: 5px;
                                        padding-top: 3px;">
                                        <div style="float: left; width: 55px; padding-left: 4px; height: 19px;">
                                            <div style="padding-bottom: 2px; padding-top: 1px;">
                                                <b>
                                                    <asp:Label ID="LBLtitle" runat="server" Text="" EnableViewState="false" CssClass="Titolo_campoSmall"
                                                        Style="padding-bottom: 2px; padding-top: 2px;"></asp:Label>&nbsp;</b></div>
                                            <div style="padding-bottom: 2px; padding-top: 1px;">
                                                <b>
                                                    <asp:Label ID="LBLroles" runat="server" Text="" EnableViewState="false" CssClass="Titolo_campoSmall"></asp:Label></b></div>
                                        </div>
                                        <div style="float: left; padding-bottom: 2px; padding-top: 1px;" class="Testo_campoSmall">
                                            <div style="padding-bottom: 2px; padding-top: 1px; text-align: left">
                                                <asp:HyperLink ID="HYPproject" Text="" NavigateUrl="" runat="server" class="ROW_ItemLink_Small_Bold"></asp:HyperLink><br>
                                            </div>
                                            <div style="padding-bottom: 2px; padding-top: 1px;">
                                                <asp:Literal ID="LTroles" runat="server" Text="" EnableViewState="false"></asp:Literal><br>
                                            </div>
                                        </div>
                                    </div> --%>
                                </td>
                                <td align="center">
                                    <asp:Literal ID="LTdeadline" EnableViewState="false" runat="server" Text=""></asp:Literal>
                                </td>
                                <td align="center">
                                    <div style="width: 165px;">
                                        <div style="float: left; width: 55px; text-align: left; padding: 0px 0px 0px 5px">
                                            <asp:Literal ID="LTcompleteness" runat="server" EnableViewState="false" Text=""></asp:Literal>
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
        <asp:Literal ID="ltlscript" runat="server" EnableViewState="false"></asp:Literal>
    </div>
</asp:Content>
