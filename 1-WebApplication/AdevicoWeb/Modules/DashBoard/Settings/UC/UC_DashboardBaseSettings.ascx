<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DashboardBaseSettings.ascx.vb" Inherits="Comunita_OnLine.UC_DashboardBaseSettings" %>
<div class="fieldobject box basicinfo first">
    <div class="fieldrow objectheader">
        <h4 class="title"><asp:literal ID="LTbaseSettingsTitle" runat="server" Text="*Basic info"></asp:literal></h4>
    </div>
    <div class="fieldrow fieldlongtext">
        <label class="fieldlabel" for="<%=TXBsettingsName.ClientID %>"><asp:literal ID="LTsettingsName" runat="server" Text="*Name:"></asp:literal><span class="mandatory">*</span></label>
        <asp:TextBox ID="TXBsettingsName" MaxLength="150" CssClass="inputtext" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RFVsettings" runat="server" ControlToValidate="TXBsettingsName" Display="Dynamic" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
    </div>
    <div class="fieldrow fielddescription">
        <label class="fieldlabel" for="<%=TXBsettingsDescription.ClientID%>"><asp:literal ID="LTsettingsDescription" runat="server" Text="*Description:"></asp:literal></label>
        <asp:TextBox ID="TXBsettingsDescription" TextMode="MultiLine" Columns="30" Rows="10" CssClass="textarea big" runat="server"></asp:TextBox>
    </div>
    <div class="fieldrow mandatorylegend"><asp:literal ID="LTmandatoryInfos" runat="server" Text="*Marked fields {0} are mandatory"></asp:literal></div>
</div>
<div class="fieldobject box profiles last">
    <div class="fieldrow objectheader">
        <h4 class="title"><asp:literal ID="LTbaseSettingsAssignmentsTitle" runat="server" Text="*"></asp:literal></h4>
    </div>
    <div class="fieldrow isdefault">
        <asp:Label ID="LBbaseSettingsForAll_t" runat="server" AssociatedControlID="CBXisForAll" Text="*Set as default:"></asp:Label>
        <asp:CheckBox ID="CBXisForAll" runat="server" data-disable="profileselect" />
        <asp:Label ID="LBbaseSettingsForAll" runat="server" AssociatedControlID="CBXisForAll"  Text="*Yes"></asp:Label>
    </div>
    <div class="fieldrow profileselector" id="DVassignments" runat="server">
        <asp:Label ID="LBbaseSettingsAssignments" runat="server" Text="*" CssClass="fieldlabel" AssociatedControlID="SLBassignments"></asp:Label>
        <div class="choseselect clearfix">
            <div class="left">
                <select runat="server" id="SLBassignments" class="partecipants chzn-select" data-id="profileselect" multiple tabindex="2">
                                                                        
                </select>
            </div>
            <div class="right">
                <span class="icons">
                     <span class="icon selectall" title="All" runat="server" id="SPNassignmentSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNassignmentSelectNone">&nbsp;</span>
                </span>
            </div>
        </div>
    </div>
</div>
<asp:Literal ID="LTmandatoryTemplate" runat="server" Visible="false"><span class="mandatory">*</span></asp:Literal>