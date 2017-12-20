<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_TagEdit.ascx.vb" Inherits="Comunita_OnLine.UC_TagEdit" %>
<div class="dlgtagedit" edit-title="<%=EditTagTitle%>" view-title="<%=ViewTagTitle%>" new-title="<%=NewTagTitle%>" ui-dialog ui-width="700" ui-height="auto" ui-modal="True">
    <div class="messages hidebeforeloading ng-cloak" ng-cloak ng-show="errorMessage!='' && errorDialog==true">
        <div class="message error">
            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
        </div>
    </div>
    <div class="fieldobject tagedit" aria-live="polite">
        <div class="fieldrow">
            <asp:Label ID="LBtagIsDefault_t" runat="server" CssClass="fieldlabel">*IsDefault:</asp:Label>
            <input type="checkbox" value="" id="CHB_isDefault" ng-model="tag.IsDefault" />
            <label class="" for="CHB_isDefault"><asp:Literal ID="LTtagIsDefault" runat="server"></asp:Literal></label> 
        </div>
        <div class="fieldrow selector" ng-show="!tag.IsDefault">
            <asp:Label ID="LBtagDefaultFor_t" runat="server" CssClass="fieldlabel">*Default for:</asp:Label>
            <span ng-show="!tag.IsReadonly">
            <select multiple class="chzn-select" ng-show="!tag.IsReadonly" chosen-tag name="CommunityTypes" data-placeholder="..." ng-model="tag.SelectedCommunityTypes" ng-options="value.Name for value in tag.CommunityTypes track by value.Id"></select>
            </span>
            <span  ng-show="tag.IsReadonly">
            <select multiple class="chzn-select" disabled="disabled" ng-show="tag.IsReadonly" chosen-tag name="CommunityTypes" data-placeholder="..." ng-model="tag.SelectedCommunityTypes" ng-options="value.Name for value in tag.CommunityTypes track by value.Id"></select>
            </span>
        </div>
        <div class="fieldrow" ng-show="tag.Organizations.length>1">
            <asp:Label ID="LBtagOrganizations_t" runat="server" CssClass="fieldlabel">*Organization:</asp:Label>
            <div class="inlinewrapper">
                <div ng-repeat="value in tag.Organizations" class="input-group">
                    <input class="input" id="Organizations_{{$index}}" name="Organizations_{{$index}}" type="checkbox" ng-model="value.Checked" ng-disabled="value.Disabled" /><label class="label" for="Organizations_{{$index}}">{{value.Name}}</label>
                </div>
            </div>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBtagTranslations_t" runat="server" CssClass="fieldlabel">*Translations:</asp:Label>
        </div>
    </div>
    <div class="tabs" ui-tabs="tag.Translations" ng-model="tag.Translations" aria-live="polite">
        <ul>
            <li ng-repeat="tns in tag.Translations"><a href="#tabs-{{tns.LanguageCode}}">{{tns.LanguageName}}</a></li>
        </ul>
        <div id="tabs-{{tns.LanguageCode}}" ng-repeat="tns in tag.Translations">
            <div class="fieldobject">
                <div class="fieldrow haslang">
                    <label class="fieldlabel wrap" for="">
                        <asp:Label ID="LBtagName_t" runat="server" CssClass="left">*Name:</asp:Label>
                        <span class="templatelanguage right" title="international">{{tns.LanguageCode}}</span></label>
                    <input class="inputtext" ng-model="tns.Title" type="text" name="" id="" ng-disabled="tag.IsReadonly">
                </div>

                <div class="fieldrow haslang">
                    <label class="fieldlabel wrap" for="">
                        <asp:Label ID="LBtagDescription_t" runat="server" CssClass="left">*Description:</asp:Label>
                        <span class="templatelanguage right" title="international">{{tns.LanguageCode}}</span></label>
                    <textarea class="textarea big" ng-model="tns.Description" ng-disabled="tag.IsReadonly"></textarea>
                </div>
            </div>
        </div>
    </div>
    <div class="fieldobject">
        <div class="fieldrow left">&nbsp;</div>
        <div class="fieldrow right">
            <asp:Button ID="BTNsaveTag" runat="server" ng-show="!tag.IsReadonly" ng-click="setTag(tag);" OnClientClick="return false;"/>
        </div>
    </div>
