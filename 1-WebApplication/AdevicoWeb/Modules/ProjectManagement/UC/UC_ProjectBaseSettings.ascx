<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ProjectBaseSettings.ascx.vb" Inherits="Comunita_OnLine.UC_ProjectBaseSettings" %>
<%@ Register TagPrefix="CTRL" TagName="SelectUsers" Src="~/Modules/Common/UC/UC_SelectUsers.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="SelectFromResources" Src="~/Modules/ProjectManagement/UC/UC_SelectOwnerFromResources.ascx" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<div class="fieldrow fieldlongtext">
    <asp:Label ID="LBprojectName_t" class="fieldlabel" AssociatedControlID="TXBname" runat="server">*Name:</asp:Label>
    <asp:TextBox ID="TXBname" runat="server" CssClass="inputtext"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RFVname" ControlToValidate="TXBname" runat="server" SetFocusOnError="true" ErrorMessage="*"></asp:RequiredFieldValidator>
    <asp:Label ID="LBprojectIsPersonal" AssociatedControlID="TXBname" runat="server" Visible="false"></asp:Label>
</div>
<div class="fieldrow fieldlongtext" id="DVactivitiesToAdd" runat="server" visible="false">
    <asp:Label ID="LBprojectActivitiesToAdd_t" class="fieldlabel" AssociatedControlID="TXBactivitiesToAdd" runat="server">*Add activities:</asp:Label>
    <asp:TextBox ID="TXBactivitiesToAdd" runat="server" Columns="4" MaxLength="100"></asp:TextBox>
    <asp:RangeValidator ID="RNVactivitiesToAdd" runat="server" SetFocusOnError="true" ControlToValidate="TXBactivitiesToAdd" Type="Integer" MinimumValue="0" MaximumValue="500" Display="Dynamic" ErrorMessage="*"></asp:RangeValidator>
    <asp:Label ID="LBprojectActivitiesToAdd" AssociatedControlID="TXBactivitiesToAdd" runat="server"></asp:Label>
</div>
<div class="fieldrow fielddescription">
    <asp:Label ID="LBprojectDescription_t" class="fieldlabel" runat="server" AssociatedControlID="CTRLeditorDescription">*Description:</asp:Label>
    <div class="inlinewrapper clearfix">
        <CTRL:CTRLeditor id="CTRLeditorDescription" runat="server" ContainerCssClass="containerclass"  ModuleCode="SRVTASK"
            LoaderCssClass="loadercssclass inputtext" EditorCssClass="editorcssclass" EditorHeight="300px" AutoInitialize="true"
            EditorWidth="500px"
            MaxTextLength="100000" />
    </div>
</div>
<div class="fieldrow fielddaterange">
    <asp:Label ID="LBstartdate_t" class="fieldlabel" runat="server" AssociatedControlID="RDPstartDate">*Start date:</asp:Label>
    <telerik:RadDatePicker ID="RDPstartDate" runat="server" DateInput-ClientEvents-OnLoad="onLoadStartDate"  >
        <ClientEvents OnDateSelected="onStartDateSelected" />
    </telerik:RadDatePicker>
    <asp:Label ID="LBendDate_t" class="alignr last" runat="server" AssociatedControlID="LBendDate"  Visible="false">*End date:</asp:Label>
    <asp:Label ID="LBendDate" class="inputtext disabled" runat="server" Visible="false"></asp:Label>
</div>
<div class="fieldrow fielddaterange">
    <asp:Label ID="LBdeadline_t" class="fieldlabel" runat="server" AssociatedControlID="RDPdeadline">*Deadline:</asp:Label>
    <telerik:RadDatePicker ID="RDPdeadline" runat="server" DateInput-ClientEvents-OnLoad="onLoadDeadline">
      
    </telerik:RadDatePicker>
     <asp:CustomValidator runat="server" ID="CSVdeadline" ClientValidationFunction="validateDeadline" ErrorMessage="*Deadline must be after start date"
            EnableClientScript="true" ControlToValidate="RDPdeadline" ></asp:CustomValidator>
</div>
<div class="fieldrow fieldduration" id="DVduration" runat="server">
     <asp:Label ID="LBduration_t" class="fieldlabel" runat="server" AssociatedControlID="TXBduration">*Duration:</asp:Label>
     <asp:TextBox ID="TXBduration" CssClass="inputtext" runat="server"></asp:TextBox>
     <asp:Label ID="LBduration" class="inputtext" runat="server" Visible="false"></asp:Label>
