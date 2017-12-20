<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_PrintSettings.ascx.vb" Inherits="Comunita_OnLine.UC_PrintSettings" %>

<%@ Register Src="~/Modules/CallForPapers/Edit/UC/UC_FontSettings.ascx" TagPrefix="CTRL" TagName="FontSettings" %>

<%@ Register Src="~/Modules/CallForPapers/UC/UC_PrintDraft.ascx" TagPrefix="CTRL" TagName="PrintDraft" %>

<%@ Register TagPrefix="CTRL" TagName="Template" Src="~/Modules/DocTemplate/UC/UC_TemplateAssociation.ascx" %>


<div class="fieldrow fieldtemplate">
    <asp:Label ID="LBtemplate_t" runat="server" CssClass="Titolo_campo fieldlabel">*Template:</asp:Label>
    <CTRL:template ID="CTRLtemplate"  runat="server" EnabledSelectedIndexChanged="False" AllowPreview="False" />
    <asp:linkbutton ID="LKBpreview" runat="server" Visible="False">*Preview</asp:linkbutton>
</div>

<hr/>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBunselected_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="RBLhiddenFields">*Unselect field:</asp:Label>
    <asp:RadioButtonList ID="RBLhiddenFields" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
        <asp:ListItem Text="*Hide All" Value="0" Selected="True"></asp:ListItem>
        <asp:ListItem Text="*Show All" Value="1"></asp:ListItem>
    </asp:RadioButtonList>
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBlayout_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="DDLlayout">*Layout:</asp:Label>
<%--    <asp:RadioButtonList ID="RBLlayout" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
        <asp:ListItem Text="*Standard" Value="0" Selected="True"></asp:ListItem>
        <asp:ListItem Text="*LeftRight" Value="1"></asp:ListItem>
    </asp:RadioButtonList>--%>
    <asp:dropdownlist ID="DDLlayout" runat="server">
        <asp:ListItem Text="Default" Value="0" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Left Right" Value="1"></asp:ListItem>
        <asp:ListItem Text="Left Right 10-90" Value="19"></asp:ListItem>
        <asp:ListItem Text="Left Right 20-80" Value="28"></asp:ListItem>
        <asp:ListItem Text="Left Right 30-70" Value="37"></asp:ListItem>
        <asp:ListItem Text="Left Right 40-60" Value="46"></asp:ListItem>
        <asp:ListItem Text="Left Right 50-50" Value="55"></asp:ListItem>
        <asp:ListItem Text="Left Right 60-40" Value="64"></asp:ListItem>
        <asp:ListItem Text="Left Right 70-30" Value="73"></asp:ListItem>
        <asp:ListItem Text="Left Right 80-20" Value="82"></asp:ListItem>
        <asp:ListItem Text="Left Right 90-10" Value="91"></asp:ListItem>
    </asp:dropdownlist>
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBhighlightMandatory" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CBXmandatory">*Mandatory field:</asp:Label>
    <asp:checkbox ID="CBXmandatory" runat="server" Text="*Highlight" Checked="false" />
</div>

<hr/>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBLSectionTitle_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CTRL_SectionTitleFont">*Font titolo sezione:</asp:Label>
    <CTRL:FontSettings runat="server" id="CTRL_SectionTitleFont" />
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBLsectionDesc_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CTRL_SectionDescFont">*Font descrizione sezione:</asp:Label>
    <CTRL:FontSettings runat="server" id="CTRL_SectionDescFont" />
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBLFieldTitle_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CTRL_FieldTitle">*Font titolo campo:</asp:Label>
    <CTRL:FontSettings runat="server" id="CTRL_FieldTitle" />
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBLFieldDesc_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CTRL_FieldDesc">*Font descrizione campo:</asp:Label>
    <CTRL:FontSettings runat="server" id="CTRL_FieldDesc" />
</div>

<div class="fieldrow fieldhidefield">
    <asp:Label ID="LBLFieldEntry_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CTRL_FieldEntry">*Font compilazione campo:</asp:Label>
    <CTRL:FontSettings runat="server" id="CTRL_FieldEntry" />
</div>

<hr/>

    <div class="fieldrow fieldhidefield">
        <asp:Label ID="LBprintDraft_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="CBXpermitDraft">*Stampa bozza:</asp:Label>
        <asp:checkbox ID="CBXpermitDraft" runat="server" Checked="False" Text="Permetti agli utenti" />
    </div>

    <div class="fieldrow fieldhidefield">
        <asp:Label ID="LBwatermark_t" runat="server" CssClass="Titolo_campo fieldlabel" AssociatedControlID="TXBwatermarkText">*Testo filigrana:</asp:Label>
        <asp:textbox ID="TXBwatermarkText" runat="server">Draft</asp:textbox>
        <%--<asp:linkbutton ID="LKBprintDraft" runat="server">*Print draft</asp:linkbutton>--%>
        <CTRL:PrintDraft runat="server" ID="CTRL_PrintDraft" />
    </div>





<asp:HiddenField runat="server" ID="HDFcallId"/>