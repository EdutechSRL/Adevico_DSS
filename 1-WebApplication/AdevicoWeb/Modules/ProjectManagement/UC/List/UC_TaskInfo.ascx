<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TaskInfo.ascx.vb"
    Inherits="Comunita_OnLine.UC_TaskInfo" %>

<div class="dlgtask <%=GetContainerCssClass()%> hiddendialog" title="<%=GetDialogTitle() %>">
<asp:MultiView ID="MLVtaskInfo" runat="server">
    <asp:View ID="VIWsettings" runat="server">     
            <div class="tabs">
                <ul>
                    <li runat="server" id="LItaskGeneralSettings"><a href="#taskGeneral"><asp:Literal ID="LTtaskGeneralSettings" runat="server">*General</asp:Literal></a></li>
                    <li runat="server" id="LItaskAttachments" visible="false"><a href="#taskAttachments"><asp:Literal ID="LTtaskAttachments" runat="server">*Attachments</asp:Literal></a></li>
                </ul>
                <div id="taskGeneral">
                    <div class="fieldobject projectdetails clearfix">
                        <div class="fieldrow projectstart left">
                            <asp:Label ID="LBactivityStartDate_t" runat="server" AssociatedControlID="LBactivityStartDate">*Start date:</asp:Label>
                            <div class="inlinewrapper">
                                <span class="editable viewmode disabled"><asp:Label ID="LBactivityStartDate" runat="server" CssClass="view"></asp:Label></span>
                            </div>
                        </div>
                        <div class="fieldrow projectend right">
                            <asp:Label ID="LBactivityEndDate_t" runat="server" AssociatedControlID="LBactivityEndDate">*End date:</asp:Label>
                            <div class="inlinewrapper">
                                <span class="editable viewmode disabled"><asp:Label ID="LBactivityEndDate" runat="server" CssClass="view"></asp:Label></span>
                            </div>
                        </div>
                    </div>
                    <div class="fieldobject">
                        <div class="fieldrow taskname">
                            <asp:Label ID="LBactivityName_t" runat="server" AssociatedControlID="LBactivityName" CssClass="fieldlabel">*Name:</asp:Label>
                            <asp:Label ID="LBactivityName" runat="server"></asp:Label>
                        </div>
                        <div class="fieldrow taskduration">
                            <asp:Label ID="LBactivityDuration_t" runat="server" AssociatedControlID="LBactivityDuration" CssClass="fieldlabel">*Duration:</asp:Label>
                            <asp:Label ID="LBactivityDuration" runat="server" ></asp:Label>
                        </div>
                        <div class="fieldrow taskdeadline" id="DVdeadline" runat="server">
                            <asp:Label ID="LBactivityDeadLine_t" runat="server" AssociatedControlID="LBactivityDeadLine" CssClass="fieldlabel">*Deadline:</asp:Label>
                            <asp:Label ID="LBactivityDeadLine" runat="server" ></asp:Label>
                        </div>
                        <div class="fieldrow taskdescription" id="DVdescription" runat="server">
                            <asp:Label ID="LBactivityDescription_t" runat="server" AssociatedControlID="LBactivityDescription" CssClass="fieldlabel">*Description:</asp:Label>
                            <div class="inlinewrapper">
                                <div class="description">
                                    <asp:Label ID="LBactivityDescription" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="fieldrow tasknote" id="DVnote" runat="server">
                            <asp:Label ID="LBactivityNote_t" runat="server" AssociatedControlID="LBactivityNote" CssClass="fieldlabel">*Note:</asp:Label>
                            <div class="inlinewrapper">
                                <div class="description">
                                    <asp:Label ID="LBactivityNote" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="fieldrow taskcompletion" id="DVmanagerComplation" runat="server">
                            <asp:Label ID="LBactivityCompletion_t" runat="server" AssociatedControlID="LBactivityCompletion" CssClass="fieldlabel">*Completion:</asp:Label>
                            <asp:Label ID="LBactivityCompletion" runat="server" CssClass="fieldinput"></asp:Label>
                        </div>
                        <div class="fieldrow taskstatus" id="DVstatus" runat="server" visible="false">
                            <asp:Label ID="LBactivityCompletionStatus_t" runat="server" CssClass="fieldlabel" AssociatedControlID="LBactivityCompletionStatus">*Status:</asp:Label>
                            <asp:Label ID="LBactivityCompletionStatus" runat="server"></asp:Label>
                        </div>
                        <div class="fieldrow taskcompletion" id="DVmyCompletion" runat="server" visible="false">
                            <asp:Label ID="LBmyActivityCompletion_t" runat="server" AssociatedControlID="TXBmyCompletion" CssClass="fieldlabel">*Completion:</asp:Label>
                            <asp:Label ID="LBmyActivityCompletion" runat="server" CssClass="fieldinput"></asp:Label><asp:TextBox ID="TXBmyCompletion" runat="server" Visible="false" CssClass="fieldinput" MaxLength="4"></asp:TextBox><asp:Label Visible="false" ID="Label3" runat="server" CssClass="fieldinput">%</asp:Label>
                        </div>
                        <div class="fieldrow" id="DVassignments" runat="server" visible="false">
                        <asp:Label ID="LBactivityCompletionResources_t" runat="server" CssClass="fieldlabel">*Resources:</asp:Label>
                        <div class="inlinewrapper">
                            <table class="table minimal resourcescompletion fullwidth">
                                <thead>
                                    <tr>
                                        <th class="resource">
                                            <asp:Literal ID="LTactivityResourceAssigned_t" runat="server">*Resource</asp:Literal>
                                        </th>
                                        <th class="completion">
                                            <asp:Literal ID="LTactivityResourceCompletion_t" runat="server">*Completion</asp:Literal>
                                        </th>
                                        <th class="actions" id="THcompletionAction" runat="server" visible="false">
                                            <span class="icons"><span class="icon actions"></span></span>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RPTresourcesCompletion" runat="server">
                                        <ItemTemplate>
                                             <tr  id="<%#Container.DataItem.IdResource %>" class="resourcecompletion">
                                                <td class="resource">
                                                    <%#Container.DataItem.DisplayName %>
                                                    <asp:Literal ID="LTidAssignment" runat="server" Text="<%#Container.DataItem.Id %>" Visible="false"></asp:Literal>
                                                </td>
                                                <td class="completion">
                                                    <asp:Label ID="LBcompletion" runat="server" text='<%#Container.DataItem.Completeness %>' CssClass="fieldinput"></asp:Label>
                                                    <asp:TextBox ID="TXBcompletion" runat="server" CssClass="fieldinput" text='<%#Container.DataItem.Completeness %>' MaxLength="4"></asp:TextBox>
                                                    <asp:Literal ID="LToldCompletion" runat="server" Text="<%#Container.DataItem.Completeness %>" Visible="false"></asp:Literal>
                                                </td>
                                                <td class="actions" id="TDcompletionAction" runat="server" visible="false">
                                                    <div class="coveredradio enabled">
                                                        <asp:RadioButtonList ID="RBLapproveUserTaskCompletion" CssClass="wclist" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="True" Text="*Approve"></asp:ListItem>
                                                            <asp:ListItem Value="False" Selected="True" Text="*Not approve"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <span class="btnswitchgroup small"><!--
                                                            --><asp:HyperLink ID="HYPapproveUserTaskCompletion" runat="server" CssClass="btnswitch first">*Approve</asp:HyperLink><!--
                                                            --><asp:HyperLink ID="HYPnotApproveUserTaskCompletion" runat="server" CssClass="btnswitch last">*Not approve</asp:HyperLink><!--
                                                    --></span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            </div>
    </asp:View>
    <asp:View ID="VIWmessage" runat="server">
        <div class="fieldobject">
            <div class="fieldrow">
                <h3><asp:Literal id="LTmessage" runat="server"></asp:Literal></h3>
            </div>
        </div>
    </asp:View>
</asp:MultiView>
    <div class="fieldobject">
        <div class="fieldrow buttons right">
            <asp:Button ID="BTNconfirmTaskCompletion" runat="server" CommandName="confirmtaskcompletion" Visible="false" />
            <asp:Button ID="BTNsaveTaskDialogSettings" runat="server" CommandName="savesettings" />
            <asp:HyperLink ID="HYPcloseTaskDialog" runat="server" CssClass="linkMenu close" Visible="false"></asp:HyperLink>
        </div>
    </div>
</div>
<asp:Literal ID="LTcssClassManager" runat="server" Visible="false">asmanager</asp:Literal>
<asp:Literal ID="LTcssClassResource" runat="server" Visible="false">asresource</asp:Literal>