</div>
<div class="fieldrow fieldcalendar">
    <asp:Label ID="LBweekcalendar_t" class="fieldlabel" for="" runat="server">*Weekly Calendar:</asp:Label>
    <div class="inlinewrapper">
        <div class="weekcalendar enabled">
            <asp:CheckBoxList ID="CBLweekcalendar" class="wclist" runat="server" RepeatLayout="Flow">
                <asp:ListItem Text="Monday" Value="Monday"></asp:ListItem>
                <asp:ListItem Text="Tuesday" Value="Tuesday"></asp:ListItem>
                <asp:ListItem Text="Wednesday" Value="Wednesday"></asp:ListItem>
                <asp:ListItem Text="Thursday" Value="Thursday"></asp:ListItem>
                <asp:ListItem Text="Friday" Value="Friday"></asp:ListItem>
                <asp:ListItem Text="Saturday" Value="Saturday"></asp:ListItem>
                <asp:ListItem Text="Sunday" Value="Sunday"></asp:ListItem>
            </asp:CheckBoxList>
            <span class="btnswitchgroup">
                <asp:HyperLink ID="HYPmonday" class="btnswitch first" href="" title="Lunedì" runat="server">Lu</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPtuesday" class="btnswitch" href="" title="Martedì"
                runat="server">Ma</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPwednesday" class="btnswitch" href="" title="Mercoledì"
                runat="server">Me</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPthursday" class="btnswitch" href="" title="Giovedì"
                runat="server">Gi</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPfriday" class="btnswitch" href="" title="Venerdì"
                runat="server">Ve</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPsaturday" class="btnswitch" href="" title="Sabato" runat="server">Sa</asp:HyperLink><!--
                --><asp:HyperLink ID="HYPsunday" class="btnswitch last" href="" title="Domenica"
                runat="server">Do</asp:HyperLink><!--
                --></span>
        </div>
    </div>
    <asp:HyperLink ID="HYPmanageCalendars" runat="server" Visible="false">*Manage calendar</asp:HyperLink>
</div>
<div class="fieldrow projectowner">
    <asp:Label ID="LBprojectOwner_t" class="fieldlabel" runat="server" AssociatedControlID="LBprojectOwner">*Project owner:</asp:Label>
    <div class="inlinewrapper clearfix">
        <asp:Label ID="LBprojectOwner" class="inputtext" runat="server"></asp:Label>   
        <div class="ddbuttonlist enabled" id="DVprojectOwner" runat="server"><!--
            --><asp:LinkButton ID="LNBselectOwnerFromResources" runat="server" CssClass="linkMenu ddbutton"></asp:LinkButton><!--
            --><asp:LinkButton ID="LNBselectOwnerFromCommunity" runat="server" CssClass="linkMenu ddbutton"></asp:LinkButton><!--
        --></div>
    </div>
</div>
<div class="fieldrow projectdefaulttaskresource hidecontent">
    <asp:Label ID="LBprojectDefaultResourceForActivity_t" class="fieldlabel" runat="server" AssociatedControlID="CBXdefaultResourceForActivity">Default resource</asp:Label>
    <div class="inlinewrapper clearfix">
        <asp:CheckBox ID="CBXdefaultResourceForActivity" runat="server" CssClass="inputtext" />
        <asp:Label ID="LBdefaultResourceForActivity" class="inline" AssociatedControlID="CBXdefaultResourceForActivity" runat="server">allow</asp:Label>
        <asp:Label ID="LBdefaultResourceForActivityDescription" runat="server" cssclass="description" AssociatedControlID="CBXdefaultResourceForActivity"> </asp:Label>
        <div class="hideme">
            <div class="choseselect clearfix resources">
                <asp:Label ID="LBselectDefaultResources_t" runat="server"></asp:Label>
                <select runat="server" id="SLBresources" class="resources chzn-select" multiple>            
                </select>
            </div>
        </div>
    </div>
</div>
<div class="fieldrow fieldavailability" id="DVavailability" runat="server" visible="false">
    <asp:Label ID="LBprojectAvailability_t" class="fieldlabel" AssociatedControlID="RBLavailability" runat="server">*Availability:</asp:Label>
    <asp:RadioButtonList id="RBLavailability" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server"></asp:RadioButtonList>
</div>

<div class="fieldrow fieldcompletion" id="DVcompletion" runat="server" visible="false">
    <asp:Label ID="LBcompletion_t" class="fieldlabel" AssociatedControlID="LBcompletion" runat="server">*Completion:</asp:Label>
    <asp:Label ID="LBcompletion" CssClass="inputtext" runat="server"></asp:Label>
</div>
<div class="fieldrow fieldstatus" id="DVstatus" runat="server" visible="false">
    <asp:Label ID="LBprojectStatus_t" class="fieldlabel" for="" runat="server" AssociatedControlID="LBprojectStatus">*Status:</asp:Label>
    <asp:Label ID="LBprojectStatus" class="inputtext" for="" runat="server">*Status:</asp:Label>
