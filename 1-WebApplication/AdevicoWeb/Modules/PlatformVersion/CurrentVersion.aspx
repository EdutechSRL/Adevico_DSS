<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="CurrentVersion.aspx.vb" Inherits="Comunita_OnLine.CurrentVersion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .Titolo_Campo
        {
            display:inline-block;
            min-width: 150px;
            vertical-align:top;
        }
        
        .Titolo_CampoSmall
        {
            display:inline-block;
            min-width: 85px;
            vertical-align:top;
        }


        .Testo_campo
        {
            display:inline-block;
            padding-right: 25px;
        }

        .Testo_campoSmall
        {
            display:inline-block;
            min-width: 75px;
            /*padding-right: 25px;*/
        }

        ul.main > li
        {
            padding: 1em;
            padding-bottom: 0.5em;
            padding-top: 0;

        }

        ul.main ul
        {
            display: inline-block;
            max-width: 600px;
        }
        ul.main ul li
        {
            list-style-type: circle;
        }

         ul.main ul li.update
        {
            list-style-type: disc;
            padding-bottom: 0.5em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">
    Versioni piattaforma
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
    <h3>Versione corrente</h3>
    <span class="Titolo_Campo">Versione</span><span class="Testo_campo">0582925b</span>
    <br />
    <span class="Titolo_Campo">Data versione</span><span class="Testo_campo">28/09/2018</span>
    <br />
    <span class="Titolo_Campo">Base Url</span>
    <span class="Testo_campo">
        <asp:Literal ID="LITbaseUrl" runat="server"></asp:Literal>
    </span>
    <br />
    <h3>Dettagli versioni</h3>
    <ul class="month">
         <li>
            <h4>Settembre 2018</h4>
             <ul>
                 <li>
                    <span class="Titolo_CampoSmall">0582925b</span>
                    <span class="Titolo_CampoSmall">28/09/2018</span>
                    <ul>
                        <li class="update">
                           Update - Aggiornamento permessi file (Module Link)
                        </li>
                    </ul>
                </li>
             </ul>
        </li>
        <li>
            <h4>Versione 1.0</h4>
            <ul class="main">
                <li>
                    <span class="Titolo_CampoSmall">--------</span>
                    <span class="Titolo_CampoSmall">--/--/----</span>
                    <ul>
                        <li class="update">
                           Update - Rilascio versine MIT
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
</asp:Content>
