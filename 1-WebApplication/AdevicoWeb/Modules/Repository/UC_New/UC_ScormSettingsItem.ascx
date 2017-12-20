<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ScormSettingsItem.ascx.vb" Inherits="Comunita_OnLine.UC_ScormSettingsItem" %>
<div class="fieldobject scormsettings">
    <div class="fieldrow completeness">
        <input type="checkbox" class="checkchange" data-changed="completeness" runat="server" id="CBXscormCompletion">
        <asp:Label ID="LBcheckScormCompletion" runat="server" AssociatedControlID="CBXscormCompletion" Text="*Completeness" CssClass="fieldlabel"></asp:Label>
    </div>
    <div class="fieldrow time">
        <input type="checkbox" class="checkchange" data-changed="time" runat="server" id="CBXtimeCompletion">
        <asp:Label ID="LBcheckTime" runat="server" AssociatedControlID="CBXtimeCompletion" Text="*Time" CssClass="fieldlabel"></asp:Label>
        <label for="" class="fieldlabel"></label>
        <span class="timeinput">
            <input class="hours checkchange" value="0" size="3" maxlength="3" runat="server" id="INhours"/>
            <span>h</span>
            <input class="minutes checkchange" value="0" size="2" maxlength="2" runat="server" id="INminutes"/>
            <span>m</span>
            <input class="seconds checkchange"  value="10" size="2" maxlength="2" runat="server" id="INseconds"/>
            <span>s</span>
        </span>
    </div>
    <div class="fieldrow mark">
        <input type="checkbox" class="checkchange" data-changed="mark" runat="server" id="CBXscoreCompletion">
        <asp:Label ID="LBcheckScore" runat="server" AssociatedControlID="CBXscoreCompletion" Text="*Mark" CssClass="fieldlabel"></asp:Label>
        <input type="text" class="checkchange" runat="server" id="INscore">
        <asp:RangeValidator ID="RNVscore" runat="server" ControlToValidate="INscore" Type="Double" SetFocusOnError="true"  Display="Dynamic"></asp:RangeValidator>
        <asp:RangeValidator ID="RNVscoreScaled" runat="server" ControlToValidate="INscore" Type="Double" MinimumValue="-1" MaximumValue="1" Enabled="false" Display="Dynamic" ErrorMessage="-1<=x<=1" SetFocusOnError="true"></asp:RangeValidator>
    </div>
</div><asp:Literal ID="LTidItem" runat="server" Visible="false"></asp:Literal><asp:Literal ID="LTforPackage" runat="server" Visible="false"></asp:Literal>