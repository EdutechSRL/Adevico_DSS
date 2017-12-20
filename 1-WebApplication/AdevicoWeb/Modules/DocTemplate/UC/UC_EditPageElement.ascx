<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditPageElement.ascx.vb" Inherits="Comunita_OnLine.UC_EditPageElement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<%@ Register Src="~/UC/Editor/UC_VisualEditor.ascx" TagName="CTRLvisualEditor" TagPrefix="CTRL" %>--%>
<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>

<%@ Register TagPrefix="CTRL" TagName="Image" Src="~/Modules/DocTemplate/Uc/UC_EditImage.ascx" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        $("#<%=BTNpgtagCurPage.ClientId %>").click(function () {

            var add = $(this).attr("rel");
            var editor = $find("<%=EditorClientId%>");
            editor.pasteHtml(add);
            return false;
        });

        $("#<%=BTNpgtagCreateDate.ClientId %>").click(function () {

            var add = $(this).attr("rel");
            var editor = $find("<%=EditorClientId%>");
            editor.pasteHtml(add);
            return false;
        });

        $("#<%=BTNpgtagCreateTime.ClientId %>").click(function () {

            var add = $(this).attr("rel");
            var editor = $find("<%=EditorClientId%>");
            editor.pasteHtml(add);
            return false;
        });
        
    });
</script>

<div class="fieldrow right code">
    <asp:Label ID="LBLcode_t" cssclass="fieldlabel" runat="server">Cod.</asp:Label>&nbsp;
    <asp:Label ID="LBLcode" cssclass="fieldlabel" runat="server">##</asp:Label>&nbsp;
</div>

<div class="fieldrow">
    <asp:Label ID="LBLalignment_t" runat="server" CssClass="fieldlabel">Alignment</asp:Label>
    <asp:RadioButtonList ID="RBLalignment" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="fieldblockwrapper">
        <asp:ListItem Text="#Left" Value="-1"></asp:ListItem>
        <asp:ListItem Text="#Center" Value="0"></asp:ListItem>
        <asp:ListItem Text="#Right" Value="1"></asp:ListItem>
    </asp:RadioButtonList>
</div>

<div class="fieldrow">
    <asp:Label ID="LBLtype_t" runat="server" CssClass="fieldlabel">Type</asp:Label>
    <asp:RadioButtonList ID="RBLtype" runat="server" AutoPostBack="true" RepeatDirection="Vertical" RepeatLayout="Flow" CssClass="fieldblockwrapper">
        <asp:ListItem Text="none" Value="none"  Selected="True"></asp:ListItem>
        <asp:ListItem Enabled="true" Text="txt" Value="txt"></asp:ListItem>
        <asp:ListItem Text="Image" Value="img"></asp:ListItem>
    </asp:RadioButtonList>
</div>

<div class="fieldrow">
    <asp:MultiView ID="MLVtype"  runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWnone" runat="server"></asp:View>
        <asp:View ID="VIWtext" runat="server">
            <div class="fieldrow">
                <asp:Label ID="LBLpageTag_t" runat="server" CssClass="fieldlabel ModuleName">#Page tag:</asp:Label>
                <div class="fieldblockwrapper editor">
                    <asp:Button ID="BTNpgtagCurPage" runat="server" CausesValidation="false" Text="test"/>
                    <asp:Button ID="BTNpgtagCreateDate" runat="server" CausesValidation="false" Text="test"/>
                    <asp:Button ID="BTNpgtagCreateTime" runat="server" CausesValidation="false" Text="test"/>
                    <%--[Document.CreateDate] [Document.PageCurrent] --%>
                    <br />
<%--                    <CTRL:CTRLvisualEditor ID="CTRLvisualEditorText" runat="server" FontNames="Verdana"
                        FontSizes="2,3,4" ToolsFile="~/RadControls/Editor/Localization/it-IT/ToolsAdvancedEvents.xml"
                        ShowScrollingSpeed="false" ShowAddDocument="false" ShowAddImage="false" ShowAddSmartTag="true"
                        EditorEnabled="true" AllowPreview="false" EditorHeight="180px" DisabledTags="youtube,slideshare"
                        EditorMaxChar="4000" />--%>
                        <CTRL:CTRLeditor id="CTRLvisualEditorText" runat="server" ContainerCssClass="containerclass" 
                            LoaderCssClass="loadercssclass" EditorHeight="350px" AllAvailableFontnames="false"
                            AutoInitialize="true" ModuleCode="SRVDOCT" MaxHtmlLength="4000">
                        </CTRL:CTRLeditor>
                </div>
            </div>
        </asp:View>
        <asp:View ID="VIWimage" runat="server">
            <CTRL:Image Id="UCimage" runat="server" ShowMeasure="True"></CTRL:Image>
        </asp:View>
    </asp:MultiView>
</div>