<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_EditBody.ascx.vb" Inherits="Comunita_OnLine.UC_EditBody" %>

<%@ Register Src="~/Modules/Common/Editor/UC_Editor.ascx" TagName="CTRLeditor" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/DocTemplate/Uc/UC_EditVersions.ascx" TagName="CTRLprevVersion" TagPrefix="CTRL" %>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $(".addTextTelerik").click(function () {
            var editor = $find("<%=EditorClientId%>");
            //var editor = $("#" + "<%--=EditorClientId--%>");
            var add = $(this).attr("rel");
            editor.pasteHtml(add);
            <% = HTMLonKeyUpScript%>
            return false;
        });
    });
</script>
<fieldset class="light">
	<legend>
        <asp:literal ID="LITtag_t" runat="server"></asp:literal>
    </legend>
    <div class="fieldobject">
        <div class="fieldrow code">
            <asp:Label ID="LBLcode_t" cssclass="fieldlabel" runat="server">Cod.</asp:Label>&nbsp;
            <asp:Label ID="LBLcode" cssclass="fieldlabel" runat="server">##</asp:Label>&nbsp;
	    </div>
        <div class="fieldrow">
            <asp:Repeater ID="RPTmodules" runat="server">                
                <ItemTemplate>
                    <div class="buttonwrapper">
                        <asp:Label ID="LBmoduleName" CssClass="fieldlabel" runat="server" Text="<%#Container.DataItem.Name %>" ></asp:Label>
                        <asp:Repeater ID="RPTplaceHolder" runat="server" DataSource="<%#Container.DataItem.Attributes%>" OnItemDataBound="RPTplaceHolder_ItemDataBound">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:Button ID="BTNattribute" runat="server" CausesValidation="false" />
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="fieldrow">
            <asp:Label ID="LBLbodyText_t" runat="server" CssClass="fieldlabel">*Body Text</asp:Label>
            <CTRL:CTRLeditor id="CTRLvisualEditorText" runat="server" ContainerCssClass="containerclass" 
                LoaderCssClass="loadercssclass" EditorHeight="350px" AllAvailableFontnames="false"
                 AutoInitialize="true" ModuleCode="SRVDOCT" MaxHtmlLength="800000">
            </CTRL:CTRLeditor>
        </div>
    </div>
</fieldset> 
<asp:Panel ID="PNLsubVersion" runat="server" Visible="false">
    
    <fieldset class="light">
	    <legend>
            <asp:literal ID="LITrevision_t" runat="server">*Revision</asp:literal>
        </legend>
        <div class="fieldrow">
            <CTRL:CTRLprevVersion ID="UCprevVersion" runat="server" />
        </div>
    </fieldset>
</asp:Panel>