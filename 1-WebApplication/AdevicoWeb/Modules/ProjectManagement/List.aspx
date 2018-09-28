<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="List.aspx.vb" Inherits="Comunita_OnLine.Projects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
     <asp:MultiView ID="MLVsettings" runat="server" ActiveViewIndex="1">
            <asp:View ID="VIWempty" runat="server">
                <br /><br /><br /><br />
                <asp:Label ID="LBemptyMessage" runat="server"></asp:Label>
                <br /><br /><br /><br />
            </asp:View>
            <asp:View ID="VIWsettings" runat="server">

                <div class="DivEpButton">
                    <div id="DVprojectsTop" class="ddbuttonlist enabled" runat="server" Visible="false"><!--
                    --><asp:LinkButton id="LBNnewProject" CssClass="linkMenu projectresources ddbutton active" runat="server" Text="*New Project" CausesValidation="false" ></asp:LinkButton><!--
                    --><asp:LinkButton id="LBNpublicProject" CssClass="linkMenu ddbutton customresource" runat="server" Text="*New Public Project" CausesValidation="false" ></asp:LinkButton><!--
                    --><asp:LinkButton id="LBNpersonalProject" CssClass="linkMenu ddbutton" runat="server" Text="*New Personal Project" CausesValidation="false" ></asp:LinkButton><!--
                 --></div>

                    <asp:HyperLink ID="HYPdashboard" runat="server" CssClass="linkMenu" Text="*Dashboard"></asp:HyperLink>
                    <asp:HyperLink ID="HYPback" runat="server" CssClass="linkMenu" Text="*Back"></asp:HyperLink>
                </div>

                <div class="fieldobject summary mysummary">
                    <div class="fieldrow summaryheader clearfix">
                        <div class="divsummarytitle">
                            <div class="left">
                                <asp:Label runat="server" id="LBsummary" Font-Size="18px">*My summary</asp:Label> 
                                <asp:Label runat="server" id="LBusername" Font-Size="16px">*User name</asp:Label> 
                            </div>
                            <div class="right">
                                <div class="summaryselector">
                                   <span class="btnswitchgroup small"><!--
                                     --><a class="btnswitch first active" href="">Week</a><!--
                                     --><a class="btnswitch" href="">Month</a><!--
                                     --><a class="btnswitch last" href="l">Year</a><!--
                                     --></span>
                                 </div>
                            </div>

                        </div>
                    </div>
                    <div class="fieldrow">
                        <div class="divsummary">
                            <div class="divsummaryinner">
                                <asp:Repeater ID="RPTsummary" runat="server">
                                    <ItemTemplate>
                                        <span class="summaryitem <%#GetItemCssClass(Container.DataItem.DisplayAs)%>">
                                            <asp:LinkButton ID="LNBitem" runat="server" ></asp:LinkButton>
                                        </span>
                                    </ItemTemplate>  
                                </asp:Repeater>
                                <asp:Literal ID="LTdefaultSummaryItem" runat="server" Visible="false">
                                    <span class="quantity">{0}</span>
                                    <span class="text">{1}</span>
                                    <span class="timeframe">{2}</span>
                                </asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="fieldobject faketelerik">
                    <div class="fieldrow">
                        <telerik:RadTabStrip ID="TBStasklist"  runat="server" Align="Justify" Width="100%" Height="20px" CausesValidation="false" AutoPostBack="true" Skin="Outlook" EnableEmbeddedSkins="true" SelectedIndex="0">
                            <Tabs>
                                <telerik:RadTab Text="**Resource" Value="0" >
                                </telerik:RadTab>
                                <telerik:RadTab Text="**Manager" Value="1" >
                                </telerik:RadTab>
                                <telerik:RadTab Text="**Admin" Value="2" >
                                </telerik:RadTab>                                
                            </Tabs>
                        </telerik:RadTabStrip>
                    </div>
                </div>
                
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label class="fieldlabel" runat="server" AssociatedControlID="DDLcommunity">Filter by:<//asp:Label>
                        <asp:DropDownList ID="DDLcommunityFilter" runat="server" AutoPostBack="true">
                            <asp:ListItem>*All communities</asp:ListItem>
                            <asp:ListItem>*Current community</asp:ListItem>
                            <asp:ListItem>*All Personal community</asp:ListItem>
                            <asp:ListItem>*current Personal community</asp:ListItem>
                            <asp:ListItem>*Portal</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="fieldrow">
                        <asp:Label CssClass="fieldlabel" AssociatedControlID="LBcommnuity" runat="server" >*Selected community:</asp:Label>
                        <asp:Label CssClass="communityname" ID="LBcommnuity" runat="server" >*Community name</asp:Label>
                    </div>
                </div>
                
                <div class="fieldobject clearfix">
                    <div class="fieldrow left">
                        <asp:Label ID="LBgroupBy" runat="server">*Group by:</asp:Label>
                        <span class="btnswitchgroup small"><!--
                        --><asp:LinkButton id="LNBcommuity" runat="server" CssClass="btnswitch first active" CausesValidation="false">*Community</asp:LinkButton><!--
                        --><asp:LinkButton id="LNBdeadline" runat="server" CssClass="btnswitch" CausesValidation="false">*Deadline</asp:LinkButton><!--
                        --><asp:LinkButton id="LNBplain" runat="server" CssClass="btnswitch last" CausesValidation="false">*Plain</asp:LinkButton><!--
                    --></span>
                    </div>    

                    <div class="fieldrow right">
                        <asp:Label ID="LBfilterBy" runat="server">*Filter by projects:</asp:Label>
                        <span class="btnswitchgroup small"><!--
                        --><asp:LinkButton id="LNBall" runat="server" CssClass="btnswitch first active" CausesValidation="false">*All</asp:LinkButton><!--
                        --><asp:LinkButton id="LNBactive" runat="server" CssClass="btnswitch" CausesValidation="false">*Active</asp:LinkButton><!--
                        --><asp:LinkButton id="LNBcompleted" runat="server" CssClass="btnswitch" CausesValidation="false">*Completed</asp:LinkButton><!--
                        --><asp:LinkButton id="LNBfuture" runat="server" CssClass="btnswitch last" CausesValidation="false">*Future</asp:LinkButton><!--
                    --></span>
                    </div>
                </div>

                <div class="tablewrapper">
                    <table class="table projects asresource treetable light fullwidth bycommunity">
                        <thead>
                            <tr>
                                <th class="name">
                                     <asp:Literal ID="LTprojectname_t" runat="server">*Long name</asp:Literal>
                                </th>
                                 <th class="status">
                                     <span class="statuslight"><span class="text"><asp:Literal ID="LTcompletionIcon_t" runat="server">Completion</asp:Literal></span></span>
                                </th>
                                 <th class="projectstatus">
                                     <asp:Literal ID="LTprojectStatus_t" runat="server">*Long name</asp:Literal>
                                </th>
                                 <th class="deadline">
                                     <asp:Literal ID="LTdeadline_t" runat="server">*Long name</asp:Literal>
                                </th>
                                <th class="roles">
                                    <asp:Literal ID="LTroles_t" runat="server">*Long name</asp:Literal>
                                </th>
                                <th class="completeness"> 
                                    <asp:Literal ID="LTcompleteness_t" runat="server">*Long name</asp:Literal> 
                                </th>
                                <th class="actions">
                                    <span class="icons">
                                        <span class="icon actions">
                                            <asp:Literal ID="LTaction_t" runat="server">*Actions</asp:Literal>
                                        </span>
                                    </span>
                                </th>
                            </tr>
                        </thead>
                    <asp:Repeater ID="RPTproject" runat="server" Visible="false"> 
                        <HeaderTemplate>
                            <tbody>
                                <tr class="community" id="cmnt-0">
                                    <td class="name" colspan="7">
                                        <asp:HyperLink ID="HYPcommunity" CssClass="text" runat="server" >*Community Name<//asp:HyperLink> 
                                    </td>   
                                </tr>
                                <tr class="continue top child-of-cmnt-0" id="cnt-0">
                                    <td colspan="7">
                                        <asp:HyperLink ID="HYPcontinue" runat="server" Visible="false">*continue from previous page</asp:HyperLink>
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Repeater ID="RPTprojectLine" runat="server">                            
                                <tr class="project child-of-cmnt-0" id="prj-0">
                                    <td class="name">
                                        <asp:HyperLink CssClass="text" runat="server">*Project Name</asp:HyperLink>
                                    </td>
                                    <td class="status">
                                        <span class="statuslight yellow" title="xx%"><span class="text">*xx%</span></span> <%--AGGIUNGERE GIALLO--%>
                                    </td>
                                    <td class="projectstatus">
                                        <asp:Literal ID="LTstatus" runat="server">*status</asp:Literal>
                                    </td>
                                    <td class="deadline">
                                        <asp:Literal ID="LTdeadline" runat="server">*01/01/2015</asp:Literal>
                                    </td>
                                     <td class="roles">
                                               
                                    </td>
                                    <td class="completeness">
                                        <span class="completeness">
                                            <span class="item personal" title="personal">50%</span>
                                            <span class="item total" title="total">50%</span>
                                        </span>
                                    </td>
                                    <td class="actions">
                                        <span class="icons">
                                            <!--<span class="icon delete"></span>-->
                                            <span class="icon view" title="Activities Tree"></span>
                                        </span>
                                    </td>    
                                </tr>
                            </asp:Repeater>
                        </ItemTemplate>

                        <FooterTemplate>
                        </tbody>
                        <tfoot>
                            
                        </tfoot>
                        </FooterTemplate>
                    </asp:Repeater>
                    <%--secondo repeater--%>
                    <asp:Repeater ID="Repeater1" runat="server" Visible="false"> 

                        <HeaderTemplate>
                            <tbody>
                                <tr class="community" id="cmnt-0">
                                    <td class="name" colspan="7">
                                        <asp:HyperLink ID="HyperLink1b" CssClass="text" runat="server" >*Community Name<//asp:HyperLink> 
                                    </td>   
                                </tr>
                                <tr class="continue top child-of-cmnt-0" id="cnt-0">
                                    <td colspan="7">
                                        <asp:HyperLink ID="HyperLink2" runat="server" Visible="false">*continue from previous page</asp:HyperLink>
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Repeater ID="RPTprojectLine" runat="server">                            
                                <tr class="project child-of-cmnt-0" id="prj-0">
                                    <td class="name">
                                        <asp:HyperLink ID="HyperLink3" CssClass="text" runat="server">*Project Name</asp:HyperLink>
                                    </td>
                                    <td class="status">
                                        <span class="statuslight yellow" title="xx%"><span class="text">*xx%</span></span> <%--AGGIUNGERE GIALLO--%>
                                    </td>
                                    <td class="projectstatus">
                                        <asp:Literal ID="LTstatus" runat="server">*status</asp:Literal>
                                    </td>
                                    <td class="deadline">
                                        <asp:Literal ID="LTdeadline" runat="server">*01/01/2015</asp:Literal>
                                    </td>
                                     <td class="roles">
                                               
                                    </td>
                                    <td class="completeness">
                                        <span class="completeness">
                                            <span class="item personal" title="personal">50%</span>
                                            <span class="item total" title="total">50%</span>
                                        </span>
                                    </td>
                                    <td class="actions">
                                        <span class="icons">
                                            <!--<span class="icon delete"></span>-->
                                            <span class="icon view" title="Activities Tree"></span>
                                        </span>
                                    </td>    
                                </tr>
                            </asp:Repeater>
                        </ItemTemplate>

                        <FooterTemplate>
                        </tbody>
                        <tfoot>
                            
                        </tfoot>
                        </FooterTemplate>
                    </asp:Repeater>
                    </table>
                </div>

            </asp:View>
            </asp:MultiView>
</asp:Content>
