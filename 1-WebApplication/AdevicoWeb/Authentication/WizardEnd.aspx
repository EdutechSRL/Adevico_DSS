<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Authentication.master"
    CodeBehind="WizardEnd.aspx.vb" Inherits="Comunita_OnLine.WizardUserProfileEnd" %>
<%@ MasterType VirtualPath="~/Authentication.Master" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPHmenu" runat="server">
    <asp:Literal ID="LTstartPage" runat="server" Visible="false" />
    <asp:Literal ID="LTbackToLoginPage" runat="server" Visible="false" />
    <asp:Literal ID="LTshibbolethLogon" runat="server" Visible="false" />
    <asp:Literal ID="LTexternalWebLogon" runat="server" Visible="false" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPHcontent" runat="server">
    <div id="form" class="section Wizard">
        <h2>
            <asp:Literal ID="LTtitleWizardCompleted" runat="server"></asp:Literal>
        </h2>
    </div>
    <div id="data_content">
        <div id="Wizard">
             <div class="StepButton">
            </div>
            <div class="StepContent">
                <div class="TopDescription">
                </div>
                <div class="StepData">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Literal ID="LTmessage" runat="server"></asp:Literal>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHbottomScripts" runat="server">
</asp:Content>
