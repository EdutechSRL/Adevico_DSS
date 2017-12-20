<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_FuzzyInputGeneric.ascx.vb" Inherits="Comunita_OnLine.UC_FuzzyInputGeneric" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyInputSimple.ascx" TagName="CTRLsimple" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyInputExtended.ascx" TagName="CTRLextended" TagPrefix="CTRL" %>
<%@ Register Src="~/Modules/Dss/UC/UC_FuzzyInputIntermediateValues.ascx" TagName="CTRLintermediate" TagPrefix="CTRL" %>

<label for="" class="fieldlabel"><asp:literal id="LTweightTitle" runat="server"></asp:literal></label>
<span class="editable fuzzy viewmode" id="SPNedit" runat="server">
    <input type="hidden" class="fuzzytype" id="HDNfuzzyType" runat="server">
    <input type="hidden" class="fuzzyvalue"  id="HDNfuzzyValue" runat="server">
    <asp:Label ID="LBweightChoose" runat="server" CssClass="view" ToolTip="*click to edit"></asp:Label>
    <span class="edit fuzzyedit" title="*Esc to cancel, Enter to save" runat="server" id="SPNfuzzyEdit">
        <div class="fuzzytabs">
            <ul>
                <asp:Repeater id="RPTratingTabs" runat="server">
                    <ItemTemplate>
                        <li>
                            <a href="#tab-<%#Container.DataItem.Id%>">
                                <%#Container.DataItem.Name%>
                            </a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <li class="tablink commands">
                    <span class="icons-ui"><!--
                    --><asp:Label ID="LBsaveRating" runat="server" CssClass="icon ok ui-icon ui-icon-check" ToolTip="*Save"></asp:Label><!--
                    --><asp:Label ID="LBcancelRating" runat="server" CssClass="icon cancel ui-icon ui-icon-close" ToolTip="*cancel"></asp:Label><!--
                --></span>
                </li>
            </ul>
            <CTRL:CTRLsimple id="CTRLsimple" runat="server" visible="false"></CTRL:CTRLsimple>
            <CTRL:CTRLextended id="CTRLextended" runat="server" visible="false"></CTRL:CTRLextended>
            <CTRL:CTRLintermediate id="CTRLintermediate" runat="server" visible="false"></CTRL:CTRLintermediate>
        </div>
    </span>
</span>