</div>
<div class="fieldrow fieldmilestone">
    <asp:Label ID="LBprojectAllowMilestones_t" class="fieldlabel" AssociatedControlID="CBXallowMilestones" runat="server">Milestones:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXallowMilestones" runat="server" CssClass="inputtext" />
        <asp:Label ID="LBprojectAllowMilestones" class="inline" AssociatedControlID="CBXallowMilestones" runat="server">allow</asp:Label>
        <div class="description">
            <asp:Label ID="LBprojectAllowMilestonesDescription" AssociatedControlID="CBXallowMilestones" runat="server"></asp:Label>
        </div>
    </div>
</div>
<div class="fieldrow fieldtreevisibility">
    <asp:Label ID="LBprojectVisibility_t" class="fieldlabel" AssociatedControlID="CBXvisibility" runat="server">Map visibility:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXvisibility" runat="server" CssClass="inputtext" />
        <asp:Label ID="LBprojectVisibility" class="inline" AssociatedControlID="CBXvisibility" runat="server"></asp:Label>
        <div class="description">
            <asp:Label ID="LBprojectVisibilityDescription" AssociatedControlID="CBXvisibility" runat="server"></asp:Label>
        </div>
    </div>
</div>
<div class="fieldrow fieldconfirmation">
    <asp:Label ID="LBprojectConfirmCompletion_t" class="fieldlabel" AssociatedControlID="CBXconfirmCompletion" runat="server">*Activity completion:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXconfirmCompletion" runat="server" CssClass="inputtext" />
        <asp:Label ID="LBprojectConfirmCompletion" class="inline" AssociatedControlID="CBXconfirmCompletion" runat="server">*Confirmed by manager</asp:Label>
        <div class="description">
            <asp:Label ID="LBprojectConfirmCompletionDescription" AssociatedControlID="CBXconfirmCompletion" runat="server"></asp:Label>
        </div>
    </div>
</div>           
<div class="fieldrow fieldprojecttype" id="DVcpm" runat="server" visible="false">
    <asp:Label ID="LBprojectDateCalculationByCpm_t" class="fieldlabel" AssociatedControlID="CBXdateCalculationByCpm" runat="server">*Automatic date:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXdateCalculationByCpm" runat="server" CssClass="inputtext mastercheck" data-uncheck=".estimatedduration" data-disable=".estimatedduration" />
        <asp:Label ID="LBprojectDateCalculationByCpm" class="inline" AssociatedControlID="CBXdateCalculationByCpm" runat="server">*Yes, by cpm</asp:Label>
        <div class="description">
            <asp:Label ID="LBprojectDateCalculationByCpmDescription" AssociatedControlID="CBXdateCalculationByCpm" runat="server"></asp:Label>
        </div>
    </div>
</div>
<div class="fieldrow fieldsummaryactivities">
    <asp:Label ID="LBprojectSummaryActivities_t" class="fieldlabel" AssociatedControlID="CBXallowSummaryActivities" runat="server">*Summary activities:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXallowSummaryActivities" runat="server" CssClass="inputtext summaryactivities" />
        <asp:Label ID="LBprojectSummaryActivities" class="inline" AssociatedControlID="CBXallowSummaryActivities" runat="server">*allow</asp:Label>
        <div class="description">
            <asp:Label ID="LBprojectSummaryActivitiesDescription" AssociatedControlID="CBXallowSummaryActivities" runat="server"></asp:Label>
        </div>
    </div> 
</div> 
<div class="fieldrow fieldestimatedduration">
    <asp:Label ID="LBallowEstimatedDuration_t" class="fieldlabel" AssociatedControlID="CBXallowEstimatedDuration" runat="server">*Estimated duration:</asp:Label>
    <div class="inlinewrapper">
        <asp:CheckBox ID="CBXallowEstimatedDuration" runat="server" CssClass="inputtext estimatedduration" />
        <asp:Label ID="LBallowEstimatedDuration" class="inline" AssociatedControlID="CBXallowEstimatedDuration" runat="server">*Allow</asp:Label>
        <div class="description">
            <asp:Label ID="LBallowEstimatedDurationDescription" AssociatedControlID="CBXallowEstimatedDuration" runat="server"></asp:Label>
        </div>
    </div> 
</div> 


<div class="view-modal view-users-community" id="DVselectFromCommunity" runat="server" visible="false">
    <CTRL:SelectUsers ID="CLTRselectUsers" runat="server" RaiseCommandEvents="True" DisplayDescription="true"
        DefaultPageSize="20" ShowSubscriptionsProfileTypeColumn="True" DefaultMaxPreviewItems="5"
        ShowItemsExceeding="true" ShowSubscriptionsFilterByProfile="false" />
</div>
<div class="view-modal view-users-project" id="DVselectFromProjectResource" runat="server" visible="false">
    <CTRL:SelectFromResources id="CTRLselectFromResources" runat="server"></CTRL:SelectFromResources>
</div>