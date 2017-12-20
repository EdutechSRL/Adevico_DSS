<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master"
    CodeBehind="AcceptLogonPolicy.aspx.vb" Inherits="Comunita_OnLine.AcceptLogonPolicy" %>

<%@ Register TagPrefix="CTRL" TagName="Policy" Src="~/Modules/ProfilePolicy/UC/UC_ProfilePolicy.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        div#Wizard
        {
            text-align: left;
            width: 950px;
            max-width: 950px;
            border: 1px solid black;
            margin-left: auto;
            margin-right: auto;
            padding: 15px;
            background-color: White;
        }
        div#data_content
        {
            padding-bottom: 3em;
            background-color: White;
        }
        div#data_content .Titolo_Campo
        {
            font-weight: bold;
        }
        
        div.StepButton
        {
            width: 949px;
            text-align: right;
        }
        div.PrivacyBox
        {
            width: 850px;
            height: 150px;
            max-height: 850px;
            height: 150px;
            overflow: auto;
            font-size: smaller;
            border: 1px solid black;
            margin-left: auto;
            margin-right: auto;
        }
        
        div.PrivacyBox ol
        {
            padding-left: 2em;
        }
        div.PrivacyBox ol li
        {
            list-style-type: decimal;
            padding-left: 0em;
        }
        
        div#form.Wizard
        {
            padding: 10px 0px 20px 0px !important;
            padding-bottom: 0px !important;
        }
        
        div.StepData
        {
            border: 1px solid black;
            margin: 5px;
            padding: 5px;
        }
        div.StepData span.Titolo_Campo
        {
            width: 12em;
            display: inline-block;
        }
        div.StepData input.Testo_Campo
        {
            width: 300px;
            display: inline-block;
        }
        div.StepData select.Testo_Campo
        {
            width: 300px;
            display: inline-block;
        }
        div.StepData span.Fieldrow
        {
            display: block;
            padding: 0.2em;
        }
        
        div#form.section
        {
            min-height: 10px !important;
            padding: 1px !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section">
        <h2>
            <asp:Literal ID="LTtitlePolicy" runat="server"></asp:Literal></h2>
    </div>
    <div id="data_content">
        <div id="Wizard">
            <div class="StepButton">
            </div>
            <div class="StepContent">
                <div class="StepData">
                    <CTRL:Policy ID="CTRLpolicy" runat="server" Visible="false"></CTRL:Policy>
                    <asp:Button ID="BTNsavePolicy" runat="server" CssClass="submit" Text="Richiesta" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
