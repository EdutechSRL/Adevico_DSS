<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_ScormSettings.ascx.vb" Inherits="Comunita_OnLine.UC_ScormSettings" %>
<%@ Register TagPrefix="CTRL" TagName="CTRLsettings" Src="~/Modules/Repository/UC_New/UC_ScormSettingsItem.ascx" %>
<div class="fieldobject scormsettings<%=CssClass%>">
    <div class="fieldrow description">
        <asp:Label ID="LBdescriptionScormSettings" runat="server" Text="*" CssClass="description"></asp:Label>
    </div>
    <div class="fieldrow selector">
        <div class="fileselector">
            <div class="filerepository mainview container_12 clearfix">
                <div class="content">
                    <div class="section box height treefile grid_12 alpha">
                        <div class="sectioninner">
                            <div class="treewrapper">
                                <asp:Literal ID="LTrenderTree" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="section box settings height grid_12 alpha omega">
                        <div class="sectioninner">
                            <div class="block settings">
                                <div class="title"><asp:Literal runat="server" ID="LTscormSettingsBoxTitle">*Settings</asp:Literal><asp:Label runat="server" CssClass="onlychanged" ID="LBscormSettingsBoxModified">*(modified)</asp:Label></div>
                                <div class="blockinner">
                                    <div class="fieldobject">
                                        <div class="fieldrow">
                                            <div class="description"><asp:Literal runat="server" ID="LTscormSettingsBoxDescription"></asp:Literal></div>
                                        </div>
                                        <div class="fieldrow">
                                            <div class="coveredradio enabled<%=CssClass%> scormsettingtype checkchange" data-parent=".section.box.settings">
                                                <asp:radiobuttonlist ID="RBLevaluationMode" RepeatLayout="Flow" CssClass="wclist" runat="server">
                                                    <asp:ListItem Value="0"></asp:ListItem>
                                                    <asp:ListItem Value="1"></asp:ListItem>
                                                    <asp:ListItem Value="2"></asp:ListItem>
                                                </asp:radiobuttonlist>
                                                <span class="btnswitchgroup small"><!--
                                                --><asp:HyperLink ID="HYPscormOriginalSettings" CssClass="btnswitch original first" runat="server" NavigateUrl="#">*Original</asp:HyperLink><!--
                                                --><asp:HyperLink ID="HYPscormPackageSettings" CssClass="btnswitch package" runat="server" NavigateUrl="#">*Package</asp:HyperLink><!--
                                                --><asp:HyperLink ID="HYPscormActivitySettings" CssClass="btnswitch activity last" runat="server" NavigateUrl="#">*Activity</asp:HyperLink><!--
                                                --></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="settings container_12 clearfix">
                                <div class="block packagesettings alpha omega grid_12">
                                    <div class="title"><asp:Literal runat="server" ID="LTscormPackageSettingsBoxTitle">*Settings</asp:Literal><asp:Label runat="server" CssClass="onlychanged" ID="LBscormPackageSettingsBoxModified">*(modified)</asp:Label></div>
                                    <div class="blockinner">
                                        <CTRL:CTRLsettings ID="CTRLsettings" runat="server" /> 
                                    </div>
                                </div>
                                <div class="block activitysettings alwayson blockmessage alpha omega grid_12">
                                    <div class="messages">
                                        <div class="message info">
                                            <asp:Literal ID="LTscormMessage" runat="server">*You're seeing a subset of the activities, based on the organization/folder you selected.</asp:Literal>
                                            <asp:HyperLink ID="HYPseeAllScormActivities" runat="server" CssClass="seeall" NavigateUrl="#">click here to see them all</asp:HyperLink>
                                        </div>
                                    </div>
                                </div>
                                <asp:Repeater ID="RPTactivities" runat="server">
                                    <ItemTemplate>
                                <div class="block activitysettings grid_6" data-id="<%#Container.DataItem.DataId %>">
                                    <div class="title"><asp:Literal ID="LTactivityBoxTitle" runat="server"></asp:Literal><asp:Label runat="server" CssClass="onlychanged" ID="LBscormActivitySettingsBoxModified">*(modified)</asp:Label></div>
                                    <div class="blockinner">
                                        <div class="fieldobject scormsettings">
                                            <CTRL:CTRLsettings ID="CTRLitemSettings" runat="server"/> 
                                        </div>
                                    </div>
                                </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div><asp:Literal ID="LTtemplateFile" runat="server" Visible="false"><span class="name"><span class="actionbuttons"><span class="#ico#"></span></span><span class="text">#name#</span></span></asp:Literal><asp:Literal ID="LTitemExtensionCssClass" runat="server" Visible="false">fileIco ext</asp:Literal><asp:Literal ID="LTitemFolderCssClass" runat="server" Visible="false">fileIco folder</asp:Literal><asp:Literal ID="LTitemUrlCssClass" runat="server" Visible="false">fileIco extlink</asp:Literal><asp:Literal ID="LTitemScormPackageCssClass" runat="server" Visible="false">fileIco scorm</asp:Literal><asp:Literal ID="LTitemMultimediaCssClass" runat="server" Visible="false">fileIco multimedia</asp:Literal>
    <asp:Literal runat="server" ID="LTtreeRoot" Visible="false">
<ul class="nestedtree noselect directories tree root" data-cookie-prefix="scorm-#uniqueIdVersion#">
    <li class="treenode directory autoOpen" id="folder-#uniqueIdVersion#-#dataid#">
        <div class="content">
            <div class="header">
                <span class="handle folder expander">&nbsp;</span>
                <span class="item">
                    <span class="selection">
                        <input type="checkbox" />
                    </span>
                    <span class="text">
                        <a href="#" class="namelink package" data-children="#datachildren#" data-id="#dataid#">
                            <span class="fileIco folder"></span>
                            <span class="name">#name#</span>
                        </a>
                        <span class="details"></span>
                    </span>
                </span>
            </div>
            <div class="footer">
                 #childrennodes#
            </div>
        </div>
    </li>
</ul>
    </asp:Literal>
    <asp:Literal runat="server" ID="LTtreeFolderNode" Visible="false">
 <li class="treenode directory autoOpen" id="folder-#dataid#">
    <div class="content">
        <div class="header">
            <span class="handle folder expander">&nbsp;</span>
            <span class="item">
                <span class="selection">
                    <input type="checkbox" />
                </span>
                <span class="text">
                    <a href="#" class="namelink" data-children="#datachildren#" data-id="#dataid#">
                        <span class="fileIco folder"></span>
                        <span class="name">#name#</span>
                    </a>
                    <span class="details"></span>
                </span>
            </span>
        </div>
        <div class="footer">
             #childrennodes#
        </div>
    </div>
</li>
    </asp:Literal>
    <asp:Literal runat="server" ID="LTtreeChildrenNodes" Visible="false">
<ul class="nestedtree directories">
    #childrennodes#
</ul>
    </asp:Literal>
    <asp:Literal ID="LTcookieTemplate" runat="server" Visible="false">comol_scorm-#uniqueIdVersion#CollapsableStatus[{0}]</asp:Literal>