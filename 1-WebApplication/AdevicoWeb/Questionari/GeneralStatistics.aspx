<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPopup.Master" CodeBehind="GeneralStatistics.aspx.vb" Inherits="Comunita_OnLine.GeneralStatistics" %>

<%@ MasterType VirtualPath="~/AjaxPopup.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link media="screen" href="stile.css?v=201604071200lm" type="text/css" rel="StyleSheet" />

    <style>
        div.optionChart
        {
            
            display: inline-block;
        }

        div.barChart {
            width: 50%;
            min-width: 250px;
        }

        div.pieChart
        {
            /*KEEP aspect ratio!*/
            width: 450px;
            height: 250px;
        }
        
        html body .question table.tableData
        {
            width: 48%;
            float: left;
            margin-right: 1%;
            margin-bottom: 22px;
        }
		td.statistics {
			border: none;
			border-bottom: solid 1px #ececec;
		}
        table.questionfreetext{
            width:100%;
        }
			
		.questionnairepage.name{
			display:block;
			margin-bottom:6px;
			margin-top:6px;
		}

        html body .question .longLabel table.tableData{
            float:none;
            width:100%;   
        }

        html body .question .longLabel div.optionChart {
            float:none;
            width:100%; /*900px;   */
            /*border: 1px solid red;*/ /*da togliere*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHservice" runat="Server">
    <asp:MultiView runat="server" ID="MLVquestionari">
        <asp:View ID="VIWdati" runat="server">
            <br />
            <asp:PlaceHolder ID="PHStat" runat="server"></asp:PlaceHolder>
            <br />
            <br />
            <asp:Label ID="LBerrorMSG" runat="server" CssClass="errore" Visible="false">No data</asp:Label>
        </asp:View>
        <asp:View runat="server" ID="VIWmessaggi">
            <asp:Label ID="LBerrore" runat="server" CssClass="errore">Non hai i permessi</asp:Label>
        </asp:View>
    </asp:MultiView>
</asp:Content>