<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="DiaryItem.aspx.vb" Inherits="Comunita_OnLine.DiaryItem" %>

<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/CommunityDiary/UC/UC_CommunityDiaryAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="./../../Graphics/Modules/CommunityDiary/css/CommunityDiary.css" />
    <script type="text/javascript" src="./../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" Visible="false" />
    <script type="text/javascript" src="./../../Jscript/Modules/CommunityDiary/CommunityDiary.js"></script>
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="communitydiary diaryitem">
        <div class="fieldobject commands clearfix">
            <div class="fieldrow left">&nbsp;</div>
            <div class="fieldrow right" id="Div1" runat="server">
                <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu" Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
                <CTRL:AttachmentsCommands ID="CTRLcommandsTop" runat="server" Visible="false" />
                <asp:LinkButton CssClass="Link_Menu" runat="server" Text="Save" ID="LNBsaveItem" CausesValidation="false" Visible="false"></asp:LinkButton>
            </div>
        </div>
        <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
        <asp:MultiView ID="MLVitemData" runat="server" ActiveViewIndex="0">
            <asp:View ID="VIWdata" runat="server">
                <div class="fieldobject">
                    <div class="fieldrow">
                        <asp:Label ID="LBtitle_t" runat="server" CssClass="fieldlabel">Title:</asp:Label>
                        <asp:TextBox ID="TXBtitle" MaxLength="500" Columns="90" CssClass="fieldinput" runat="server" Width="730px"></asp:TextBox>
                    </div>
                    <div class="fieldrow">
                        <asp:Label ID="LBvisibleTo_t" runat="server" CssClass="fieldlabel">Visible To:</asp:Label>
                        <div class="inlinewrapper">
                            <span class="inputgroup">
                                <asp:RadioButtonList ID="RBLvisibleTo" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Selected="true" Value="True">All</asp:ListItem>
                                    <asp:ListItem Value="False">Only by owner</asp:ListItem>
                                </asp:RadioButtonList>
                            </span>
                        </div>
                    </div>
                    <div class="fieldrow dates clearfix">
                        <asp:UpdatePanel ID="UDPdata" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div runat="server" id="DIVinsertData" visible="false" class="clearfix">
                                    <div class="fieldrow">
                                        <asp:Label ID="LBrepeat_t" runat="server" CssClass="fieldlabel">Repeat:</asp:Label>
                                        <div class="inlinewrapper">
                                            <span class="inputgroup">
                                                <asp:RadioButtonList ID="RBLrepeat" runat="server" CssClass="Testo_campoSmall" RepeatDirection="Horizontal"
                                                    RepeatLayout="Flow" AutoPostBack="true">
                                                    <asp:ListItem Selected="true" Value="0">None</asp:ListItem>
                                                    <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </span>
                                        </div>
                                    </div>
                                    <div style="text-align: left; clear: both;" id="DIVweekly" runat="server" visible="false" class="fieldrow">
                                        <div>
                                            <asp:Label ID="LBfromDate_t" runat="server" CssClass="fieldlabel" AssociatedControlID="RDPstartDay">From:</asp:Label>
                                            <div class="inlinewrapper">
                                                <span class="inputgroup">
                                                    <telerik:RadDatePicker ID="RDPstartDay" runat="server" />
                                                    <asp:Label ID="LBtoDate_t" runat="server" CssClass="" AssociatedControlID="RDPendDay">To:</asp:Label>&nbsp;
                                                    <telerik:RadDatePicker ID="RDPendDay" runat="server" />
                                                    <asp:CompareValidator ID="CMVdate" runat="server" ControlToCompare="RDPstartDay" ControlToValidate="RDPendDay" Type="Date" Operator="GreaterThan" ErrorMessage="<"></asp:CompareValidator>
                                                    <asp:CheckBox ID="CBXshowDataInfoWeek" runat="server" CssClass="Testo_campoSmall" />
                                                </span>
                                            </div>
                                        </div>
                                        <div style="text-align: left; clear: both;">
                                            <span class="fieldlabel">&nbsp;</span>
                                            <div class="inlinewrapper boxed">
                                                <asp:Label ID="LBinfo" runat="server" CssClass="description">
						                            <P>	The system creates a week of repeats in the days and times indicated taking as a start date and end date selected by those two calendars
                                                    above. Then select only the days that interest you and indicate the start and end times corresponding lesson.</P>
                                                </asp:Label>
                                                <asp:Repeater ID="RPTweek" runat="server">
                                                    <HeaderTemplate>
                                                        <div style="text-align: left; clear: both;">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="text-align: left; height: 22px; clear: both;">
                                                            <div style="float: left; width: 30px;">
                                                                <asp:CheckBox ID="CBXday" runat="server" />
                                                            </div>
                                                            <div style="float: left; width: 80px;">
                                                                <asp:Label ID="LBday" runat="server"></asp:Label>
                                                                <asp:Literal ID="LTdaynumber" runat="server" Visible="false"></asp:Literal>
                                                            </div>
                                                            <div style="float: left;">
                                                                <asp:Label ID="LBfromHour_t" runat="server" CssClass="Titolo_campoSmall">From:</asp:Label>
                                                                <asp:DropDownList ID="DDLstartHour" runat="server" CssClass="Testo_campoSmall">
                                                                    <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                                    <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                    <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                    <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                    <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                    <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                    <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                    <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                    <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;:&nbsp;
                                                                <asp:DropDownList ID="DDLstartMinute" runat="server" CssClass="Testo_campoSmall">
                                                                    <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                    <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div style="float: left; padding-left: 5px;">
                                                                <asp:Label ID="LBtoHour_t" runat="server" CssClass="Titolo_campoSmall">To:</asp:Label>
                                                                <asp:DropDownList ID="DDLendHour" runat="server" CssClass="Testo_campoSmall">
                                                                    <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                                    <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                                    <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                    <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                                    <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                                    <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                                    <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                                    <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                                    <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                                    <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;:&nbsp;
                                                                <asp:DropDownList ID="DDLendMinute" runat="server" CssClass="Testo_campoSmall">
                                                                    <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                                    <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="DIVeditData" class="fieldrow clearfix">
                                    <asp:Label ID="LBdate" runat="server" CssClass="fieldlabel" AssociatedControlID="RDPday">Day:</asp:Label>
                                    <div class="inlinewrapper">
                                        <asp:CheckBox ID="CBXshowDataInfo" runat="server" CssClass="inputgroup" />
                                        <telerik:RadDatePicker ID="RDPday" runat="server" />
                                        <span class="inputgroup">
                                            <asp:Label ID="LBfrom_t" runat="server" CssClass="">From:</asp:Label>
                                            <asp:DropDownList ID="DDLstartHour" runat="server" CssClass="Testo_campoSmall">
                                                <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;:&nbsp;
                                            <asp:DropDownList ID="DDLstartMinute" runat="server" CssClass="Testo_campoSmall">
                                                <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span class="inputgroup">&nbsp;
                                            <asp:Label ID="LBto_t" runat="server" CssClass="">To:</asp:Label>
                                            <asp:DropDownList ID="DDLendHour" runat="server" CssClass="Testo_campoSmall">
                                                <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                                <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                                <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                                <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                                <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                                <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                                <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;:&nbsp;
                                            <asp:DropDownList ID="DDLendMinute" runat="server" CssClass="Testo_campoSmall">
                                                <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span class="inputgroup">
                                            <asp:Button ID="BTNsavedData" runat="server" Text="Start Date" CssClass="PulsanteFiltro"></asp:Button>
                                        </span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="fieldrow">
                        <asp:Label ID="LBplace_t" runat="server" CssClass="fieldlabel">Place:</asp:Label>
                        <div class="inlinewrapper">
                            <asp:TextBox ID="TXBplace" runat="server" MaxLength="200" Columns="90" Height="40px" Width="730px" CssClass="fieldinput" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="fieldrow textarea">
                        <asp:Label ID="LBlink_t" runat="server" CssClass="fieldlabel">Links:<br/>(separated by a comma)</asp:Label>
                        <div class="inlinewrapper">
                            <asp:TextBox ID="TXBlink" runat="server" MaxLength="200" Columns="90" Height="40px" Width="730px" CssClass="fieldinput" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="fieldrow">
                        <asp:Label ID="LBdescription_t" runat="server" CssClass="fieldlabel">Description:</asp:Label>
                        <div class="inlinewrapper">
                            <CTRL:CTRLeditor ID="CTRLvisualEditorDescription" runat="server" ContainerCssClass="containerclass"
                                LoaderCssClass="loadercssclass" EditorHeight="450px" EditorWidth="730px" AllAvailableFontnames="true"
                                AutoInitialize="true" ModuleCode="SRVlez"></CTRL:CTRLeditor>
                        </div>
                    </div>
                    <div class="fieldrow">
                        <asp:Label ID="LBnote_t" runat="server" CssClass="fieldlabel">Note:</asp:Label>
                        <div class="inlinewrapper">
                            <CTRL:CTRLeditor ID="CTRLvisualEditorNote" runat="server" ContainerCssClass="containerclass"
                                LoaderCssClass="loadercssclass" EditorHeight="180px" EditorWidth="730px" AllAvailableFontnames="true"
                                AutoInitialize="true" ModuleCode="SRVlez"></CTRL:CTRLeditor>
                        </div>
                    </div>
                    <div id="DIVfiles" runat="server" class="fieldrow">
                        <span class="fieldlabel">
                            <asp:Literal ID="LTitemFiles_t" runat="server">Diary Item's files</asp:Literal>
                        </span>
                        <div class="inlinewrapper">
                            <CTRL:AttachmentsCommands ID="CTRLattachmentsCommandsBottom" runat="server" Visible="false" />
                        
                            <div class="tablewrapper">
                                <asp:Repeater ID="RPTitemFiles" runat="server">
                                    <HeaderTemplate>
                                        <table class="table minimal attachedfiles fullwidth edititem<%=GetAttachmentsCssClass() %>">
                                            <thead>
                                                <tr>
                                                    <th class="filename">&nbsp;</th>                           
                                                    <th class="visibilitylesson" id="THvisibilityLesson" runat="server"><asp:Literal ID="LTvisibilityLessonHeader" runat="server">*Item</asp:Literal></th>
                                                    <th class="visibilityrepository" id="THvisibilityRepository" runat="server"><asp:Literal ID="LTvisibilityRepositoryHeader" runat="server">*Community</asp:Literal></th>
                                                    <th class="actions" id="THactions" runat="server"><asp:Literal ID="LTattachmentactionsHeader" runat="server">*Actions</asp:Literal></th>
                                                </tr>
                                            </thead>
                                            <tbody>                                                                                                                            
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="filename">
                                                <CTRL:DisplayItem ID="CTRLdisplayItem" runat="server" EnableAnchor="true" DisplayExtraInfo="false" DisplayLinkedBy="false" />
                                            </td>
                                            <td class="visibilitylesson" id="TDvisibilityLesson" runat="server">
                                                <asp:Literal ID="LTvisibleForItem" runat="server"></asp:Literal>
                                            </td>
                                            <td class="visibilityrepository" id="TDvisibilityRepository" runat="server">
                                                <asp:Literal ID="LTvisibleForRepository" runat="server"></asp:Literal>
                                            </td>
                                            <td class="actions" id="TDactions" runat="server">
                                                <COL:DialogLinkButton runat="server" id="LNBhide" Visible="false" Text="*" CausesValidation="false" CommandName="editvisibility" CssClass="ROW_ItemLink_Small"></col:DialogLinkButton>
                                                <span class="icons">
                                                    <asp:LinkButton ID="LNBunlinkAttachment" runat="server" CssClass="icon unlink" Visible="false" CommandName="unlink" CommandArgument='<%#Container.DataItem.Attachment.Id %>'></asp:LinkButton>
                                                    <asp:LinkButton ID="LNBvirtualDeleteAttachment" runat="server" Visible="false" CssClass="icon virtualdelete" CommandName="virtualdelete" CommandArgument='<%#Container.DataItem.Attachment.Id%>'>&nbsp;</asp:LinkButton>
                                                    <asp:LinkButton ID="LNBrecoverAttachment" runat="server" Visible="false" CssClass="icon recover" CommandName="recover" CommandArgument='<%#Container.DataItem.Attachment.Id%>'>&nbsp;</asp:LinkButton>
                                                    <asp:LinkButton ID="LNBdeleteAttachment" runat="server" CssClass="icon delete needconfirm" Visible="false" CommandName="delete" CommandArgument='<%#Container.DataItem.Attachment.Id %>'></asp:LinkButton>
                                                    <asp:HyperLink ID="HYPdownload" runat="server" Text="D" CssClass="icon download" Visible="false" />
                                                    <asp:HyperLink ID="HYPstats" runat="server" Text="S" CssClass="icon stats" Visible="false" />
                                                    <asp:HyperLink ID="HYPeditMetadata" runat="server" Text="M" CssClass="icon editmetadata" Visible="false" />
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="VIWempty" runat="server"></asp:View>
        </asp:MultiView>
    </div>
    <CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" DisplayInfo="false"/>
    <CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity" DisplayInfo="false"/>
    <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity" DisplayInfo="false"/>
    <COL:Dialog runat="server" id="DLGmoduleFileItemVisibility" DialogClass="moduleItemVisibility" ServerSideCancel="false"></COL:Dialog>
    <COL:Dialog runat="server" id="DLGrepositoryFileItemVisibility" DialogClass="repositoryItemVisibility" ServerSideCancel="false"></COL:Dialog>
    <asp:Literal ID="LTtemplateVisibile" runat="server" Visible="false"><div class="visibility itemgreen"><span>#visibility#</span></div></asp:Literal>
    <asp:Literal ID="LTtemplateNotVisibile" runat="server" Visible="false"><div class="visibility itemgray"><span>#visibility#</span></div></asp:Literal>
    <asp:Literal ID="LTtemplateIgnore" runat="server" Visible="false"><div class="visibility itemempty"><span>#visibility#</span></div></asp:Literal>
</asp:Content>