<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ActivityEditor.ascx.vb" Inherits="Comunita_OnLine.UC_ActivityEditor" %>
<%@ Register TagPrefix="CTRL" TagName="SelectResources" Src="~/Modules/ProjectManagement/UC/UC_SelectResources.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="input" Src="~/Modules/Common/UC/UC_InLineInput.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
<asp:MultiView ID="MLVsettings" runat="server">
    <asp:View ID="VIWsettings" runat="server">
        <input type="hidden" id="HDNselectedTab" class="selectedtab" runat="server" />
        <div class="tabs">
            <ul>
                <li><a href="#editDetails"><asp:Literal ID="LTeditSettings" runat="server"></asp:Literal></a></li>
                <li runat="server" id="LIeditLinks"><a href="#editLinks"><asp:Literal ID="LTeditLinks" runat="server"></asp:Literal></a></li>
                <li runat="server" id="LIeditResources"><a href="#editResources"><asp:Literal ID="LTeditResources" runat="server"></asp:Literal></a></li>
                <li runat="server" id="LIcompletion"><a href="#editCompletion"><asp:Literal ID="LTeditCompletion" runat="server"></asp:Literal></a></li>
            </ul>
            <div id="editDetails">
                <div class="fieldobject projectdetails clearfix">
                    <div class="fieldrow projectstart left">
                        <asp:Label ID="LBactivityStartDate_t" runat="server" AssociatedControlID="CTRLstartDateInput">*Start date:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLstartDateInput" ContainerCssClass="editable" EditCssClass="datepicker" InputMaxChar="10" DataType="Date"></CTRL:input>
                    </div>
                    <div class="fieldrow projectend right">
                        <asp:Label ID="LBactivityEndDate_t" runat="server" AssociatedControlID="CTRLendDateInput">*End date:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLendDateInput" ContainerCssClass="editable disabled" EditCssClass="datepicker" ReadOnlyInput="true" InputMaxChar="10" DataType="Date" ValidationEnabled="false"></CTRL:input>
                    </div>
                </div>
                <div class="fieldobject details">
                    <div class="fieldrow taskname">
                        <asp:Label ID="LBactivityName_t" runat="server" AssociatedControlID="TXBname" CssClass="fieldlabel">*Name:</asp:Label>
                        <asp:TextBox ID="TXBname" runat="server" CssClass="fieldinput"></asp:TextBox>
                    </div>
                    <div class="fieldrow taskduration">
                        <asp:Label ID="LBactivityDuration_t" runat="server" AssociatedControlID="CTRLdurationInput" CssClass="fieldlabel">*Duration:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLdurationInput" ContainerCssClass="editable" InputMaxChar="5"  ValidationEnabled="true"></CTRL:input>
                        <asp:Label ID="LBactivityDuration" runat="server" Visible="false" CssClass="fieldinput"></asp:Label>
                    </div>
                    <div class="fieldrow taskdeadline">
                        <asp:Label ID="LBactivityDeadLine_t" runat="server" AssociatedControlID="CTRLdeadlineInput" CssClass="fieldlabel">*Deadline:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLdeadlineInput" ContainerCssClass="editable" EditCssClass="datepicker" InputMaxChar="10" DataType="Date" ValidationEnabled="false"></CTRL:input>
                    </div>
                    <div class="fieldrow taskdescription">
                        <asp:Label ID="LBactivityDescription_t" runat="server" AssociatedControlID="TXBdescription" CssClass="fieldlabel">*Description:</asp:Label>
                        <asp:TextBox ID="TXBdescription" runat="server" CssClass="textarea big" TextMode="MultiLine" Columns="30" Rows="10"></asp:TextBox>
                    </div>
                    <div class="fieldrow tasknote">
                        <asp:Label ID="LBactivityNote_t" runat="server" AssociatedControlID="TXBnote" CssClass="fieldlabel">*Note:</asp:Label>
                        <asp:TextBox ID="TXBnote" runat="server" CssClass="textarea big" TextMode="MultiLine" Columns="30" Rows="10"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="editLinks" >
                <div class="fieldobject links" runat="server" id="DVeditLinks">
                    <div class="fieldrow newlink" id="DVnewLink" runat="server" visible="false">
                        <asp:Label ID="LBactivityNewPredecessor_t" runat="server" AssociatedControlID="SLBavailablePredecessors" cssclass="fieldlabel">*New Link:</asp:Label>
                        <select runat="server" id="SLBavailablePredecessors" class="tasks chzn-select" multiple  tabindex="2">            
                        </select>
                        <asp:LinkButton ID="LNBaddPredecessorToActivity" runat="server" CssClass="linkMenu">*Add</asp:LinkButton>
                    </div>
                    <div class="fieldrow links">
                        <asp:Label ID="LBactivityPredecessor_t" runat="server" AssociatedControlID="SLBavailablePredecessors" cssclass="fieldlabel">*Predecessors:</asp:Label>
                        <div class="inlinewrapper">
                            <table class="table minimal tasklinks fullwidth">
                                <thead>
                                    <tr>
                                        <th class="id">
                                            <asp:Literal ID="LTlinkIdActivity_t" runat="server">ID</asp:Literal>
                                        </th>
                                        <th class="name">
                                            <asp:Literal ID="LTlinkActivityName_t" runat="server">Name</asp:Literal>
                                        </th>
                                        <th class="link">
                                            <asp:Literal ID="LTlinkType_t" runat="server">Link Type</asp:Literal>
                                          </th>
                                        <th class="leadlag">
                                            <asp:Literal ID="LTlinkLeadlag" runat="server">Lead / Lag</asp:Literal>
                                        </th>
                                        <th class="actions" id="THlinkActions" runat="server" visible="false">
                                            <span class="icons"><span class="icon actions"></span></span>
                                        </th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="RPTpredecessors" runat="server">
                                        <ItemTemplate>
                                             <tr class="tasklink">
                                                <td class="id">
                                                    <asp:Literal ID="LTidLink" runat="server" Text="<%#Container.DataItem.Id %>" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTuniqueId" runat="server" Text="<%#Container.DataItem.UniqueId %>" Visible="false"></asp:Literal>
                                                     <%#Container.DataItem.RowNumber %>
                                                </td>
                                                <td class="name resizablecol">
                                                    <%#Container.DataItem.Name %>
                                                </td>
                                                <td class="link">
                                                    <asp:DropDownList ID="DDLlinkType" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="leadlag">
                                                    <asp:TextBox id="TXBleadlag" runat="server" Text="<%#Container.DataItem.Leadlag %>"></asp:TextBox>
                                                </td>
                                                <td class="actions" id="TDlinkActions" runat="server" visible="false">
                                                    <span class="icons"><asp:LinkButton ID="LNBvirtualDeletePredecessor" runat="server" CssClass="icon delete needconfirm" Visible="false" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.UniqueId %>"></asp:LinkButton></span>
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
            <div id="editResources">
                <div class="fieldobject resources" runat="server" id="DVeditResources">
                    <div class="fieldrow resources">
                        <CTRL:SelectResources id="CTRLselectActivityResources" runat="server" ></CTRL:SelectResources>
                    </div>
                </div>
            </div>
            <div id="editCompletion">
                <div class="fieldobject projectdetails clearfix">
                    <div class="fieldrow projectstart left">
                        <asp:Label ID="LBactivityCompletionStartDate_t" runat="server" AssociatedControlID="CTRLcompletionStartDate">*Start date:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLcompletionStartDate" ContainerCssClass="editable disabled" ReadOnlyInput="true" ValidationEnabled="false"></CTRL:input>
                    </div>
                    <div class="fieldrow projectend right">
                        <asp:Label ID="LBactivityCompletionEndDate_t" runat="server" AssociatedControlID="CTRLcompletionEndDate">*End date:</asp:Label>
                        <CTRL:input runat="server" ID="CTRLcompletionEndDate" ContainerCssClass="editable disabled" ReadOnlyInput="true" ValidationEnabled="false"></CTRL:input>
                    </div>
                </div>
                <div class="fieldobject">
                    <div class="fieldrow taskcompletion">
                        <asp:Label ID="LBactivityCompletion_t" runat="server" AssociatedControlID="LBactivityCompletion">*Completion:</asp:Label>
                        <asp:Label ID="LBactivityCompletion" runat="server" CssClass="fieldinput"></asp:Label><asp:TextBox ID="TXBcompletion" runat="server" Visible="false" CssClass="fieldinput"></asp:TextBox><asp:Label Visible="false" ID="LBcompletionPercentage" runat="server" CssClass="fieldinput">%</asp:Label>
                        <asp:RequiredFieldValidator ID="RFVcompletion" runat="server"  Visible="false" SetFocusOnError="true" ControlToValidate="TXBcompletion" EnableClientScript="true"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RNVcompletion" runat="server" Visible="false" SetFocusOnError="true" ControlToValidate="TXBcompletion" MinimumValue="0" MaximumValue="100" Type="Integer" EnableClientScript="true" ></asp:RangeValidator>
                        <asp:LinkButton ID="LNBconfirmActivityCompletion" runat="server" CssClass="linkMenu" CausesValidation="true" Visible="false"></asp:LinkButton>
                    </div>
                    <div class="fieldrow taskstatus">
                        <asp:Label ID="LBactivityCompletionStatus_t" runat="server" CssClass="fieldlabel" AssociatedControlID="DDLstatus">*Status:</asp:Label>
                        <asp:DropDownList ID="DDLstatus" runat="server"></asp:DropDownList>
                    </div>
                    <div class="fieldrow taskresources" id="DVassignments" runat="server">
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
                                             <tr  id="<%#Container.DataItem.IdResource %>" class="resourcecompletion <%#GetRowCssClass(Container.DataItem.Deleted) %>">
                                                <td class="resource">
                                                    <%#Container.DataItem.DisplayName %>
                                                    <asp:Literal ID="LTidAssignment" runat="server" Text="<%#Container.DataItem.Id %>" Visible="false"></asp:Literal>
                                                    <asp:Literal ID="LTuniqueId" runat="server" Text="<%#Container.DataItem.UniqueId %>" Visible="false"></asp:Literal>
                                                </td>
                                                <td class="completion">
                                                    <asp:Label ID="LBcompletion" runat="server" text='<%#Container.DataItem.Completeness %>' CssClass="fieldinput"></asp:Label>
                                                    <asp:TextBox ID="TXBcompletion" runat="server" CssClass="fieldinput" text='<%#Container.DataItem.Completeness %>'></asp:TextBox>%
                                                    <asp:RequiredFieldValidator ID="RFVcompletion" runat="server" SetFocusOnError="true" ControlToValidate="TXBcompletion" EnableClientScript="true"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="RNVcompletion" runat="server" SetFocusOnError="true" ControlToValidate="TXBcompletion" MinimumValue="0" MaximumValue="100" Type="Integer" EnableClientScript="true"></asp:RangeValidator>
                                                </td>
                                                <td class="actions" id="TDcompletionAction" runat="server" visible="false">
                                                    <span class="icons"><asp:LinkButton ID="LNBvirtualDeleteAssignment" runat="server" CssClass="icon delete needconfirm" Visible="false" CommandName="virtualdelete" CommandArgument="<%#Container.DataItem.UniqueId %>"></asp:LinkButton></span>
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
    <asp:View ID="VIWunknownActivity" runat="server">
        <h3><asp:Literal id="LTmessage" runat="server"></asp:Literal></h3>
    </asp:View>
</asp:MultiView>

<div class="fieldrow buttons right" id="DVcommands" runat="server">
    <div class="fieldrow buttons left">
        <asp:Button ID="BTNdeleteCurrentActivity" runat="server" CssClass="linkMenu delete needconfirm" Visible="false"  />
    </div>
    <div class="fieldrow buttons right">
        <asp:Button ID="BTNsaveCurrentActivity" runat="server" CssClass="linkMenu" Visible="false" />
        <asp:LinkButton ID="LNBcloseCurrentActivity" runat="server" CssClass="linkMenu" CausesValidation="false" />
    </div>
 </div>
<asp:Literal ID="LTdurationEstimatedRegex" runat="server" Visible="false">^(\d+)(\?)?$</asp:Literal>
<asp:Literal ID="LTdurationRegex" runat="server" Visible="false">\d</asp:Literal>