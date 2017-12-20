<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_DisplayFileDetail.ascx.vb" Inherits="Comunita_OnLine.UC_DisplayFileInfo" %>

  <style type="text/css">
    .DFD_Info
    {
        
        }
    .DFD_TitoloLabel
    {
        
        }
        
    .DFD_DownDet
    {
        padding-left: 2em;
        }
  </style>

<div id="DivGenericInfo" class="DFD_Info">
    <asp:Label ID="Lbl_DetFile_t" runat="server" CssClass="Titolo_campo">t_Dettagli file</asp:Label>
    &nbsp;&nbsp;&nbsp;
    <asp:Label ID="Lbl_FileName" runat="server" CssClass="Testo_campo">#NomeFile</asp:Label>
    <br /><br />
    <asp:Label ID="Lbl_ComService_t" runat="server" CssClass="Titolo_campoSmall">t_Comunità/Servizio</asp:Label>&nbsp;
    <asp:Label ID="Lbl_ComService" runat="server" CssClass="Testo_campoSmall">#Com/Service Name</asp:Label>
    <br />
    <asp:Label ID="Lbl_Path_t" runat="server" CssClass="Titolo_campoSmall">t_Percorso</asp:Label>&nbsp;
    <asp:Label ID="Lbl_Path" runat="server" CssClass="Testo_campoSmall">#Obj/object/</asp:Label>
    <br />
    <asp:Label ID="Lbl_Size_t" runat="server" CssClass="Titolo_campoSmall">t_Size</asp:Label>&nbsp;
    <asp:Label ID="Lbl_Size" runat="server" CssClass="Testo_campoSmall">#Size</asp:Label> Kb
    <br />
    <asp:Label ID="Lbl_UbloadBy_t" runat="server" CssClass="Titolo_campoSmall">t_Caricato da</asp:Label>&nbsp;
    <asp:Label ID="Lbl_UbloadBy" runat="server" CssClass="Testo_campoSmall">#Tizio Caio</asp:Label>&nbsp;
    <asp:Label ID="Lbl_UploadedOn" runat="server" CssClass="Testo_campoSmall">il ##/##/#### alle ##.##</asp:Label>
    <br />
    <asp:Label ID="Lbl_TotalDownload_t" runat="server" CssClass="Titolo_campoSmall">t_Totale Download</asp:Label>&nbsp;
    <asp:Label ID="Lbl_TotalDownload" runat="server" CssClass="Testo_campoSmall">###</asp:Label>
    <br />
</div>

<asp:Panel ID="PnlDownloadDetail" runat="server">
    <br />
    <asp:Label ID="Lbl_DetDownload_t" runat="server" CssClass="Titolo_campoSmall">Dettagli Download</asp:Label>
    <br />
    <div class="DFD_DownDet">
        <asp:Repeater ID="RptDownSummary" runat="server">
            <ItemTemplate>
                <br />
                <asp:Label ID="Lbl_PersonName" runat="server" CssClass="Testo_campoSmall">Nome Cognome</asp:Label>
                &nbsp;
                (<asp:Label ID="Lbl_NumDown" runat="server" CssClass="Testo_campoSmall"># Download</asp:Label>)
                <br />
                <div class="DFD_DownDet">
                <asp:Repeater ID="RptDownDetail" runat="server">
                    <ItemTemplate>
                        <asp:Label id="Lbl_DD_Date" runat="server" CssClass="Testo_campoSmall">dd/MM/aaaa hh:mm</asp:Label>
                        &nbsp;
                        <asp:Label ID="Lbl_DD_Service_t" runat="server" CssClass="Testo_campoSmall">from</asp:Label>
                        &nbsp;
                        <asp:Label ID="Lbl_DD_Service" runat="server" CssClass="Testo_campoSmall">ServiceName</asp:Label>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <br />
    </div>
</asp:Panel>