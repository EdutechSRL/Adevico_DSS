<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master"
    CodeBehind="CommunityDiary.aspx.vb" Inherits="Comunita_OnLine.CommunityDiary" %>
<%@ Register TagPrefix="COL" TagName="Dialog" Src="~/Modules/EduPath/UC/UCDialog.ascx" %>
<%@ Register TagPrefix="COL" Assembly="Comunita_OnLine" Namespace="Comunita_OnLine.MyUC" %>

<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsCommands" Src="~/Modules/Repository/Common/UC_ModuleAttachmentInlineCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="DisplayItem" Src="~/Modules/Repository/Common/UC_ModuleRenderAction.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Attachments" Src="~/Modules/CommunityDiary/UC/UC_CommunityDiaryAddAttachment.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="AttachmentsHeader" Src="~/Modules/Repository/Common/UC_ModuleAttachmentJqueryHeaderCommands.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ModalPlayerHeader" Src="~/Modules/Repository/UC_New/UC_ModalPlayerHeader.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" href="./../../Graphics/Modules/CommunityDiary/css/CommunityDiary.css" />
    <script type="text/javascript" src="./../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    <CTRL:AttachmentsHeader ID="CTRLattachmentsHeader" runat="server" Visible="false"  />
    <script type="text/javascript" src="./../../Jscript/Modules/CommunityDiary/CommunityDiary.js"></script>
    <CTRL:ModalPlayerHeader ID="CTRLmodalPlayerHeader" runat="server" />
    <!--[if lt IE 9]>
    <style media="print">
        .DIV_MP_Content{
                border:none !important;
            }

            .communitydiary .lessons .lesson .lessonheader .lezcommands *,
            .communitydiary .lessons .lesson .lessonheader .lezfiles *,
            .attachedfiles td.actions *{
                display:none;
            }

            #header, #toolbar, #branding, #serviceinfo{
                display:none !important;
            }

            .communitydiary .lessons .lesson{
                display: inline-table;
                width:100%;
                
            }

	        .hidetoprinter{
                display:none;
            }

            .papershadow{
                box-shadow:none;
            }

            .papershadow:after,
            .papershadow:before{
                content:"";
                box-shadow:none;
            }
    </style>
    <![endif]-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHservice" runat="server">
    <div class="communitydiary diaryitems">
        <div class="fieldobject commands clearfix hidetoprinter">
            <div class="fieldrow left">
                <span class="inputgroup">
                    <asp:Label ID="LBorderby_t" runat="server" CssClass="Titolo_CampoSmall">Order by:&nbsp;</asp:Label>
                    <asp:RadioButtonList ID="RBLorderby" RepeatDirection="Horizontal" CssClass="FiltroCampoSmall"
                        RepeatLayout="Flow" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="0" Selected="True">From First to Last</asp:ListItem>
                        <asp:ListItem Value="1">From Last to First</asp:ListItem>
                    </asp:RadioButtonList>
                </span>
            </div>
            <div class="fieldrow right">
                <asp:HyperLink ID="HYPprintItems" runat="server" CssClass="Link_Menu print" Visible="false" Text="Print Items" Height="18px" Target="_blank"></asp:HyperLink>
                <COL:DialogLinkButton runat="server" id="LNBdeleteItems" Visible="false" Text="Delete diary" CausesValidation="false" CommandName="confirmDeleteDiary" CssClass="Link_Menu"></col:DialogLinkButton>
                <COL:DialogLinkButton runat="server" id="LNBdeleteSelectedItems" Visible="false" Text="Delete selected items" CausesValidation="false" CommandName="confirmDeleteSelectItems" CssClass="Link_Menu"></col:DialogLinkButton>
                <asp:HyperLink ID="HYPimport" runat="server" CssClass="Link_Menu" Visible="false" Text="Import" Height="18px"></asp:HyperLink>
                <asp:HyperLink ID="HYPaddItem" runat="server" CssClass="Link_Menu" Visible="false" Text="Add item" Height="18px"></asp:HyperLink>
            </div>
        </div>
        <CTRL:Messages ID="CTRLmessages" runat="server" Visible="false" />
    <asp:MultiView ID="MLVitems" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWitems" runat="server">
            <h2 class="title"><asp:Label ID="LBtitle" runat="server">Items</asp:Label></h2>
            <div class="lessons">                                
        <asp:Repeater ID="RPTitemsDetails" runat="server">
            <ItemTemplate>
                <div class="lesson papershadow <%# me.BackGroundItem(Container.DataItem,container.itemtype)%>">
                    <div class="lessonheader container_12 clearfix">
                        <div class="lessonheaderitem lezcommands grid_2 alpha">
                            <asp:CheckBox ID="CBXitem" runat="server" />
                            <span class="icons">
                                <asp:HyperLink ID="HYPedit" runat="server" Target="_self" CssClass="icon edit">Gestione materiale</asp:HyperLink>
                                <COL:DialogLinkButton runat="server" id="LNBdeleteItem" CssClass="icon delete" CausesValidation="false"  CommandName="confirmdeleteitem" CommandArgument='<%# Container.DataItem.ID%>'></col:DialogLinkButton>   
                            </span>
                            <span class="lessonid">
                                <a name="item<%# Container.DataItem.LessonNumber.tostring() %>"></a>
                                <a name="<%# Container.DataItem.ID.tostring() %>"></a>
                                    <asp:Label ID="LBlez" runat="server" CssClass="">Lez.&nbsp;</asp:Label>
                                <asp:Literal ID="LTitemID" runat="server" Visible="false" Text='<%# Container.DataItem.ID.tostring()%>'></asp:Literal>
                                <asp:Literal ID="LTidEvent" runat="server" Visible="false" Text='<%# Container.DataItem.IdEvent.tostring()%>'></asp:Literal>
                                <asp:Literal ID="LTlessonNumber" runat="server" Visible="false" Text='<%# Container.DataItem.LessonNumber.tostring()%>'></asp:Literal>
                            </span>
                            &nbsp;
                        </div>
                        <div class="lessonheaderitem lezheader grid_6">                                        
                            <asp:Label ID="LBitemHeader" runat="server" CssClass="lessontitle"></asp:Label>&nbsp;
                        </div>
                        <div class="lessonheaderitem lezfiles grid_4 omega">
                            <CTRL:AttachmentsCommands ID="CTRLcommands" runat="server" Visible="false"  />
                        </div>
                    </div>
                    <div class="lessoncontent">
                        <div class="lessoninner">
                            <div class="fieldobject diaryitem">
                                <div id="DIVtitle" class="fieldrow" runat="server">
                                    <asp:Label ID="LBtitle_t" runat="server" CssClass="fieldlabel">Title:</asp:Label>
                                    <div class="inlinewrapper">
                                        <span class="field">
                                            <%# Container.DataItem.EventItem.Title%>
                                        </span>
                                    </div>
                                </div>
                                <div id="DIVduration" class="fieldrow" runat="server">                                            
                                    <asp:Label ID="LBduration_t" runat="server" CssClass="fieldlabel">Duration:</asp:Label>                                            
                                    <div class="inlinewrapper">
                                        <asp:Label ID="LBduration" runat="server" CssClass="field"></asp:Label>
                                    </div>
                                </div>
                                <div id="DIVplace" class="fieldrow" runat="server">                                                
                                    <asp:Label ID="LBplace_t" runat="server" CssClass="fieldlabel">Place:</asp:Label>                                                
                                    <div class="inlinewrapper">
                                        <span class="field">
                                            <%# Container.DataItem.EventItem.Place%>
                                        </span>
                                    </div>
                                </div>
                                <div id="DIVtext" class="fieldrow" runat="server">                                                
                                    <asp:Label ID="LBdescription_t" runat="server" CssClass="fieldlabel">Description:</asp:Label>                                                
                                    <div class="inlinewrapper">
                                        <div class="field renderedtext">
                                            <asp:Literal ID="LTdescription" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>                                   
                                <div id="DIVlink" class="fieldrow" runat="server">                                                
                                    <asp:Label ID="LBlink" runat="server" CssClass="fieldlabel">Link:</asp:Label>                                                
                                    <div class="inlinewrapper">
                                        <asp:Label ID="LBlinkDisplay" runat="server" CssClass="field links"></asp:Label>
                                    </div>
                                </div>
                                <div id="DIVnote" class="fieldrow" runat="server">                                                
                                    <asp:Label ID="LBnote_t" runat="server" CssClass="fieldlabel">Link:</asp:Label>                                                
                                    <div class="inlinewrapper">
                                        <div class="field renderedtext">
                                            <asp:Literal ID="LTnote" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div id="DIVmateriale" class="fieldrow" runat="server">                                                
                                    <asp:Label runat="server" ID="LBmateriale_t" CssClass="fieldlabel">Materiale:</asp:Label>&nbsp;         
                                    <div class="tablewrapper">
                                        <asp:Repeater ID="RPTitemFiles" runat="server" OnItemCommand="RPTitemFiles_ItemCommand">
                                            <HeaderTemplate>
                                                <table class="table minimal attachedfiles fullwidth <%=GetAttachmentsCssClass() %>">
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
                                                        <COL:DialogLinkButton runat="server" id="LNBhide" Visible="false" Text="Edit" CausesValidation="false" CommandName="editvisibility" CssClass="ROW_ItemLink_Small"></col:DialogLinkButton>
                                                        <span class="icons">
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
                        <div id="DIVitemSeparator" runat="server"></div>
                    </div>
                </div>                               
            </ItemTemplate>
        </asp:Repeater>                
            </div>
            <COL:Dialog runat="server" id="DLGremoveItem" DialogClass="removeItem" ServerSideCancel="false"></COL:Dialog>
            <COL:Dialog runat="server" id="DLGclearItems" DialogClass="deleteDiary" ServerSideCancel="false"></COL:Dialog>
            <COL:Dialog runat="server" id="DLGremoveItems" DialogClass="removeItems" ServerSideCancel="false"></COL:Dialog>
            <COL:Dialog runat="server" id="DLGmoduleFileItemVisibility" DialogClass="moduleItemVisibility" ServerSideCancel="false"></COL:Dialog>
            <COL:Dialog runat="server" id="DLGrepositoryFileItemVisibility" DialogClass="repositoryItemVisibility" ServerSideCancel="false"></COL:Dialog>
        </asp:View>
        <asp:View ID="VIWnoLesson" runat="server">            
            <div class="messages">
                <div class="message info">
                    <asp:Label ID="LBnoLesson" runat="server"></asp:Label>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWerrors" runat="server">
            <CTRL:Messages ID="CTRLerrorMessages" runat="server" Visible="false" />
        </asp:View>
    </asp:MultiView>
   </div>
    <CTRL:Attachments ID="CTRLinternalUpload" runat="server" visible="false" UploaderCssClass="dlguploadtomoduleitem" DisplayInfo="true"/>
    <CTRL:Attachments ID="CTRLcommunityUpload" runat="server" visible="false"  UploaderCssClass="dlguploadtomoduleitemandcommunity" DisplayInfo="true"/>
    <CTRL:Attachments ID="CTRLlinkFromCommunity" runat="server" visible="false"  UploaderCssClass="dlglinkfromcommunity" DisplayInfo="true"/>
    <asp:Literal ID="LTtemplateVisibile" runat="server" Visible="false"><div class="visibility itemgreen"><span>#visibility#</span></div></asp:Literal>
    <asp:Literal ID="LTtemplateNotVisibile" runat="server" Visible="false"><div class="visibility itemgray"><span>#visibility#</span></div></asp:Literal>
    <asp:Literal ID="LTtemplateIgnore" runat="server" Visible="false"><div class="visibility itemempty"><span>#visibility#</span></div></asp:Literal>
</asp:Content>