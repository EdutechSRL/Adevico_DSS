<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_InLineInput.ascx.vb" Inherits="Comunita_OnLine.UC_InLineInput" %>
<div class="inlinewrapper">
    <%--<asp:Literal ID="LTspanContainerOpen" runat="server"></asp:Literal>--%>
    <span id="SPNcontainer" runat="server">
        <asp:Label ID="LBinput" runat="server" CssClass="view"></asp:Label>
        <asp:Literal ID="LTspanEditOpen" runat="server"></asp:Literal>
            <input id="INtextboxInput" runat="server" type="text" /><input type="hidden" runat="server" id="HDNinputInEditMode" />
            <asp:RegularExpressionValidator ID="RXVnput" runat="server" SetFocusOnError="true" ControlToValidate="INtextboxInput"  Enabled="false" EnableClientScript="true" > </asp:RegularExpressionValidator>
            <asp:RangeValidator ID="RNVinput" runat="server" SetFocusOnError="true" ControlToValidate="INtextboxInput" Enabled="false" EnableClientScript="true" Type="Date" ></asp:RangeValidator>
            <span class="icons"><!--
                --><asp:Label cssclass="icon ok xs" ToolTip="*save" id="LBactionSave" runat="server"></asp:Label><!--
                --><asp:Label cssclass="icon cancel xs" ToolTip="*cancel" id="LBactionCancel" runat="server"></asp:Label><!--
                --></span>
        <asp:Literal ID="LTspanEditClose" runat="server"></span></asp:Literal>
    </span>
   <%-- <asp:Literal ID="LTspanContainerClose" runat="server"></span></asp:Literal>--%>
</div>

<%--<asp:Literal ID="LTspanContainer" runat="server" Visible="false"><span class="{0} viewmode"></asp:Literal>--%>
<asp:Literal ID="LTspanEdit" runat="server" Visible="false"><span class="edit {0}" title="{1}"></asp:Literal>
<asp:Literal ID="LTviewModeCssClass" runat="server" Visible="false">viewmode</asp:Literal>
<asp:Literal ID="LTeditModeCssClass" runat="server" Visible="false">editmode</asp:Literal>
<asp:Literal ID="LTdisabledModeCssClass" runat="server" Visible="false">disabled</asp:Literal>

<%-- <span class="editable viewmode">
                                <span class="view" title="click to edit">01/01/2013</span>
                                <span class="edit" title="Esc to cancel, Enter to save">
      
                                    <span class="icons"><!--
                                    --><span class="icon ok xs" title="save"></span><!--
                                    --><span class="icon cancel xs" title="cancel"></span><!--
                                    --></span>
                                </span>
                            </span>--%>