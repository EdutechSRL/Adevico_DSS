<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="ManagementTaskFile.aspx.vb" Inherits="Comunita_OnLine.ManagementTaskFile" %>

<%@ Register TagPrefix="CTRL" TagName="CommunityFile" Src="~/Modules/Repository/UC/UC_CompactFileUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="InternalFileUploader" Src="~/Modules/Repository/UC/UC_CompactInternalFileUploader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="ItemManagementFile" Src="~/Modules/Common/UC/UC_OtherModuleItemFiles.ascx" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        {
            list-style-type: none;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function HideCommunityUpload() {
            $("#<%=Me.DVcommunity.ClientID %>").hide();
            return true;
        }
        function HideWorkBookUpload() {
            ProgressStart();
            $("#<%=Me.DVinternal.ClientID %>").hide();
            return true;
        }
        function ProgressStart() {
            getRadProgressManager().startProgressPolling();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="server">
    <div id="Div1" style="width: 900px; text-align: right; padding-top: 5px; margin: 0px auto;
        clear: both;" runat="server">
        <asp:HyperLink ID="HYPbackToItems" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to items" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPbackToItem" runat="server" CssClass="Link_Menu"
            Visible="false" Text="Back to item" Height="18px"></asp:HyperLink>
        <asp:HyperLink ID="HYPmultipleUpload" runat="server" CssClass="Link_Menu"
            Text="Multiple upload" Height="18px" NavigateUrl="~/TaskList/ItemMultipleUpload.aspx"></asp:HyperLink>
    </div>
    <div>
        <div runat="server" id="DVcommunityLink">
            <b>
                <asp:Literal ID="LTaddFromCommunity_t" runat="server" >Import file from community</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <asp:Literal ID="LTlinkToCommunity" runat="server">In this way you can link one or more community file to this item</asp:Literal>
                &nbsp;&nbsp;&nbsp;<asp:Button ID="BTNlinkCommunityFile" runat="server" Text="Link" />
            </div>
            <br />
            <br />
        </div>
        <div runat="server" id="DVcommunity">
            <b>
                <asp:Literal ID="LTuploadToCommunity_t" runat="server" >Upload file into community and diary item</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                <div style="float: left; width: 700;">
                    <CTRL:CommunityFile ID="CTRLCommunityFile" runat="server" AjaxEnabled="false" UpdatePermissionButton="False" />
                </div>
                <div style="float: left; width: 100; padding-top: 230px; padding-left:10px;">
                    <span style="vertical-align:bottom;">
                    <asp:Button ID="BTNaddCommunityFile" runat="server" Text="Link" /></span>
                </div>
            </div>
        </div>
        <div runat="server" visible="false" id="DVinternal" style="text-align:left;  clear:both;">
            <br />
            <br />
            <b>
                <asp:Literal ID="LTuploadToDiaryItem_t" runat="server"> Upload file ONLY into diary item</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px; padding-top: 25px;">
                <div style="float: left; width: 700;">
                    <CTRL:InternalFileUploader ID="CTRLinternalFileUploader" runat="server" AjaxEnabled="false" ViewTypeSelector="true" />
                </div>
                 <div style="float: left; width: 100; padding-top: 10px; padding-left:10px;">
                    <span style="vertical-align:bottom;">
                        <asp:Button ID="BTNaddToItem" runat="server" Text="Link" />
                    </span>
                </div>
            </div>
            <br />
            <br />
        </div>
        <div id="DVfileList" style="text-align:left; clear:both;">
            <br />
            <b>
                <asp:Literal ID="LTitemFiles_t" runat="server">Diary Item's files</asp:Literal></b>
            <hr />
            <div style="width: 900px; padding-bottom: 10px;">
                 <CTRL:ItemManagementFile ID="CTRLItemManagementFile" runat="server" />
            </div>
        </div>
    </div>
    <div id="confirmAction">
        <asp:UpdatePanel ID="UDPaddRequestedFile" UpdateMode="Conditional" ChildrenAsTriggers="true"
            runat="server">
            <ContentTemplate>
                <asp:Literal ID="LTrequestedFileId" runat="server" Visible="false"></asp:Literal>
                <asp:MultiView ID="MLVrequestedFile" runat="server" ActiveViewIndex="0">
                    <asp:View ID="VIWrequestedFileEmpty" runat="server">
                    </asp:View>
                    <asp:View ID="VIWrequestedFileAdd" runat="server">
                        <div class="columnsEdit">
                            <div class="column left w20">
                                &nbsp;
                            </div>
                            <div class="column right w70">
                                <asp:Label ID="LBrequestedFileInfo" runat="server" CssClass="testo_campoSmall"></asp:Label>
                            </div>
                        </div>
                        <div class="columnsEdit">
                            <div class="column left w20">
                                <asp:Label ID="LBrequestedFileTitle" runat="server" CssClass="Titolo_campoSmall"
                                    AssociatedControlID="TXBrequestedFile">File da allegare:</asp:Label>
                            </div>
                            <div class="column right w70">
                                <asp:TextBox ID="TXBrequestedFile" runat="server" CssClass="w90 testo_campoSmall"
                                    Columns="60"></asp:TextBox>
                            </div>
                        </div>
                        <div class="columnsEdit">
                            <div class="column left w20">
                                <asp:Label ID="LBrequestedFileSubmitters" runat="server" CssClass="Titolo_campoSmall"
                                    AssociatedControlID="CBLsubmittersType">Associa a:</asp:Label>
                            </div>
                            <div class="column right w70">
                                <asp:CheckBoxList ID="CBLsubmittersType" runat="server" RepeatDirection="Horizontal"
                                    RepeatColumns="2" RepeatLayout="flow" CssClass="partecipant">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="columnsEdit">
                            <div class="column left w20">
                                <asp:Label ID="LBrequestedFileMandatoryTitle" runat="server" CssClass="Titolo_campoSmall"
                                    AssociatedControlID="CBXmandatory">Obbligatorio:</asp:Label>
                            </div>
                            <div class="column right w70">
                                <asp:CheckBox ID="CBXmandatory" runat="server" />
                            </div>
                        </div>
                        <div class="columnsEdit">
                            <div class="column left w20">
                            </div>
                            <div class="column right w70">
                                <asp:Button ID="BTNundoAddRequestedFile" runat="server" Text="Chiudi" OnClientClick="closeDialog('addRequestedFile');return false;"
                                    CssClass="Link_Menu" />
                                <asp:Button ID="BTNsaveRequestedFile" runat="server" Text="Aggiungi" CssClass="Link_Menu"
                                    CausesValidation="false" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="VIWrequestedFileRemove" runat="server">
                        <div class="columnsEdit">
                            <div class="column left w15">
                            </div>
                            <div class="column right w80">
                                <asp:Label ID="LBconfirmRequestedFileDelete" runat="server" CssClass="Testo_campoSmall"></asp:Label>
                            </div>
                        </div>
                        <div class="columnsEdit">
                            <div class="column left w15">
                            </div>
                            <div class="column right w80">
                                <asp:Button ID="BTNundoRequestedFileRemove" runat="server" Text="Undo" OnClientClick="closeDialog('addRequestedFile');return false;"
                                    CssClass="Link_Menu" />
                                <asp:Button ID="BTNconfirmRequestedFileRemove" runat="server" Text="Cancella" CssClass="Link_Menu"
                                    CausesValidation="false" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
