<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_MeasureConverter.ascx.vb" Inherits="Comunita_OnLine.UC_MeasureConverter" %>

<%@ Register TagPrefix="CTRL" TagName="Measures" Src="~/Modules/DocTemplate/Uc/UC_Measure.ascx"%>

<fieldset class="light">
	<legend>
        <asp:literal ID="LITtag_t" runat="server">Measure converter</asp:literal>
    </legend>

    <div class="fieldobject">
        <CTRL:Measures ID="UC_MeasureIn" runat="Server" ShowDot="true" Label=" " />
        <CTRL:Measures ID="UC_MeasureOut" runat="Server" ShowDot="true" Label=">>"/>
        <div class="buttonwrapper">
            <asp:LinkButton ID="LKB_Calculate" runat="server" Text="Calculate"></asp:LinkButton>
        </div>
    </div>
</fieldset>