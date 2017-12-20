<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="SendMessage.aspx.vb" Inherits="Comunita_OnLine.SendMessage" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register TagPrefix="CTRL" TagName="List" Src="~/Modules/Templates/UC/UC_TemplateList.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Send" Src="~/Modules/Templates/UC/UC_MessageEdit.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="MessageEditHeader" Src="~/Modules/Templates/UC/UC_MessageEditHeader.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="Messages" Src="~/Modules/Common/UC/UC_ActionMessages.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Graphics/Modules/TemplateMessages/css/TemplateMessages.css" rel="Stylesheet" />
    <link href="../../Jscript/Modules/Common/Choosen/chosen.css" rel="Stylesheet" />
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.simpleEqualize.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/Choosen/chosen.jquery.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.inputActivator.js"></script>

    <link rel="stylesheet" href="../../Graphics/Plugins/jquery.treeTable/jquery.treeTable.css"/>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.collapsableTreeAdv.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.treeTable.js"></script>
    <script type="text/javascript" src="../../Jscript/Modules/Common/jquery.ddbuttonlist.js"></script>
    
    <script language="javascript" type="text/javascript">
        var ddbuttonlist = false;
        $(function () {
            $("table.treetable").treeTable({ clickableNodeNames: true });
            $(".ddbuttonlist.enabled").dropdownButtonList();
            ddbuttonlist = true;
        })
     </script>
     <CTRL:MessageEditHeader ID="CTRLmessageEditHeader" runat="server" Visible="true" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <asp:MultiView ID="MLVcontent" runat="server">
        <asp:View ID="VIWdefault" runat="server">
            <h3><asp:Literal ID="LTdefaultMessage" runat="server"></asp:Literal></h3>
        </asp:View>
        <asp:View ID="VIWcontent" runat="server">
            <div class="">
                <div class="viewbuttons clearfix" id="DVbackUrl" runat="server">
                    <asp:HyperLink ID="HYPbackUrl" runat="server" Text="*Back" CssClass="Link_Menu" Visible="false"></asp:HyperLink>
                </div>
                <div class="tabswrapper clearfix" id="DVtab" runat="server">
                    <CTRL:Messages ID="CTRLmessages"  runat="server" Visible="false" />
                     <telerik:radtabstrip id="TBSmessages" runat="server" align="Justify" width="100%"
                        height="20px" causesvalidation="false" autopostback="false" skin="Outlook" enableembeddedskins="true">
                        <tabs>
                            <telerik:RadTab text="*Templates" value="List" visible="false"></telerik:RadTab>
                            <telerik:RadTab text="*Send message" value="Send" visible="false"></telerik:RadTab>
                            <telerik:RadTab text="*Messages sent" value="Sent" visible="false"></telerik:RadTab>
                        </tabs>
                    </telerik:radtabstrip>
                </div>
                <asp:MultiView ID="MLVtabcontent" runat="server">
                    <asp:View ID="VIWlist" runat="server">
                        <div class="tabcontent templatelist">
                            <div class="contentwrapper clearfix">
                                <CTRL:List id="CTRLtemplatesList" runat="server" RaiseTemplateSelection="true" RaiseApplyFiltersEvent="true" RaisePageChangedEvent="true" RaiseSessionTimeoutEvent="true"/>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="VIWsend" runat="server">
                        <div class="tabcontent sendmessage">
                            <div class="contentwrapper clearfix">
                                <CTRL:Send id="CTRLsend" runat="server" AllowDelete="true" AllowPreview="true"/>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>