</div>

<div class="dlgtaglink" edit-title="<%=EditTagLinkTitle%>" view-title="<%=ViewTagLinkTitle%>" new-title="" ui-dialog ui-width="auto" ui-height="auto" ui-modal="True">
    <div class="messages hidebeforeloading" ng-show="errorMessage!='' && errorDialog==true">
        <div class="message error">
            <span class="icons"><span class="icon">&nbsp;</span></span>{{errorMessage}}
        </div>
    </div>
    <div class="fieldobject taglink">
        <div class="fieldrow description">
            <div class="description">
                <asp:Literal ID="LTapplyTagDescription" runat="server"></asp:Literal>
            </div>
        </div>        
        <div class="fieldrow">
            <asp:Label ID="LBapplyTagOn_t" runat="server" CssClass="fieldlabel">*Apply on:</asp:Label>
            <input type="checkbox" id="communitywithouttags" ng-model="tagLink.OnlyCommunityWithoutTag" />
            <label for="communitywithouttags"><asp:Literal ID="LTapplyTagOn" runat="server">*only on community without tags</asp:Literal></label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBapplyThisTagOn_t" runat="server" CssClass="fieldlabel"></asp:Label>
            <input type="checkbox" id="communitywithoutthistag" ng-model="tagLink.OnlyCommunityWithoutThisTag" />
            <label for="communitywithoutthistag"><asp:Literal ID="LTapplyThisTagOn" runat="server">*only on community without this tag</asp:Literal></label>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBapplyTagOnAllCommunityTypes_t" runat="server" CssClass="fieldlabel">*Community Types:</asp:Label>
            <input type="checkbox" id="allcommunitytypes" ng-model="tagLink.AllCommunityTypes" /><label for="allcommunitytypes"><asp:Literal ID="LTapplyTagOnAllCommunityTypes" runat="server">All community Types</asp:Literal></label>
        </div>
        <div class="fieldrow" ng-show="!tagLink.AllCommunityTypes">
            <asp:Label ID="LBselectCommunityTypesForTagApply" runat="server" CssClass="fieldlabel">*Selections:</asp:Label>
            <div class="inlinewrapper">
                <div ng-repeat="value in tagLink.CommunityTypes" class="input-group">
                    <input class="input" id="CommunityType_{{$index}}" ng-disabled="value.Disabled" name="CommunityType_{{$index}}" type="checkbox" ng-model="value.Checked" /><label class="label" for="CommunityType_{{$index}}">{{value.Name}}</label>
                </div>
            </div>
        </div>
    </div>
    <div class="fieldobject">
        <div class="fieldrow left">&nbsp;</div>
        <div class="fieldrow right">
            <asp:Button ID="BTNapplyTag" runat="server" ng-show="!tagLink.IsReadonly" ng-click="setTagLink(tagLink);" OnClientClick="return false;"/>
        </div>
    </div>
</div>

<div class="dialog dlgtagbulk" title="<%=BulkInsertTile%>">
    <div class="fieldobject tagbulk">
        <div class="fieldrow description">
            <div class="description">
                <asp:Literal ID="LTbulkInsertDescription" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="fieldrow help">
            <div class="helptext">
                <asp:Label ID="LBtagBulkInsert"  AssociatedControlID="TXBbulkInsert" runat="server">*One tag for row, on each row default translation#<b>{0}</b>)</asp:Label>
            </div>
        </div>
        <div class="fieldrow edit">
            <asp:TextBox ID="TXBbulkInsert" runat="server" TextMode="MultiLine" CssClass="textarea" Rows="10" Columns="100"></asp:TextBox>
        </div>
    </div>
    <div class="fieldobject">
        <div class="fieldrow left">&nbsp;</div>
        <div class="fieldrow right">
            <asp:Button ID="BTNbulkInsert" runat="server" Text="*Bulk insert"/>
        </div>
    </div>
</div>