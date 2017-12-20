<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="Attachments.aspx.vb" Inherits="Comunita_OnLine.ProjectAttachments" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/ProjectManagement/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Common/UC/UC_AttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayFile" Src="~/Modules/Repository/UC/UC_ModuleRepositoryAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayUrl" Src="~/Modules/Common/UC/UC_DisplayUrlItem.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EditUrl" Src="~/Modules/Common/UC/UC_EditUrlItems.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="EditUrlHeader" Src="~/Modules/Common/UC/UC_EditUrlItemsHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/ProjectManagement/UC/UC_PMaddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Common/UC/UC_AttachmentJqueryHeaderCommands.ascx" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/ProjectManagement/Css/ProjectManagement.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/ProjectManagement/projectmanagement.js"></script>
    <CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />
    <CTRL:EditUrlHeader ID="CTRLeditUrlItemsHeader" runat="server" EditingCssClass="dlgeditinlineurl" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="LBnoResources" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <br />
        </asp:View>
        <asp:View ID="VIWsettings" runat="server">
            <div class="contentwrapper edit clearfix persist-area">
                <div class="column left persist-header copyThis">
                    <CTRL:WizardSteps runat="server" ID="CTRLsteps"></CTRL:WizardSteps>
                </div>
                <div class="column right resizeThis">
                    <div class="rightcontent">
                        <div class="header">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPgoToProjectMapTop" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardTop" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsTop" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>                                
                            </div>
                            <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <div class="fieldobject toolbar clearfix">
                                    <div class="fieldrow right">
                                        <CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="false" />
                                    </div>
                                </div>
                                <ul class="userselections">
                                    <li class="userselection filesummary">
                                        <div class="userselectioncontent">
                                            <div class="title clearfix">
                                                <div class="left">
                                                    <label class="title"><asp:literal ID="LTprojectAttacments" runat="server">*Project attachements</asp:literal></label>
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
                                                    </span>
                                                </div>
                                                <div class="clearer"></div>
                                            </div>
                                            <div class="footer">
                                                <!--Options-->
                                                <div class="tablewrapper">
                                                    <table class="table light attachments fullwidth" data-max="15">
                                                        <thead>
                                                            <tr>
                                                                <th class="check" id="THcheckbox" runat="server">
                                                                    <span class="headercheckbox leftside">
                                                                        <input type="checkbox" name="" id="Checkbox1" />
                                                                    </span>
                                                                </th>
                                                                <th class="attachmentname"><asp:literal ID="LTattachmentnameHeader" runat="server">*Name</asp:literal></th>
                                                                <th class="attachmenttype"><asp:literal ID="LTattachmenttypeHeader" runat="server">*Type</asp:literal></th>
                                                                <th class="actions" id="THactions" runat="server"><asp:literal ID="LTattachmentactionsHeader" runat="server">*Actions</asp:literal></th>
                                                            </tr>
                                                        </thead>
                                                        <asp:Repeater id="RPTattachments" runat="server">
                                                            <HeaderTemplate>
                                                                <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="<%#GetCssClass(Container.DataItem.Attachment.Type) %>">
                                                                    <td class="check"  id="TDcheckbox" runat="server">
                                                                        <span class="submittercheckbox">
                                                                            <input type="checkbox" runat="server" id="CBXattachment" />
                                                                            <asp:Literal ID="LTidLink" runat="server" Visible="false" Text='<%#Container.DataItem.Attachment.IdAttachmentLink %>'></asp:Literal>
                                                                            <asp:Literal ID="LTidAttachment" runat="server" Visible="false" Text='<%#Container.DataItem.Attachment.IdAttachment %>'></asp:Literal>
                                                                        </span>
                                                                    </td>
                                                                    <td class="attachmentname">
                                                                        <CTRL:DisplayUrl ID="CTRLdisplayUrl" runat="server" Visible="false" />
                                                                        <CTRL:DisplayFile ID="CTRLdisplayFile" runat="server" />
                                                                    </td>
                                                                    <td class="attachmenttype"><asp:literal ID="LTattachmentType" runat="server"></asp:literal></td>
                                                                    <td class="actions" id="TDactions" runat="server">
                                                                        <span class="icons">
                                                                            <asp:HyperLink ID="HYPstats" runat="server" Text="S" CssClass="icon stats" Visible="false" />
                                                                            <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="M" CssClass="icon editmetadata" Visible="false" />
                                                                            <asp:LinkButton ID="LNBeditUrl" runat="server" CommandName="edit" CssClass="icon edit" CommandArgument='<%#Container.DataItem.Attachment.IdAttachmentLink & "|" & Container.DataItem.Attachment.IdAttachment %>' Visible="false"></asp:LinkButton>
                                                                            <asp:LinkButton ID="LNBvirtualDeleteAttachment" runat="server" CommandName="virtualdelete" CssClass="icon virtualdelete" CommandArgument='<%#Container.DataItem.Attachment.IdAttachmentLink %>' Visible="false"></asp:LinkButton>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr id="TRempty" runat="server" visible="false">
                                                                    <td id="TDemptyItems" runat="server" colspan="4">
                                                                        <asp:Label ID="LBprojectAttachmentsEmptyItems" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                </tbody>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                        <tfoot id="TFcommands" runat="server" visible="false">
                                                            <tr>
                                                                <td id="TDfooter" runat="server" colspan="4">
                                                                    <asp:Button ID="BTNexportToCommunityFolder" runat="server" CssClass="linkMenu" Text="*Save selected as community files" Visible="false"/>
                                                                    <asp:Button ID="BTNvirtualDeleteSelectedAttacchments" runat="server" CssClass="linkMenu" Text="*Delete selected" Visible="false"/>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPgoToProjectMapBottom" class="linkMenu" runat="server" Text="*Project map" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToResourceDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToManagerDashboardBottom" class="linkMenu" runat="server" Text="*Resource dashboard" Visible="false"></asp:HyperLink>
                                <asp:HyperLink ID="HYPbackToProjectsBottom" class="linkMenu" runat="server" Text="*Back" Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />
            <CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity"/>
            <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity"/>
            <CTRL:Attachments ID="CTRLaddUrls" runat="server" visible="false" UploaderCssClass="dlgaddurltomoduleitem" />
            <CTRL:EditUrl ID="CTRLeditUrl" runat="server" EditingCssClass="dlgediturl"/>
        </asp:View>
    </asp:MultiView>
    <asp:Literal ID="LTediturlWidth" runat="server" Visible="false">600</asp:Literal>
    <asp:Literal ID="LTediturlHeight" runat="server" Visible="false">200</asp:Literal>
</asp:Content>