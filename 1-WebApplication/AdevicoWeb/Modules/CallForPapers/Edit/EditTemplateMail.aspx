<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="EditTemplateMail.aspx.vb" Inherits="Comunita_OnLine.EditTemplateMail" %>
<%@ Register TagPrefix="CTRL" TagName="WizardSteps" Src="~/Modules/CallForPapers/UC/UC_WizardSteps.ascx" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditor.ascx" TagName="CTRLtemplateMail" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailEditorHeader.ascx" TagName="CTRLtemplateMailHeader" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessage.ascx" TagName="CTRLmailMessage" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Common/UC/UC_MailMessageHeader.ascx" TagName="CTRLmailMessageHeader" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../Graphics/Modules/CallForPapers/css/callforpapers.css" rel="Stylesheet" />
    <link href="../../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.blockableFieldset.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.inputActivator.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../../Jscript/Modules/CallForPapers/callforpapers.js"></script>
    <script type="text/javascript">
        $(function () {
            $("fieldset.expandable").blockableFieldset({
                blockedClass: "disabled"
            });

            $("input.activator").inputActivator();
        });

        $(document).ready(function () {
            $(".view-modal.view-previewmail").dialog({
                appendTo: "form",
                closeOnEscape: false,
                modal: true,
                width: 700,
                height: 450,
                minHeight: 200,
                minWidth: 400,
                open: function (type, data) {
                    //$(this).parent().appendTo("form");
                    //$(this).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
            });
        });
    </script>

    <link href="../../../Modules/Common/UC/UC_MailEditor.css" rel="Stylesheet" />

    <script type="text/javascript">
        String.prototype.replaceAll = function (s1, s2) { return this.replace(new RegExp(s1, "g"), s2); }
    </script>
    <CTRL:CTRLtemplateMailHeader ID="CTRLtemplateMailHeader" runat="server" />
    <CTRL:CTRLmailMessageHeader ID="CTRLmailMessageHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView id="MLVsettings" runat="server" ActiveViewIndex="1">
        <asp:View ID="VIWempty" runat="server">
            <br /><br /><br /><br />
            <asp:Label ID="LBnocalls" runat="server"></asp:Label>
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
                                <asp:button ID="BTNsaveTemplatesTop" runat="server" Text="Save"/>
                            </div>
                             <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                        </div>
                        <div class="contentouter">
                            <div class="content">
                                <!-- @Start TEMPLATES -->
                                <div class="treetop clearfix">
                                    <div class="left"></div>
                                    <div class="DivEpButton clearfix">
                                        <asp:Button ID="BTNaddTemplate" runat="server" Text="New template" />   
                                    </div>
                                </div>
                                <ul class="templates">
                                    <li class="template edit" runat="server" id="LItemplateAdd" visible="false">
                                        <div class="templatecontent">
                                            <div class="title clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBtemplateAddTitle" runat="server" CssClass="templatetitle"></asp:Label>
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
                                                    </span>
                                                </div>
                                                <div class="clearer"></div>
                                            </div>
                                            <div class="clearfix">
                                                <div class="left">
                                                    <asp:Label ID="LBtemplateName_t" runat="server" AssociatedControlID="TXBtemplateName" CssClass="fieldlabel"></asp:Label>
                                                    <asp:TextBox ID="TXBtemplateName" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="right">
                                                    <span class="icons">
                                                    </span>
                                                </div>
                                                <div class="clearer"></div>
                                            </div>
                                            <div class="footer">
                                                <div class="choseselect clearfix">
                                                    <div class="left">
                                                        <asp:Label ID="LBtemplateSubmitters_t" runat="server" AssociatedControlID="SLBsubmitters" CssClass="Titolo_campo fieldlabel"></asp:Label>
                                                        <select runat="server" id="SLBsubmitters" class="partecipants chzn-select filtered" multiple tabindex="2">
                                                                        
                                                        </select>
                                                    </div>
                                                    <div class="right">
											            <span class="icons">
												            <span class="icon selectall filtered" title="All" runat="server" id="SPNsubmittersSelectAll">&nbsp;</span><span class="icon selectnone filtered" title="None" runat="server" id="SPNsubmittersSelectNone">&nbsp;</span>
											            </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <hr/>
                                            <div class="fieldobject">
                                                <CTRL:CTRLtemplateMail id="CTRLtemplate" ContainerLeft="[" ContainerRight="]" runat="server" AllowCopyToSender="False" AllowNotifyToSender="False" 
                                                  MustValidate="false" RaiseUpdateEvent="false" 
                                                 AllowFormatSelection="False"  />
                                                <div class="fieldrow clearfix">
                                                    <div class="right">
                                                        <asp:Button ID="BTNundo" runat="server" CommandName="undoAdd" />
                                                        <asp:Button ID="BTNpreviewMailFromTemplate" runat="server" CommandName="preview" />
                                                        <asp:Button ID="BTNaddNewTemplate" runat="server" CommandName="addtemplate" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <asp:Repeater id="RPTtemplates" runat="server">
                                        <ItemTemplate>
                                            <asp:Literal ID="LTidTemplate" runat="server" Visible="false"></asp:Literal>
                                            <li class="template" runat="server" id="LItemplateRow">
                                                <div class="templatecontent">
                                                    <div class="title clearfix" id="DVinEditingTitle" runat="server" visible="false">
                                                        <div class="left">
                                                            <asp:Label ID="LBtemplateEditTitle" runat="server" CssClass="templatetitle"></asp:Label>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="clearfix" id="DVinEditingHeader" runat="server">
                                                        <div class="left">
                                                            <asp:Label ID="LBtemplateNameHeader_t" runat="server" AssociatedControlID="TXBtemplateNameHeader" CssClass="fieldlabel"></asp:Label>
                                                            <asp:TextBox ID="TXBtemplateNameHeader" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">
                                                    
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="title clearfix" id="DVreadOnlyHeader" runat="server">
                                                        <div class="left">
                                                            <asp:Label ID="LBtemplateName_t" runat="server" AssociatedControlID="TXBtemplateName"></asp:Label>
                                                            <asp:TextBox ID="TXBtemplateName" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="right">
                                                            <span class="icons">
                                                                <asp:Button id="BTNeditTemplate" runat="server" CommandName="edit" CssClass="icon edit" />
                                                                <asp:Button id="BTNdeleteTemplate" runat="server" CommandName="virtualdelete" CssClass="icon delete" />
                                                            </span>
                                                        </div>
                                                        <div class="clearer"></div>
                                                    </div>
                                                    <div class="footer">
                                                        <div class="choseselect clearfix">
                                                            <div class="left">
                                                                <asp:Label ID="LBtemplateSubmitters_t" runat="server" AssociatedControlID="SLBsubmitters" CssClass="Titolo_campo"></asp:Label>
                                                                <select runat="server" id="SLBsubmitters" class="partecipants chzn-select filtered" multiple tabindex="2">
                                                                        
                                                                </select>
                                                            </div>
                                                            <div class="right">
											                    <span class="icons">
												                    <span class="icon selectall filtered" title="All" runat="server" id="SPNsubmittersSelectAll">&nbsp;</span><span class="icon selectnone filtered" title="None" runat="server" id="SPNsubmittersSelectNone">&nbsp;</span>
											                    </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr runat="server" id="HRediting" visible="false"/>
                                                    <div class="fieldobject" id="DVediting" runat="server" visible="false">
                                                        <CTRL:CTRLtemplateMail id="CTRLtemplate" ContainerLeft="[" ContainerRight="]" runat="server" AllowCopyToSender="False" AllowNotifyToSender="False" 
                                                         MustValidate="false" RaiseUpdateEvent="false" AllowFormatSelection="False"  />
                                                        <div class="fieldrow clearfix">
                                                            <div class="right">
                                                                <asp:Button ID="BTNundo" runat="server" CommandName="undoAdd" />
                                                                <asp:Button ID="BTNpreviewMailFromTemplate" runat="server" CommandName="preview" />
                                                                <asp:Button ID="BTNsaveTemplate" runat="server" CommandName="savetemplate" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <!-- @End TEMPLATES -->
                            </div>
                        </div>
                        <div class="footer">
                            <div class="DivEpButton">
                                <asp:HyperLink ID="HYPbackBottom" runat="server" CssClass="Link_Menu" Text="Back"></asp:HyperLink>
                                <asp:HyperLink ID="HYPpreviewCallBottom" runat="server" CssClass="Link_Menu" Text="*Preview" Visible="false" Target="_blank"></asp:HyperLink>
                                <asp:button ID="BTNsaveTemplatesBottom" runat="server" Text="Save"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
    <div class="view-modal view-previewmail" id="DVpreview" runat="server" visible="false">
        <CTRL:CTRLmailMessage ID="CTRLmailpreview" runat="server" AllowSendMail="true" DisplayTopWindowCloseButton="true" EditAddressTo="true" />
        <div class="fieldobject clearfix">
            <div class="fieldrow right">
                <asp:Button ID="BTNcloseMailMessageWindow" runat="server" CssClass="Link_Menu" />
            </div>
        </div>
    </div>
</asp:Content>