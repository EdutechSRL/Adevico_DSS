<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SceltaPermessi.ascx.vb" Inherits="Comunita_OnLine.UC_SceltaPermessi" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<input type="hidden" id="HDN_servizioID" runat="server" name="HDN_servizioID" />
<input type="hidden" id="HDN_azione" runat="server" />
<input type="hidden" id="HDN_associati" runat="server" />
<input type="hidden" id="HDN_definito" runat="server" name="HDN_definito" />
<input id="HDNhasSetup" type="hidden" runat="server" name="HDNhasSetup" />
<asp:Table ID="TBLassociaPermessi" runat="server" HorizontalAlign="Center" Width="800px" Visible="true" GridLines="none">
    <asp:TableRow>
        <asp:TableCell Height="40px">&nbsp</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="LBinfoAssocia_t" runat="server" CssClass="info_black11"></asp:Label>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell CssClass="top" Width="50px">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="top">
                        <asp:Label ID="LBpermessi_t" runat="server" CssClass="Titolo_CampoSmall">Permessi:</asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="CBLpermessi" runat="server" RepeatDirection="Vertical" CssClass="Testo_CampoSmall"></asp:CheckBoxList>
                    </td>
                </tr>
            </table>

        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
