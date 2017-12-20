<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditAttachments.aspx.vb" Inherits="Comunita_OnLine.EditAttachments" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Header" Src="~/Modules/CallForPapers/UC/UC_CallHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/CallForPapers/UC/UC_CallAddAttachments.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <CTRL:Header ID="CTRLheader" runat="server" />
    <CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" />
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
  <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <CTRL:Messages ID="CTRLemptyMessage"  runat="server"/>
            <br /><br /><br /><br />
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
                                <asp:HyperLink ID="HYPbackTop" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallTop" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveAttachmentsTop" runat="server" Text="Save"/>
                            </div>
                            <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                 <!-- @Start Attachments -->
                                 <div class="DivEpButton">
                                    <CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="false" />
                                 </div>
                                <ul class="sections attachedfiles" runat="server" id="ULviewAttachments">
                                    <li class="section clearfix autoOpen">
                                        <div class="externalleft"></div>
                                        <div class="sectioncontent">
                                            <div class="innerwrapper">
                                                <div class="internal clearfix">
            					                    <span class="left">
            						                    <asp:Label ID="LBattachments_t" runat="server" CssClass="title">Attachments</asp:Label>
            					                    </span>
            					                    <span class="right">
            						                    <span class="icons"></span>
            					                    </span>
                                                </div>
                                                <div class="sectiondetails"></div>
                                            </div>
                                            <div class="clearer"></div>
                                            
                                            <asp:Repeater ID="RPTattachments" runat="server">
                                                <HeaderTemplate>
                                                    <ul class="fields filesattached">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <li class="cfield fileattached clearfix" id="attachment_<%#Container.DataItem.Id %>">
                                                    <asp:Literal ID="LTidAttachment" Visible="false" runat="server" Text="<%#Container.DataItem.Id %>"></asp:Literal>
                                                    <input type="hidden" id="HDNdisplayOrder" runat="server" class="hiddendisplayorder"/>
                                                    <div class="externalleft">
                                                        <asp:Label ID="LBmoveAttachment" cssclass="movecfield" runat="server">M</asp:Label>
                                                    </div>
                                                    <div class="fieldcontent">
                                                        <div class="internal clearfix">
            						                        <span class="left">
            							                        <span class="title">
	            							                        <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="false" DisplayExtraInfo="false" DisplayLinkedBy="false"  />
            							                        </span>
            						                        </span>
                                                            <span class="right">
            							                        <span class="icons">
                                                                    <asp:HyperLink ID="HYPstats" runat="server" Text="S" CssClass="icon stats" Visible="false" />
                                                                    <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="M" CssClass="icon editmetadata" Visible="false" />
                                                                    <asp:Button id="BTNvirtualDelete" runat="server" Text="D" CommandName="virtualDelete" cssclass="icon delete"/>
                                                                </span>
            							                    </span>
                                                        </div>
                                                        <div class="fieldobject ">
                                                            <div class="fieldrow fielddescription clearfix">
                                                                <asp:Label id="LBdescriptionAttachment_t" CssClass="Titolo_campo fieldlabel" runat="server" AssociatedControlID="TXBdescription"></asp:Label>
                                                                <asp:TextBox ID ="TXBdescription" runat="server" CssClass="textarea" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                            <div class="fieldrow clearfix attachmentforall">
                                                                <asp:Label id="LBattachmentForAll_t" CssClass="Titolo_campo fieldlabel" runat="server" AssociatedControlID="CBXforAll"></asp:Label>
                                                                <input type="checkbox" id="CBXforAll" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="fieldfooter">
                                                            <div class="choseselect clearfix">
                                                                <div class="left">
                                                                    <asp:Label ID="LBattachmentSubmitters_t" runat="server" AssociatedControlID="SLBsubmitters" CssClass="Titolo_campo fieldlabel"></asp:Label>
                                                                    <select runat="server" id="SLBsubmitters" class="partecipants chzn-select" multiple tabindex="2">
                                                                        
                                                                    </select>
                                                                </div>
                                                                <div class="right">
											                        <span class="icons">
												                        <span class="icon selectall" title="All" runat="server" id="SPNattchSelectAll">&nbsp;</span><span class="icon selectnone" title="None" runat="server" id="SPNattchSelectNone">&nbsp;</span>
											                        </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="clearer"></div>
                                                </li>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </ul>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </li>
                                </ul>
                                <!-- @End Attachments -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton" id="DVbuttonBottom" runat="server" visible="false">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveAttachmentsBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
           </div>
        </asp:View>
    </asp:MultiView>
    <CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" />
</asp:Content>