<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Editor.ascx.vb" Inherits="Comunita_OnLine.UC_Editor" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="CTRL" TagName="LiteEditor" Src="~/Modules/Common/Editor/Type/UC_LiteEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TelerikEditor" Src="~/Modules/Common/Editor/Type/UC_TelerikEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TextareaEditor" Src="~/Modules/Common/Editor/Type/UC_TextareaEditor.ascx" %>
<%@ Register TagPrefix="CTRL" TagName="TinyEditor" Src="~/Modules/Common/Editor/Type/UC_TinyEditor.ascx" %>

<div style="text-align:left;" class="editorloader <%=LoaderCssClass %>">
     <asp:MultiView ID="MLVeditor" runat="server" ActiveViewIndex="0">
        <asp:View ID="VIWnone" runat="server">
    
        </asp:View>
        <asp:View ID="VIWtextareaEditor" runat="server">
            <CTRL:TextareaEditor id="CTRLtextarea" runat="server"></CTRL:TextareaEditor>
        </asp:View>
        <asp:View ID="VIWliteEditor" runat="server">
            <CTRL:LiteEditor id="CTRLliteEditor" runat="server"></CTRL:LiteEditor>
        </asp:View>
        <asp:View ID="VIWtinyEditor" runat="server">
             <CTRL:TinyEditor id="CTRLtinyEditor" runat="server"></CTRL:TinyEditor>
        </asp:View>
        <asp:View ID="VIWtelerikEditor" runat="server">
            <CTRL:TelerikEditor id="CTRLtelerikEditor" runat="server"></CTRL:TelerikEditor>
        </asp:View>
    </asp:MultiView>
</div>