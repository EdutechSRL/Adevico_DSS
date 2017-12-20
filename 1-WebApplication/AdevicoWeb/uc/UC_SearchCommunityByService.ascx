<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_SearchCommunityByService.ascx.vb"
    Inherits="Comunita_OnLine.UC_SearchCommunityByService" %>
<style type="text/css">
    .RigaFiltro
    {
        width: 100%;
        text-align: left;
        padding-bottom: 5px;
        clear: left;
    }
    .StartCella120
    {
        width: 110px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella
    {
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella100
    {
        width: 100px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella120
    {
        width: 110px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella130
    {
        width: 130px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella150
    {
        width: 150px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .Cella200
    {
        width: 190px;
        text-align: left;
        float: left;
        position: relative;
        padding-right: 10px;
    }
    .RigaFiltroSearch
    {
        text-align: right;
        padding-bottom: 5px;
        clear: left;
    }
</style>

<script type="text/javascript" language="Javascript">

    function ChangeCommunityCheckBoxState(id, checkState) {
        var cb = document.getElementById(id);
        if (cb != null)
            cb.checked = checkState;
    }

    function ChangeCommunityAllCheckBoxStates(checkState) {
        // Toggles through all of the checkboxes defined in the CheckBoxIDs array
        // and updates their value to the checkState input parameter
        if (CheckCommunityBoxIDs != null) {
            for (var i = 0; i < CheckCommunityBoxIDs.length; i++)
                ChangeCommunityCheckBoxState(CheckCommunityBoxIDs[i], checkState);
        }
    }
</script>

<div id="DIVcontenitoreSearch" style="width: 880px; padding: 5px 5px 5px 5px">
    <div id="DIVfilters" runat="server">
        <div id="DIVvalue" runat="server" class="RigaFiltro">
            <div class="StartCella120">
                <asp:Label ID="LBtipoComunita_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLTipo">Tipo Comunita</asp:Label>
            </div>
            <div class="Cella200">
                <asp:DropDownList ID="DDLTipo" runat="server" SkinID="Filter" AutoPostBack="true" CssClass="FiltroCampoSmall10">
                </asp:DropDownList>
            </div>
            <div class="Cella100">
                <asp:Label ID="LBtipoRicerca_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLTipoRicerca">Tipo Ricerca</asp:Label>
            </div>
            <div class="Cella120">
                <asp:DropDownList ID="DDLTipoRicerca" runat="server" SkinID="Filter" CssClass="FiltroCampoSmall10">
                    <asp:ListItem Value="1" Selected="true">Nome inizia per</asp:ListItem>
                    <asp:ListItem Value="2">Nome contiene</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="Cella">
                <asp:TextBox ID="TXBValore" runat="server" SkinID="Filter" MaxLength="100" AutoPostBack="False"
                    Columns="40" CssClass="FiltroCampoSmall10"></asp:TextBox>
                    
  <%--                    <div class="Cella">
                &nbsp;<asp:Label ID="LBvalore_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="TXBValore">Valore</asp:Label>
            </div>--%>
            </div>
        </div>
        <div id="DIVtype" runat="server" class="RigaFiltro">
            <div class="StartCella120">
                <asp:Label ID="LBorganizzazione_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLorganizzazione">Organizzazione:&nbsp;</asp:Label>
            </div>
            <div class="Cella">
                <asp:DropDownList ID="DDLorganizzazione" runat="server" AutoPostBack="True" SkinID="Filter" CssClass="FiltroCampoSmall10">
                </asp:DropDownList>
            </div>
        </div>
        <div id="DIVstatusOther" runat="server" class="RigaFiltro">
            <div class="StartCella120">
                <asp:Label ID="LBstatoComunita_t" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLstatoComunita">Stato:</asp:Label>
            </div>
            <div class="Cella200">
                <asp:DropDownList ID="DDLstatoComunita" runat="server" SkinID="Filter" AutoPostBack="True" CssClass="FiltroCampoSmall10">
                    <asp:ListItem Value="0" Selected="true">Attivate</asp:ListItem>
                    <asp:ListItem Value="1">Archiviate</asp:ListItem>
                    <asp:ListItem Value="2">Bloccate</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="DIVcourse" runat="server">
                <div class="Cella120">
                    <asp:Label ID="LBannoAccademico_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLannoAccademico">Anno Accademico:</asp:Label>
                </div>
                <div class="Cella130">
                    <asp:DropDownList ID="DDLannoAccademico" runat="server" AutoPostBack="True" SkinID="Filter" CssClass="FiltroCampoSmall10">
                    </asp:DropDownList>
                </div>
                <div class="Cella">
                    <asp:Label ID="LBperiodo_c" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLperiodo">Periodo:&nbsp;</asp:Label>
                </div>
                <div class="Cella">
                    <asp:DropDownList ID="DDLperiodo" runat="server" AutoPostBack="True" SkinID="Filter" CssClass="FiltroCampoSmall10">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="DIVdegreeType" runat="server">
                <div class="Cella120">
                    <asp:Label ID="LBcorsoDiStudi_t" runat="server" CssClass="FiltroVoceSmall10" AssociatedControlID="DDLtipoCorsoDiStudi">Tipo Cdl:&nbsp;</asp:Label>
                </div>
                <div class="Cella">
                    <asp:DropDownList ID="DDLtipoCorsoDiStudi" runat="server" AutoPostBack="True" SkinID="Filter" CssClass="FiltroCampoSmall10">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="DIVsearch" runat="server" class="RigaFiltroSearch">
                <asp:Button ID="BTNCerca" runat="server" CssClass="PulsanteFiltro" Text="Cerca" CausesValidation="False">
                </asp:Button>
            </div>
        </div>
    </div>
    <div>
        <asp:GridView ID="GRVcomunita" runat="server" DataKeyNames="CommunitySubscriptedID"
            AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true" UseAccessibleHeader="true"
            PageSize="25" CssClass="table light fullwidth">
            <FooterStyle Font-Bold="True" ForeColor="White" />
            <HeaderStyle CssClass="ROW_header_Small" />
            <RowStyle CssClass="ROW_Normal_Small" />
            <AlternatingRowStyle CssClass="ROW_Alternate_Small" />
            <SelectedRowStyle BackColor="Aquamarine" />
            <Columns>
                <asp:BoundField DataField="CommunitySubscriptedID" Visible="False" />
                <asp:CommandField SelectText="Seleziona" ShowSelectButton="True" HeaderText="Opzioni"
                    AccessibleHeaderText="Seleziona la comunit&#224; desiderata" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="CBXselectAll" ToolTip="Click here to select/deselect all rows"
                            runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CBXselect" runat="server" />
                        <asp:Label ID="LBcommunityID" runat="server" Text='<%# Container.DataItem.CommunitySubscripted.id %>'
                            Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                    <ItemTemplate>
                        <%#Container.DataItem.CommunitySubscripted.Name%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo comunit&#224;" SortExpression="Type">
                    <ItemTemplate>
                        <%#Container.DataItem.CommunitySubscripted.Type.Name%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>
